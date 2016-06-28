using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public struct Vector2
    {
        public double X;
        public double Y;

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void ReadFromStream(BinaryReader binaryReader)
        {
            X = binaryReader.ReadDouble();
            Y = binaryReader.ReadDouble();
        }

        public void WriteToStream(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(X);
            binaryWriter.Write(Y);
        }
    }
}
