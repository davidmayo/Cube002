using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace LogicalCube
{
    class Program
    {
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



            Cube cube;

            Solution solution;

            int scramblesToGenerate = 1000;
            bool showCubes = true;
            int count = 0;
            int successfulSolveCount = 0;
            int spuriousCount = 0;
            int failureCount = 0;
            string result;
            Stopwatch watch = Stopwatch.StartNew();
            foreach( var scramble in SolutionTest.GenerateScramblesEnumerable(scramblesToGenerate))
            {
                count++;
                Console.WriteLine($"#{count}\tSCRAMBLE: {scramble}");

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
