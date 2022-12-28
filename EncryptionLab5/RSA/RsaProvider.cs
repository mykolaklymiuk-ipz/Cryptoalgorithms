using System.Numerics;
using System.Security.Cryptography;

namespace RSA
{
    class RsaProvider
	{
		private static RNGCryptoServiceProvider	rngCSP = new RNGCryptoServiceProvider();
		
		private static BigInteger[]	testPrime = {2, 3, 5, 7, 11, 13 ,17, 19,
				23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71,73, 79, 83, 89, 97, 101,
				103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
				179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251};

		private static bool	TestPrime(BigInteger number)
		{
			int i;

			for (i = 0; i < testPrime.Length; i++)
			{
				if (number < testPrime[i])
					break;
				if ((number % testPrime[i]) == 0)
					return true;
			}
			if (i == 0)
				return false;
			return true;
		}

		private static bool	TestMillerRabin(BigInteger n, int rounds)
		{
			int	s;
			BigInteger t;

			if (n == 1)
				return false;

			t = n - 1;
			s = 0;

			while(t % 2 == 0)
			{
				t = t / 2;
				s++;
			}

			for (int i = 0; i < rounds; i++)
			{
				RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
				byte[] buffer = new byte[n.ToByteArray().LongLength];
				BigInteger a;
				BigInteger x;

				if ( n == 0 || n == 1 || n == 4 )
					return false;

				if( n == 2 || n == 3 )
					return true;

				do
				{
					rng.GetBytes(buffer);
					a = new BigInteger(buffer);
					if( a < 0 )
						a = -a;
				}
				while (a < 2 || a >= n - 2);

				x = BigInteger.ModPow(a, t, n);

				if (x == 1 || x == n - 1)
					continue;

				for (int r = 1; r < s; r++)
				{
					x = BigInteger.ModPow(x, 2, n);
					if (x == 1)
						return false;

					if (x == n - 1)
						break;
				}

				if (x != n - 1)
					return false;
			}
			return (true);
		}
        private static BigInteger GeneratePrime()
		{
			byte[]		buffer;
			BigInteger	result;

            result = 1;
			buffer = new byte[1];
			while (!TestMillerRabin(result, 100))
			{
				result = 1;
				while (!TestPrime(result))
				{
					rngCSP.GetBytes(buffer);
                    result = new BigInteger(buffer);

					if (result < 0)
						result = -result;
				}
			}
			return result;
		}

        private static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (a != b)
            {
				if ( a > b )
					a -= b;
                else
					b -= a;
            }
			return (a);
        }
		private static BigInteger GetE(BigInteger phi )
		{
			BigInteger e;

			e = 3;
            while (e < phi)
			{
				if (GCD(e, phi) == 1)
					return e;
				e++;
			}
			return -1;
		}
		private static BigInteger ComputeEulersFunc(BigInteger n)
		{
			BigInteger result = n;

			for (BigInteger i = 2; i * i < n; i++)
			{
				if (n % i == 0)
				{
					while (n % i == 0)
						n /= i;
					result -= result / i;
				}
			}

			if (n > 1)
				result -= result / n;
			return result;
		}

		private static BigInteger GetD(BigInteger e, BigInteger phi)
		{
            BigInteger[,] table = new BigInteger[2,3];
			table[0, 0] = table[1, 0] = phi;
			table[0, 1] = e;
			table[1, 1] = 1;
            while (true)
            {
                table[0, 2] = table[0, 0] - (table[0, 0] / table[0, 1]) * table[0, 1];

                if ((table[1, 2] = table[1, 0] - (table[0, 0] / table[0, 1]) * table[1, 1]) < 0)
                {
					while (table[1,2] < 0)
                        table[1, 2] = table[1, 2] + phi; //mod %
                }

                if( table[0, 2] == 1 )
					return table[1, 2];

                table[0, 0] = table[0, 1];
                table[1, 0] = table[1, 1];
                table[0, 1] = table[0, 2];
                table[1, 1] = table[1, 2];
            }

        }
		public static BigInteger[] GenerateRSAKeys()
		{
			BigInteger p = GeneratePrime();
			BigInteger q = GeneratePrime();
			BigInteger[] result = new BigInteger[3];
			BigInteger n;
			BigInteger phi;
			BigInteger e;
			BigInteger d;

			n = BigInteger.Multiply(p, q);
            phi = BigInteger.Multiply(BigInteger.Subtract(p, 1), BigInteger.Subtract(q, 1)); //ComputeEulersFunc(n);
			e = GetE( phi);
            d = GetD(e, phi);
            result[0] = e;
			result[1] = d;
			result[2] = n;

			return result;
		}

		public static BigInteger Encrypt(BigInteger e, BigInteger n, BigInteger m) =>
			BigInteger.Pow(m, (int)e) % n;

		public static BigInteger Decrypt(BigInteger d, BigInteger n, BigInteger c) =>
			BigInteger.Pow(c, (int)d) % n;
	}
}

