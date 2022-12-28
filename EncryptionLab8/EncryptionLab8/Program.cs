using EncryptionLab8;
using System.Text;

var textToEncrypt = "some text to encrypt";
var textInBytes = Encoding.UTF8.GetBytes(textToEncrypt);

var (encrypted, p, q, hReceiver) = ElGamalProvider.Encrypt(textInBytes);

var decryptedBytes = ElGamalProvider.Decrypt(encrypted, p, q, hReceiver);

Console.WriteLine("text before encryption: " + textToEncrypt);
Console.WriteLine("text after decryption: " + Encoding.UTF8.GetString(decryptedBytes));
