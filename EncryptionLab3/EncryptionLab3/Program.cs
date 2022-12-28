using EncryptionLab3;
using System;

Console.Write("Input encryption key: ");
var key = Console.ReadLine();

Console.Write("Input message to encrypt: ");
var message = Console.ReadLine();

var encryptedMessage = DesProvider.Encrypt(message, key);
Console.WriteLine($"Encrypted message: {Convert.ToHexString(encryptedMessage)}");

var decryptedMessage = DesProvider.Decrypt(encryptedMessage, key);
Console.WriteLine($"Decrypted message: {decryptedMessage}");

