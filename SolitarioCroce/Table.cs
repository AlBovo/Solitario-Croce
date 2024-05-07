namespace SolitarioCroce
{
    /// <summary>
    /// This class can be used to rappresent the table in the game.
    /// </summary>
    class Table
    {
        /// <summary>
        /// Array of the stacks with all the cards used in the game.
        /// Indexes from 0-4 rappresent the game stacks.
        /// Index 5 rappresents the deck's stack.
        /// </summary>
        private Stack<Card>[] stacks = new Stack<Card>[6];

        /// <summary>
        /// Array of the bases with all the last cards pushed in the game.
        /// </summary>
        private Card[] bases = new Card[4];

        public void ChangeCardStack(int stackFrom, int stackTo)
        {
            if (stackFrom < 0 || stackFrom > 4)
                throw new ArgumentException("The id of the first stack is not valid");

            if (stackTo < 0 || stackTo > 4)
                throw new ArgumentException("The id of the second stack is not valid");

            if (stackFrom == stackTo)
                return;

            if (stacks[stackFrom] == null || stacks[stackFrom].Count == 0)
                throw new ArgumentException("Cannot extract card from empty stack.");

            Card cardFrom = stacks[stackFrom].Peek();

            if (stacks[stackTo].Count == 0)
            {
                stacks[stackTo].Push(cardFrom);
                return;
            }

            Card cardTo = stacks[stackTo].Peek();

            if (cardTo.Seed != cardFrom.Seed && cardTo.Value == cardFrom.Value - 1)
                stacks[stackTo].Push(stacks[stackFrom].Pop());
        }

        public Card[] GetCardsFromStacks()
        {
            Card[] cards = new Card[6];

            for (int i = 0; i < cards.Length; i++)
            {
                if (stacks[i].Count == 0)
                    cards[i] = null;
                else
                    cards[i] = stacks[i].Peek();
            }

            return cards;
        }

        public Card[] GetCardsFromBases()
        {
            Card[] cards = new Card[4];

            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = bases[i]; // bases[i] can be null
            }

            return cards;
        }

        public void AddCardBase(Card card)
        {
            if (card.Value == 1)
            {
                bases[(int)card.Seed] = card;
                return;
            }

            if (bases[(int)card.Seed].Value == card.Value - 1)
                bases[(int)card.Seed] = card;
            else
                throw new ArgumentException("The card cannot be added in any base.");
        }

        public Table()
        {
            for (int i = 0; i < 6; i++)
                stacks[i] = new Stack<Card>();
        }
    }
}