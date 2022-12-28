using System.Numerics;
using System.Security.Cryptography;

namespace EncryptoinLab10;

class DsaProvider
{
    private static int L = 1024, N = 160;
    private const int primeTestIterations = 16;
    public static BigInteger p, q, h, a, b;

    public DsaProvider(BigInteger _p, BigInteger _q)
    {
        p = _p;
        q = _q;

        GenerateKeyPart();
    }

    public DsaProvider(int _L, int _N)
    {
        L = _L;
        N = _N;
        GenerateKey();
    }

    public (BigInteger s1, BigInteger s2) GenerateDigitalSignature(byte[] text)
    {
        BigInteger r = RandomInt(1, q - 1);
        BigInteger r_ = ModInverse(r, q);
        BigInteger s1 = BigInteger.ModPow(h, r, p) % q;

        SHA1 H = new SHA1Managed();
        BigInteger hash = new BigInteger(H.ComputeHash(text));
        BigInteger s2 = (r_ * (hash + a * s1)) % q;

        return (s1, s2);
    }

    public bool VerifyDigitalSignature(byte[] text, (BigInteger s1, BigInteger s2) signature)
    {
        BigInteger s_ = ModInverse(signature.s2, q);

        SHA1 H = new SHA1Managed();
        BigInteger hash = new BigInteger(H.ComputeHash(text));

        BigInteger u1 = (hash * s_) % q;

        if (u1 < 0)
            u1 += q;

        BigInteger u2 = (s_ * signature.s1) % q;

        BigInteger t = ((BigInteger.ModPow(h, u1, p) * BigInteger.ModPow(b, u2, p)) % p) % q;

        if (t == signature.s1)
            return true;

        return false;
    }

    static void GenerateKey()
    {
        p = RandomPrime(L / 8);

        do
        {
            q = RandomPrime(N / 8);

            if (q % 2 == 0)
                q--;
        }
        while ((p - 1) % q != 0);

        GenerateKeyPart();
    }

    static void GenerateKeyPart()
    {
        do
        {
            h = BigInteger.ModPow(RandomInt(2, p - 2), (p - 1) / q, p);
        }
        while (BigInteger.ModPow(h, q, p) != 1);

        a = RandomInt(2, q);
        b = BigInteger.ModPow(h, a, p);
    }

    static BigInteger RandomInt(int size)
    {
        Random rng = new Random(DateTime.Now.Millisecond);

        byte[] resultBytes = new byte[size];

        resultBytes[0] = (byte)rng.Next(1, 256);

        for (int i = 1; i < size; i++)
            resultBytes[i] = (byte)rng.Next(0, 256);

        return BigInteger.Abs(new BigInteger(resultBytes));
    }

    static BigInteger RandomInt(BigInteger min, BigInteger max)
    {
        var rng = new RNGCryptoServiceProvider();

        byte[] resultBytes = new byte[max.ToByteArray().Length];
        BigInteger result;

        do
        {
            rng.GetBytes(resultBytes);
            result = BigInteger.Abs(new BigInteger(resultBytes));
        }
        while (result < min || result > max);

        return result;
    }

    public static BigInteger RandomPrime(int size)
    {
        BigInteger prime;

        do
        {
            prime = RandomInt(size);

            if (prime % 2 == 0)
                prime--;
        }
        while (!RabinMillerTest(prime));

        return prime;
    }

    static BigInteger RandomPrime(BigInteger min, BigInteger max)
    {
        BigInteger prime;

        do
        {
            prime = RandomInt(min + 1, max);

            if (prime % 2 == 0)
                prime--;
        }
        while (!RabinMillerTest(prime));

        return prime;
    }

    static bool RabinMillerTest(BigInteger source)
    {
        if (source == 2 || source == 3)
            return true;

        if (source < 2 || source % 2 == 0)
            return false;

        BigInteger d = source - 1;
        int s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        var rng = new RNGCryptoServiceProvider();
        byte[] bytes = new byte[source.ToByteArray().Length];
        BigInteger a;

        for (int i = 0; i < primeTestIterations; i++)
        {
            do
            {
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a < 2 || a >= source - 2);

            BigInteger x = BigInteger.ModPow(a, d, source);

            if (x == 1 || x == source - 1)
                continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, source);

                if (x == 1)
                    return false;

                if (x == source - 1)
                    break;
            }

            if (x != source - 1)
                return false;
        }

        return true;
    }

    public static BigInteger ModInverse(BigInteger number, BigInteger modulo)
    {
        BigInteger t = 0, newT = 1, r = modulo, newR = number;

        while (newR != 0)
        {
            BigInteger quotient = r / newR;

            BigInteger tmp = newT;
            newT = t - quotient * newT;
            t = tmp;

            tmp = newR;
            newR = r - quotient * newR;
            r = tmp;
        }

        if (t > modulo)
            t -= modulo;

        while (t < 0)
            t += modulo;

        return t;
    }
}

