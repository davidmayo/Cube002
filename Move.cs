using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class Move
    {
        public List<int[]> Cycles { get => cycles; }
        private IndexMap map;
        private string moveString;

        // Face turns
        private static List<int[]> R_face;
        private static List<int[]> U_face;
        private static List<int[]> F_face;
        private static List<int[]> L_face;
        private static List<int[]> D_face;
        private static List<int[]> B_face;
        
        // Slice turns
        private static List<int[]> M_slice;
        private static List<int[]> E_slice;
        private static List<int[]> S_slice;

        private static List<int[]> nullMove;

        private void InitializeBaseMoves()
        {
            R_face = new List<int[]>() {
                new int[]{ map["RUF"], map["RUB"], map["RDB"], map["RDF"] }, // Face corners
                new int[]{ map["RU" ], map["RB" ], map["RD" ], map["RF" ] }, // Face edges
                new int[]{ map["URF"], map["BUR"], map["DBR"], map["FRD"] }, // Adjacent corners map["UB"]
                new int[]{ map["UBR"], map["BDR"], map["DFR"], map["FUR"] }, // Adjacent corners map["UBR"]
                new int[]{ map["FR" ], map["UR" ], map["BR" ], map["DR" ] }  // Adjacent edges
            };

            U_face = new List<int[]>() {
                new int[]{ map["UBL"], map["UBR"], map["URF"], map["UFL"] }, // Face corners
                new int[]{ map["UB" ], map["UR" ], map["UF" ], map["UL" ] }, // Face edges
                new int[]{ map["FUL"], map["LUB"], map["BUR"], map["RUF"] }, // Adjacent corners map["UB"]
                new int[]{ map["FUR"], map["LUF"], map["BUL"], map["RUB"] }, // Adjacent corners map["UBR"]
                new int[]{ map["FU" ], map["LU" ], map["BU" ], map["RU" ] }  // Adjacent edges
            };

            F_face = new List<int[]>() {
                new int[]{ map["FUL"], map["FUR"], map["FRD"], map["FDL"] }, // Face corners
                new int[]{ map["FU" ], map["FR" ], map["FD" ], map["FL" ] }, // Face edges
                new int[]{ map["UFL"], map["RUF"], map["DFR"], map["LDF"] }, // Adjacent corners map["UB"]
                new int[]{ map["URF"], map["RDF"], map["DFL"], map["LUF"] }, // Adjacent corners map["UBR"]
                new int[]{ map["UF" ], map["RF" ], map["DF" ], map["LF" ] }  // Adjacent edges
            };

            L_face = new List<int[]>() {
                new int[]{ map["LUB"], map["LUF"], map["LDF"], map["LDB"] }, // Face corners
                new int[]{ map["LU" ], map["LF" ], map["LD" ], map["LB" ] }, // Face edges
                new int[]{ map["UBL"], map["FUL"], map["DFL"], map["BDL"] }, // Adjacent corners map["UB"]
                new int[]{ map["UFL"], map["FDL"], map["DBL"], map["BUL"] }, // Adjacent corners map["UBR"]
                new int[]{ map["UL" ], map["FL" ], map["DL" ], map["BL" ] }  // Adjacent edges
            };

            D_face = new List<int[]>() {
                new int[]{ map["DFR"], map["DBR"], map["DBL"], map["DFL"] }, // Face corners
                new int[]{ map["DF" ], map["DR" ], map["DB" ], map["DL" ] }, // Face edges
                new int[]{ map["FDL"], map["RDF"], map["BDR"], map["LDB"] }, // Adjacent corners map["UB"]
                new int[]{ map["FRD"], map["RDB"], map["BDL"], map["LDF"] }, // Adjacent corners map["UBR"]
                new int[]{ map["FD" ], map["RD" ], map["BD" ], map["LD" ] }  // Adjacent edges
            };

            B_face = new List<int[]>() {
                new int[]{ map["BUR"], map["BUL"], map["BDL"], map["BDR"] }, // Face corners
                new int[]{ map["BU" ], map["BL" ], map["BD" ], map["BR" ] }, // Face edges
                new int[]{ map["UBL"], map["LDB"], map["DBR"], map["RUB"] }, // Adjacent corners map["UB"]
                new int[]{ map["UBR"], map["LUB"], map["DBL"], map["RDB"] }, // Adjacent corners map["UBR"]
                new int[]{ map["UB" ], map["LB" ], map["DB" ], map["RB" ] }  // Adjacent edges
            };

            M_slice = new List<int[]>() {
                new int[]{ map["U"  ], map["F"  ], map["D"  ], map["B"  ] }, // Equator centers
                new int[]{ map["UB" ], map["FU" ], map["DF" ], map["BD" ] }, // Equator edges map["UB"]
                new int[]{ map["UF" ], map["FD" ], map["DB" ], map["BU" ] }  // Equator edges map["UBR"]
            };

            E_slice = new List<int[]>() {
                new int[]{ map["F"  ], map["R"  ], map["B"  ], map["L"  ] }, // Equator centers
                new int[]{ map["FL" ], map["RF" ], map["BR" ], map["LB" ] }, // Equator edges map["UB"]
                new int[]{ map["FR" ], map["RB" ], map["BL" ], map["LF" ] }  // Equator edges map["UBR"]
            };

            S_slice = new List<int[]>() {
                new int[]{  map["U"  ], map["R"  ], map["D"  ], map["L"  ] }, // Equator centers
                new int[]{  map["UL" ], map["RU" ], map["DR" ], map["LD" ] }, // Equator edges map["UB"]
                new int[]{  map["UR" ], map["RD" ], map["DL" ], map["LU" ] }  // Equator edges map["UBR"]
            };

            // Null move (do nothing)
            nullMove = new List<int[]>() {};
        }

        private List<int[]> cycles;

        private void ProcessMultipleMoveString( string movesString )
        {

            var moves = movesString.Split(" ");

            foreach (var move in moves)
                ProcessSingleMoveString(move);
        }

        public override string ToString()
        {
            return moveString;
        }

        private void ProcessSingleMoveString( string singleMove )
        {
            // Null move
            if( string.IsNullOrEmpty(singleMove))
            {
                cycles.AddRange(nullMove);
            }

            // Right face moves
            else if (singleMove == "R")
            {
                cycles.AddRange(R_face);
            }
            else if (singleMove == "R2")
            {
                cycles.AddRange(R_face);
                cycles.AddRange(R_face);
            }
            else if ( singleMove == "R'" )
            {
                cycles.AddRange(R_face);
                cycles.AddRange(R_face);
                cycles.AddRange(R_face);
            }

            // Left face moves
            else if ( singleMove == "L")
            {
                cycles.AddRange(L_face);
            }
            else if ( singleMove == "L2")
            {
                cycles.AddRange(L_face);
                cycles.AddRange(L_face);
            }
            else if ( singleMove == "L'")
            {
                cycles.AddRange(L_face);
                cycles.AddRange(L_face);
                cycles.AddRange(L_face);
            }

            // Up face moves
            else if ( singleMove == "U")
            {
                cycles.AddRange(U_face);
            }
            else if ( singleMove == "U2")
            {
                cycles.AddRange(U_face);
                cycles.AddRange(U_face);
            }
            else if ( singleMove == "U'")
            {
                cycles.AddRange(U_face);
                cycles.AddRange(U_face);
                cycles.AddRange(U_face);
            }

            // Down face moves
            else if ( singleMove == "D")
            {
                cycles.AddRange(D_face);
            }
            else if ( singleMove == "D2")
            {
                cycles.AddRange(D_face);
                cycles.AddRange(D_face);
            }
            else if ( singleMove == "D'")
            {
                cycles.AddRange(D_face);
                cycles.AddRange(D_face);
                cycles.AddRange(D_face);
            }

            // Front face moves
            else if ( singleMove == "F")
            {
                cycles.AddRange(F_face);
            }
            else if ( singleMove == "F2")
            {
                cycles.AddRange(F_face);
                cycles.AddRange(F_face);
            }
            else if ( singleMove == "F'")
            {
                cycles.AddRange(F_face);
                cycles.AddRange(F_face);
                cycles.AddRange(F_face);
            }

            // Back face moves
            else if (singleMove == "B")
            {
                cycles.AddRange(B_face);
            }
            else if (singleMove == "B2")
            {
                cycles.AddRange(B_face);
                cycles.AddRange(B_face);
            }
            else if (singleMove == "B'")
            {
                cycles.AddRange(B_face);
                cycles.AddRange(B_face);
                cycles.AddRange(B_face);
            }

            // M slice move
            else if (singleMove == "M")
            {
                cycles.AddRange(M_slice);
            }
            else if (singleMove == "M2")
            {
                cycles.AddRange(M_slice);
                cycles.AddRange(M_slice);
            }
            else if (singleMove == "M'")
            {
                cycles.AddRange(M_slice);
                cycles.AddRange(M_slice);
                cycles.AddRange(M_slice);
            }

            // E slice move
            else if (singleMove == "E")
            {
                cycles.AddRange(E_slice);
            }
            else if (singleMove == "E2")
            {
                cycles.AddRange(E_slice);
                cycles.AddRange(E_slice);
            }
            else if (singleMove == "E'")
            {
                cycles.AddRange(E_slice);
                cycles.AddRange(E_slice);
                cycles.AddRange(E_slice);
            }

            // S slice move
            else if (singleMove == "S")
            {
                cycles.AddRange(S_slice);
            }
            else if (singleMove == "S2")
            {
                cycles.AddRange(S_slice);
                cycles.AddRange(S_slice);
            }
            else if (singleMove == "S'")
            {
                cycles.AddRange(S_slice);
                cycles.AddRange(S_slice);
                cycles.AddRange(S_slice);
            }

            // X cube rotation
            else if (singleMove == "x")
                ProcessMultipleMoveString("R M' L'");
            else if (singleMove == "x2")
                ProcessMultipleMoveString("R2 M2 L2");
            else if (singleMove == "x'")
                ProcessMultipleMoveString("R' M L");

            // Y cube rotation
            else if (singleMove == "y")
                ProcessMultipleMoveString("U E' D'");
            else if (singleMove == "y2")
                ProcessMultipleMoveString("U2 E2 D2");
            else if (singleMove == "y'")
                ProcessMultipleMoveString("U' E D");

            // Z cube rotation
            else if (singleMove == "z")
                ProcessMultipleMoveString("F S B'");
            else if (singleMove == "z2")
                ProcessMultipleMoveString("F2 S2 B2");
            else if (singleMove == "z'")
                ProcessMultipleMoveString("F' S' B");

            // r wide slice move
            else if( singleMove == "r")
                ProcessMultipleMoveString("R M'");
            else if (singleMove == "r2")
                ProcessMultipleMoveString("R2 M2");
            else if (singleMove == "r'")
                ProcessMultipleMoveString("R' M");

            // l wide slice move
            else if (singleMove == "l")
                ProcessMultipleMoveString("L M");
            else if (singleMove == "l2")
                ProcessMultipleMoveString("L2 M2");
            else if (singleMove == "l'")
                ProcessMultipleMoveString("L' M'");

            // u wide slice move
            else if (singleMove == "u")
                ProcessMultipleMoveString("U E'");
            else if (singleMove == "u2")
                ProcessMultipleMoveString("U2 E2");
            else if (singleMove == "u'")
                ProcessMultipleMoveString("U' E");

            // d wide slice move
            else if (singleMove == "d")
                ProcessMultipleMoveString("D E");
            else if (singleMove == "d2")
                ProcessMultipleMoveString("D2 E2");
            else if (singleMove == "d'")
                ProcessMultipleMoveString("D' E'");

            // r wide slice move
            else if (singleMove == "r")
                ProcessMultipleMoveString("R M'");
            else if (singleMove == "r2")
                ProcessMultipleMoveString("R2 M2");
            else if (singleMove == "r'")
                ProcessMultipleMoveString("R' M");

            // l wide slice move
            else if (singleMove == "l")
                ProcessMultipleMoveString("L M");
            else if (singleMove == "l2")
                ProcessMultipleMoveString("L2 M2");
            else if (singleMove == "l'")
                ProcessMultipleMoveString("L' M'");

            else
            {
                throw new ArgumentException("Invalid or unsupported move [" + singleMove + "].");
            }
        }
        public Move(string moveString)
        {
            cycles = new List<int[]>();
            map = new IndexMap();
            this.moveString = moveString;


            InitializeBaseMoves();
            ProcessMultipleMoveString(moveString);
        }
    }
}
