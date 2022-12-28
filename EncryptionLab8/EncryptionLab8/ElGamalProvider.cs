using static System.Math;

namespace EncryptionLab8;

public static class ElGamalProvider
{
    public static (int[] enctypted, int p, int q, int hReceiver) Encrypt(byte[] arr)
    {
        var (q, g, key, hSender) = GenerateEncryptionParams();

        var s = ModularExponentiation(hSender, key, q);
        var p = ModularExponentiation(g, key, q);
        var hReceiver = ModularExponentiation(p, key, q);

        var encryptedMessage = arr.Select(it => it ^ s).ToArray();
        return (encryptedMessage, p, q, hReceiver);
    }

    public static byte[] Decrypt(int[] encrypted, int p, int q, int hReceiver)
    {
        var key = GenerateKey(q);
        return encrypted.Select(it => (byte)(it ^ hReceiver)).ToArray();
    }

    public static int Gcd(int a, int b)
    {
        while (a > 0 && b > 0)
            if (a > b)
                a %= b;
            else
                b %= a;
        return a + b;
    }

    private static int ModularExponentiation(int a, int b, int c)
    {
        var x = 1;
        var y = a;

        while (b > 0)
        {
            if (b % 2 != 0)
                x = (x * y) % c;
            y = (int)(Pow(y, 2) % c);
            b /= 2;
        }
        
        return x % c;
    }

    private static (int q, int g, int key, int hSender) GenerateEncryptionParams()
    {
        var random = new Random();

        var q = random.Next((int)Pow(2, 16), (int)Pow(2, 30));
        var g = random.Next(2, q);
        var key = GenerateKey(q);

        return (q, g, key, ModularExponentiation(g, key, q));
    }

    private static int GenerateKey(int q)
    {
        int key;
        do
            key = new Random().Next((int)Pow(2, 16), q);
        while (Gcd(q, key) != 1);
        
        return key;
    }
}

