using System.Security.Cryptography;
using System.Text;

namespace ARM.Core.Helpers;

public static class Encryptor
{
    public static string EncryptString(string content)
    {
        var provider = SHA1.Create();
        var encoding = new UnicodeEncoding();
        return Encoding.Unicode.GetString(provider.ComputeHash(encoding.GetBytes(content)));
    }
}