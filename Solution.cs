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
        public IndexMap map;
        public Solution( Cube cube )
        {
            this.cube = cube;
            initialCube = new Cube(cube.ToCanonicalString());
            moveList = new List<Move>();
            map = new IndexMap();

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

        private void SolveWhiteCrossPiece(string piece, int destinationSquare, List<Move> prepMoves = null)
        {
            List<Move> algorithmMoves = new List<Move>();

            if( !(prepMoves is null))
            {
                algorithmMoves.AddRange(prepMoves);
            }

            Move move;
            // A string like "UF"
            string destSquareString = map[destinationSquare];

            // A string like "G";
            string destinationSide = piece.Substring(1, 1);
            
            // an int like 16    
            int pieceSquare = cube.FindPiece(piece);

            // Prep: Rotate cube so white is U and the side being solved is F
            while (!Square.IsSamePiece(Square.F, Square.GetSquare(cube.FindPiece(destinationSide))))
            {
                move = new Move("y");
                algorithmMoves.Add(move);
                cube.MakeMove(move);
                Console.WriteLine("Making move [{0}].", move);
            }

            bool isAlreadySolved = false;
            bool isInBottomLayer = map.IsBottomLayer( pieceSquare );
            bool isInMiddleLayer = map.IsMiddleLayer(pieceSquare);
            bool isInTopLayer = map.IsTopLayer(pieceSquare);

            if( isInBottomLayer )
            {
                // If the piece is white-facet down, it's oriented
                bool isOriented = map.IsFace(pieceSquare, 'D');


                // Rotate D to put target below destination center square
                while( !Square.IsSamePiece(Square.FD, Square.GetSquare(cube.FindPiece(piece))))
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
                while( !Square.IsSamePiece(Square.FR,Square.GetSquare(cube.FindPiece(piece))))
                {
                    move = new Move("y");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }

                int pieceIndex = cube.FindPiece(piece);
                bool whiteSideFront = map.IsFace(pieceIndex, 'F');

                if(whiteSideFront)
                {
                    // Do R' D' R to place piece in bottom layer, properly oriented
                    move = new Move("R' D' R");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);

                    // Call this function recursively
                    SolveWhiteCrossPiece(piece, destinationSquare, algorithmMoves);

                }
                else
                {
                    // Do F D F' to place piece in bottom layer, properly oriented
                    move = new Move("F D F'");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);

                    // Call this function recursively
                    SolveWhiteCrossPiece(piece, destinationSquare, algorithmMoves);
                }
            }

            // Top layer
            else
            {
                // Rotate cube so piece is in UF position
                while (!Square.IsSamePiece(Square.UF, Square.GetSquare(cube.FindPiece(piece))))
                {
                    move = new Move("y");
                    algorithmMoves.Add(move);
                    cube.MakeMove(move);
                    Console.WriteLine("Making move [{0}].", move);
                }

                int pieceLocation = cube.FindPiece(piece);

                // Piece is oriented if white side is U
                bool isOriented = map.IsFace(pieceLocation, 'U');

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
                        SolveWhiteCrossPiece(piece, destinationSquare, algorithmMoves);
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
                    SolveWhiteCrossPiece(piece, destinationSquare, algorithmMoves);
                }
            }


            this.moveList.AddRange(algorithmMoves);
        }

        private void MoveCubeToStandardOrientation()
        {
            // Move white side to UP
            int whiteSquare = cube.FindPiece("W");
            Move whiteOrientationMove;
            if (whiteSquare == map["U"])
            {
                whiteOrientationMove = new Move("");
            }
            else if (whiteSquare == map["F"])
                whiteOrientationMove = new Move("x");
            else if (whiteSquare == map["R"])
                whiteOrientationMove = new Move("z'");
            else if (whiteSquare == map["B"])
                whiteOrientationMove = new Move("x'");
            else if (whiteSquare == map["L"])
                whiteOrientationMove = new Move("z");
            else if (whiteSquare == map["D"])
                whiteOrientationMove = new Move("x2");
            else
                throw new Exception("Unable to find white center.");    // SHould never happen - FindPiece() would have thrown.

            // Add move to list
            moveList.Add(whiteOrientationMove);

            // Do the move
            cube.MakeMove(whiteOrientationMove);


            // Move green side to FRONT
            int greenSquare = cube.FindPiece("G");
            Move greenOrientationMove;
            if (greenSquare == map["F"])
            {
                greenOrientationMove = new Move("");
            }
            else if (greenSquare == map["R"])
                greenOrientationMove = new Move("y");
            else if (greenSquare == map["B"])
                greenOrientationMove = new Move("y2");
            else if (greenSquare == map["L"])
                greenOrientationMove = new Move("y'");
            else if (greenSquare == map["D"])
                throw new Exception("Green center can't be down if white is up.");
            else if (greenSquare == map["U"])
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
