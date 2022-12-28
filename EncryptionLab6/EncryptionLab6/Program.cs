using EncryptionLab5;

(byte a, byte b) input =     (0xD4, 0x02);
(byte a, byte b) secInput =  (0xBF, 0x03);

var result = GaloisFieldsProvider.GaloisFieldsMultiplication(input.a, input.b);
var secResult = GaloisFieldsProvider.GaloisFieldsMultiplication(secInput.a, secInput.b);

Console.WriteLine($"multiplication({BytesToString(input)}) = {ByteToString(result)}");
Console.WriteLine($"multiplication({BytesToString(secInput)}) = {ByteToString(secResult)}");

static string BytesToString((byte a, byte b) tuple) =>
    $"{ByteToString(tuple.a)}, {ByteToString(tuple.b)}";

static string ByteToString(byte b) =>
    $"{BitConverter.ToString(new[] { b })}";

