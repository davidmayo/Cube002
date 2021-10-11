using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube002
{
    class TestPlayground
    {
        static void Main()
        {
            //string position = "XXXXYXXXX/XXXXRXRRR/XXXXGXGGG/XXXXOXOOO/XXXXBXBBB/WWWWWWWWW";
            string scramble = "U L2 B2 D L2 B' R2 B' D L' B";
            //Cube cube = new Cube(position);
            Cube cube = new Cube();

            //cube.MakeMove("M2 E2 S2");

            cube.MakeMove(scramble);

            Console.WriteLine("\tSCRAMBLE: {0}", scramble);
            cube.WriteColoredCube();

            var watch = Stopwatch.StartNew();
            Solution sol = new Solution(cube);
            watch.Stop();
            sol.cube.WriteColoredCube();

            Console.WriteLine("SOLUTION: {0}", sol);
            Console.WriteLine("TIME: {0}", watch.Elapsed);

            return;
            var list = Square.AllSquares;

            foreach( var square in list )
            {
                int num = square;
                Console.WriteLine("{0} [num={5,-2}]   face={1}   top={2,-5}   middle={3,-5}   bottom={4,-5}", square, square.GetFace(), square.IsTopLayer(), square.IsMiddleLayer(), square.IsBottomLayer(), num);
                var others = square.GetAllPieceSquares();

                foreach (var other in others)
                    Console.WriteLine("  OTHER: {0}", other);
            }

            Cubie cubie = Cubie.C0;
            Console.WriteLine(cubie);
        }
    }
}
