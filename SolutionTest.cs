using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class SolutionTest
    {

        public static IEnumerable<string> GenerateScramblesEnumerable(int count = 100, int scrambleLength = 100, int seed = 0)
        {
            Random rand = new Random(seed);

            HashSet<int> dummy = new HashSet<int>();

            for (int i = 0; i < count; i++)
                dummy.Add(i);

            foreach (int integer in dummy)
            {
                yield return GenerateRandomScramble(scrambleLength, rand);
            }
        }
        public static List<string> GenerateScrambles(int count = 100, int scrambleLength = 10000, int seed = 0)
        {
            Random rand = new Random(seed);

            List<string> returnValue = new List<string>();

            for (int i = 0; i < count; i++)
            {
                returnValue.Add(GenerateRandomScramble(scrambleLength, rand));
            }

            return returnValue;
        }

        public static string GenerateRandomScramble(int scrambleLength = 25, Random rand = null)
        {
            string[] faces = { "U", "D", "L", "R", "F", "B"};
            string[] modifiers = { "", "'", "2" };

            if ( rand is null )
                rand = new Random();

            List<string> moves = new List<string>();

            string lastFace = "";
            string thisFace;
            string thisModifier;
            for( int i = 0; i < scrambleLength; i++)
            {
                // Get a random face that is different than the last face
                do
                {
                    thisFace = faces[rand.Next(6)];
                } while (thisFace == lastFace);

                lastFace = thisFace;

                // Get a random modifier
                thisModifier = modifiers[rand.Next(3)];

                moves.Add($"{thisFace}{thisModifier}");
            }
            return string.Join(" ", moves);
        }

        public static bool IsSolvedUpperCross( Cube cube )
        {
            // Upper face squares
            if (cube[Square.UF] != cube[Square.U] ||
                cube[Square.UR] != cube[Square.U] ||
                cube[Square.UB] != cube[Square.U] ||
                cube[Square.UL] != cube[Square.U])
            {
                return false;
            }

            // Vertical face edges
            if (cube[Square.F] != cube[Square.FU] ||
                cube[Square.R] != cube[Square.RU] ||
                cube[Square.B] != cube[Square.BU] ||
                cube[Square.L] != cube[Square.LU])
            {
                return false;
            }
            return true;
        }

        public static bool IsSolvedUpperLayer(Cube cube)
        {
            // Upper cross
            if( !IsSolvedUpperCross( cube ))
            {
                return false;
            }

            // Upper face squares
            if (cube[Square.UFR] != cube[Square.U] ||
                cube[Square.URB] != cube[Square.U] ||
                cube[Square.UBL] != cube[Square.U] ||
                cube[Square.ULF] != cube[Square.U])
            {
                return false;
            }

            // Vertical face edges
            if (cube[Square.F] != cube[Square.FUR] ||
                cube[Square.F] != cube[Square.FUL] ||

                cube[Square.R] != cube[Square.RUF] ||
                cube[Square.R] != cube[Square.RUB] ||

                cube[Square.B] != cube[Square.BUR] ||
                cube[Square.B] != cube[Square.BUL] ||

                cube[Square.L] != cube[Square.LUF] ||
                cube[Square.L] != cube[Square.LUB])
            {
                return false;
            }
            return true;
        }

        public static bool IsSolvedUpperTwoLayers( Cube cube )
        {
            if( !IsSolvedUpperCross( cube ) )
            {
                return false;
            }

            // Middle layer edges
            if (cube[Square.F] != cube[Square.FL] ||
                cube[Square.F] != cube[Square.FR] ||

                cube[Square.R] != cube[Square.RF] ||
                cube[Square.R] != cube[Square.RB] ||

                cube[Square.B] != cube[Square.BR] ||
                cube[Square.B] != cube[Square.BL] ||

                cube[Square.L] != cube[Square.LF] ||
                cube[Square.L] != cube[Square.LB])
            {
                return false;
            }
            return true;
        }

        public static bool IsSolvedOrientBottomEdges(Cube cube)
        {
            if (!IsSolvedUpperTwoLayers(cube))
                return false;

            return cube[Square.DF] == cube[Square.D] &&
                   cube[Square.DR] == cube[Square.D] &&
                   cube[Square.DB] == cube[Square.D] &&
                   cube[Square.DL] == cube[Square.D];
        }

        public static bool IsSolvedPermuteBottomEdges(Cube cube)
        {
            if (!IsSolvedOrientBottomEdges(cube))
                return false;

            return cube[Square.FD] == cube[Square.F] &&
                   cube[Square.RD] == cube[Square.R] &&
                   cube[Square.BD] == cube[Square.B] &&
                   cube[Square.LD] == cube[Square.L];
        }


        // PRECONDITION: YELLOW is DOWN and FRONT is GREEN
        public static bool IsSolvedPermuteBottomCorners( Cube cube )
        {
            if (!IsSolvedPermuteBottomEdges(cube))
                return false;

            // Locations of the bottom corners
            if( !cube.FindPiece("YGR").IsSamePiece(Square.DFR) ||
                !cube.FindPiece("YRB").IsSamePiece(Square.DBR) ||
                !cube.FindPiece("YBO").IsSamePiece(Square.DBL) ||
                !cube.FindPiece("YOG").IsSamePiece(Square.DFL) )
            {
                return false;
            }
            return true;
            
        }


        public static bool IsSolved( Cube cube )
        {
            for( int faceNumber = 0; faceNumber < 6; faceNumber++ )
            {
                int homeSquareIndex = faceNumber * 9;
                for ( int offset = 1; offset < 9; offset++)
                {
                    int offsetIndex = homeSquareIndex + offset;


                    Square homeSquare = Square.GetSquare(homeSquareIndex);
                    Square offsetSquare = Square.GetSquare(offsetIndex);
                    if( cube[homeSquare] != cube[offsetSquare ] )
                    {
                        //Console.WriteLine($"DEBUG: {homeSquare} is {cube[homeSquare]}, {offsetSquare} is {cube[offsetSquare]}");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
