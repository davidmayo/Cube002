using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Cube002
{
    class Cube
    {
        public char[] Squares { get; }
        private char[] squares;
        private IndexMap map;

        private static readonly string solvedPositionCanonicalString = "WWWWWWWWW/GGGGGGGGG/RRRRRRRRR/BBBBBBBBB/OOOOOOOOO/YYYYYYYYY";

        public Cube() : this(solvedPositionCanonicalString )
        {
        }

        public Cube( string canonicalString)
        {
            map = new IndexMap();

            ProcessCanonicalString(canonicalString);
        }

        /// <summary>
        /// Find the given piece. The index returned will be the square index of the FIRST character of the piece
        /// </summary>
        /// <param name="pieceString">A string like "WBR" representing </param>
        /// <returns></returns>
        public int FindPiece(string pieceString)
        {
            if (string.IsNullOrEmpty(pieceString))
                throw new ArgumentException("Piece string was empty.");
            if (pieceString.Length == 1)
                return FindCenterPiece(pieceString);
            else if (pieceString.Length == 2)
                return FindEdgePiece(pieceString);
            else if (pieceString.Length == 3)
                return FindCornerPiece(pieceString);

            throw new ArgumentException("Piece string was wrong size.");
        }

        private int FindCornerPiece(string pieceString)
        {
            var cornerSquares = IndexMap.Corners;

            foreach (int cornerSquare in cornerSquares)
            {
                var thisCornerSquareIndexes = IndexMap.GetSquaresFromIndex(cornerSquare);
                if (squares[thisCornerSquareIndexes[0]] == pieceString[0] &&
                    squares[thisCornerSquareIndexes[1]] == pieceString[1] &&
                    squares[thisCornerSquareIndexes[2]] == pieceString[2])
                {
                    return thisCornerSquareIndexes[0];
                }

            }
            throw new Exception("Could not find piece [" + pieceString + "].");
        }


        private int FindEdgePiece(string pieceString)
        {
            var edgeSquares = IndexMap.Edges;

            foreach (int edgeSquare in edgeSquares)
            {
                var thisEdgeSquareIndexes = IndexMap.GetSquaresFromIndex(edgeSquare);

                if( squares[thisEdgeSquareIndexes[0]] == pieceString[0] &&
                    squares[thisEdgeSquareIndexes[1]] == pieceString[1] )
                {
                    return thisEdgeSquareIndexes[0];
                }
                
            }
            throw new Exception("Could not find piece [" + pieceString + "].");
        }

        //public void RotateToStandardPosition()
        //{
        //    int whiteSquare = FindPiece("W");
        //    if (whiteSquare == map["U"])
        //    {
        //        // Do nothing
        //    }
        //    else if (whiteSquare == map["F"])
        //        MakeMove("x");
        //    else if (whiteSquare == map["R"])
        //        MakeMove("z'");
        //    else if (whiteSquare == map["B"])
        //        MakeMove("x'");
        //    else if (whiteSquare == map["L"])
        //        MakeMove("z");
        //    else if (whiteSquare == map["D"])
        //        MakeMove("x2");
        //    else
        //        throw new Exception("Unable to find white center.");    // SHould never happen - FindPiece() would have thrown.
        //
        //    int greenSquare = FindPiece("G");
        //    if (greenSquare == map["F"])
        //    {
        //        // Do nothing
        //    }
        //    else if (greenSquare == map["R"])
        //        MakeMove("y");
        //    else if (greenSquare == map["B"])
        //        MakeMove("y2");
        //    else if (greenSquare == map["L"])
        //        MakeMove("y'");
        //    else if (greenSquare == map["D"])
        //        throw new Exception("Green center can't be down if white is up.");
        //    else if (greenSquare == map["U"])
        //        throw new Exception("Green center can't be up if white is up.");
        //    else
        //        throw new Exception("Unable to find green center.");    // Should never happen - FindPiece() would have thrown.
        //
        //
        //}

        private int FindCenterPiece(string pieceString)
        {
            var centerSquares = IndexMap.Centers;

            foreach( int centerSquare in centerSquares )
            {
                if (squares[centerSquare] == pieceString[0])
                    return centerSquare;
            }
            throw new Exception("Could not find piece [" + pieceString + "].");
        }

        public void CycleSquares(params Square[] cycleSquares)
        {
            // if null, do nothing
            if (cycleSquares is null)
                return;

            // If 0 or 1, do nothing
            if (cycleSquares.Length <= 1)
                return;


            int count = cycleSquares.Length;
            int sourceIndex;
            int destinationIndex;

            // Create a temp one from the end of the indexes
            destinationIndex = cycleSquares[count - 1].Index;
            char tempChar = this.squares[destinationIndex];

            // Iterate over squares
            // In REVERSE order, "pushing" forward
            for (int i = count - 2; i >= 0; i--)
            {
                sourceIndex = cycleSquares[i].Index;
                destinationIndex = cycleSquares[i + 1].Index;

                this.squares[destinationIndex] = this.squares[sourceIndex];
            }

            // Copy temp into the first one
            destinationIndex = cycleSquares[0].Index;

            this.squares[destinationIndex] = tempChar;
        }

        //public void CycleSquares( params int[] indexes)
        //{
        //    if(indexes is null)
        //    {
        //        return;
        //    }
        //
        //    // If 0 or 1, do nothing
        //    if( indexes.Length <= 1)
        //    {
        //        return;
        //    }
        //
        //
        //    int count = indexes.Length;
        //    int sourceIndex;
        //    int destinationIndex;
        //
        //    // Create a temp one from the end of the indexes
        //    destinationIndex = indexes[count - 1];
        //    char tempChar = squares[destinationIndex];
        //
        //    // Iterate over squares
        //    for( int i = count - 2; i >= 0; i-- )
        //    {
        //        sourceIndex = indexes[i];
        //        destinationIndex = indexes[i + 1];
        //
        //        squares[destinationIndex] = squares[sourceIndex];
        //    }
        //
        //    // Copy temp into the first one
        //    destinationIndex = indexes[0];
        //
        //    squares[destinationIndex] = tempChar;
        //}

        public void MakeMove( Move move )
        {
            MakeMove(move.Cycles);
        }
        //public void MakeMove( List<int[]> cycles )
        //{
        //    if (cycles is null)
        //        return;
        //    foreach (var cycle in cycles)
        //        CycleSquares(cycle);
        //}

        public void MakeMove(List<Square[]> cycles)
        {
            if (cycles is null)
                return;
            foreach (var cycle in cycles)
                CycleSquares(cycle);
        }

        public void MakeMove(string moveString)
        {
            Move move = new Move(moveString);
            
            foreach (var cycle in move.Cycles)
                CycleSquares(cycle);
        }

        public void WriteColoredCube()
        {
            if (Console.CursorLeft == 0)
                Console.WriteLine();

            string netString = ToNetString();

            int row = 0;
            int col = 0;
            bool isSquareChar;

            foreach( char ch in netString.ToCharArray())
            {
                if( ch == '\n')
                {
                    row++;
                    col = 0;
                }
                else
                {
                    col++;
                }

                // Top and bottom faces
                if ((1 <= row && row <= 3) || (9 <= row && row <= 11))
                {
                    if (12 <= col && col <= 20)
                        isSquareChar = true;
                    else
                        isSquareChar = false;
                }

                // Middle row of faces
                else if(5 <= row && row <= 7)
                {
                    // First face
                    if ((2 <= col && col <= 10))
                        isSquareChar = true;
                    
                    // Second face
                    else if ((12 <= col && col <= 20))
                        isSquareChar = true;

                    // Third face
                    else if ((22 <= col && col <= 30))
                        isSquareChar = true;

                    // Fourth face
                    else if ((32 <= col && col <= 40))
                        isSquareChar = true;

                    else
                        isSquareChar = false;
                }
                else
                {
                    isSquareChar = false;
                }

                System.Drawing.Color foreground = ColorTranslator.FromHtml("#808080");
                if( isSquareChar)
                {
                    foreground = ch switch
                    {
                        'W' => Colors.White,
                        'G' => Colors.Green,
                        'R' => Colors.Red,
                        'B' => Colors.Blue,
                        'O' => Colors.Orange,
                        'Y' => Colors.Yellow,
                        'X' => Colors.Unspecified,
                        _ => Colors.Default
                    };
                }
                else
                {
                    //; = ConsoleColor.Black;
                }

                string str = new string(ch, 1);
                Console.Write(Ansi.Convert(str, foreground));

            }
        }

        public string ToNetString()
        {
            string rv = "";

            string[] squareStringArray = new string[54];

            for( int index = 0; index < 54; index++ )
            {
                squareStringArray[index] = new string(squares[index], 1);
            }

            rv += string.Format("          ┌─────────┐\n");
			rv += string.Format("        U │ {0}  {1}  {2} │   " + ToCanonicalString() + "\n", squareStringArray);
			rv += string.Format("          │ {3}  {4}  {5} │\n", squareStringArray);
			rv += string.Format("  L       │ {6}  {7}  {8} │       R         B\n", squareStringArray);
            rv += string.Format("┌─────────┼─────────┼─────────┬─────────┐\n");
			rv += string.Format("│ {36}  {37}  {38} │ {09}  {10}  {11} │ {18}  {19}  {20} │ {27}  {28}  {29} │\n", squareStringArray);
			rv += string.Format("│ {39}  {40}  {41} │ {12}  {13}  {14} │ {21}  {22}  {23} │ {30}  {31}  {32} │\n", squareStringArray);
			rv += string.Format("│ {42}  {43}  {44} │ {15}  {16}  {17} │ {24}  {25}  {26} │ {33}  {34}  {35} │\n", squareStringArray);
            rv += string.Format("└─────────┼─────────┼─────────┴─────────┘\n");
            rv += string.Format("          │ {45}  {46}  {47} │\n", squareStringArray);
            rv += string.Format("          │ {48}  {49}  {50} │\n", squareStringArray);
            rv += string.Format("        D │ {51}  {52}  {53} │\n", squareStringArray);
            rv += string.Format("          └─────────┘\n");


            return rv;
        }
        public string ToCanonicalString()
        {
            string rv = string.Join("", squares);

            rv = rv.Insert(9*5, "/");
            rv = rv.Insert(9*4, "/");
            rv = rv.Insert(9*3, "/");
            rv = rv.Insert(9*2, "/");
            rv = rv.Insert(9*1, "/");

            return rv;
        }

        private void ProcessCanonicalString( string canonicalString )
        {
            canonicalString = canonicalString.Replace("/", "").ToUpper();
            if( canonicalString.Length != 54)
            {
                throw new ArgumentException("String must have 54 letters");
            }

            foreach( char ch in canonicalString.ToCharArray())
            {
                if ( ch != 'W' &&
                     ch != 'Y' &&
                     ch != 'G' &&
                     ch != 'B' &&
                     ch != 'R' &&
                     ch != 'O' &&
                     ch != 'X' )
                {
                    throw new ArgumentException("String contained invalid character");
                }
            }

            squares = canonicalString.ToCharArray();
        }
    }
}
