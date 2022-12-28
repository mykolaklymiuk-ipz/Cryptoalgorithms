using EncryptionLab6;
using System.Text;
using static System.Console;

var text = "test string consists of more than ten bytes";

var result = AesProvider.Encrypt(Encoding.UTF8.GetBytes(text), 
    new byte[]{ 0x61, 0x62, 0x63, 0x64, 
                0x61, 0x62, 0x63, 0x64, 
                0x61, 0x62, 0x63, 0x64, 
                0x61, 0x62, 0x63, 0x64, });

Write(@$"initial text: {text}
result:
in bytes: ");

result.ToList().ForEach(x => Write(x.ToString() + " "));
WriteLine("\nin string: " + Encoding.UTF8.GetString(result));
