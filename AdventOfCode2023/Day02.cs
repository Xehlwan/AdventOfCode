using AdventOfCode.Lib;

namespace AdventOfCode2023
{
    public class Day02 : PuzzleDayBase
    {
        public Day02() : base(2)
        {
        }

        public override void SolveA()
        {
            Set bag = new Set()
            {
                Red = 12,
                Green = 13,
                Blue = 14,
            };

            int sumIds = 0;

            foreach (var game in GetGames())
            {
                if (game.IsPossible(bag))
                    sumIds += game.Id;
            }

            Presentation.ShowAnswer(sumIds);
        }

        public override void SolveB()
        {
            int sumPowers = 0;

            foreach (var game in GetGames())
            {
                Set cubes = game.GetLeastPossibleCubes();
                sumPowers += cubes.Red * cubes.Green * cubes.Blue;
            }

            Presentation.ShowAnswer(sumPowers);
        }

        private IEnumerable<Game> GetGames()
        {
            foreach (var line in Input.GetLines())
            {
                yield return ParseGame(line);
            }
        }

        private static Game ParseGame(string line)
        {
            string[] sections = line.Split(new[] { ':', ';' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            Game game = new();
            game.Id = int.Parse(sections[0].AsSpan()[5..]);

            for (int i = 1; i < sections.Length; i++)
            {
                Set set = new();
                string[] colors = sections[i].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (var color in colors)
                {
                    if (color.EndsWith("red"))
                    {
                        set.Red = int.Parse(color.AsSpan()[..^4]);
                    }
                    else if (color.EndsWith("green"))
                    {
                        set.Green = int.Parse(color.AsSpan()[..^6]);
                    }
                    else if (color.EndsWith("blue"))
                    {
                        var blue = color.AsSpan()[..^5];
                        set.Blue = int.Parse(blue);
                    }
                }
                game.Sets.Add(set);
            }

            return game;
        }

        private struct Set
        {
            public int Blue;
            public int Green;
            public int Red;
        }

        private class Game
        {
            public bool IsPossible(Set maximum)
            {
                foreach (var set in Sets)
                {
                    if (set.Red > maximum.Red || set.Green > maximum.Green || set.Blue > maximum.Blue)
                        return false;
                }
                return true;
            }

            public Set GetLeastPossibleCubes()
            {
                Set result = new();
                foreach (var set in Sets)
                {
                    if (set.Red > result.Red)
                        result.Red = set.Red;
                    if (set.Green > result.Green)
                        result.Green = set.Green;
                    if (set.Blue > result.Blue)
                        result.Blue = set.Blue;
                }
                return result;
            }

            public int Id;
            public List<Set> Sets = new();
        }
    }
}