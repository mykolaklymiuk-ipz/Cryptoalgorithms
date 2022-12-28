using static System.Math;

namespace encryption_lab_4;


static class EncryptionHelper
{
    public static int Gcd(int a, int b)
    {
        while (a > 0 && b > 0)
            if (a > b)
                a %= b;
            else
                b %= a;
        return a + b;
    }

    public static (int result, int x, int y) MyGcdExtended(int a, int b)
    {
        var initialValues = a > b ? (greater: a, smaller: b) : (greater: b, smaller: a);
        var (greater, smaller) = initialValues;

        var bezoutCoeficientS = new Queue<int>(new[] { 1, 0 });
        var bezoutCoeficientT = new Queue<int>(new[] { 0, 1 });

        var multiplicity = 0;

        do
        {
            var remainder = greater % smaller;
            multiplicity = greater / smaller;
            greater = smaller;
            smaller = remainder;

            bezoutCoeficientS.Enqueue(bezoutCoeficientS.Dequeue() - multiplicity * bezoutCoeficientS.Peek());
            bezoutCoeficientT.Enqueue(bezoutCoeficientT.Dequeue() - multiplicity * bezoutCoeficientT.Peek());
        }
        while (smaller != 0);

        return (greater, bezoutCoeficientS.Peek(), bezoutCoeficientT.Peek());
    }

    public static (int xn, int yn, int result) GcdExtended(int a, int b)
    {
        int x0 = 1, xn = 1, y0 = 0, yn = 0, x1 = 0, y1 = 1, f, r = a % b;

        while (r > 0)
        {
            f = a / b;
            xn = x0 - f * x1;
            yn = y0 - f * y1;

            x0 = x1;
            y0 = y1;
            x1 = xn;
            y1 = yn;
            a = b;
            b = r;
            r = a % b;
        }

        return (xn, yn, b);
    }

    public static int InverseElement(int a, int n) =>
        GcdExtended(a, n).result;

    public static int Phi(int n)
    {
        var result = n; 
        for (int i = 2; Pow(i, 2) <= n; ++i)
            if (n % i == 0)
            {
                while (n % i == 0)
                    n /= i;
                result -= result / i;
            }
        return n > 1 ? result -= result / n : result;
    }

    public static int InverseElementEuler(int a, int p) =>
        (int)(Pow(a, Phi(p)) / p);
}
