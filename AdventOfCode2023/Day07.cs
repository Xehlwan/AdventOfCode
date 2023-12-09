using AdventOfCode.Lib;

internal class Day07 : PuzzleDayBase
{
    List<Hand> _hands;
    //List<Hand> _exampleData = new()
    //{
    //    new Hand("32T3K",765),
    //    new Hand("T55J5",684),
    //    new Hand("KK677",28),
    //    new Hand("KTJJT",220),
    //    new Hand("QQQJA",483),
    //};

    //List<Hand> _moreTestData;

    public Day07() : base(7)
    {
        _hands = ParseHands(Input.GetLines());
    }

    public override void SolveA()
    {
        var hands = _hands;
        hands.Sort(new HandComparer());

        long totalWinnings = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            var hand = hands[i];
            int rank = i + 1;
            //Presentation.Log($"Rank {rank} | {hand}");
            totalWinnings += hand.Bid * rank;
        }

        Presentation.ShowAnswer(totalWinnings);
    }

    public override void SolveB() 
    {
        var hands = _hands;
        hands.Sort(new HandComparerJoker());

        long totalWinnings = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            var hand = hands[i];
            int rank = i + 1;
            Presentation.Log($"Rank {rank} | {hand}");
            totalWinnings += hand.Bid * rank;
        }

        Presentation.ShowAnswer(totalWinnings);
    }

    private static List<Hand> ParseHands(List<string> lines)
    {
        var hands = new List<Hand>();
        foreach (var line in lines)
        {
            string[] parts = line.Split(' ');
            hands.Add(new(parts[0], int.Parse(parts[1])));
        }

        return hands;
    }

    private class HandComparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            int handComparison = GetHandValue(x) - GetHandValue(y);
            if (handComparison != 0)
                return handComparison;

            return CompareCards(x, y);
        }

        private static int CompareCards(Hand x, Hand y)
        {
            CardComparer comparer = new();
            for (int i = 0; i < x.Cards.Length; i++)
            {
                int cardComparison = comparer.Compare(x.Cards[i], y.Cards[i]);
                if (cardComparison != 0)
                    return cardComparison;
            }
            return 0;
        }

        private static int GetHandValue(Hand hand) => DetermineHandType(hand.Cards) switch
        {
            HandType.FiveOfAKind => 7,
            HandType.FourOfAKind => 6,
            HandType.FullHouse => 5,
            HandType.ThreeOfAKind => 4,
            HandType.TwoPair => 3,
            HandType.OnePair => 2,
            HandType.HighCard => 1,
            _ => throw new NotImplementedException()
        };

        private static HandType DetermineHandType(char[] cards)
        {
            Dictionary<char, int> cardCount = new();
            foreach (var card in cards)
            {
                if (!cardCount.TryAdd(card, 1))
                {
                    cardCount[card] += 1;
                }
            }
            int highest = 0;
            int secondHighest = 0;
            foreach (var card in cardCount)
            {
                if (card.Value >= highest)
                {
                    secondHighest = highest;
                    highest = card.Value;
                }
                else if (card.Value > secondHighest)
                {
                    secondHighest = card.Value;
                }
            }
            HandType type = (highest, secondHighest) switch
            {
                (5, _) => HandType.FiveOfAKind,
                (4, _) => HandType.FourOfAKind,
                (3, 2) => HandType.FullHouse,
                (3, _) => HandType.ThreeOfAKind,
                (2, 2) => HandType.TwoPair,
                (2, 1) => HandType.OnePair,
                _ => HandType.HighCard
            };

            return type;
        }
    }

    private class HandComparerJoker : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            int handComparison = GetHandValue(x) - GetHandValue(y);
            if (handComparison != 0)
                return handComparison;

            return CompareCards(x, y);
        }

        private static int CompareCards(Hand x, Hand y)
        {
            CardComparerJoker comparer = new();
            for (int i = 0; i < x.Cards.Length; i++)
            {
                int cardComparison = comparer.Compare(x.Cards[i], y.Cards[i]);
                if (cardComparison != 0)
                    return cardComparison;
            }
            return 0;
        }

        private static int GetHandValue(Hand hand) => DetermineHandType(hand.Cards) switch
        {
            HandType.FiveOfAKind => 7,
            HandType.FourOfAKind => 6,
            HandType.FullHouse => 5,
            HandType.ThreeOfAKind => 4,
            HandType.TwoPair => 3,
            HandType.OnePair => 2,
            HandType.HighCard => 1,
            _ => throw new NotImplementedException()
        };

        private static HandType DetermineHandType(char[] cards)
        {
            Dictionary<char, int> cardCount = new();
            foreach (var card in cards)
            {
                if (!cardCount.TryAdd(card, 1))
                {
                    cardCount[card] += 1;
                }
            }
            int highest = 0;
            int secondHighest = 0;
            int jokers = 0;
            foreach (var card in cardCount)
            {
                if (card.Key == 'J')
                {
                    jokers = card.Value;
                }
                else if (card.Value >= highest)
                {
                    secondHighest = highest;
                    highest = card.Value;
                }
                else if (card.Value > secondHighest)
                {
                    secondHighest = card.Value;
                }
            }
            HandType type = (highest + jokers, secondHighest) switch
            {
                (5, _) => HandType.FiveOfAKind,
                (4, _) => HandType.FourOfAKind,
                (3, 2) => HandType.FullHouse,
                (3, _) => HandType.ThreeOfAKind,
                (2, 2) => HandType.TwoPair,
                (2, 1) => HandType.OnePair,
                _ => HandType.HighCard
            };
            return type;
        }
    }

    private class CardComparer : IComparer<char>
    {
        public int Compare(char x, char y)
        {
            return GetValue(x) - GetValue(y);
        }

        private static int GetValue(char c) => c switch
        {
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => 0
        };
    }

    private class CardComparerJoker : IComparer<char>
    {
        public int Compare(char x, char y)
        {
            return GetValue(x) - GetValue(y);
        }

        private static int GetValue(char c) => c switch
        {
            'J' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => 0
        };
    }

    private enum HandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }

    private class Hand
    {
        public char[] Cards { get; init; }
        public int Bid { get; init; }

        public Hand(string cards, int bid)
        {
            (Cards, Bid) = (cards.ToCharArray(), bid);
            if (Cards.Length != 5)
                throw new ArgumentException(nameof(cards), "Must be a string of exactly 5 letters.");
        }

        

        public override string ToString()
        {
            return $"Hand: {string.Join("",Cards)}, Bid: {Bid}";
        }
    }
}