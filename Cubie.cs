using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    /// <summary>
    /// Class to validate cubies
    /// Work in progress.
    /// </summary>
    class Cubie
    {
        public Type Type { get => type; }
        public List<char> Sides { get => sides; }
        public string SquareColorString { get => squareColorString; }
        public string PieceID { get => pieceID; }

        public static Cubie C0  = new Cubie("WOB");
        public static Cubie C1  = new Cubie("WBR");
        public static Cubie C2  = new Cubie("WRG");
        public static Cubie C3  = new Cubie("WGO");
        public static Cubie C4  = new Cubie("YGR");
        public static Cubie C5  = new Cubie("YRB");
        public static Cubie C6  = new Cubie("YBO");
        public static Cubie C7  = new Cubie("YOG");
        
        public static Cubie E0  = new Cubie("WB");
        public static Cubie E1  = new Cubie("WR");
        public static Cubie E2  = new Cubie("WG");
        public static Cubie E3  = new Cubie("WO");
        public static Cubie E4  = new Cubie("GO");
        public static Cubie E5  = new Cubie("GR");
        public static Cubie E6  = new Cubie("BR");
        public static Cubie E7  = new Cubie("BO");
        public static Cubie E8  = new Cubie("YG");
        public static Cubie E9  = new Cubie("YR");
        public static Cubie E10 = new Cubie("YB");
        public static Cubie E11 = new Cubie("YO");

        public static Cubie F0  = new Cubie("W");
        public static Cubie F1  = new Cubie("G");
        public static Cubie F2  = new Cubie("R");
        public static Cubie F3  = new Cubie("B");
        public static Cubie F4  = new Cubie("L");
        public static Cubie F5  = new Cubie("D");

        private List<char> sides;
        private Type type;
        private string squareColorString;
        private string pieceID;

        public Cubie( string str)
        {
            if( str.Length == 1 )
                type = Type.Center;
            else if( str.Length == 2 )
                type = Type.Edge;
            else if (str.Length == 3)
                type = Type.Edge;
            else
                throw new ArgumentException("Invalid string passed [" + str + "].");

            sides = new List<char>();

            sides.AddRange(str.ToCharArray());
            PutPrimaryFacetFirst();

            ClassifyCubie();
            
            squareColorString = string.Join("", sides);
        }

        private void PutPrimaryFacetFirst()
        {
            // If it's a center, nothing to be done
            if( type == Type.Center )
                return;

            // Primary facet is W or Y, if it exists
            // Otherwise, it's G or B side
            
            int primaryFacetIndex = sides.IndexOf('W');
            if( primaryFacetIndex < 0 )
                primaryFacetIndex = sides.IndexOf('Y');
            if (primaryFacetIndex < 0)
                primaryFacetIndex = sides.IndexOf('G');
            if (primaryFacetIndex < 0)
                primaryFacetIndex = sides.IndexOf('B');

            List<char> newSides = new List<char>();

            if ( type == Type.Edge )
            {
                int secondFacetIndex = (primaryFacetIndex + 1) % 2;

                newSides.Add(sides[primaryFacetIndex]);
                newSides.Add(sides[secondFacetIndex]);
            }
            else if( type == Type.Corner )
            {
                int secondFacetIndex = (primaryFacetIndex + 1) % 3;
                int thirdFacetIndex  = (primaryFacetIndex + 2) % 3;


                newSides.Add(sides[primaryFacetIndex]);
                newSides.Add(sides[secondFacetIndex]);
                newSides.Add(sides[thirdFacetIndex]);
            }

            sides = newSides;
        }

        private void ClassifyCubie()
        {
            if (this == C0)
                pieceID = "C0";
            else if (this == C1)
                pieceID = "C1";
            else if (this == C2)
                pieceID = "C2";
            else if (this == C3)
                pieceID = "C3";
            else if (this == C4)
                pieceID = "C4";
            else if (this == C5)
                pieceID = "C5";
            else if (this == C6)
                pieceID = "C6";
            else if (this == C7)
                pieceID = "C7";

            else if (this == E0)
                pieceID = "E0";
            else if (this == E1)
                pieceID = "E1";
            else if (this == E2)
                pieceID = "E2";
            else if (this == E3)
                pieceID = "E3";
            else if (this == E4)
                pieceID = "E4";
            else if (this == E5)
                pieceID = "E5";
            else if (this == E6)
                pieceID = "E6";
            else if (this == E7)
                pieceID = "E7";
            else if (this == E8)
                pieceID = "E8";
            else if (this == E9)
                pieceID = "E9";
            else if (this == E10)
                pieceID = "E10";
            else if (this == E11)
                pieceID = "E11";

            else if (this == F0)
                pieceID = "F0";
            else if (this == F1)
                pieceID = "F1";
            else if (this == F2)
                pieceID = "F2";
            else if (this == F3)
                pieceID = "F3";
            else if (this == F4)
                pieceID = "F4";
            else if (this == F5)
                pieceID = "F5";

            else
            {
                //throw new ArgumentException("Invalid piece [" + this.SquareColorString + "].");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", pieceID, SquareColorString);
        }
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Cubie)
                return this.SquareColorString == ((Cubie)obj).SquareColorString;
            else
                return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
