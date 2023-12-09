using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib;

public static class Presentation
{
    public static void Divider()
    {
        Console.Write("\n--------------------\n");
    }

    public static void TitleA(int day)
    {
        Console.Clear();
        ShowTitle(day, 'A');
    }

    public static void TitleB(int day)
    {
        Divider();
        ShowTitle(day, 'B');
    }

    public static void ShowAnswer(int answer) => ShowAnswer(answer.ToString());
    public static void ShowAnswer(long answer) => ShowAnswer(answer.ToString());

    public static void ShowAnswer(string answer) => Console.WriteLine($"[ANSWER: {answer}]");

    private static void ShowTitle(int day, char part)
    {
        Console.Write($"DAY {day:D2} [{part}]\n\nResult:\n");
    }

    public static void ShowDaySelection(int year, Dictionary<int, Lazy<PuzzleDayBase>> puzzles)
    {
        bool validInput = false;
        do
        {
            Console.WriteLine($"Advent of Code {year} \n");

            Console.Write("Day: ");
            validInput = TryGetDay(Console.ReadLine(), puzzles);

            if (!validInput)
                Console.WriteLine("Invalid selection, try again.\n");

        } while (!validInput);
    }

    private static bool TryGetDay(string? input, Dictionary<int, Lazy<PuzzleDayBase>> puzzles)
    {
        bool valid = int.TryParse(input, out int day);
        if (valid && puzzles.TryGetValue(day, out var puzzle))
        {
            puzzle.Value.Run();
            return true;
        }

        return false;
    }
}