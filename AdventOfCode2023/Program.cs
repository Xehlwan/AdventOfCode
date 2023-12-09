using AdventOfCode.Lib;
using AdventOfCode2023;

Dictionary<int, Lazy<PuzzleDayBase>> puzzles = new()
{
    {1, new(() => new Day01()) },
    {2, new(() => new Day02()) },
    {3, new(() => new Day03()) },
    {4, new(() => new Day04()) },
    {5, new(() => new Day05()) },
    {6, new(() => new Day06()) },
};

Presentation.ShowDaySelection(2023, puzzles);