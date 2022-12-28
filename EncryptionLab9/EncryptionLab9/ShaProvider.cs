using System.Collections;
using System.Text;

namespace EncryptionLab9;

public static class ShaProvider
{
    private const int _hashLength = 64;

    private static readonly int[] _h = new int[]
    {
        0x6a09e667,
        unchecked((int)0xbb67ae85),
        0x3c6ef372,
        unchecked((int)0xa54ff53a),
        0x510e527f,
        unchecked((int)0x9b05688c),
        0x1f83d9ab,
        0x5be0cd19
    };

    private static readonly int[] _k = new int[]
    {
        0x428a2f98, 0x71374491, unchecked((int)0xb5c0fbcf), unchecked((int)0xe9b5dba5), 0x3956c25b, 0x59f111f1, unchecked((int)0x923f82a4), unchecked((int)0xab1c5ed5),
        unchecked((int)0xd807aa98), 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, unchecked((int)0x80deb1fe), unchecked((int)0x9bdc06a7), unchecked((int)0xc19bf174),
        unchecked((int)0xe49b69c1), unchecked((int)0xefbe4786), 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
        unchecked((int)0x983e5152), unchecked((int)0xa831c66d), unchecked((int)0xb00327c8), unchecked((int)0xbf597fc7), unchecked((int)0xc6e00bf3), unchecked((int)0xd5a79147), 0x06ca6351, 0x14292967,
        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, unchecked((int)0x81c2c92e), unchecked((int)0x92722c85),
        unchecked((int)0xa2bfe8a1), unchecked((int)0xa81a664b), unchecked((int)0xc24b8b70), unchecked((int)0xc76c51a3), unchecked((int)0xd192e819), unchecked((int)0xd6990624), unchecked((int)0xf40e3585), 0x106aa070,
        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, unchecked((int)0x84c87814), unchecked((int)0x8cc70208), unchecked((int)0x90befffa), unchecked((int)0xa4506ceb), unchecked((int)0xbef9a3f7), unchecked((int)0xc67178f2)
    };

    public static byte[] Hash(this string text)
    {
        var _h = new int[]
        {
            0x6a09e667,
            unchecked((int)0xbb67ae85),
            0x3c6ef372,
            unchecked((int)0xa54ff53a),
            0x510e527f,
            unchecked((int)0x9b05688c),
            0x1f83d9ab,
            0x5be0cd19
        };

    var textInBytes = Encoding.UTF8.GetBytes(text).Append((byte)0b1000_0000).ToArray();

        textInBytes = textInBytes.Concat(new byte[_hashLength - textInBytes.Length - 1]).Append((byte)text.Length).ToArray();
        var intArray = new int[16];
        new BitArray(textInBytes).CopyTo(intArray, 0);

        intArray = intArray.Concat(new int[_hashLength - intArray.Length]).ToArray();

        for (int i = 16; i < intArray.Length; i++)
        {
            var s0 = intArray[i - 15].RightRotate(7) ^ intArray[i - 15].RightRotate(18) ^ intArray[i - 15].RightRotate(3);
            var s1 = intArray[i - 2].RightRotate(17) ^ intArray[i - 2].RightRotate(19) ^ intArray[i - 2].RightRotate(10);
            intArray[i] = intArray[i - 16] + s0 + intArray[i - 7] + s1;
        }

        var (a, b, c, d, e, f, g, h) = (_h[0], _h[1], _h[2], _h[3], _h[4], _h[5], _h[6], _h[7] );

        for (int i = 0; i < _hashLength; i++)
        {
            var s1 = e.RightRotate(6) ^ e.RightRotate(11) ^ e.RightRotate(25);
            var ch = (e & f) ^ ((e ^ int.MaxValue) & g);
            var tmp = h + s1 + ch + _k[i] + intArray[i];
            var s0 = a.RightRotate(2) ^ a.RightRotate(13) ^ a.RightRotate(22);
            var maj = (a & b) ^ (a & c) ^ (b & c);
            var tmp2 = s0 + maj;
            
            h = g;
            g = f;
            f = e;
            e = d + tmp2;
            d = c;
            c = b;
            b = a;
            a = tmp + tmp2;
        }
        
        _h[0] += a;
        _h[1] += b;
        _h[2] += c;
        _h[3] += d;
        _h[4] += e;
        _h[5] += f;
        _h[6] += g;
        _h[7] += h;

        var result = new byte[_hashLength / 2];
        new BitArray(_h).CopyTo(result, 0);
        return result;
    }

    public static int RightRotate(this int arrayElement, int count) =>
        (arrayElement >> count) | (arrayElement << (32 - count));
}
