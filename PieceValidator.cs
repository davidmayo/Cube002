using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class PieceValidator
    {
        private readonly Cube cube;

        // Position will be like "C0" for corner 0, "E0" for edge 0, and "F0" for face (center) 0
        // square is the int index of the piece's square that is facing in its primary direction
        private Dictionary<string, int> positionToSquare;
        private Dictionary<int, string> squareToPosition;

        public PieceValidator( Cube cube )
        {
            this.cube = cube;

            InitializeDicts();
        }

        private void InitializeDicts()
        {
            positionToSquare = new Dictionary<string, int>();
            squareToPosition = new Dictionary<int, string>();

            // Add center pieces
            positionToSquare.Add("F0", 4);
            positionToSquare.Add("F1", 13);
            positionToSquare.Add("F2", 22);
            positionToSquare.Add("F3", 31);
            positionToSquare.Add("F4", 40);
            positionToSquare.Add("F5", 49);


            // Add edge pieces
            
            // Up edges
            positionToSquare.Add("E0", 1);
            positionToSquare.Add("E1", 5);
            positionToSquare.Add("E2", 7);
            positionToSquare.Add("E3", 3);

            // Equator edges
            positionToSquare.Add("E4", 12);
            positionToSquare.Add("E5", 14);
            positionToSquare.Add("E6", 30);
            positionToSquare.Add("E7", 32);

            // Down edges
            positionToSquare.Add("E8", 46);
            positionToSquare.Add("E9", 50);
            positionToSquare.Add("E10", 52);
            positionToSquare.Add("E11", 48);


            // Add corner pieces

            // Up corners
            positionToSquare.Add("C0",  0);
            positionToSquare.Add("C1",  2);
            positionToSquare.Add("C2",  8);
            positionToSquare.Add("C3",  6);

            // Down corners
            positionToSquare.Add("C4", 47);
            positionToSquare.Add("C5", 53);
            positionToSquare.Add("C6", 51);
            positionToSquare.Add("C7", 45);


            // Create the inverse for each of these entries
            foreach (var pair in positionToSquare)
                squareToPosition.Add(pair.Value, pair.Key);

            // Then add entries for the non-primary facets

            // White edges
            squareToPosition.Add(28, "E0");
            squareToPosition.Add(19, "E1");
            squareToPosition.Add(10, "E2");
            squareToPosition.Add(37, "E3");

            // Equator edges
            squareToPosition.Add(41, "E4");
            squareToPosition.Add(21, "E5");
            squareToPosition.Add(23, "E6");
            squareToPosition.Add(39, "E7");

            // Yellow edges
            squareToPosition.Add(16, "E8");
            squareToPosition.Add(25, "E9");
            squareToPosition.Add(34, "E10");
            squareToPosition.Add(43, "E11");

            // White corners
            squareToPosition.Add(36, "C0");
            squareToPosition.Add(29, "C0");

            squareToPosition.Add(27, "C1");
            squareToPosition.Add(20, "C1");

            squareToPosition.Add(18, "C2");
            squareToPosition.Add(11, "C2");

            squareToPosition.Add( 9, "C3");
            squareToPosition.Add(38, "C3");

            // Yellow corners
            squareToPosition.Add(17, "C4");
            squareToPosition.Add(24, "C4");

            squareToPosition.Add(26, "C5");
            squareToPosition.Add(33, "C5");

            squareToPosition.Add(35, "C6");
            squareToPosition.Add(42, "C6");

            squareToPosition.Add(44, "C7");
            squareToPosition.Add(15, "C7");

        }

        private static List<char> validColors = new List<char>() { 'W', 'G', 'R', 'B', 'O', 'Y' };
        
        private static List<string> validCenters = new List<string>() {
            "W",
            "G",
            "R",
            "B",
            "O",
            "Y"
        };

        private static List<string> validEdges = new List<string>() {
            // White edges
            "WG", "GW",
            "WR", "RW",
            "WB", "BW",
            "WO", "OW",
            
            // Yellow edges
            "YG", "GY",
            "YR", "RY",
            "YB", "BY",
            "YO", "OY",

            // Other edges
            "RG", "GR",
            "RB", "BR",
            "OG", "GO",
            "OB", "BO"
        };

        // ALways listed CLOCKWISE
        private static List<string> validCorners = new List<string>() {
            // White corners
            "WRG", "RGW", "GWR",
            "WBR", "BRW", "RWB",
            "WOB", "OBW", "BWO",
            "WGO", "GOW", "OWG",
            
            // Yellow corners
            "YGR", "GRY", "RYG",
            "YRB", "RBY", "BYR",
            "YBO", "BOY", "OYB",
            "YOG", "OGY", "GYO",
        };

        public static bool IsValid(string piece)
        {
            if (string.IsNullOrEmpty(piece))
                return false;

            // If there are bad colors, it's not vlid
            foreach (char ch in piece.ToCharArray())
            {
                if (!validColors.Contains(ch))
                    return false;
            }

            if (piece.Length == 3)
                return IsValidCorner(piece);
            else if (piece.Length == 2)
                return IsValidEdge(piece);
            else if (piece.Length == 1)
                return IsValidCenterPiece(piece);
            else
                return false;
        }

        private static bool IsValidCenterPiece(string piece)
        {
            return validCenters.Contains(piece);
        }

        private static bool IsValidEdge(string piece)
        {
            return validEdges.Contains(piece);
        }

        private static bool IsValidCorner(string piece)
        {
            return validCenters.Contains(piece);
        }
    }
}
