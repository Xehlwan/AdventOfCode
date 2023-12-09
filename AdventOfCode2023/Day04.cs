using AdventOfCode.Lib;

internal class Day04 : PuzzleDayBase
{
    private List<Card> _cards;

    public Day04() : base(4)
    {
        _cards = ParseCards(Input);
    }

    public override void SolveA()
    {
        int sumPoints = 0;
        foreach (var card in _cards)
        {
            int matches = card.GetMatches();

            if (matches == 0)
                continue;

            sumPoints += (int)(Math.Pow(2, matches - 1));
        }

        Presentation.ShowAnswer(sumPoints);
    }

    public override void SolveB()
    {
        // Ensure cards are in correct order.
        _cards.Sort((x, y) => x.Id - y.Id);

        int cardCount = 0;

        for (int i = 0; i < _cards.Count; i++)
        {
            cardCount += _cards[i].Copies;
            for (int m = 1; m <= _cards[i].GetMatches(); m++)
            {
                _cards[i + m].Copies += _cards[i].Copies;
            }
        }

        Presentation.ShowAnswer(cardCount);
    }

    private static List<Card> ParseCards(PuzzleInput input)
    {
        using var reader = input.GetReader();

        List<Card> cards = new();
        var separators = new[] { ':', '|' };
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            Card card = new();
            string[] parts = line.Split(separators, StringSplitOptions.TrimEntries);
            card.Id = int.Parse(parts[0][5..]);
            foreach (string number in parts[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                card.WinningNumbers.Add(int.Parse(number));
            }
            foreach (string number in parts[2].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                card.MyNumbers.Add(int.Parse(number));
            }
            cards.Add(card);
        }
        return cards;
    }

    private class Card
    {
        public int Copies { get; set; } = 1;
        public int Id { get; set; }
        public List<int> MyNumbers { get; } = new();
        public List<int> WinningNumbers { get; } = new();

        public int GetMatches()
        {
            int matches = 0;
            foreach (int number in MyNumbers)
            {
                if (WinningNumbers.Contains(number))
                    matches++;
            }
            return matches;
        }
    }
}