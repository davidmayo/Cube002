using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class Solution
    {
        public List<Move> moveList;
        public Cube cube;
        public Cube initialCube;

        public Solution( Cube cube )
        {
            this.cube = cube;
            initialCube = new Cube(cube.ToCanonicalString());
            moveList = new List<Move>();

            MoveCubeToStandardOrientation();

            // First layer
            SolveWhiteCross();

            // Second layer
            SolveMiddleLayerEdges();

            // Final layer
        }

        private void SolveMiddleLayerEdges()
        {
            // TODO
        }

        
        public override string ToString()
        {
            return string.Join(' ', moveList);
        }

        private static List<Move> RemoveReduncantMoves(List<Move> moves)
        {
            // TODO
            return moves;
        }

        private void SolveWhiteCross()
        {
            Dictionary<string, int> pieceDestinations = new Dictionary<string, int>();
            pieceDestinations.Add("WG", Square.UF.Index);
            pieceDestinations.Add("WR", Square.UR.Index);
            pieceDestinations.Add("WB", Square.UB.Index);
            pieceDestinations.Add("WO", Square.UL.Index);

            foreach( var pair in pieceDestinations)
            {
                SolveWhiteCrossPiece(pair.Key, pair.Value);
            }
        }

        private void SolveWhiteCrossPiece(string pieceString, int destinationSquareIndex, List<Move> prepMoves = null)
        {
            List<Move> algorithmMoves = new List<Move>();

            if( !(prepMoves is null))
            {
                algorithmMoves.AddRange(prepMoves);
            }

            Move move;

            // A string like "G";
            string destinationSide = pieceString.Substring(1, 1);
            

            Square pieceSquare_ = cube.FindPieceSquare(pieceString);

            // Prep: Rotate cube so white is U and the side being solved is F
            while (!Square.IsSamePiece(Square.F, cube.FindPieceSquare(destinationSide)))
            {
                move = new Move("y");
                algorithmMoves.Add(move);
                cube.MakeMove(move);
                Console.WriteLine("Making move [{0}].", move);
            }

            pieceSquare_ = cube.FindPieceSquare(pieceString);

            bool isAlreadySolved = false;
            bool isInBottomLayer = Square.IsBottomLayer( pieceSquare_ );
            bool isInMiddleLayer = Square.IsMiddleLayer(pieceSquare_);
            bool isInTopLayer = Square.IsTopLayer(pieceSquare_);

            if( isInBottomLayer )
            {
                // If the piece is white-facet down, it's oriented
                bool isOriented = Square.IsFace( 'D', pieceSquare_);


                // Rotate D to put target below destination center square
                while( !Square.IsSamePiece(Square.FD, cube.FindPieceSquare(pieceString)))
                {
                    move = new Move("D");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }

                if ( !isOriented )
                {
                    // Do an F2 to place edge
                    move = new Move("D R F' R'");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }
                else
                {
                    // Do an F2 to place edge
                    move = new Move("F2");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }
            }

            else if( isInMiddleLayer )
            {
                // Rotate cube so white is U and piece is in FR slot
                while( !Square.IsSamePiece(Square.FR,cube.FindPieceSquare(pieceString)))
                {
                    move = new Move("y");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }

                pieceSquare_ = cube.FindPieceSquare(pieceString);
                bool whiteSideFront = Square.IsFace('F', pieceSquare_);

                if(whiteSideFront)
                {
                    // Do R' D' R to place piece in bottom layer, properly oriented
                    move = new Move("R' D' R");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);

                    // Call this function recursively
                    SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);

                }
                else
                {
                    // Do F D F' to place piece in bottom layer, properly oriented
                    move = new Move("F D F'");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);

                    // Call this function recursively
                    SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);
                }
            }

            // Top layer
            else
            {
                // Rotate cube so piece is in UF position
                while (!Square.IsSamePiece(Square.UF, cube.FindPieceSquare(pieceString)))
                {
                    move = new Move("y");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }

                pieceSquare_ = cube.FindPieceSquare(pieceString);

                // Piece is oriented if white side is U
                bool isOriented = Square.IsFace('U', pieceSquare_);

                if( isOriented)
                {
                    isAlreadySolved = false;
                    if (isAlreadySolved)
                    {
                        // Do nothing
                    }
                    else
                    {
                        // Solve the edge with F2
                        move = new Move("F2");
                        algorithmMoves.Add(move);
                        cube.MakeMove(move);
                        Console.WriteLine("Making move [{0}].", move);

                        // Call this function recursively
                        SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);
                    }
                }
                else
                {
                    // Do F R' D' R to put piece in bottom layer correctly oriented
                    move = new Move("F R' D' R");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);

                    // Call this function recursively
                    SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);
                }
            }


            this.moveList.AddRange(algorithmMoves);
        }

        private void MoveCubeToStandardOrientation()
        {
            // Move white side to UP
            Square whiteSquare = cube.FindPieceSquare("W");

            Move whiteOrientationMove;
            
            if (whiteSquare == Square.U)
            {
                whiteOrientationMove = new Move("");
            }
            else if (whiteSquare == Square.F)
                whiteOrientationMove = new Move("x");
            else if (whiteSquare == Square.R)
                whiteOrientationMove = new Move("z'");
            else if (whiteSquare == Square.B)
                whiteOrientationMove = new Move("x'");
            else if (whiteSquare == Square.L)
                whiteOrientationMove = new Move("z");
            else if (whiteSquare == Square.D)
                whiteOrientationMove = new Move("x2");
            else
                throw new Exception("Unable to find white center.");    // SHould never happen - FindPiece() would have thrown.

            // Add move to list
            moveList.Add(whiteOrientationMove);

            // Do the move
            cube.MakeMove(whiteOrientationMove);


            // Move green side to FRONT
            Square greenSquare = cube.FindPieceSquare("G");

            Move greenOrientationMove;
            if (greenSquare == Square.F)
            {
                greenOrientationMove = new Move("");
            }
            else if (greenSquare == Square.R)
                greenOrientationMove = new Move("y");
            else if (greenSquare == Square.B)
                greenOrientationMove = new Move("y2");
            else if (greenSquare == Square.L)
                greenOrientationMove = new Move("y'");
            else if (greenSquare == Square.D)
                throw new Exception("Green center can't be down if white is up.");
            else if (greenSquare == Square.U)
                throw new Exception("Green center can't be up if white is up.");
            else
                throw new Exception("Unable to find green center.");    // Should never happen - FindPiece() would have thrown.

            // Add move to list
            moveList.Add(greenOrientationMove);

            // Do the move
            cube.MakeMove(greenOrientationMove);
        }
    }
}
