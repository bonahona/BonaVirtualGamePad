using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public class NetworkPackage
    {
        public IPEndPoint SourceEndpoint { get; set; }

        public NetworkPackageType PackageType;

        public Vector2 LeftStick;
        public Vector2 RightStick;
        public Vector2 Triggers;
        public UInt32 ButtonMask;
        public String AdditionalData;

        public NetworkPackage(NetworkPackageType packageType = NetworkPackageType.Unknown)
        {
            PackageType = packageType;

            LeftStick = new Vector2();
            RightStick = new Vector2();
            Triggers = new Vector2();

            AdditionalData = "";
        }

        // Reads a chunk of the data and return the offset of the data after this package has been read
        public int Read(byte[] data, int offset)
        {
            var memoryStream = new MemoryStream(data, offset, data.Length - offset);
            var binaryReader = new BinaryReader(memoryStream);
            PackageType = (NetworkPackageType)binaryReader.ReadInt32();
            LeftStick.ReadFromStream(binaryReader);
            RightStick.ReadFromStream(binaryReader);
            Triggers.ReadFromStream(binaryReader);
            ButtonMask = binaryReader.ReadUInt32();
            AdditionalData = binaryReader.ReadString();

            var result = (int)memoryStream.Position + offset;
            binaryReader.Close();
            memoryStream.Close();

            return result;
        }

        public byte[] ToByteArray()
        {
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.Write((Int32)PackageType);
            LeftStick.WriteToStream(binaryWriter);
            RightStick.WriteToStream(binaryWriter);
            Triggers.WriteToStream(binaryWriter);
            binaryWriter.Write(ButtonMask);
            binaryWriter.Write(AdditionalData);

            var result = new byte[memoryStream.Length];
            memoryStream.Position = 0;
            memoryStream.Read(result, 0, (int)memoryStream.Length);

            binaryWriter.Close();
            memoryStream.Close();

            return result;
        }
    }
}
