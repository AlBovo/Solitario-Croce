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
        public bool ChangeCardStack(int stackFrom, int stackTo)
        {
            if (stackFrom < 0 || stackFrom > 5)
                throw new ArgumentException("The id of the first stack is not valid");

            if (stackTo < 0 || stackTo > 5)
                throw new ArgumentException("The id of the second stack is not valid");

            if (stackFrom == stackTo)
                return false;

            if (stacks[stackFrom] == null || stacks[stackFrom].Count == 0)
                return false;
                //throw new ArgumentException("Cannot extract card from empty stack.");

            Card cardFrom = stacks[stackFrom].Peek();

            if (stacks[stackTo].Count == 0)
            {
                stacks[stackTo].Push(cardFrom);
                stacks[stackFrom].Pop();
                return true;
            }

            Card cardTo = stacks[stackTo].Peek();

            if (cardTo.Seed != cardFrom.Seed && cardTo.Value == cardFrom.Value + 1)
            {
                stacks[stackTo].Push(stacks[stackFrom].Pop());
                return true;
            }
            return false;
        }

        /// <summary>
        /// A function to find the cards at the top of stacks (indexes in range [0;5])
        /// </summary>
        /// <returns>An array of cards at the top of each stack.</returns>
        public Card GetCardsFromStacks(int index)
        {
            if (stacks[index].Count == 0)
                return null;
            else
                return stacks[index].Peek();
        }

        /// <summary>
        /// Add a card to the picked cards stack.
        /// </summary>
        /// <param name="card">The card to add in the stack.</param>
        public void AddCardToPicked(Card card)
        {
            stacks[5].Push(card);
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
        public bool ChangeCardBase(int stackFrom, int baseTo)
        {
            if (stackFrom < 0 || stackFrom > 5)
                throw new ArgumentException("The id of the stack is not valid");

            if (baseTo < 0 || baseTo > 3)
                throw new ArgumentException("The id of the base is not valid");
            Card card;
            try
            {
               card = stacks[stackFrom].Peek();
            }
            catch (Exception e)
            {
                return false;
            }

            if (bases[baseTo].Seed != card.Seed)
                return false;

            if (bases[baseTo].Value != card.Value - 1)
                return false;

            bases[baseTo] = stacks[stackFrom].Pop();
            return true;
        }

        /// <summary>
        /// This functions returns the status of the current game.
        /// </summary>
        /// <returns>1 if lost, 0 if not finished, 2 if won.</returns>
        public byte GetStatusGame()
        {
            bool isWon = true, isLost = true;
            for (int i = 0; i < 6; i++)
            {
                if (stacks[i].Count > 0)
                {
                    isWon = false;
                    break;
                }
            }
            if (isWon && deck.IsEmpty())
                return 2;

            for (int i = 0; i < 5; i++)
            {
                Card card, secondCard;
                if (stacks[i].TryPeek(out card))
                {
                    // try the deck
                    if (!deck.IsEmpty())
                    {
                        isLost = false;
                        break;
                    }

                    // try all the bases card -> base
                    for (int j = 0; j < 4; j++)
                    {
                        if (bases[j].Seed == card.Seed && bases[j].Value == card.Value - 1)
                        {
                            isLost = false; 
                            break;
                        }
                    }

                    // try all the stacks card -> stack
                    for (int j = 0; j < 5; j++)
                    {
                        if(i == j) continue;

                        if (stacks[j].Count == 0)
                        {
                            isLost = false;
                            break;
                        }
                        else if (stacks[j].TryPeek(out secondCard))
                        {
                            if(secondCard.Value - 1 == card.Value &&  secondCard.Seed != card.Seed)
                            {
                                isLost = false;
                                break;
                            }
                        }
                    }

                    // try stack of the deck stack -> card
                    if (stacks[5].TryPeek(out secondCard))
                    {
                        if (secondCard.Value == card.Value - 1 && secondCard.Seed != card.Seed)
                        {
                            isLost = false;
                            break;
                        }
                    }
                }
            }

            if (isLost)
                return 1;

            return 0;
        }

        public Table()
        {
            for (int i = 0; i < 6; i++)
            {
                stacks[i] = new Stack<Card>();

                if (i <= 4)
                    stacks[i].Push(deck.GetCard());

                if (i <= 3)
                    bases[i] = new Card((Card.Seeds)i, 0);
            }
        }
    }
}