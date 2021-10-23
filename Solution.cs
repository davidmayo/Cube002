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

        private static readonly bool debugToConsole = true;
        public Solution( Cube cube )
        {
            this.cube = cube;
            initialCube = new Cube(cube.ToCanonicalString());
            moveList = new List<Move>();

            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("DEBUG: Starting state", "FF0000"));
                cube.WriteColoredCube();
            }
            MoveCubeToStandardOrientation();

            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("DEBUG: After moving to standard orientation", "FF0000"));
                cube.WriteColoredCube();
            }
            // First layer
            
            SolveWhiteCross();

            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After white cross", "FF0000"));
                cube.WriteColoredCube();
            }
            SolveWhiteCorners();
            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After white corners", "FF0000"));
                cube.WriteColoredCube();
            }
            // Second layer
            SolveMiddleLayerEdges();
            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After middle layer", "FF0000"));
                cube.WriteColoredCube();
            }
            // Final layer

            OrientYellowEdges();

            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After OrientYellowEdges", "FF0000"));
                cube.WriteColoredCube();
            }
            PermuteYellowEdges();
            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After PermuteYellowEdges", "FF0000"));
                cube.WriteColoredCube();
            }
            PermuteYellowCorners();
            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After PermuteYellowCorners", "FF0000"));
                cube.WriteColoredCube();
            }

            OrientYellowCorners();
            if (debugToConsole)
            {
                Console.WriteLine(AnsiColor.Convert("\n\nDEBUG: After OrientYellowCorners", "FF0000"));
                cube.WriteColoredCube();

                Console.WriteLine("\n\nSOLUTION: {0}\nSOLUTION LENGTH {1}", string.Join(" ", moveList), moveList.Count);
            }
        }

        private void PermuteYellowCorners()
        {
            // TODO: Count correct corners
            int correctlyPositionedCorners = 0;

            int rotationOffset = 0;
            switch( cube[Square.F])
            {
                case 'G':
                    rotationOffset = 0;
                    break;
                case 'O':
                    rotationOffset = 1;
                    break;
                case 'B':
                    rotationOffset = 2;
                    break;
                case 'R':
                    rotationOffset = 3;
                    break;
                default:
                    throw new InvalidOperationException("Invalid front face: " + cube[Square.F]);
            }

            List<(Square, string)> cornerPositions = new List<(Square,string)>()
            {
                (Square.UFR, "YOG"),
                (Square.UBR, "YBO"),
                (Square.UBL, "YRB"),
                (Square.UFL, "YGR")
            };

            for ( int index = 0; index < 4; index++ )
            {
                int offsetIndex = (index + rotationOffset) % 4;
                Square correctLocation = cornerPositions[index].Item1;
                Square actualLocation = cube.FindPiece(cornerPositions[offsetIndex].Item2);

                if( correctLocation.IsSamePiece(actualLocation))
                {
                    correctlyPositionedCorners++;
                }
            }

            //Console.WriteLine("DEBUG: correctlyPositionedCorners = " + correctlyPositionedCorners);

            if (correctlyPositionedCorners == 0)
            {
                // Do U R U' L' U R' U' L
                AddMoveToSolution("U R U' L' U R' U' L", "Do sequence until a corner is positioned correctly.");

                // Recursive call
                PermuteYellowCorners();
            }
            else if (correctlyPositionedCorners == 1)
            {
                // Rotate cube so correctly positioned corner is in UFR
                bool isUFRcorrect = false;
                string correctCornerString = "" + cube[Square.U] + cube[Square.F] + cube[Square.R];
                isUFRcorrect = Square.UFR.IsSamePiece(cube.FindPiece(correctCornerString));
                while( !isUFRcorrect )
                {
                    // Rotate cube
                    AddMoveToSolution("y", "Rotate the cube until the UFR cubie is in the right position.");

                    correctCornerString = "" + cube[Square.U] + cube[Square.F] + cube[Square.R];
                    isUFRcorrect = Square.UFR.IsSamePiece(cube.FindPiece(correctCornerString));
                }


                // Do U R U' L' U R' U' L
                AddMoveToSolution("U R U' L' U R' U' L", "Do sequence until all corners are positioned correctly.");

                // Recursive call
                PermuteYellowCorners();
            }
            else if (correctlyPositionedCorners == 2)
            {
                // Impossible position.
                // TODO
            }
            else if (correctlyPositionedCorners == 3)
            {
                // Impossible position.
                // TODO
            }
            else
            {
                // All 4 corners correctly positioned.
                return;
            }
        }

        private void OrientYellowCorners()
        {
            // Flip cube so YELLOW is DOWN
            // TODO



            // Find count of unsolved corners
            // TODO
            int unsolvedCorners = 4;

            for ( int rotationCount = 0; rotationCount < 4; rotationCount++)
            {
                AddMoveToSolution("y", "Rotate cube looking for solved DFR corners [This will be optimized away].");
                if (!IsDFRsolved())
                    unsolvedCorners++;
            }


            if( unsolvedCorners == 0)
            {
                // Cube is solved
                return;
            }

            else if( unsolvedCorners == 1 )
            {
                // IMPOSSIBLE POSITION
                // TODO
            }
            else
            {
                // Determine if DFR is solved
                // TODO
                bool isDFRsolved = false;
                isDFRsolved = IsDFRsolved();

                // Rotate


                if( isDFRsolved )
                {
                    // Rotate B (Note: Rotate B, NOT y!!!!)
                    // TODO
                }
                else
                {
                    // Do R U R' U' until DFR is solved
                    while ( ! isDFRsolved )
                    {
                        // Do R U R' U' 
                        AddMoveToSolution("R U R' U'", "Do four move sequence until the corner is solved");

                        // Determine if DFR is solved
                        // TODO
                        isDFRsolved = IsDFRsolved();
                    }
                }
            }
        }

        private bool IsDFRsolved()
        {
            // Determine if it's the correct piece
            // It is the correct piece (in the context of solving OrientYellowCorners iff
            //     1. Its colors are the same as the D, FD, and RD square
            //  && 2. Its primary facet is DFR
            string targetPieceString = "" + cube[Square.D] + cube[Square.FD] + cube[Square.RD];

            Square targetPieceLocation = cube.FindPiece(targetPieceString);

            return targetPieceLocation == Square.DFR;

            // Determine if it's the correct orientation
        }

        private void PermuteYellowEdges()
        {
            // These are the pairs of squares to look at for counting how many 
            // HACKY: List of tuples
            List<(Square, Square)> edgeAdjacentFaceTuples = new List<(Square,Square)>()
            {
                (Square.RU, Square.R),
                (Square.FU, Square.F),
                (Square.LU, Square.L),
                (Square.BU, Square.B)
            };

            int solvedEdges = 0;

            bool isCorrectAlignment = false;

            while (!isCorrectAlignment)
            {
                AddMoveToSolution("U", "Rotate U until at least two edges are correctly positioned.");

                solvedEdges = 0;
                foreach (var edgeAdjacentFace in edgeAdjacentFaceTuples)
                {
                    Square topSquare = edgeAdjacentFace.Item1;
                    Square botSquare = edgeAdjacentFace.Item2;

                    if (cube[topSquare] == cube[botSquare])
                        solvedEdges++;
                }

                isCorrectAlignment = solvedEdges >= 2;
            }



            // Rotate U until number of solved edges is at least 2
            // TODO

            if ( solvedEdges == 0 || solvedEdges == 1 || solvedEdges == 3)
            {
                // UNSOLVABLE CUBE
                // I don't think either case can ever happen on a cube that has passed piece validation
                // But I'm not totally sure
                // TODO
            }
            else if( solvedEdges == 2 )
            {
                // V or I case

                // TODO: Determine case
                bool isCaseV = true;
                
                if( isCaseV)
                {
                    // Rotate cube until solved edges are in BU and RU
                    // i.e., until BU.color == B.color && RU.color == R.color
                    while (!(cube[Square.BU] == cube[Square.B] && cube[Square.RU] == cube[Square.R]))
                    {
                        AddMoveToSolution("y", "Two edges next to each other are in the correct position. Rotate the cube until the solved edges are BU and RU.");
                    }

                    // Do R U R' U R U2 R' U to solve the edges
                    AddMoveToSolution("R U R' U R U2 R' U", "Solve the edges.");


                }
                else
                {
                    // I case

                    // Do R U R' U R U2 R' U to transform to V case
                    AddMoveToSolution("R U R' U R U2 R' U", "Two edges across from each other are in the correct position. Transform to having two edges next to each other in correct position.");

                    // Recursive call
                    PermuteYellowEdges();
                }
            }

            // 4 solved edges, so this step is done
            else
            {
                // Done
                return;
            }
        }

        // Precondition: UP is YELLOW
        private void OrientYellowEdges()
        {
            /*
             * Cases:
             *                x 
             * All wrong:   x Y x
             *                x
             *                
             *                
             *                          Y                             
             * V (any orientation):   x Y Y
             *                          x
             *  
             *  
             *                          Y
             * I (any orientation):   x Y x
             *                          Y
             *                          
             *                          
             *                Y
             * All good:    Y Y Y
             *                Y
             */

            List<Square> edgeSquares = new List<Square>() { Square.UB, Square.UR, Square.UF, Square.UL};

            List<int> solvedEdgesIndexes = new List<int>();

            char solvedColor = 'Y';

            for( int index = 0; index < 4; index++ )
            {
                Square edgeSquare = edgeSquares[index];
                if ( cube[edgeSquare] == solvedColor)
                {
                    solvedEdgesIndexes.Add(index);
                }
            }

            // If 1 or 3 solved edges, then cube is UNSOLVABLE
            if( solvedEdgesIndexes.Count == 1 || solvedEdgesIndexes.Count == 3)
            {
                // Unsolvable
                // TODO
            }

            // If 4 solved edges, this is done.
            else if( solvedEdgesIndexes.Count == 4)
            {
                return;
            }

            // If no solved edges, just do the algorithm without alignment
            else if( solvedEdgesIndexes.Count == 0)
            {
                // Do F R U R' U' F'
                AddMoveToSolution(new Move("F R U R' U' F'"), "No edges are right. Do the sequence to solve two edges.");

                // Recursively call
                OrientYellowEdges();
            }

            // This could be a V or an I case
            else if( solvedEdgesIndexes.Count == 2)
            {
                // TODO: Determine case
                // The only I cases are (0 && 2) || (1 && 3)
                bool isCaseI = (solvedEdgesIndexes[0] == 0 && solvedEdgesIndexes[1] == 2) ||
                               (solvedEdgesIndexes[0] == 1 && solvedEdgesIndexes[1] == 3);

                if( isCaseI )
                {
                    // Rotate cube until solved pieces are in UL and UR
                    // This is the same as "Rotate cube until UR is YELLOW"
                    while( cube[Square.UR] != solvedColor)
                    {
                        AddMoveToSolution("y", "Two edges are right in an 'I' shape. Rotate the cube until the solved edges are UL and UR.");
                    }

                    // Do F R U R' U' F'
                    AddMoveToSolution("F R U R' U' F'", "Align all yellow pieces");

                    // Recursively call
                    OrientYellowEdges();
                }
                else
                {
                    // Rotate cube until solved pieces are in UL and UB
                    while (!(cube[Square.UL] == solvedColor && cube[Square.UB] == solvedColor))
                    {
                        AddMoveToSolution("y", "Two edges are right in an 'V' shape. Rotate the cube until the solved edges are UL and UB.");
                    }

                    AddMoveToSolution("F R U R' U' F'", "Convert 'V' case to 'I' case.");

                    // Recursively call
                    OrientYellowEdges();
                }

            }
        }

        private void SolveWhiteCorners()
        {
            // Flip the cube over
            AddMoveToSolution(new Move("x2"), "Flip the cube over.");

            //MoveCubeToStandardOrientation();

            List<string> pieceDestinations = new List<string>();
            pieceDestinations.Add("WRG");
            pieceDestinations.Add("WBR");
            pieceDestinations.Add("WOB");
            pieceDestinations.Add("WGO");

            foreach (string cornerPiece in pieceDestinations)
            {
                SolveWhiteCornerPiece(cornerPiece);
            }
        }

        // Precondition: D is white
        // Precondition: cornerPiece lists the WHTIE square first.
        //    So only WRG, WBR, WOB, and WGO are valid
        private void SolveWhiteCornerPiece(string cornerPiece)
        {
            // Rotate cube until target corner is in BFR position
            // Cube is in correct position if and only if:
            // F is cornerPiece[1] && R is cornerPiece[2]
            // (That's actually redundant. Only need to test one or the other.)
            string frontColor = cornerPiece.Substring(1, 1);
            while( cube.FindPiece(frontColor) != Square.F )
            {
                AddMoveToSolution("y", "Rotate cube until the " + frontColor + " side is in front.");
            }

            Square location = cube.FindPiece(cornerPiece);

            if ( location.IsBottomLayer())
            {
                // isSolved := (DFR == White)
                bool isSolved = location.IsFace('D');

                //Console.WriteLine( "Piece is in face: " + location.GetFace() );

                if (isSolved)
                {
                    // Do nothing
                    // Piece is solved
                    return;
                }
                else
                {
                    //location = cube.FindPiece(cornerPiece);

                    // Rotate cube until PIECE is in DFR
                    while (!cube.FindPiece(cornerPiece).IsSamePiece(Square.DFR))
                    {
                        AddMoveToSolution("y", "Rotate cube until " + cornerPiece + " corner is in DFR position.");
                    }

                    // Do R U R' U' to place in top layer
                    AddMoveToSolution("R U R' U'", "Bring " + cornerPiece + " corner into top layer.");

                    //Console.WriteLine("At recursive call, cornerPiece is in " + cube.FindPiece(cornerPiece));

                    // Call recursively
                    SolveWhiteCornerPiece(cornerPiece);
                }
            }
            else
            {
                // Do U until piece is in UFR position
                while (!cube.FindPiece(cornerPiece).IsSamePiece(Square.UFR))
                {
                    AddMoveToSolution("U", "Rotate upper face until " + cornerPiece + " corner is in UFR position.");

                }

                AddMoveToSolution("R U R' U'", "Bring " + cornerPiece + " corner into bottom layer.");
                // Do R U R' U' to place in bottom layer

                //Console.WriteLine("At recursive call, cornerPiece is in " + cube.FindPiece(cornerPiece));

                // Call recursively
                SolveWhiteCornerPiece(cornerPiece);

            }
        }

        // Precondition: D is white
        private void SolveMiddleLayerEdges()
        {
            // List the side that's in FR first
            List<string> pieceDestinations = new List<string>();
            pieceDestinations.Add("RG");
            pieceDestinations.Add("GO");
            pieceDestinations.Add("OB");
            pieceDestinations.Add("BR");

            foreach (string edgePiece in pieceDestinations)
            {
                SolveMiddleLayerEdge(edgePiece);
            }
        }

        // Precondition: D is white
        // Precondition: edgePiece is given to be solved where [0] is the FRONT side
        private void SolveMiddleLayerEdge(string edgePiece)
        {
            

            char desiredFrontColor = edgePiece[0];
            bool isMiddleLayer = cube.FindPiece(edgePiece).IsMiddleLayer();
            //Console.WriteLine("  isMiddleLayer = {0}", isMiddleLayer);
            if ( isMiddleLayer )
            {
                // Rotate until PIECE is in FR position
                while (!cube.FindPiece(edgePiece).IsSamePiece(Square.FR))
                {
                    AddMoveToSolution("y", "Move target piece " + edgePiece + " to FR position.");
                }

                // isSolved := (F.color == desiredFrontColor)
                bool isSolved = cube[Square.F] == desiredFrontColor;
                //Console.WriteLine("  isMiddleLayer = {0}", isMiddleLayer);

                if ( isSolved )
                {
                    // Do nothing, piece is solved
                    return;
                }
                else
                {
                    // Rotate until PIECE is in FR position
                    while (!cube.FindPiece(edgePiece).IsSamePiece(Square.FR))
                    {
                        AddMoveToSolution("y", "Rotate cube until target piece " + edgePiece + " to FR position.");
                    }

                    // Do R U R' U' F' U' F to put it into the top layer
                    AddMoveToSolution("R U R' U' F' U' F", "Move target piece " + edgePiece + " to TOP layer position.");

                    // Recursively call
                    SolveMiddleLayerEdge(edgePiece);
                }
            }
            else
            {
                // Rotate cube until target DESTINATION is in FR position.
                // This will occur when F.color == desiredFrontColor
                while (cube[Square.F] != desiredFrontColor)
                {
                    AddMoveToSolution("y", "Rotate cube until " + edgePiece + " DESTINATION is in FR position.");
                }

                // Rotate upper face until piece is in FU position
                while (!cube.FindPiece(edgePiece).IsSamePiece(Square.FU))
                {
                    AddMoveToSolution("U", "Move target piece " + edgePiece + " into FU position.");
                }


                // if FU.color == F.color, it's aligned to be inserted from LEFT upper position.
                // otherwise, it's aligned to be inserted from BACK upper position
                bool isAlignedForLeftUpperInsertion = cube[Square.FU] == cube[Square.F];

                if( isAlignedForLeftUpperInsertion)
                {
                    // Do U to place in LEFT upper position
                    AddMoveToSolution("U", "Move target piece " + edgePiece + " into position to be inserted in the front.");


                    // Do R U R' U' F' U' F to solve edge
                    AddMoveToSolution("R U R' U' F' U' F", "Solve " + edgePiece + ".");

                    // TODO
                }
                else
                {
                    // Do U2 to place in BACK upper position
                    AddMoveToSolution("U2", "Move target piece " + edgePiece + " into position to be inserted in the front.");

                    // Do F' U' F U R U R' to solve edge
                    AddMoveToSolution("F' U' F U R U R'", "Solve " + edgePiece + ".");
                    // TODO
                }


            }
        }

        public override string ToString()
        {
            return string.Join(' ', moveList);
        }

        private void AddMoveToSolution( Move move, string caption = "", string longCaption = "")
        {
            moveList.Add(move);
            if (debugToConsole)
            {
                Console.WriteLine("Move #{2,-3} : {0,-2} : {1}", move, caption, moveList.Count);
            }
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
