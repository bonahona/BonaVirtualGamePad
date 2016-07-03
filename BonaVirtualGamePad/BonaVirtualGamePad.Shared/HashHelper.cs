using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BonaVirtualGamePad.Shared
{
    public static class HashHelper
    {
        private static SHA1CryptoServiceProvider Sha1Generator;

        static HashHelper()
        {
            Sha1Generator = new SHA1CryptoServiceProvider();
        }

        public static string HashString(String subject)
        {
            var buffer = Sha1Generator.ComputeHash(Encoding.UTF8.GetBytes(subject));

            var result = BitConverter.ToString(buffer);
            result.Replace("-", "");
            return result;
        }
    }
}
