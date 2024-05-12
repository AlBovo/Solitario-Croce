using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitarioCroce
{
    
    /// <summary>
    /// class Card, represents a placeable card on the table.
    /// </summary>
    class Card
    {
        /// <summary>
        /// enum representing the possible seeds of the cards
        /// </summary>
        public enum Seeds
        {
            Denari,
            Coppe,
            Spade,
            Bastoni
        }

        /// <summary>
        /// Seed guide:
        /// - A = Denari
        /// - B = Coppe
        /// - C = Spade
        /// - D = Bastoni
        /// </summary>
        public Seeds Seed { get; }

        /// <summary>
        /// 1 <= value <= 10
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// the global path to represent the image in the game
        /// </summary>
        /// <returns> the global path of the card image </returns>
        public string Path()
        {
            return "pack://application:,,,/images/" + Value.ToString() + (char)(Seed + 'A') + ".jpg";   // add path to image
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="seed"> enum Seeds </param>
        /// <param name="value"> 1 <= value <= 10 </param>
        public Card(Seeds seed, int value)
        {
            Seed = seed;
            Value = value;
        }
    }

    /// <summary>
    /// represents the deck of card in the game
    /// </summary>
    class Deck
    {
        private static Random rnd = new Random();
        private Queue<Card> deck = new Queue<Card>();
        
        private static Card[] Shuffle(Card[] cards)
        {
            for(int i = 0; i < cards.Length; i++)
            {
                int first_pos = rnd.Next(0, cards.Length);
                int second_pos;

                while (true)
                {
                    second_pos = rnd.Next(0, cards.Length);
                    if (first_pos != second_pos) break;
                }

                Card temp = cards[first_pos];
                cards[first_pos] = cards[second_pos];
                cards[second_pos] = temp;             
            }
            return cards;
        }

        /// <summary>
        /// gets the first card of the deck
        /// </summary>
        /// <returns>the card at the top of the deck</returns>
        /// <exception cref="Exception">thrown if the deck is Empty</exception>
        public Card GetCard()
        {
            if (deck.Count == 0)
                throw new Exception("deck is Empty.");

            return deck.Dequeue();
        }

        /// <summary>
        /// see if the deck is empty
        /// </summary>
        /// <returns>True if empty, false otherwise</returns>
        public bool IsEmpty()
        {
            return deck.Count == 0;
        }

        /// <summary>
        /// constructor of the class, automatically shuffles the deck
        /// </summary>
        public Deck()
        {
            Card[] possible_cards = new Card[40];

            for (int i = 0; i < 40; i++)
                possible_cards[i] = new Card(value: i % 10 + 1, seed: (Card.Seeds)(i / 10));
            
            foreach(Card card in Shuffle(possible_cards))
                deck.Enqueue(card);
        }
    }
}
