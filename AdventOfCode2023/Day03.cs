using AdventOfCode.Lib;

namespace AdventOfCode2023
{
    public class Day03 : PuzzleDayBase
    {
        private List<string> _lines;

        public Day03() : base(3)
        {
            _lines = Input.GetLines();
        }

        public override void SolveA()
        {
            List<PartNumber> parts = GetParts();
            int sumPartnumbers = 0;
            foreach (PartNumber part in parts)
            {
                if (part.HasAdjacentSymbol(_lines))
                    sumPartnumbers += part.Number;
            }

            Presentation.ShowAnswer(sumPartnumbers);
        }

        public override void SolveB()
        {
            List<PartNumber> parts = GetParts();
            Dictionary<(int x, int y), (int adjacent, int ratio)> potentialGears = new();
            foreach (PartNumber part in parts)
            {
                foreach (var potentialGear in part.GetPotentialGears(_lines))
                {
                    if (potentialGears.TryGetValue(potentialGear, out var value))
                    {
                        value.adjacent++;
                        value.ratio *= part.Number;
                        potentialGears[potentialGear] = value;
                    }
                    else
                    {
                        potentialGears[potentialGear] = (1, part.Number);
                    }
                }
            }

            int sumRatios = 0;
            foreach (var value in potentialGears.Values)
            {
                if (value.adjacent == 2)
                    sumRatios += value.ratio;
            }

            Presentation.ShowAnswer(sumRatios);
        }

        private List<PartNumber> GetParts()
        {
            List<PartNumber> parts = new();
            for (int y = 0; y < _lines.Count; y++)
            {
                var line = _lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (char.IsDigit(line[x]))
                    {
                        int start = x;
                        int end = x;
                        x++;
                        for (; x <= line.Length; x++)
                        {
                            if (x < line.Length && char.IsDigit(line[x]))
                            {
                                end++;
                            }
                            else
                            {
                                PartNumber part = new()
                                {
                                    Line = y,
                                    StartPosition = start,
                                    EndPosition = end,
                                    Number = int.Parse(line.AsSpan()[start..(end + 1)])
                                };
                                parts.Add(part);
                                break;
                            }
                        }
                    }
                }
            }

            return parts;
        }

        private struct PartNumber
        {
            public int EndPosition;

            public int Line;

            public int Number;

            public int StartPosition;

            public bool HasAdjacentSymbol(List<string> lines)
            {
                int left = Math.Max(0, StartPosition - 1);
                int right = Math.Min(lines[0].Length - 1, EndPosition + 1);
                if (Line > 0)
                {
                    for (int x = left; x <= right; x++)
                    {
                        if (IsSymbol(lines[Line - 1][x]))
                            return true;
                    }
                }
                if (Line + 1 < lines.Count)
                {
                    for (int x = left; x <= right; x++)
                    {
                        if (IsSymbol(lines[Line + 1][x]))
                            return true;
                    }
                }
                return IsSymbol(lines[Line][left]) || IsSymbol(lines[Line][right]);
            }

            public List<(int x, int y)> GetPotentialGears(List<string> lines)
            {
                int left = Math.Max(0, StartPosition - 1);
                int right = Math.Min(lines[0].Length - 1, EndPosition + 1);
                List<(int x, int y)> potentialGears = new();


                if (Line > 0)
                {
                    for(int x = left; x<= right; x++)
                    {
                        if (lines[Line - 1][x] == '*')
                            potentialGears.Add((x: x, y: Line - 1));
                    }
                }
                if (Line + 1 < lines.Count)
                {
                    for (int x = left; x <= right; x++)
                    {
                        if (lines[Line + 1][x] == '*')
                            potentialGears.Add((x: x, y: Line + 1));
                    }
                }
                if (left < StartPosition)
                {
                    if (lines[Line][left] == '*')
                        potentialGears.Add((x: left, y: Line));
                }
                if (right > EndPosition) {
                    if (lines[Line][right] == '*')
                        potentialGears.Add((x: right, y: Line));
                }

                return potentialGears;
            }

            private static bool IsSymbol(char c) => !(c == '.' || char.IsDigit(c));
        }
    }
}