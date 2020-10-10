using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public enum CardEnum : byte { spade, heart, club, diamond };
namespace Desktop_Minigames
{
    [Serializable]
    public class Card : IComparable
    {
        public static int CARD_LENGTH_BYTES = 8;
        private int num;
        private CardEnum shape;

        public Card(int num, CardEnum shape)
        {
            this.num = num;
            this.shape = shape;
        }
        public Card()
        {

        }
        public Card(Card other)
        {
            this.num = other.num;
            this.shape = other.shape;
        }
        public int GetNum()
        {
            return this.num;
        }

        public CardEnum GetShape()
        {
            return this.shape;
        }

        public void SetNum(int num)
        {
            this.num = num;
        }

        public void SetShape(CardEnum shape)
        {
            this.shape = shape;
        }

        public override string ToString()
        {
            return this.num + " - " + getCardName();
        }
        public string getCardName()
        {
            if (shape == CardEnum.diamond)
                return "Diamond";
            else if (shape == CardEnum.club)
                return "Club";
            else if (shape == CardEnum.spade)
                return "Spade";
            return "Heart";

        }
        public static byte[] Serialize(Card card)
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(card.num);
                    writer.Write((int)(card.shape));
                }
                return m.ToArray();
            }
        }
        public static byte[] SerializeArr(Card[] cards)
        {
            List<byte> data = new List<byte>();

            for (int i = 0; i < cards.Length; i++)
            {
                byte[] data1 = Card.Serialize(cards[i]);
                for (int j = 0; j < CARD_LENGTH_BYTES; j++)
                {
                    data.Add(data1[j]);
                }
            }
            return data.ToArray();
        }
        public static Card[] DesserializeArr(byte[] data)
        {
            /*
             Card = 8 Bytes


            [1,2,1,1,2,1,2,1,2]
            */

            int lengthOfArr = data.Length / CARD_LENGTH_BYTES;
            Card[] resultArr = new Card[lengthOfArr];
            for (int i = 0; i < lengthOfArr; i++)
            {
                resultArr[i] = Card.Desserialize(data);
                if (i != lengthOfArr - 1)
                {
                    for (int j = 0; j < data.Length - CARD_LENGTH_BYTES; j++)
                    {
                        data[j] = data[j + CARD_LENGTH_BYTES];
                    }
                }
            }
            return resultArr;
        }
        public static Card Desserialize(byte[] data)
        {
            Card result = new Card();
            using (MemoryStream m = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.num = reader.ReadInt32();
                    result.shape = (CardEnum)reader.ReadInt32();
                }
            }
            return result;
        }

        public int CompareTo(object obj)
        {
            Card other = obj as Card;
            if (other == null)
                return 1;

            if (this.GetShape() == other.GetShape())
                return this.num > other.num ? -1 : 1;

            return this.GetShape() > other.GetShape() ? 1 : -1;

        }
    }
}
