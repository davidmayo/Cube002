using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    /// <summary>
    /// Generate solution
    /// 
    /// Work in progress
    /// </summary>
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
            SolveWhiteCorners();

            // Second layer
            SolveMiddleLayerEdges();

            // Final layer
        }

        private void SolveWhiteCorners()
        {
            // TODO
        }

        private void SolveMiddleLayerEdges()
        {
            // TODO
        }

        
        public override string ToString()
        {
            return string.Join(' ', moveList);
        }

        private void AddMoveToSolution( Move move, string caption = "", string longCaption = "")
        {
            moveList.Add(move);
            Console.WriteLine("Making move {0,-2}: {1}", move, caption);
            cube.MakeMove(move);
        }

        private void AddMoveToSolution(MoveSequence moves, string caption = "", string longCaption = "")
        {
            foreach (Move move in moves.Moves)
            {
                AddMoveToSolution( move, caption, longCaption);
            }
        }

        private void AddMoveToSolution(string moveString, string caption = "", string longCaption = "")
        {
            MoveSequence moves = new MoveSequence(moveString);
            foreach (Move move in moves.Moves)
            {
                AddMoveToSolution(move, caption, longCaption);
            }
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


            // A string like "G";
            string destinationSide = pieceString.Substring(1, 1);
            

            Square pieceSquare_ = cube.FindPiece(pieceString);

            // Prep: Rotate cube so white is U and the side being solved is F
            while (!Square.IsSamePiece(Square.F, cube.FindPiece(destinationSide)))
            {
                AddMoveToSolution("y", "Rotating so green side is in front");
            }

            pieceSquare_ = cube.FindPiece(pieceString);

            bool isAlreadySolved = false;
            bool isInBottomLayer = Square.IsBottomLayer( pieceSquare_ );
            bool isInMiddleLayer = Square.IsMiddleLayer(pieceSquare_);
            bool isInTopLayer = Square.IsTopLayer(pieceSquare_);

            if( isInBottomLayer )
            {
                // If the piece is white-facet down, it's oriented
                bool isOriented = Square.IsFace( 'D', pieceSquare_);


                // Rotate D to put target below destination center square
                while( !Square.IsSamePiece(Square.FD, cube.FindPiece(pieceString)))
                {
                    AddMoveToSolution("D", $"Rotating D until {pieceString} edge piece is in FRONT-DOWN position");
                }

                if ( !isOriented )
                {
                    AddMoveToSolution("D R F' R'", $"Flip {pieceString} piece and put it into FRONT-UP position.");
                }
                else
                {
                    // Do an F2 to place edge
                    AddMoveToSolution("F2", $"Put {pieceString} piece into FRONT-UP position.");
                }
            }

            else if( isInMiddleLayer )
            {
                // Rotate cube so white is U and piece is in FR slot
                while( !Square.IsSamePiece(Square.FR,cube.FindPiece(pieceString)))
                {
                    AddMoveToSolution("y", $"Rotate until {pieceString} edge is in FRONT-RIGHT location.");
                }

                pieceSquare_ = cube.FindPiece(pieceString);
                bool whiteSideFront = Square.IsFace('F', pieceSquare_);

                if(whiteSideFront)
                {
                    AddMoveToSolution("R' D' R", $"I THINK THIS IS WRONG");

                    // Call this function recursively
                    SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);

                }
                else
                {
                    // Do F D F' to place piece in bottom layer, properly oriented
                    AddMoveToSolution("F D F'", $"Place {pieceString} edge into BOTTOM layer, properly oriented.");

                    // Call this function recursively
                    SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);
                }
            }

            // Top layer
            else
            {
                // Rotate cube so piece is in UF position
                while (!Square.IsSamePiece(Square.UF, cube.FindPiece(pieceString)))
                {
                    AddMoveToSolution("y", $"Rotate cube until {pieceString} edge is in UPPER-FRONT position.");
                }

                pieceSquare_ = cube.FindPiece(pieceString);

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
                        AddMoveToSolution("F2", $"Solve {pieceString} edge in UPPER-FRONT position.");

                        // Call this function recursively
                        SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);
                    }
                }
                else
                {
                    // Do F R' D' R to put piece in bottom layer correctly oriented
                    AddMoveToSolution("F R' D' R", $"Move {pieceString} edge to bottom layer, properly oriented");

                    // Call this function recursively
                    SolveWhiteCrossPiece(pieceString, destinationSquareIndex, algorithmMoves);
                }
            }


            this.moveList.AddRange(algorithmMoves);
        }

        private void MoveCubeToStandardOrientation()
        {
            // Move white side to UP
            Square whiteSquare = cube.FindPiece("W");

            //Move whiteOrientationMove;

            if (whiteSquare == Square.U)
            {
                //whiteOrientationMove = new SingleMove("");
                //AddMoveToSolution("");
            }
            else if (whiteSquare == Square.F)
                AddMoveToSolution("x", "Rotate cube to put WHITE face UP");
            else if (whiteSquare == Square.R)
                AddMoveToSolution("z'", "Rotate cube to put WHITE face UP");
            else if (whiteSquare == Square.B)
                AddMoveToSolution("x'", "Rotate cube to put WHITE face UP");
            else if (whiteSquare == Square.L)
                AddMoveToSolution("z", "Rotate cube to put WHITE face UP");
            else if (whiteSquare == Square.D)
                AddMoveToSolution("x2", "Rotate cube to put WHITE face UP");
            else
                throw new Exception("Unable to find white center.");    // SHould never happen - FindPiece() would have thrown.

            // Move green side to FRONT
            Square greenSquare = cube.FindPiece("G");

            if (greenSquare == Square.F)
            {
                AddMoveToSolution("");
            }
            else if (greenSquare == Square.R)
                AddMoveToSolution("y", "Rotate cube to put GREEN face FRONT");
            else if (greenSquare == Square.B)
                AddMoveToSolution("y2", "Rotate cube to put GREEN face FRONT");
            else if (greenSquare == Square.L)
                AddMoveToSolution("y'", "Rotate cube to put GREEN face FRONT");
            else if (greenSquare == Square.D)
                throw new Exception("Green center can't be down if white is up.");
            else if (greenSquare == Square.U)
                throw new Exception("Green center can't be up if white is up.");
            else
                throw new Exception("Unable to find green center.");    // Should never happen - FindPiece() would have thrown.
        }
    }
}
