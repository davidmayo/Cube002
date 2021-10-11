using System.Collections.Generic;

namespace Cube002
{
    
    class IndexMap
    {
        private readonly Dictionary<string, int> squareToIndex;
        private readonly Dictionary<int, string> indexToSquare;


        public static bool IsSamePiece( int square1, int square2 )
        {
            var allPieceSquares = GetSquaresFromIndex(square1);

            return allPieceSquares.Contains(square2);
        }

        public bool IsFace( int index, char face)
        {
            string pieceLocation = this[index];

            // pieceLocation will always have the facet corrsponding to index listed first
            // e.g., 11 maps to FUR, not UFR or RUF
            // So we can just check to see if the first char of pieceLocation
            // equals face.
            return pieceLocation[0] == face;
        }

        private static List<int> centerIndexes = new List<int>()
        {
            4,
            13,
            22,
            31,
            40,
            49
        };

        private static List<int> edgeIndexes = new List<int>()
        {
            // U edges
            1, 28,  // E0
            5, 19,  // E1
            7, 10,  // E2
            3, 37,  // E3

            // Equator edges
            12, 41, // E4
            14, 21, // E5
            30, 23, // E6
            32, 39,  // E7

            // D edges
            46, 16, // E8
            50, 25, // E9
            52, 34, // E10
            48, 43  // E11
        };

        private static List<int> cornerIndexes = new List<int>()
        {
            // U corners
            0, 36, 29,  // C0
            2, 27, 20,  // C1
            8, 18, 11,  // C2
            6, 9, 38,   // C3
                    
            // D corners
            47, 17, 24, // C4
            53, 26, 33, // C5
            51, 35, 42, // C6
            45, 44, 15  // C7
        };

        private static List<int> topLayerIndexes = new List<int>() {
            0, 1, 2, 3, 4, 5, 6, 7, 8,  // U
            9, 10, 11,  // F
            18, 19, 20, // R
            27, 28, 29, // B
            36, 37, 28  // L
        };

        private static List<int> middleLayerIndexes = new List<int>() {
            12, 13, 14, // F
            21, 22, 23, // R
            30, 31, 32, // B
            39, 40, 41  // L
        };

        private static List<int> bottomLayerIndexes = new List<int>() {
            45, 46, 47, 48, 49, 50, 51, 52, 53, // D
            15, 16, 17, // F
            24, 25, 26, // R
            33, 34, 35, // B
            42, 43, 44  // L
        };

        public static List<int> Centers
        {
            get => centerIndexes;
        }

        /// <summary>
        /// Get list of squareIndexes for all edges
        ///    The % 2 == 0 indexes are the primary facets
        ///    The % 2 == 1 indexes are the other facets
        ///    And the edge index is squareIndex / 2 
        /// </summary>
        public static List<int> Edges
        {
            get => edgeIndexes;
        }

        /// <summary>
        /// Get list of squareIndexes for all edges
        ///    The % 3 == 0 indexes are the primary facets
        ///    The % 3 == 1 indexes are the clockwise facets
        ///    The % 3 == 2 indexes are the counterclockwise facets
        ///    And the corner index is squareIndex / 3 
        /// </summary>
        public static List<int> Corners
        {
            get => cornerIndexes;
        }

        public bool IsTopLayer( int index)
        {
            return topLayerIndexes.Contains(index);
        }

        public char GetFace( int index)
        {
            return this[index][0];
        }

        public bool AreSameFace( int index1, int index2 )
        {
            return GetFace(index1) == GetFace(index2);
        }

        public bool IsMiddleLayer(int index)
        {
            return middleLayerIndexes.Contains(index);
        }

        public bool IsBottomLayer(int index)
        {
            return bottomLayerIndexes.Contains(index);
        }

        public static List<int> GetSquaresFromIndex( int square )
        {
            List<int> rv = new List<int>();


            if( Centers.Contains( square ))
            {
                rv.Add(square);
            }
            else if( Edges.Contains(square))
            {
                int index = Edges.IndexOf(square);

                int firstSquareIndex = (index / 2) * 2;
                int secondSquareIndex = firstSquareIndex + 1;

                int firstSquare = Edges[firstSquareIndex];
                int secondSquare = Edges[secondSquareIndex];


                if ( square == firstSquare )
                {
                    rv.Add(firstSquare);
                    rv.Add(secondSquare);
                }
                else
                {
                    rv.Add(secondSquare);
                    rv.Add(firstSquare);
                }
            }
            else if (Corners.Contains(square))
            {
                int index = Corners.IndexOf(square);

                int firstSquareIndex  = (index / 3) * 3;
                int secondSquareIndex = firstSquareIndex + 1;
                int thirdSquareIndex  = secondSquareIndex + 1;

                int firstSquare = Corners[firstSquareIndex];
                int secondSquare = Corners[secondSquareIndex];
                int thirdSquare = Corners[thirdSquareIndex];


                if (square == firstSquare)
                {
                    rv.Add(firstSquare);
                    rv.Add(secondSquare);
                    rv.Add(thirdSquare);
                }
                else if( square == secondSquare)
                {
                    rv.Add(secondSquare);
                    rv.Add(thirdSquare);
                    rv.Add(firstSquare);
                }
                else
                {
                    rv.Add(thirdSquare);
                    rv.Add(firstSquare);
                    rv.Add(secondSquare);
                }
            }
            return rv;
        }

