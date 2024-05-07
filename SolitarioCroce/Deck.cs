using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitarioCroce
{
    class Card
    {
        /// <summary>
        /// class Card, represents a placeable card on the table.
        /// Seed guide:
        /// - D = Denari
        /// - C = Coppe
        /// - S = Spade
        /// - B = Bastoni
        /// </summary>
        public char seed { get; }
        public int value { get; }

        public Card(char seed, int value)
        {
            this.seed = seed;
            this.value = value;
        }
    }
    class Deck
    {
        private Queue<Card> deck = new Queue<Card>();
        
        public Deck()
        {
            for(int i = 0; i < 40; i++)
            {
                
            }
        }
    }
}
