using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib;

public class PuzzleInput
{
    private string _raw;

    public string Raw => _raw;

    public PuzzleInput(string inputId, string inputDirectory = "Input")
    {
        string path = Path.Combine(AppContext.BaseDirectory, inputDirectory, inputId + ".txt");
        _raw = File.ReadAllText(path);
    }

    public PuzzleInput(int day, string inputDirectory = "Input") : this(day.ToString("D2"), inputDirectory)
    {
    }

    public List<int> GetInts()
    {
        var list = new List<int>();
        var sr = GetReader();
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            if (int.TryParse(line, out int val))
            {
                list.Add(val);
            }
        }
        return list;
    }

    public StringReader GetReader() => new StringReader(_raw);

    public List<string> GetLines()
    {
        var list = new List<string>();
        var sr = GetReader();
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            list.Add(line);
        }
        return list;
    }
}