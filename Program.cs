using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Cube002
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




            //string position = "XXXXYXXXX/XXXXRXRRR/XXXXGXGGG/XXXXOXOOO/XXXXBXBBB/WWWWWWWWW";
            string scramble = "U L2 B2 D L2 B' R2 B' D L' B x y z";
            Cube cube = new Cube();
            cube.MakeMove(scramble);
            
            Console.WriteLine("\n\tSCRAMBLE: {0}", scramble);
            Console.WriteLine("\n\tSCRAMBLED CUBE:");

            cube.WriteColoredCube();

            Console.Write("\nStarting solve . . . ");
            var watch = Stopwatch.StartNew();
            Solution sol = new Solution(cube);
            watch.Stop();
            Console.WriteLine("Solution found.\n");

            Console.WriteLine("SOLUTION GENERATION TIME: {0}\n", watch.Elapsed);
            Console.WriteLine("SOLUTION LENGTH: {0} moves\n", sol.moveList.Count);

            Console.WriteLine("SOLUTION:\n{0}", sol);

            Console.WriteLine("\nApplying solution moves to cube . . . ");
            cube.MakeMove(sol.moveList);

            Console.WriteLine("\nCube after applying solution:");
            cube.WriteColoredCube();

            string str = "BOY";
            string str2 = "BYO";
            for( int i = -5; i < 5; i++)
            {
                string rotated = PieceValidator.RotateString(str2, i);
                bool equal = PieceValidator.AreEquivalentUnderRotation(str, rotated);

                Console.WriteLine($"Shifting {str2} by {i} yields {rotated}. AreIdentical to BOY={equal}");
            }

            PieceValidator pv = new PieceValidator(cube);

            return;
        }
    }
}
