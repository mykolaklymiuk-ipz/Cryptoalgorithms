using EncryptoinLab9;
using System.Numerics;
using System.Text;

var text = "some text";
var bytes = Encoding.UTF8.GetBytes(text);

var p = new BigInteger(1234);
var q = new BigInteger(4567);

var dsa = new DsaProvider(p, q);


Console.WriteLine($"signing text: '{text}'");
var signingOutput = dsa.GenerateDigitalSignature(bytes);

Console.WriteLine($"signature is valid(using previous password) = {dsa.VerifyDigitalSignature(bytes, signingOutput)}");

