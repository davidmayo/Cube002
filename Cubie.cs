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

        public static Cubie cor0  = new Cubie("WOB");
        public static Cubie cor1  = new Cubie("WBR");
        public static Cubie cor2  = new Cubie("WRG");
        public static Cubie cor3  = new Cubie("WGO");
        public static Cubie cor4  = new Cubie("YGR");
        public static Cubie cor5  = new Cubie("YRB");
        public static Cubie cor6  = new Cubie("YBO");
        public static Cubie cor7  = new Cubie("YOG");
        
        public static Cubie edg00  = new Cubie("WB");
        public static Cubie edg01  = new Cubie("WR");
        public static Cubie edg02  = new Cubie("WG");
        public static Cubie edg03  = new Cubie("WO");
        public static Cubie edg04  = new Cubie("GO");
        public static Cubie edg05  = new Cubie("GR");
        public static Cubie edg06  = new Cubie("BR");
        public static Cubie edg07  = new Cubie("BO");
        public static Cubie edg08  = new Cubie("YG");
        public static Cubie edg09  = new Cubie("YR");
        public static Cubie edg10 = new Cubie("YB");
        public static Cubie edg11 = new Cubie("YO");

        public static Cubie cen0  = new Cubie("W");
        public static Cubie cen1  = new Cubie("G");
        public static Cubie cen2  = new Cubie("R");
        public static Cubie cen3  = new Cubie("B");
        public static Cubie cen4  = new Cubie("L");
        public static Cubie cen5  = new Cubie("D");

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
            if (this == cor0)
                pieceID = "cor0";
            else if (this == cor1)
                pieceID = "cor1";
            else if (this == cor2)
                pieceID = "cor2";
            else if (this == cor3)
                pieceID = "cor3";
            else if (this == cor4)
                pieceID = "cor4";
            else if (this == cor5)
                pieceID = "cor5";
            else if (this == cor6)
                pieceID = "cor6";
            else if (this == cor7)
                pieceID = "cor7";

            else if (this == edg00)
                pieceID = "edg00";
            else if (this == edg01)
                pieceID = "edg01";
            else if (this == edg02)
                pieceID = "edg02";
            else if (this == edg03)
                pieceID = "edg03";
            else if (this == edg04)
                pieceID = "edg04";
            else if (this == edg05)
                pieceID = "edg05";
            else if (this == edg06)
                pieceID = "edg06";
            else if (this == edg07)
                pieceID = "edg07";
            else if (this == edg08)
                pieceID = "edg08";
            else if (this == edg09)
                pieceID = "edg09";
            else if (this == edg10)
                pieceID = "edg10";
            else if (this == edg11)
                pieceID = "edg11";

            else if (this == cen0)
                pieceID = "cen0";
            else if (this == cen1)
                pieceID = "cen1";
            else if (this == cen2)
                pieceID = "cen2";
            else if (this == cen3)
                pieceID = "cen3";
            else if (this == cen4)
                pieceID = "cen4";
            else if (this == cen5)
                pieceID = "cen5";

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
