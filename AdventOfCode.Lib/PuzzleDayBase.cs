using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Lib
{
    public class PuzzleDayBase
    {
        private PuzzleInput? _input;
        public int Day { get; }
        public string InputDirectory { get; }
        protected PuzzleInput Input
        {
            get
            {
                if (_input == null)
                    _input = new(Day, InputDirectory);
                return _input;
            }
        }

        public PuzzleDayBase(int day, string inputDirectory = "Input") => (Day, InputDirectory) = (day, inputDirectory);
        public void Run()
        {
            Presentation.TitleA(Day);
            SolveA();
            Presentation.TitleB(Day);
            SolveB();
        }

        public virtual void SolveA() => ShowNoSolution();
        public virtual void SolveB() => ShowNoSolution();

        private static void ShowNoSolution() => Console.WriteLine("Missing: No solution.");


    }
}
