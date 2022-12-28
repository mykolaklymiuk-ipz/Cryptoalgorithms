
using EncryptionLab11;

var p = new Point { x = 17, y = 25 };

var isLaying = ElipticCurveProvider.PointLayOnFunc(p);

var q = ElipticCurveProvider.GetQ(p);

var qPlusP = p.AddP(q);


Console.WriteLine($"P = (x: {p.x}, y: {p.y})");
Console.WriteLine($"point is laying in curve: {isLaying}");
Console.WriteLine($"2P = Q = (x: {q.x}, y: {q.y})");
Console.WriteLine($"Q + P = ({qPlusP.x}, {qPlusP.y})");

var allPoints = p.GetAllPoints();
Console.WriteLine($"d: {allPoints.Count}");
Console.WriteLine($"points:");
allPoints.ForEach(point => Console.WriteLine($"x: {point.x}, y: {point.y}"));

