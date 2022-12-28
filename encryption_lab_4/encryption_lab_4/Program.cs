using static encryption_lab_4.EncryptionHelper;

var x = 612;
var y = 342;


var result = GcdExtended(x, y);

var myResult = MyGcdExtended(x, y);

Console.WriteLine($"Gcd({x}, {y}) = " + Gcd(x, y));
Console.WriteLine($"GcdExtended({x}, {y}) = " + result.result + " / " + result.yn + " / " + result.xn + " / ");
Console.WriteLine($"MyGcdExtended({x}, {y}) = " + myResult.result + " / " + myResult.x + " / " + myResult.y + " / ");
Console.WriteLine("InverseElement(3, 7) = " + InverseElement(3, 7));
Console.WriteLine("Phi(1000) = " + Phi(1000));
Console.WriteLine("InverseElementEuler(3, 7) = " + InverseElementEuler(3, 7));
