using System;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cube002
{
    class Program
    {
        static void Main()
        {
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
            sol.cube.WriteColoredCube();

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
