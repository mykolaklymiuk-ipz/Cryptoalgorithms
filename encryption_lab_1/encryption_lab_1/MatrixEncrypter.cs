using System;
using System.Linq;

namespace encryption_lab_1
{
    public static class MatrixEncrypter
    {
        public static char[][] Encrypt(string columnKeyWord, string rawKeyWord, string text)
        {
            var maxLength = columnKeyWord.Length * rawKeyWord.Length;

            if (text.Length > maxLength)
                throw new Exception("text is longer then keywords allows");

            text = text + new string(' ', maxLength - text.Length);

            return Enumerable.Range(0, text.Length / columnKeyWord.Length)
                .Select(i => text.Substring(i * columnKeyWord.Length, columnKeyWord.Length))
                .Zip(rawKeyWord, (s, k) => (s, k))
                .OrderBy(it => it.k)
                .Select(it => it.s
                    .Zip(columnKeyWord, (s, k) => (s, k))
                    .OrderBy(t => t.k)
                    .Select(t => t.s)
                    .ToArray())
                .ToArray();
        }
    }
}

