using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public interface IPlatform
    {
        bool FileExists(String filename);
        bool SaveDataToFile(byte[] data, String filename);
        byte[] LoadFromFile(String filename);
        String HashString(String subject);
    }
}
