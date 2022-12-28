using System.Numerics;
using System.Collections.Generic;
using System.Linq;

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
        System.Console.WriteLine($"e: '{keys[0]}'");
        System.Console.WriteLine($"d: '{keys[1]}'");
        System.Console.WriteLine($"n: '{keys[2]}'");
        
        var encrypted = EncryptMessage(text, keys[0], keys[2]);

        var rsa = string.Join(" ", encrypted.Select(PrintBigIntegerAsHex));
        
        System.Console.WriteLine($"Encrypted RSA: '{rsa}'");
        
        var decrypted = DecryptMessage(encrypted, keys[1], keys[2]);
        System.Console.WriteLine($"decrypted message: '{decrypted}'");
    }

    private static string PrintBigIntegerAsHex(BigInteger s)
    {
        return string.Join("", s.ToByteArray().Select(b => b.ToString("x2")));
    }
}