        public IndexMap()
        {
            squareToIndex = new Dictionary<string, int>();
            indexToSquare = new Dictionary<int, string>();

            // CENTER squares
            
            squareToIndex.Add("U",  4);
            squareToIndex.Add("F", 13);
            squareToIndex.Add("R", 22);
            squareToIndex.Add("B", 31);
            squareToIndex.Add("L", 40);
            squareToIndex.Add("D", 49);


            // EDGE squares

            // UPPER edge squares
            squareToIndex.Add("UB",  1);
            squareToIndex.Add("UR",  5);
            squareToIndex.Add("UF",  7);
            squareToIndex.Add("UL",  3);


            // FRONT edge squares
            squareToIndex.Add("FU", 10);
            squareToIndex.Add("FR", 14);
            squareToIndex.Add("FD", 16);
            squareToIndex.Add("FL", 12);

            // RIGHT edge square
            squareToIndex.Add("RU", 19);
            squareToIndex.Add("RB", 23);
            squareToIndex.Add("RD", 25);
            squareToIndex.Add("RF", 21);

            // BACK edge square
            squareToIndex.Add("BU", 28);
            squareToIndex.Add("BL", 32);
            squareToIndex.Add("BD", 34);
            squareToIndex.Add("BR", 30);


            // LEFT edge square
            squareToIndex.Add("LU", 37);
            squareToIndex.Add("LF", 41);
            squareToIndex.Add("LD", 43);
            squareToIndex.Add("LB", 39);

            // DOWN edge square
            squareToIndex.Add("DF", 46);
            squareToIndex.Add("DR", 50);
            squareToIndex.Add("DB", 52);
            squareToIndex.Add("DL", 48);


            // CORNER squares

            // UPPER corner squares
            squareToIndex.Add("UBL",  0);
            squareToIndex.Add("UBR",  2);
            squareToIndex.Add("UFR",  8);
            squareToIndex.Add("UFL",  6);

            // FRONT corner squares
            squareToIndex.Add("FUL",  9);
            squareToIndex.Add("FUR", 11);
            squareToIndex.Add("FDR", 17);
            squareToIndex.Add("FDL", 15);

            // RIGHT corner squares
            squareToIndex.Add("RUF", 18);
            squareToIndex.Add("RUB", 20);
            squareToIndex.Add("RDB", 26);
            squareToIndex.Add("RDF", 24);

            // BACK corner squares
            squareToIndex.Add("BUR", 27);
            squareToIndex.Add("BUL", 29);
            squareToIndex.Add("BDL", 35);
            squareToIndex.Add("BDR", 33);

            // LEFT corner squares
            squareToIndex.Add("LUB", 36);
            squareToIndex.Add("LUF", 38);
            squareToIndex.Add("LDF", 44);
            squareToIndex.Add("LDB", 42);

            // DOWN corner squares
            squareToIndex.Add("DFL", 45);
            squareToIndex.Add("DFR", 47);
            squareToIndex.Add("DBR", 53);
            squareToIndex.Add("DBL", 51);

            // Add the reverse map of all these to the indexToSquare Dictionary
            foreach (var pair in squareToIndex)
                indexToSquare.Add(pair.Value, pair.Key);

            // For each CORNER square, need to add a second entry
            // Since "Front face, upper right corner"
            // Can be written either "FUR" or "FRU"
            // Both should map to the same index (11)
            var keys = new List<string>(squareToIndex.Keys);
            foreach ( var key in keys)
            {
                int value = squareToIndex[key];

                // If key length is not 3 chars long, it's not a corner
                if (key.Length != 3)
                    continue;

                string newKey = "" + key[0] + key[2] + key[1];
                squareToIndex.Add(newKey, value);
            }
        }

        // Accessors for [] operator
        public int this[string square]
        {
            get => squareToIndex[square.ToUpper()];
        }

        public string this[int index]
        {
            get => indexToSquare[index];
        }
    }
}