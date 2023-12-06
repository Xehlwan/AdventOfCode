using AdventOfCode.Lib;
using AdventOfCode2023;

Dictionary<int, PuzzleDayBase> puzzles = new()
{
    {1, new Day01() },
    {2, new Day02() },
    {3, new Day03() },
};

Presentation.ShowDaySelection(2023, puzzles);