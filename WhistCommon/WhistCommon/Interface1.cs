using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_Minigames
{
    public interface WhistCommon
    {
        public int GetId(string name);
        public List<Card> GetHand(int clientid);
    }

    [Serializable]
    
    public class Card
    {
        private int num;
        private string shape;

        public Card(int num, string shape)
        {
            this.num = num;
            this.shape = shape;
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

        public string GetShape()
        {
            return this.shape;
        }

        public void SetNum(int num)
        {
            this.num = num;
        }

        public void SetShape(string shape)
        {
            this.shape = shape;
        }

        public override string ToString()
        {
            return this.num + " - " + this.shape;
        }


    }
    public class Packet
    {
        private Card[] cards;
        public Packet()
        {
            cards = new Card[52];

            for (int i = 0; i < 13; i++)
            {
                this.cards[i] = (new Card(i, "Spades"));
                this.cards[i + 13] = (new Card(i, "Hearts"));
                this.cards[i + 26] = (new Card(i, "Diamonds"));
                this.cards[i + 39] = (new Card(i, "Clubs"));
            }


        }
        public Card[] GetCards()
        {
            return cards;
        }
        public void Shuffle()
        {
            Random random = new Random();
            for (int i = 0; i < 52; i++)
            {
                int ind = random.Next(i, 52);
                Card temp = cards[i];
                cards[i] = cards[ind];
                cards[ind] = temp;
            }
        }

        public List<Card>[] GetPcards()
        {
            List<Card>[] pcards = new List<Card>[4];
            for (int i = 0; i < 13; i++)
            {
                pcards[0].Add(cards[i]);
                pcards[1].Add(cards[i + 13]);
                pcards[2].Add(cards[i + 26]);
                pcards[3].Add(cards[i + 39]);
            }

            return pcards;
        }
    }
    public class Player
    {
        private List<Card> pcards;
        private int id;
        private string name;

        public Player(List<Card> pcards, int id, string name)
        {
            this.pcards = pcards;
            this.id = id;
            this.name = name;
        }
        
        public List<Card> Getpcards()
        {
            return pcards;
        }

        int Getid()
        {
            return id;
        }

        string Getname()
        {
            return name;
        }
    }
}

