using AdventOfCode.Lib;

namespace AdventOfCode2023;

public class Day01 : PuzzleDayBase
{
    private Dictionary<string, int> numbers = new()
    {
        {"0", 0 },
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9},
        {"one", 1},
        {"two", 2},
        {"three", 3 },
        {"four", 4},
        {"five", 5 },
        {"six", 6 },
        {"seven", 7 },
        {"eight", 8 },
        {"nine", 9 }
    };

    public Day01() : base(1)
    {
    }

    public override void SolveA()
    {
        int sum = 0;
        foreach (string line in Input.GetLines())
        {
            string number = string.Concat(line.First(c => char.IsDigit(c)), line.Last(c => char.IsDigit(c)));
            sum += int.Parse(number);
        }

        Presentation.ShowAnswer(sum);
    }

    public override void SolveB()
    {
        int sum = 0;
        foreach (string line in Input.GetLines())
        {
            int firstIndex = int.MaxValue;
            int firstDigit = -1;
            int lastIndex = int.MinValue;
            int lastDigit = -1;
            foreach (var digit in numbers)
            {
                int first = line.IndexOf(digit.Key);
                int last = line.LastIndexOf(digit.Key);
                if (first >= 0 && first < firstIndex)
                {
                    firstIndex = first;
                    firstDigit = digit.Value;
                }
                if (last >= 0 && last > lastIndex)
                {
                    lastIndex = last;
                    lastDigit = digit.Value;
                }
            }
            //Console.WriteLine($"{line} | {firstDigit}, {lastDigit}");
            sum += int.Parse(string.Concat(firstDigit, lastDigit));
        }

        Presentation.ShowAnswer(sum);
    }
}