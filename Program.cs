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
            //Cube cube = new Cube(position);
            Cube cube = new Cube();

            //cube.MakeMove("M2 E2 S2");

            cube.MakeMove(scramble);

            Console.WriteLine("\tSCRAMBLE: {0}", scramble);
            cube.WriteColoredCube();

            var watch = Stopwatch.StartNew();
            Solution sol = new Solution(cube);
            watch.Stop();
            //sol.cube.WriteColoredCube();
            Console.WriteLine("SOLUTION GENERATION TIME: {0}", watch.Elapsed);
            return;

            Console.WriteLine("SOLUTION: {0}", sol);
            Console.WriteLine("TIME: {0}", watch.Elapsed);

            Console.WriteLine("G location: {0}",cube.FindPiece("YBO"));

            //Move move1 = new Move("R2");

            //Move move2 = new Move("R2");

            ///Move move3 = Sequence.CombineMoves(move1, move2);

            //Console.WriteLine("{0} + {1} = {2}", move1, move2, move3);

            //Sequence seq = new Sequence(new Move("R U R' U'"));
            ;
            //Console.WriteLine("SEQ: {0}    LEN: {1}",seq, seq.Length);

            cube = new Cube();
            cube.WriteColoredCube();

            Move sm = new Move("R'");



            MoveSequence mm = new MoveSequence("R R F U U U U D U");
            //mm = new MultipleMoves("R R");
            Console.WriteLine("Making moves: {0}", mm);
            mm.RemoveRedundancy();
            Console.WriteLine("After removing redundancy: {0}", mm);
            Console.WriteLine("Inverse sequence: {0}", mm.GetInverseSequenced());

            ;

            cube.MakeMove(mm);
            
            cube.WriteColoredCube();
            
        }
    }
}
