using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

namespace encryption_lab_2
{
    public static class SelectionEncrypter
    {
        private static int Cols => 6;
        private static int Rows => 5;
        private static string Alphabet => "абвгдеєжзиійклмнопрстуфхцчшьюя";

        public static string Encrypt(string text)
        {
            var alphabet = GenerateAlphabet();
            var positions = text.Select(c => alphabet.Find(ch => ch.ch == c).pos);
            var numCodes = positions.Select(it => it.y).Concat(positions.Select(it => it.x)).ToArray();

            List<(int, int)> groupedCodes = new();

            for (int i = 0; i < numCodes.Length; i+=2)
                groupedCodes.Add((numCodes[i + 1], numCodes[i]));

            return new string(groupedCodes.Select(cod => alphabet.Find(ch => ch.pos == cod).ch).ToArray());
        }

        private static List<(char ch, (int x, int y) pos)> GenerateAlphabet() =>
            Alphabet.Zip(
                Range(0, Rows).SelectMany(y => Range(0, Cols).Select(x => (x, y))), 
                (ch, pos) => (ch, pos)
            ).Select(it => (it.ch, it.pos)).ToList();
    }
}

