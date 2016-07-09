using BonaVirtualGamePad.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BonaVirtualGamePad.SharedWindows
{
    public class WindowsPlatform : IPlatform
    {
        protected SHA1CryptoServiceProvider Sha1Generator;

        public WindowsPlatform()
        {
            Sha1Generator = new SHA1CryptoServiceProvider();
        }

        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public byte[] LoadFromFile(string filename)
        {
            var fileContent = File.ReadAllText(filename);
            return Encoding.UTF8.GetBytes(fileContent);
        }

        public bool SaveDataToFile(byte[] data, string filename)
        {
            File.WriteAllBytes(filename, data);

            return true;
        }

        public string HashString(String subject)
        {
            var buffer = Sha1Generator.ComputeHash(Encoding.UTF8.GetBytes(subject));

            var result = BitConverter.ToString(buffer);
            result.Replace("-", "");
            return result;
        }
    }
}
