using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damka
{
    class Utilities
    {
        public static byte SetByteValue(byte b, int value, bool firstHalf)
        {
            if (firstHalf)
            {
                b = (byte)((value << 4) | (b & 0xf));
            }
            else
            {
                b = (byte)((b & 0xf0) | value);
            }
            return b;
        }

        public static int GetByteValue(byte b, bool firstHalf)
        {
            if (firstHalf)
            {
                return (b & 0xF0) >> 4;
            }
            else
            {
                return b & 0x0F;
            }
        }
        public static void Serialize(Dictionary<byte[], int[]> dictionary, Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            foreach (var obj in dictionary.ToList())
            {
                if (!(obj.Key == null || obj.Value == null))
                {
                    writer.Write(obj.Key);
                    for (int i = 0; i < 2; i++)
                    {
                        writer.Write(obj.Value[i]);
                    }
                }
            }
            writer.Flush();
        }

        public static Dictionary<byte[], int[]> Deserialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            var dictionary = new Dictionary<byte[], int[]>(new MyEqualityComparer());
            int couter = 0;
            try
            {
                while (reader.PeekChar() != -1)
                {
                    couter++;
                    byte[] key = reader.ReadBytes(17);
                    int[] value = new int[2];
                    for (int i = 0; i < 2; i++)
                    {
                        value[i] = reader.ReadInt32();
                    }
                    dictionary.Add(key, value);
                }
            }
            catch (Exception)
            {

            }
            reader.Close();
            return dictionary;
        }
    }

    public class MyEqualityComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(byte[] obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }   
}
