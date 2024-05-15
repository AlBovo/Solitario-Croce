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
        /// Index 5 rappresents the picked cards' stack.
        /// </summary>
        private Stack<Card>[] stacks = new Stack<Card>[6];

        /// <summary>
        /// The deck of this instance of the table.
        /// </summary>
        public Deck deck = new Deck();

        /// <summary>
        /// Array of the bases with all the last cards pushed in the game.
        /// </summary>
        private Card[] bases = new Card[4];

        /// <summary>
        /// Change card position from a stack to another stack.
        /// </summary>
        /// <param name="stackFrom">id of the starter stack.</param>
        /// <param name="stackTo">id of the stack where is moved the card.</param>
        /// <exception cref="ArgumentException">Some of the parameters are not correct.</exception>
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

        /// <summary>
        /// A function to find the cards at the top of stacks (indexes in range [0;5])
        /// </summary>
        /// <returns>An array of cards at the top of each stack.</returns>
        public Card[] GetCardsFromStacks()
        {
            Card[] cards = new Card[5];

            for (int i = 0; i < cards.Length; i++)
            {
                if (stacks[i].Count == 0)
                    cards[i] = null;
                else
                    cards[i] = stacks[i].Peek();
            }

            return cards;
        }

        /// <summary>
        /// A function to find the cards at the top of bases (indexes in range [0;3])
        /// </summary>
        /// <returns>An array of cards at the top of each bases.</returns>
        public Card[] GetCardsFromBases()
        {
            Card[] cards = new Card[4];

            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = bases[i]; // bases[i] can be null
            }

            return cards;
        }

        /// <summary>
        /// Change card position from a stack to a new base.
        /// </summary>
        /// <param name="stackFrom">id of the starter stack.</param>
        /// <param name="stackTo">id of the base where is moved the card.</param>
        /// <exception cref="ArgumentException">Some of the parameters are not correct.</exception>
        public void ChangeCardBase(int stackFrom, int baseTo)
        {
            if (stackFrom < 0 || stackFrom > 4)
                throw new ArgumentException("The id of the stack is not valid");

            if (baseTo < 0 || baseTo > 3)
                throw new ArgumentException("The id of the base is not valid");

            Card card = stacks[stackFrom].Peek();

            if (bases[baseTo].Seed != card.Seed)
                return;

            if (bases[baseTo].Value != card.Value - 1)
                return;

            bases[baseTo] = stacks[stackFrom].Pop();
        }

        public Table()
        {
            for (int i = 0; i < 5; i++)
            {
                stacks[i] = new Stack<Card>();

                if (i <= 4)
                    stacks[i].Push(deck.GetCard());
            }
        }
    }
}