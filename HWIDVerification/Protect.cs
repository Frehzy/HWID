using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HWIDVerification
{
    public class Protect
    {
        public static string Encrypt(string word)
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            byte[] secret = ProtectedData.Protect(unicodeEncoding.GetBytes(word), null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(secret);
        }

        public static string Decipher(string word)
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            byte[] backagain = Convert.FromBase64String(word);
            byte[] clearbytes = ProtectedData.Unprotect(backagain, null, DataProtectionScope.CurrentUser);
            return unicodeEncoding.GetString(clearbytes);
        }
    }
}
