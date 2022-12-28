using System.Numerics;
using System.Collections.Generic;

namespace RSA;

class Program
{

    private static List<BigInteger> EncryptMessage(string message, BigInteger e, BigInteger n)
    {
        List<BigInteger> secretMessage;

        secretMessage = new List<BigInteger>();
        foreach (char letter in message)
            secretMessage.Add(RsaProvider.Encrypt(e, n, letter));

        return secretMessage;
    }

    private static string DecryptMessage(List<BigInteger> c, BigInteger d, BigInteger n)
    {
        List<char> message;

        message = new List<char>();
        foreach (var el in c)
            message.Add((char)RsaProvider.Decrypt(d, n, el));

        return new string(message.ToArray());
    }

    static void Main(string[] args)
    {
        var keys = RsaProvider.GenerateRSAKeys();

        var text = "some text";

        System.Console.WriteLine($"text: '{text}'");
        var encrypted = EncryptMessage(text, keys[0], keys[2]);

        var decrypted = DecryptMessage(encrypted, keys[1], keys[2]);
        System.Console.WriteLine($"decrypted message: '{decrypted}'");
    }
}

