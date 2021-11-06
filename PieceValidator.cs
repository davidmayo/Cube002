using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cube002
{
    class PieceValidator
    {
        public static List<string> ValidCenters { get => validCenters; }
        public static List<string> ValidEdges { get => validEdges; }
        public static List<string> ValidCorners { get => validCorners; }

        private static readonly List<List<Square>> allPieces = new List<List<Square>>()
        {
            // Centers
            new List<Square>() {Square.U},
            new List<Square>() {Square.F},
            new List<Square>() {Square.R},
            new List<Square>() {Square.B},
            new List<Square>() {Square.L},
            new List<Square>() {Square.D},

            // Edges
            new List<Square>() {Square.UF, Square.FU},
            new List<Square>() {Square.UR, Square.RU},
            new List<Square>() {Square.UB, Square.BU},
            new List<Square>() {Square.UL, Square.LU},

            new List<Square>() {Square.FR, Square.RF},
            new List<Square>() {Square.BR, Square.RB},
            new List<Square>() {Square.FL, Square.LF},
            new List<Square>() {Square.BL, Square.LB},

            new List<Square>() {Square.DF, Square.FD},
            new List<Square>() {Square.DR, Square.RD},
            new List<Square>() {Square.DB, Square.BD},
            new List<Square>() {Square.DL, Square.LD },

            // Corners
            new List<Square>() {Square.UFR, Square.RUF, Square.FUR},
            new List<Square>() {Square.UBR, Square.BUR, Square.RUB},
            new List<Square>() {Square.UFL, Square.FUL, Square.LUF},
            new List<Square>() {Square.UBL, Square.LUB, Square.BUL},

            new List<Square>() {Square.DFR, Square.FDR, Square.RDF},
            new List<Square>() {Square.DBR, Square.RDB, Square.BDR},
            new List<Square>() {Square.DFL, Square.LDF, Square.FDL},
            new List<Square>() {Square.DBL, Square.BDL, Square.LDB},
        };

        private static readonly List<string> validCenters = new List<string>()
        {
            "W",
            "G",
            "R",
            "B",
            "O",
            "Y"
        };
        private static readonly List<string> validEdges = new List<string>()
        {
            // White edges (white first)
            "WG",
            "WR",
            "WB",
            "WO",

            // Equatorial edges (green or blue first)
            "GR",
            "BR",
            "BO",
            "GO",
            
            // Yellow edges (yellow first)
            "YG",
            "YO",
            "YB",
            "YR"
        };
        private static readonly List<string> validCorners = new List<string>()
        {
            // White corners (white first, then clockwise)
            "WRG",
            "WBR",
            "WOB",
            "WGO",

            // Yellow corners (yellow first, then clockwise)
            "YOG",
            "YBO",
            "YRB",
            "YGR"
        };

        //private static readonly List<string> validPieces = validCenters.Concat(validEdges).Concat(validCorners).ToList();

        //private List<string> availableCenterPieces;
        //private List<string> availableEdgePieces;
        //private List<string> availableCornerPieces;

        private List<string> availablePieces;

        //private List<string> usedCenterPieces;
        //private List<string> usedEdgePieces;
        //private List<string> usedCornerPieces;

        private Cube cube;

        private bool isValidCube;

        public PieceValidator(Cube cube)
        {
            // Initialize the available lists to include every valid piece
            // AND every valid rotation of a valid piece.
            List<string> availableCenterPieces = new List<string>();
            foreach (var validEdge in validCenters)
            {
                availableCenterPieces.Add(RotateString(validEdge, 0));
            }

            List<string> availableEdgePieces = new List<string>();
            foreach( var validEdge in validEdges)
            {
                availableEdgePieces.Add(RotateString(validEdge, 0));
                availableEdgePieces.Add(RotateString(validEdge, 1));
            }

            List<string> availableCornerPieces = new List<string>();
            foreach (var validCorner in validCorners)
            {
                availableCornerPieces.Add(RotateString(validCorner, 0));
                availableCornerPieces.Add(RotateString(validCorner, 1));
                availableCornerPieces.Add(RotateString(validCorner, 2));
            }

            // Concatenate them all together for available pieces
            availablePieces = availableCenterPieces.Concat(availableEdgePieces).Concat(availableCornerPieces).ToList();

            // Initialize the usedPieces lists
            // DO WE NEED THESE FOR ANYTHING? I don't think so...
            //usedCenterPieces = new List<string>();
            //usedEdgePieces = new List<string>();
            //usedCornerPieces = new List<string>();
            
            this.cube = cube;

            try
            {
                ValidateCenterPieces();
                ValidateEdgePieces();
                ValidateCornerPieces();

                isValidCube = true;
            }
            catch (Exception)
            {
                isValidCube = false;
            }

            ;
        }

        private void ValidateEdgePieces()
        {
            List<(Square, Square)> edgePieceTuples = new List<(Square, Square)>()
            {
                (Square.UF, Square.FU),
                (Square.UR, Square.RU),
                (Square.UB, Square.BU),
                (Square.UL, Square.LU),

                (Square.FR, Square.RF),
                (Square.BR, Square.RB),
                (Square.FL, Square.LF),
                (Square.BL, Square.LB),

                (Square.DF, Square.FD),
                (Square.DR, Square.RD),
                (Square.DB, Square.BD),
                (Square.DL, Square.LD),
            };

            foreach (var edgePieceTuple in edgePieceTuples)
            {
                var primarySquare = edgePieceTuple.Item1;
                var secondSquare = edgePieceTuple.Item2;

                string pieceString = string.Format("{0}{1}", cube[primarySquare], cube[secondSquare]);
                ConsumePiece(pieceString);
            }
        }

        private void ValidateCornerPieces()
        {
            List<(Square, Square, Square)> cornerPieceTuples = new List<(Square, Square, Square)>()
            {
                (Square.UFR, Square.RUF, Square.FUR),
                (Square.UBR, Square.BUR, Square.RUB),
                (Square.UFL, Square.FUL, Square.LUF),
                (Square.UBL, Square.LUB, Square.BUL),

                (Square.DFR, Square.FDR, Square.RDF),
                (Square.DBR, Square.RDB, Square.BDR),
                (Square.DFL, Square.LDF, Square.FDL),
                (Square.DBL, Square.BDL, Square.LDB),
            };

            foreach( var cornerPieceTuple in cornerPieceTuples)
            {
                var primarySquare = cornerPieceTuple.Item1;
                var secondSquare = cornerPieceTuple.Item2;
                var thirdSquare = cornerPieceTuple.Item3;
                string pieceString = string.Format("{0}{1}{2}", cube[primarySquare], cube[secondSquare], cube[thirdSquare]);
                Console.WriteLine("  DEBUG: pieceString = " + pieceString);

                ConsumePiece(pieceString);
            }
        }

        private void ValidateCenterPieces()
        {
            List<Square> centerSquares = new List<Square>()
            {
                Square.U,
                Square.F,
                Square.R,
                Square.B,
                Square.L,
                Square.D
            };

            foreach( var centerSquare in centerSquares )
            {
                string pieceString = string.Format("{0}", cube[centerSquare]);
                ConsumePiece(pieceString);
            }
        }

        /// <summary>
        /// given a string like "BX" or "XB", return all possible colors for the "X" face"
        /// </summary>
        /// <param name="pieceString"></param>
        /// <returns></returns>
        //public List<char> GetPossibleEdgeFaceColors(string pieceString)
        //{
        //    // If X is at the beginning, swap so that the X is at the end
        //    if (pieceString[0] == 'X')
        //        pieceString = string.Format("{0}{1}", pieceString[1], pieceString[0]);
        //
        //    char targetColorChar = pieceString[0];
        //
        //    List<string> possibleEdgePieces;
        //    List<char> returnValue = new List<char>();
        //
        //    if ( targetColorChar == 'X')
        //        possibleEdgePieces = new List<string>(availableEdgePieces);
        //    else
        //        possibleEdgePieces = availableEdgePieces.Where(item => item.Contains(targetColorChar)).ToList();
        //
        //    // Iterate over available piece
        //    foreach (string possibleEdgePiece in possibleEdgePieces)
        //    {
        //        // iterate over each char of the string
        //        foreach (char ch in possibleEdgePiece.ToCharArray())
        //        {
        //            // If the char is not already in the returnValue List, add it
        //            if (!returnValue.Contains(ch))
        //                returnValue.Add(ch);
        //        }
        //    }
        //    return returnValue;
        //
        //}

        /// <summary>
        /// Get all the rotations of a string. So "BOY" yields {"BOY", "YBO", "OYB"}
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetAllStringRotations( string str )
        {
            List<string> returnValue = new List<string>();
            
            if ( string.IsNullOrEmpty(str))
            {
                return returnValue;
            }
            
            for( int offset = 0; offset < str.Length; offset++)
            {
                returnValue.Add(RotateString(str, offset));
            }
            return returnValue;
        }

        /// <summary>
        /// Rotate a string forward by a given amount. Default amount is +1. So "BOY" becomes "YBO"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="rotationAmount"></param>
        /// <returns></returns>
        public static string RotateString(string str, int rotationAmount = 1)
        {
            char[] charArray = str.ToCharArray();

            int length = str.Length;

            for( int index = 0; index < length; index++ )
            {
                int forwardIndex = PositiveMod( (index + rotationAmount), length);
                charArray[forwardIndex] = str[index];
            }

            return new string(charArray);
        }

        /// <summary>
        /// Get the integer in [0, divisor) that is congruent to dividend (mod divisor)
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        public static int PositiveMod( int dividend, int divisor )
        {
            int mod = dividend % divisor;
            if (mod < 0)
                mod += Math.Abs(divisor);
            return mod;
        }

        /// <summary>
        /// Determine if two strings are equivalent after rotating through some number of positions. So "BOY" is equivalent to "OYB" but not "BYO"
        /// </summary>
        /// <param name="string1"></param>
        /// <param name="string2"></param>
        /// <returns></returns>
        public static bool EqualsUnderRotation(string string1, string string2)
        {
            if (string1.Length != string2.Length)
                return false;

            for( int offset = 0; offset < string1.Length; offset++)
            {
                string string2offset = RotateString(string2, offset);
                if (string1 == string2offset)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// given a string like "BXX" or "XBX", return all possible colors for the face that is CLOCKWISE from the first given face
        /// </summary>
        /// <param name="pieceString"></param>
        /// <returns></returns>
        //public List<char> GetPossibleClockwiseFaceColors(string pieceString)
        //{
        //    //// If X is at the beginning, swap so that the X is at the end
        //    //if (pieceString[0] == 'X')
        //    //    pieceString = string.Format("{0}{1}", pieceString[1], pieceString[0]);
        //
        //    char primaryColor = pieceString[0];
        //    char secondColor = pieceString[1];
        //    char thirdColor = pieceString[2];
        //
        //    char targetColorChar = pieceString[0];
        //
        //    List<string> possibleCornerPieces = new List<string>(availableEdgePieces);
        //    List<char> returnValue = new List<char>();
        //
        //    // If the CLOCKWISE color is already set, it can't change
        //    if (secondColor != 'X')
        //        return new List<char>() { secondColor };
        //
        //    // If the piece is completely blank, it could be anything availableCornerPieces
        //    if ( pieceString == "XXX")
        //        possibleCornerPieces = new List<string>(availableEdgePieces);
        //
        //    // If primaryColor is blank, it's no restriction.
        //    // Otherwise, limit possibleCornerPieces to pieces that contain primaryColor
        //    if( primaryColor != 'X' )
        //    {
        //        // Remove possibleCornerPieces elements that don't contain primaryColor
        //        // TODO
        //    }
        //
        //    // If thirdColor is blank, it's no restriction.
        //    // Otherwise, limit possibleCornerPieces to pieces that contain thirdColor
        //    if ( thirdColor != 'X')
        //    {
        //        // Remove possibleCornerPieces elements that don't contain thirdColor
        //        // TODO
        //    }
        //
        //
        //    if ( primaryColor != 'X')
        //    if (targetColorChar == 'X')
        //        possibleCornerPieces = new List<string>(availableEdgePieces);
        //    else
        //        possibleCornerPieces = availableEdgePieces.Where(item => item.Contains(targetColorChar)).ToList();
        //
        //    // Iterate over available piece
        //    foreach (string possibleEdgePiece in possibleCornerPieces)
        //    {
        //        // iterate over each char of the string
        //        foreach (char ch in possibleEdgePiece.ToCharArray())
        //        {
        //            // If the char is not already in the returnValue List, add it
        //            if (!returnValue.Contains(ch))
        //                returnValue.Add(ch);
        //        }
        //    }
        //    return returnValue;
        //
        //}

        public void ConsumePiece(string piece)
        {
            if( IsValidPiece(piece))
            {
                foreach (var rotatedString in GetAllStringRotations(piece))
                {
                    availablePieces.Remove(rotatedString);
                }
            }
            else
            {
                ;// THROW AN ERROR
            }
        }

        private bool IsValidPiece(string piece)
        {
            return availablePieces.Contains(piece);
        }

        /// <summary>
        /// Convert a string of a piece into standard orientation. I.E., white/yellow first if it exists, and green/blue first if it doesn't
        /// </summary>
        /// <param name="piece">1, 2, or 3 char string representing the colors of a piece in clockwise order</param>
        /// <returns></returns>
        private string ConvertToStandardOrientation(string piece)
        {
            // Find the location in the string of the primary facet
            int primaryFacetIndex;

            // Use W or Y face, if it exists:
            if( piece.Contains('W'))
                primaryFacetIndex = piece.IndexOf('W');
            else if (piece.Contains('Y'))
                primaryFacetIndex = piece.IndexOf('Y');

            // If no W or Y, use G or B face
            else if (piece.Contains('G'))
                primaryFacetIndex = piece.IndexOf('G');
            else if (piece.Contains('B'))
                primaryFacetIndex = piece.IndexOf('B');

            // If no W, Y, G, or B faces, piece is invalid
            else
            {
                throw new ArgumentException("Invalid piece: " + piece + " [does not contain W, Y, G, or B face.]");
            }

            if( piece.Length == 1)
            {
                return piece;
            }
            else if( piece.Length == 2 )
            {
                int secondFacetIndex = (primaryFacetIndex + 1) % 2;

                return string.Format("{0}{1}", piece[primaryFacetIndex], piece[secondFacetIndex]);
            }
            else if( piece.Length == 3 )
            {
                int secondFacetIndex = (primaryFacetIndex + 1) % 3;
                int thirdFacetIndex  = (primaryFacetIndex + 2) % 3;

                return string.Format("{0}{1}{2}", piece[primaryFacetIndex], piece[secondFacetIndex], piece[thirdFacetIndex]);
            }
            else
            {
                throw new ArgumentException("Invalid piece: " + piece + " [Invalid piece length]");
            }
        }
    }
}
