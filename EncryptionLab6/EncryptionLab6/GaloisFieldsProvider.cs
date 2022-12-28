namespace EncryptionLab5;

public static class GaloisFieldsProvider
{
    public static byte GaloisFieldsMultiplication(byte a, byte b)
    {
        byte result = 0;
        for (int i = 0; i < 8; i++)
        {
            if ((b & 1) != 0)
                result ^= a;
            var hi_bit_set = (byte)(a & 0x80);
            a <<= 1;
            if (hi_bit_set != 0)
                a ^= 0x1b; /* x^8 + x^4 + x^3 + x + 1 */
            b >>= 1;
        }
        return result;
    }
}

