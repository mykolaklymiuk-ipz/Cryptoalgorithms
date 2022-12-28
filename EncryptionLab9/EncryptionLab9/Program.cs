using EncryptionLab9;

var text = "a";

var result = BitConverter.ToString(text.Hash());

Console.WriteLine(result);