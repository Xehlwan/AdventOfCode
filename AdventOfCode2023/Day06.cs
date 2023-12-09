using AdventOfCode.Lib;

internal class Day06 : PuzzleDayBase
{
    private List<Race> _races;

    public Day06() : base(6)
    {
        _races = ParseRaces(Input);
    }

    public override void SolveA()
    {
        int multipliedWaysToWin = 1;
        foreach (var race in _races)
        {
            int time = (int)race.Time;
            int median = time / 2;
            int waysToWin = 0;

            for (int i = median; i > 0; i--)
            {
                int distance = CalculateDistance(i, time);
                if (distance < race.Distance)
                    break;
                waysToWin++;
            }
            for (int i = median + 1; i < race.Time; i++)
            {
                int distance = CalculateDistance(i, time);
                if (distance < race.Distance)
                    break;
                waysToWin++;
            }
            multipliedWaysToWin *= waysToWin;
        }

        Presentation.ShowAnswer(multipliedWaysToWin);
    }

    public override void SolveB()
    {
        Race race = ParseRace(Input);
        long median = race.Time / 2;
        long waysToWin = 0;

        for (long i = median; i > 0; i--)
        {
            long distance = CalculateDistance(i, race.Time);
            if (distance <= race.Distance)
                break;
            waysToWin++;
        }
        for (long i = median + 1; i < race.Time; i++)
        {
            long distance = CalculateDistance(i, race.Time);
            if (distance <= race.Distance)
                break;
            waysToWin++;
        }

        Presentation.ShowAnswer(waysToWin);
    }

    private Race ParseRace(PuzzleInput input)
    {
        string time = Input.GetLines()[0][5..].Replace(" ", "");
        string distance = Input.GetLines()[1][9..].Replace(" ", "");
        return new Race()
        {
            Time = long.Parse(time),
            Distance = long.Parse(distance)
        };
    }

    public static long CalculateDistance(long buttonHeld, long timeLimit) => (timeLimit - buttonHeld) * buttonHeld;

    public static int CalculateDistance(int buttonHeld, int timeLimit) => (timeLimit - buttonHeld) * buttonHeld;

    private List<Race> ParseRaces(PuzzleInput input)
    {
        var races = new List<Race>();
        List<string> lines = Input.GetLines();
        string[] times = lines[0].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        string[] distances = lines[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < times.Length; i++)
        {
            Race race = new()
            {
                Time = int.Parse(times[i]),
                Distance = int.Parse(distances[i])
            };
            races.Add(race);
        }
        return races;
    }

    private readonly struct Race
    {
        public long Distance { get; init; }
        public long Time { get; init; }
    }
}