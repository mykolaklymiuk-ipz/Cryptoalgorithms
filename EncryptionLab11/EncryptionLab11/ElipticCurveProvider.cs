using static System.Math;

namespace EncryptionLab11;


public static class ElipticCurveProvider
{
    private static int _a = 1;
    private static int _b = 1;
    private static int _p = 23;

    // task 1
    public static bool PointLayOnFunc(Point p) =>
        (Pow(p.x, 3) + p.x + 1) % _p == Pow(p.y, 2) % _p;

    // task 2
    public static Point GetQ(Point p)
    {
        var sDevided = (int)(3 * Pow(p.x, 2) + _a);
        var sDevider = 2 * p.y;

        var lambda = (sDevided.GetMod(_p) / sDevider.GetMod(_p)); 

        var q = new Point();
        
        q.x = ((int)Pow(lambda, 2) - 2 * p.x).GetMod(_p);
        q.y = (lambda * (p.x - q.x) - p.y).GetMod(_p);

        return q;
    }

    // task 3
    public static Point AddP(this Point p, Point pToAdd)
    {
        if(p.x == pToAdd.x && p.y == pToAdd.y) 
            return GetQ(p);

        var resultPoint = new Point();

        var sDevided = pToAdd.y - p.y;
        var sDevider = pToAdd.x - p.x;
        
        if(sDevider == 0)
            throw new ArgumentException("a must not be zero");

        var lambda = 0;
        if(sDevider < 0) 
        {
            var modDevider = sDevider.GetMod(_p);
            var coefDevider = GcdExtended(modDevider, _p).y;
            lambda = (sDevided.GetMod(_p) * coefDevider.GetMod(_p)).GetMod(_p);
        } 
        else
            lambda = (sDevided.GetMod(_p) / sDevider.GetMod(_p)).GetMod(_p);

        resultPoint.x = ((int)Pow(lambda, 2) - p.x - pToAdd.x).GetMod(_p);
        resultPoint.y = (lambda * (p.x - resultPoint.x) - p.y).GetMod(_p);

        return resultPoint;
    }

    // n = (result of this method) mod p
    private static int GetMod(this int n, int p) =>
        n >= 0
            ? n % p
            : p + (n % p);


    public static (int result, int x, int y) GcdExtended(int a, int b)
    {
        if (a == 0)
            throw new ArgumentException("a must not be zero");

        var initialValues = a > b ? (greater: a, smaller: b) : (greater: b, smaller: a);
        var (greater, smaller) = initialValues;

        var bezoutCoeficientS = new Queue<int>(new[] { 1, 0 });
        var bezoutCoeficientT = new Queue<int>(new[] { 0, 1 });

        do
        {
            var remainder = greater % smaller;
            var multiplicity = greater / smaller;
            greater = smaller;
            smaller = remainder;

            bezoutCoeficientS.Enqueue(bezoutCoeficientS.Dequeue() - multiplicity * bezoutCoeficientS.Peek());
            bezoutCoeficientT.Enqueue(bezoutCoeficientT.Dequeue() - multiplicity * bezoutCoeficientT.Peek());
        }
        while (smaller != 0);

        return (greater, bezoutCoeficientS.Peek(), bezoutCoeficientT.Peek());
    }

    public static List<Point> GetAllPoints(this Point point)
    {
        var q = GetQ(point);
        var allPoints = new List<Point> { point, q };

        try
        {
            while (true)
            {
                var lastPoint = allPoints.Last();
                allPoints.Add(point.AddP(lastPoint));
            }
        }
        catch (ArgumentException)
        {
            allPoints.Add(new Point { x = 0, y = 0 });
            return allPoints;
        }
    }
}

public struct Point
{
    public int x;
    public int y;
}
