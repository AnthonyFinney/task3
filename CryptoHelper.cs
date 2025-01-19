using System.Security.Cryptography;
using System.Text;

namespace T3;

public static class CryptoHelper {
    public static int GenerateSecureRandomBit(int min, int max) {
        byte[] randomByte = new byte[1];
        RandomNumberGenerator.Fill(randomByte);
        int range = max - min + 1;
        return (randomByte[0] % range) + min;
    }

    public static byte[] Generate256BitKey() {
        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    public static string ComputeHMAC(int randomByte, byte[] key) {
        HMACSHA256 hmac = new HMACSHA256(key);
        byte[] bitBytes = Encoding.UTF8.GetBytes(randomByte.ToString());
        byte[] hash = hmac.ComputeHash(bitBytes);
        return Convert.ToHexString(hash);
    }
}