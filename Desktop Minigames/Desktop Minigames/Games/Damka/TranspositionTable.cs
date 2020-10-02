using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Damka
{
    class TranspositionTable
    {
        Dictionary<byte[], int[]> dictionary;
        public TranspositionTable(string file)
        {
            this.dictionary = Utilities.Deserialize(File.OpenRead(file));
        }
        public void Add(DamkaBoard board, bool turn, int depth, int score)
        {
            byte[] key = board.GetByteArray(turn);
            if (!this.dictionary.ContainsKey(key))
            {
                try
                {
                    this.dictionary.Add(key, new int[2] { depth, score });
                }
                catch
                {
                    return;
                }
            }
        }

        public void Filter(int minimumDepth)
        {
            foreach (var key in this.dictionary.Keys.ToList())
            {
                int[] value = this.dictionary[key];
                if (value[0] < minimumDepth)
                {
                    this.dictionary.Remove(key);
                }
            }
        }

        public int[] Check(DamkaBoard board, bool turn, int depth)
        {
            byte[] key = board.GetByteArray(turn);
            if(this.dictionary.ContainsKey(key))
            {
                int[] results = (int[])(this.dictionary[key]);
                if (depth > results[0])
                {
                    return new int[2] { -20, 0 };
                }
                return results;
            }
            return new int[2] { -15, 0 };
        }
        public void Update(DamkaBoard board, bool turn, int depth, int score)
        {
            this.dictionary[board.GetByteArray(turn)] = new int[2] { depth, score };
        }

        public void Save(string file)
        {
            Utilities.Serialize(this.dictionary, File.Create(file));
        }
    }
}
