using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Cube002
{
    class Program
    {
        
        private static List<string> interestingScrambles = new List<string>()
        {
            // Produces spurious solve: Middle layer two pieces wrong
            "U B' F' R F D B' R D F' R' B' U' R' U' U2 F' B U' U' F' B R' B U2",

            // Produces spurious solve: Middle layer two pieces wrong
            "B R2 F B2 U U D' B F D2 R F2 U2 B' R' U2 R2 R' D2 U' U2 R' B2 F' F",

            // Produces spurious solve: Middle layer two pieces wrong
            "U R2 F D D D2 D2 U2 D' F' B D' D D' B F2 B D U R B' F2 R B2 B2",

            // Produces spurious solve: Middle layer two pieces wrong
            "B2 R' F' U F2 D' B F R' U2 D' F' B' B U2 R' B F' D2 F' F2 F' U U F'",

            // Spurious solve. Many errors in white
            "F F' F2 B D' F B2 F' R D2 D2 B D' D2 R U2 B2 U' B U D2 B U2 D' F'",
        };

        private static List<string> scrambles = new List<string>()
        {
            "D2 R' D' D' B2 U' U2 R U F F2 U2 F' U' R D' F' U F U F R2 U' F U'",
            "R' U' B2 D R' F2 F U' R R D2 F2 F2 B2 F' R F B2 R B U' D' D D F'",
            "F2 U F' B R2 F2 R2 F' R F2 B R2 F2 D F2 D F R R' B2 B F' R F R",
            "U B' F' R F D B' R D F' R' B' U' R' U' U2 F' B U' U' F' B R' B U2",
            "D' U' D U2 F' F F2 B' R U R' D2 D D' F R' U B' B' U2 F2 U' B F2 F2",
            "U R R D2 F2 F R2 F F U R2 D D B' R' B B2 U B' F U R' B2 B2 U2",
            "D' R2 R2 F2 B2 U' B2 F2 U2 F R' D' R2 B' R D2 F B2 B2 U B' U' D B2 D'",
            "B R2 F B2 U U D' B F D2 R F2 U2 B' R' U2 R2 R' D2 U' U2 R' B2 F' F",
            "R' B2 D' U2 B' F2 U' U' U D2 R D2 R' F' F2 D2 D R' F F2 U2 D F' R2 B2",
            "F B F' D U2 D F' B F2 D2 R2 D' F2 B2 F2 B D R B2 D U D D B' F",
            "D' F2 B2 F' R D' F' D D B2 F' B2 R F2 D2 F' D2 R' D U R' F2 U' U2 R'",
            "B B' F' B U' U R2 F R' D D D' F2 B' U U2 D D U D D' U2 U2 R R2",
            "D2 U2 U' D' B' B' R2 B2 F2 U F' B B2 F' U' D' F' F2 D' F2 U2 U D2 F' U'",
            "U2 U U2 U F' R' F2 B' U2 R' U2 D U F R2 R' U' B D2 R F2 D' D D' U2",
            "F2 D U2 F D U F' U2 U' D' B2 R B B2 D R R2 U2 D B F' U B2 D2 R",
            "D2 R' D2 D B2 B B' U2 B' B2 U U' F2 F' F' F B B2 R' F U' D' R2 F2 F'",
            "R' F' B' D2 B2 F2 B' B' R2 B' B' D' B F' U2 R' U' U F2 D2 R' R2 D2 R' R2",
            "R' U2 B2 B D F2 R2 D2 F R2 R2 B2 D B2 B' R F R2 F' U' R2 R' F2 R D'",
            "F2 U' D F' B R2 U R F B' R2 R2 D' F2 U2 R D D2 U' U F' B R2 R2 F",
            "R R2 R' R B2 F D' U' U2 D2 U2 D2 B2 R2 B B' U B' D' U' U D' U F2 D",
            "B2 D' U F' B' F D2 B' R' B U F U' D R B B2 F2 R F' B D' D' B2 R'",
            "F F U2 D' B' R U R2 B2 B F D2 U2 D' R' R2 R2 B D R R B2 R2 F2 F2",
            "F F' F2 B D' F B2 F' R D2 D2 B D' D2 R U2 B2 U' B U D2 B U2 D' F'",
            "U2 R' D' R' B' D' D2 U B2 F B D' R2 U F' U2 B D2 B F B F' D2 D' U",
            "F2 F B' F R2 B2 R F' B F D2 F' B' D2 B' U U2 F2 F' U B2 R' R' D R",
            "F2 B2 R' B2 B B R B' F B2 D2 F U U2 D2 F' R' D' R U2 D U' D F' D2",
            "R F U' B R2 R' U' F2 D2 F2 R U' D2 U2 D2 U U R2 D2 U' U' R U D2 U",
            "B2 B F2 D2 R R D2 D' U B U' U' D2 B' B' F' R B F' R2 R U' F2 R' F",
            "F' D' D' B2 F2 R F' F2 F2 F2 F2 R2 F B' R2 F' B U F2 R' B U2 D' U' B'",
            "D' D2 D' F' D2 B U2 R' B' B F2 F D' B U B' U F U' U U' F2 D2 U' D",
            "F' U' U2 R2 D' U' D D' U2 R' R2 D2 F2 U2 R U F' R' R' R' U' D2 B2 B U",
            "F' R' R' R' D R2 F F' U' B2 D2 F2 U' U2 R2 B U U2 R R' F B B F F",
            "D' F2 F' D D2 F' U' U2 R2 F D2 U2 D2 U B' D2 F2 D' F2 B2 R2 R2 B2 B' D2",
            "U B R2 B' F2 R2 U U B D R' R2 B F' D2 D2 F' B' U2 B2 R2 U F2 B' F'",
            "F' U2 D2 U U2 F' B U U2 F' F' D R B' B2 F' B R' F2 B U' D' F' F' D2",
            "D2 D U R B2 R' B2 U B' U' D B R2 D2 D' R2 F2 D' D2 B B2 B2 U2 U' U",
            "D2 R D U B D' R B2 F' B2 B2 B' D' U F B2 U' B2 D' F2 D' D B2 D U2",
            "B' R D' R2 D U2 U2 F' F' R' B F' R' F' U D2 B B' D2 D B2 D' U2 B R'",
            "R R2 D2 R2 R2 U R F' F B2 U2 U2 D B' D R' U2 B2 F U' D U R2 U D",
            "B B2 F2 R' B' B2 R' D B' F2 B' R2 U' D R2 R2 D' R B2 U' F' B U2 R D'",
            "B2 B' F R F U U2 D D R2 F U2 R' B2 B' D2 B' U2 R2 F R' U B' R2 D",
            "U' F U' R' R' U2 B' F U2 R2 F' D2 U2 D' R' R' R D2 R D2 D U D2 B2 B",
            "D' B' D F2 U' F2 F2 R D F2 U' R2 U U2 B D2 D2 B' B' D2 B2 D2 D' B' U'",
            "F2 B F' R R2 U' U B2 R' F' D' R2 R2 R D D2 D2 R' B' F2 D2 F' F' D D'",
            "U' U' D' D' R' B' U F' F' R' B R' D D' R R B B2 U F R U U' F2 F2",
            "F' R' U' F R2 B' U B' B B U R' B2 U' D' B D2 F' R2 D' U2 R' U R' B'",
            "R' D' F2 U B' R' R R2 F2 F2 F2 F2 U2 F' F' R U F' U2 B U' U D B2 D2",
            "F' U2 D B' U' F2 U' U D B D U2 F U' D2 F' F B2 U2 F' B2 F B' B' R2",
            "U' F' F' U2 D2 U2 R' B B' B B2 F' F2 F2 B D2 R2 F F' R2 F F B' F2 D'",
            "U R' D2 U' D' B U2 R B2 F' R' D R B2 B' F' B F2 D' R F R B' F U",
            "F2 D U R R2 F' D2 R R D R2 F2 B D2 F2 D' D R F F' R2 B' U' B2 U",
            "R R2 B2 R U' B' R' U2 R2 B' B2 D B2 B B U2 F2 D2 B D2 U' F B B2 D2",
            "D' B2 F2 D2 U B R2 F' R' F2 B2 B' U R2 R2 F2 D' B U2 F2 R D' R R' D'",
            "B' F' U2 B2 D' U' R2 B' R' U' U2 D' B2 D' U' U U2 D' R' B B' F2 D' R' R",
            "R' F2 U' R' F2 F' B2 U' F' F2 F F2 B F' D' R U2 B' F U' B2 R2 R' F2 R2",
            "R' B' R2 U2 D D' U' U' D' R R' F' F2 R2 B' B' B2 R R2 R2 D' R R U2 R'",
            "R2 U2 B2 B2 U' B' R2 R F2 F R D2 F' U' B2 B F2 B' R U' D' U2 B2 U2 D'",
            "U2 F2 D B2 R F R2 U' U B2 U' R' F' U D F' B2 U F' B2 U2 D2 F2 B2 B'",
            "B U U2 U F' F' U R2 U' B' U' F B' R2 U D2 R2 B' B' D' U R B' U F",
            "D2 F D' F2 U2 B' U B B' U' B2 D' U2 U F' R D U2 D' R U2 F D' F U",
            "F U2 D' B' R' D2 B2 F' D U B' B B F D2 R' R2 B2 F' R B2 R2 R' B' R2",
            "B2 D' F2 F B B2 D' U2 D2 R' R2 R B D' B' U2 F R R2 B D2 U2 D U D'",
            "R' D2 U' B' B' D' U' U U' F2 U' U B D B D2 B2 B R' B D B2 U R D'",
            "U2 R D F' R' U2 R2 F2 U' D' D2 B2 R F2 R2 F2 R' B' D R' B' B' D' D' D2",
            "U' B' F2 B D2 F2 B U2 R' B' D2 R' F' R D' U B' R' R2 F2 B' D U2 D2 D",
            "R B F' R D' D U' F2 B2 R R' D' F' D2 D2 F2 F2 R R2 D B2 U F' D R2",
            "B2 B D D D2 R2 F2 R R2 F' U' B' D U' U2 U2 U R2 R2 D U2 R2 D2 D2 F2",
            "B B2 U2 R B' U2 R U2 F2 R D R2 D F D2 B' F' D2 B' U2 F2 F D' D' B'",
            "D D2 F2 U2 D' D2 R' B' U' B R B U' F R U D' D' F2 D' U F' F B D",
            "U R2 F D D D2 D2 U2 D' F' B D' D D' B F2 B D U R B' F2 R B2 B2",
            "F D2 R R2 B2 U R U' B U' R F U D F2 F2 U' D B U' U D2 B B D",
            "R D2 D2 R F2 R2 R' F' F' F2 U2 D2 U B2 U' B' U' B D2 U' F2 F2 R' F R2",
            "D' U' U D2 F2 F2 D U2 R2 D2 U D U B F U' R F F' B2 R F' U' B D",
            "U' D2 R2 R' F' D' U' F' R2 R B' D U2 R B2 R B2 F2 U F' D' D R U' D",
            "U U U2 D' F R2 B2 D B' R2 U R U' F B F2 U' U B U B' U' D' D2 F'",
            "D2 B2 B' D2 R' U' D R2 B D' B B2 D2 D' D2 R2 F' D' F2 F2 R U D' B' D2",
            "R D D2 B2 F' R2 F' R' R' F' U' R2 F2 D' F' B' U2 F U' B' U F B2 D' R2",
            "F D' B R2 F2 U' U2 R2 R2 R F' R' R' F' F B' F B' B' R' F' U' B D2 B2",
            "D' B B F F2 U R' B' B' R' D D2 D2 R F' U' B B' D D D F2 D' D2 D2",
            "B2 B R2 R R' F2 D2 U U2 F F' F U R' U2 B F2 U2 B U B R B2 F2 F2",
            "D' U D2 D2 B B' F R' B2 U' U' F2 U R F2 U D R B2 B' D U U' R2 F'",
            "D U U2 D' R' R2 R' F' U' D F2 R F2 R B2 R D U2 D2 D' D2 R' F2 D2 F'",
            "F2 R2 F2 D B2 U U' D U U' D F' F2 U R D2 D2 U D' D' R2 R B' B' R'",
            "U' R2 B R' D2 U2 F2 D' R U U' R2 D' D B U2 B B' B R2 F' R' B' F2 F2",
            "R2 U' B2 F D D2 B2 U' B' D2 D2 R2 F2 D R2 B B U2 R B' F2 B' R U' F",
            "F' U' R2 B' U2 D2 D2 B D F D B' R2 R2 U' B2 F' D R' U' B2 U D' R2 D2",
            "D2 R U2 B' D' U' R2 F2 B' B B' U2 B B F' B2 R2 R2 F' B F2 U B R' D'",
            "F' F' D' D F2 F2 F D2 R' D' D' F2 R2 B2 D' R B2 R R' U R D2 B' U2 D",
            "B2 R' F' U F2 D' B F R' U2 D' F' B' B U2 R' B F' D2 F' F2 F' U U F'",
            "D' U2 D2 B2 F R D2 R2 U2 U F' R' F' F' R D U' R2 D' D2 D' F U' B' D",
            "R U2 D B2 U' U2 R D2 U2 B' D2 R D U' F' B D' F R B U2 D2 R D2 U2",
            "R D2 B D2 F2 D2 D F' D2 R2 F2 B F U' D D' R' F B R2 D2 B2 B R R2",
            "F' F' F2 D R U B2 B' D D2 U' F2 D2 U2 U2 B B D2 R D' R2 F2 D' D2 U'",
            "B' U' U' R U U U2 R' D B F2 D2 R F2 R2 R' B2 U2 D' F' B' F2 R' R' D'",
            "F2 U' U2 D' F2 D U' R2 B' D U2 R' R2 F' F F' B B2 B2 D' R B2 U' R U2",
            "F R' U D' F2 D' R' U2 D D U U2 F2 F2 R2 B2 F D R' D2 U F F2 U' R'",
            "B2 U2 F U U' U' B F F B2 F2 U F' B' U D2 U' R D2 U' R D2 R2 R' B'",
            "B' D B2 B' D' F U D D B' D R' R F' F' F D U F R U2 U' F' D R'",
            "B B' F' D F F2 F R2 B' F B R2 D' D' F B B F U' D D F D D R2",
            "F R R2 U2 B F R D U2 R U2 U2 U' R2 F B2 F' D2 B2 D2 R' F' D F R",
        };


        // All this stuff is needed for ANSI color in conhost
        // https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e
        // https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/
        private const int STD_INPUT_HANDLE = -10;

        private const int STD_OUTPUT_HANDLE = -11;

        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        private const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        // ReSharper restore InconsistentNaming

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
        // End ANSI color stuff

        static void Main()
        {
            // Hacks to get ANSI color in conhost
            // https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e
            // https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/
            var iStdIn = GetStdHandle(STD_INPUT_HANDLE);
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

            if (!GetConsoleMode(iStdIn, out uint inConsoleMode))
            {
                Console.WriteLine("failed to get input console mode");
                //Console.ReadKey();
                return;
            }
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                //Console.ReadKey();
                return;
            }

            inConsoleMode |= ENABLE_VIRTUAL_TERMINAL_INPUT;
            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING; //| DISABLE_NEWLINE_AUTO_RETURN;

            if (!SetConsoleMode(iStdIn, inConsoleMode))
            {
                Console.WriteLine($"failed to set input console mode, error code: {GetLastError()}");
                //Console.ReadKey();
                return;
            }
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                //Console.ReadKey();
                return;
            }
            // End ANSI color hacks.



            //foreach( var scram in SolutionTest.GenerateScrambles(1000))
            //{
            //    Console.WriteLine(scram);
            //}

            Cube cube;

            Solution solution;

            int scramblesToGenerate = 1000;
            bool showCubes = false;
            int count = 0;
            int successfulSolveCount = 0;
            int spuriousCount = 0;
            int failureCount = 0;
            string result;
            Stopwatch watch = Stopwatch.StartNew();
            foreach( var scramble in SolutionTest.GenerateScramblesEnumerable(scramblesToGenerate))
            {
                count++;
                //Console.WriteLine($"\t#{count}\tSCRAMBLE: {scramble}");

                cube = new Cube();
                cube.MakeMove(scramble);
                try
                {
                    solution = new Solution(cube);
                    cube.MakeMove(solution.moveList);

                    if (SolutionTest.IsSolved(cube))
                    {
                        result = "solved";
                        successfulSolveCount++;

                    }
                    else
                    {
                        result = "spurious";
                        spuriousCount++;
                    }
                    Console.WriteLine($"\tRESULT: {result.ToUpper(),-20}SOLUTION LENGTH: {solution.moveList.Count}");
                }
                catch ( Exception exc)
                {
                    result = "failure";
                    failureCount++;
                }
                Console.WriteLine($"\tCUMULATIVE: SUCCESS: {successfulSolveCount,-8}   FAIL: {failureCount,-8}   SPURIOUS: {spuriousCount,-8}");
                Console.WriteLine($"\tCUMULATIVE: SUCCESS: {successfulSolveCount / (double)count,-8:p}   FAIL: {failureCount / (double)count,-8:p}   SPURIOUS: {spuriousCount / (double)count,-8:p}");


                //Console.WriteLine($"\t#{count}     SUCCESS: {successfulSolveCount}     FAILURE: {count - successfulSolveCount - spuriousCount}    SPURIOUS: {spuriousCount}");
                if( showCubes )
                    cube.WriteColoredCube();
                Console.WriteLine("\n\n\n");
            }
            watch.Stop();
            Console.WriteLine($"\tELAPSED TIME: {watch.Elapsed}   PER SCRAMBLE: {watch.ElapsedMilliseconds / (double)scramblesToGenerate}ms");


            return;

        }
    }
}
