using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class Move
    {
        public List<Square[]> Cycles { get => cycles; }

        private string moveString;

        // Face turns
        private static List<Square[]> faceR;
        private static List<Square[]> faceU;
        private static List<Square[]> faceF;
        private static List<Square[]> faceL;
        private static List<Square[]> faceD;
        private static List<Square[]> faceB;


        // Slice turns
        private static List<Square[]> sliceM;
        private static List<Square[]> sliceE;
        private static List<Square[]> sliceS;

        private List<Square[]> cycles;

        public Move(string moveString)
        {
            //cycles = new List<int[]>();
            cycles = new List<Square[]>();
            this.moveString = moveString;


            InitializeBaseMoves();
            ProcessMultipleMoveString(moveString);
        }

        private void InitializeBaseMoves()
        {
            faceR = new List<Square[]>()
            {
                new Square[]{ Square.RUF, Square.RUB, Square.RDB, Square.RDF }, // Face corners
                new Square[]{ Square.RU,  Square.RB,  Square.RD,  Square.RF  }, // Face edges
                new Square[]{ Square.URF, Square.BUR, Square.DBR, Square.FRD }, // Adjacent corners 1
                new Square[]{ Square.UBR, Square.BDR, Square.DFR, Square.FUR }, // Adjacent corners 2
                new Square[]{ Square.FR,  Square.UR,  Square.BR,  Square.DR  }  // Adjacent edges
            };
            
            faceU = new List<Square[]>() {
                new Square[]{ Square.UBL, Square.UBR, Square.URF, Square.UFL }, // Face corners
                new Square[]{ Square.UB,  Square.UR,  Square.UF,  Square.UL  }, // Face edges
                new Square[]{ Square.FUL, Square.LUB, Square.BUR, Square.RUF }, // Adjacent corners 1
                new Square[]{ Square.FUR, Square.LUF, Square.BUL, Square.RUB }, // Adjacent corners 2
                new Square[]{ Square.FU,  Square.LU,  Square.BU,  Square.RU  }  // Adjacent edges
            };

            faceF = new List<Square[]>() {
                new Square[]{ Square.FUL, Square.FUR, Square.FRD, Square.FDL }, // Face corners
                new Square[]{ Square.FU,  Square.FR,  Square.FD,  Square.FL  }, // Face edges
                new Square[]{ Square.UFL, Square.RUF, Square.DFR, Square.LDF }, // Adjacent corners 1
                new Square[]{ Square.URF, Square.RDF, Square.DFL, Square.LUF }, // Adjacent corners 2
                new Square[]{ Square.UF,  Square.RF,  Square.DF,  Square.LF  }  // Adjacent edges
            };

            faceL = new List<Square[]>() {
                new Square[]{ Square.LUB, Square.LUF, Square.LDF, Square.LDB }, // Face corners
                new Square[]{ Square.LU,  Square.LF,  Square.LD,  Square.LB  }, // Face edges
                new Square[]{ Square.UBL, Square.FUL, Square.DFL, Square.BDL }, // Adjacent corners 1
                new Square[]{ Square.UFL, Square.FDL, Square.DBL, Square.BUL }, // Adjacent corners 2
                new Square[]{ Square.UL,  Square.FL,  Square.DL,  Square.BL  }  // Adjacent edges
            };
            faceD = new List<Square[]>() {
                new Square[]{ Square.DFR, Square.DBR, Square.DBL, Square.DFL }, // Face corners
                new Square[]{ Square.DF,  Square.DR,  Square.DB,  Square.DL  }, // Face edges
                new Square[]{ Square.FDL, Square.RDF, Square.BDR, Square.LDB }, // Adjacent corners 1
                new Square[]{ Square.FRD, Square.RDB, Square.BDL, Square.LDF }, // Adjacent corners 2
                new Square[]{ Square.FD,  Square.RD,  Square.BD,  Square.LD  }  // Adjacent edges
            };
            faceB = new List<Square[]>() {
                new Square[]{ Square.BUR, Square.BUL, Square.BDL, Square.BDR }, // Face corners
                new Square[]{ Square.BU,  Square.BL,  Square.BD,  Square.BR  }, // Face edges
                new Square[]{ Square.UBL, Square.LDB, Square.DBR, Square.RUB }, // Adjacent corners 1
                new Square[]{ Square.UBR, Square.LUB, Square.DBL, Square.RDB }, // Adjacent corners 2
                new Square[]{ Square.UB,  Square.LB,  Square.DB,  Square.RB  }  // Adjacent edges
            };

            sliceM = new List<Square[]>() {
                new Square[]{ Square.U,   Square.F,   Square.D,   Square.B   }, // Slice centers
                new Square[]{ Square.UB,  Square.FU,  Square.DF,  Square.BD  }, // Slice edges 1
                new Square[]{ Square.UF,  Square.FD,  Square.DB,  Square.BU  }  // Slice edges 2
            };

            sliceE = new List<Square[]>() {
                new Square[]{ Square.F,   Square.R,   Square.B,   Square.L   }, // Slice centers
                new Square[]{ Square.FL,  Square.RF,  Square.BR,  Square.LB  }, // Slice edges 1
                new Square[]{ Square.FR,  Square.RB,  Square.BL,  Square.LF  }  // Slice edges 2
            };

            sliceS = new List<Square[]>() {
                new Square[]{  Square.U,   Square.R,   Square.D,   Square.L   }, // Slice centers
                new Square[]{  Square.UL,  Square.RU,  Square.DR,  Square.LD  }, // Slice edges 1
                new Square[]{  Square.UR,  Square.RD,  Square.DL,  Square.LU  }  // Slice edges 2
            };
        }


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
            if ( string.IsNullOrEmpty(singleMove))
            {
                return;
            }

            // Right face moves
            if (singleMove == "R")
            {
                cycles.AddRange(faceR);
            }
            else if (singleMove == "R2")
            {
                cycles.AddRange(faceR);
                cycles.AddRange(faceR);
            }
            else if (singleMove == "R'")
            {
                cycles.AddRange(faceR);
                cycles.AddRange(faceR);
                cycles.AddRange(faceR);
            }

            // Left face moves
            else if (singleMove == "L")
            {
                cycles.AddRange(faceL);
            }
            else if (singleMove == "L2")
            {
                cycles.AddRange(faceL);
                cycles.AddRange(faceL);
            }
            else if (singleMove == "L'")
            {
                cycles.AddRange(faceL);
                cycles.AddRange(faceL);
                cycles.AddRange(faceL);
            }

            // Up face moves
            else if (singleMove == "U")
            {
                cycles.AddRange(faceU);
            }
            else if (singleMove == "U2")
            {
                cycles.AddRange(faceU);
                cycles.AddRange(faceU);
            }
            else if (singleMove == "U'")
            {
                cycles.AddRange(faceU);
                cycles.AddRange(faceU);
                cycles.AddRange(faceU);
            }

            // Down face moves
            else if (singleMove == "D")
            {
                cycles.AddRange(faceD);
            }
            else if (singleMove == "D2")
            {
                cycles.AddRange(faceD);
                cycles.AddRange(faceD);
            }
            else if (singleMove == "D'")
            {
                cycles.AddRange(faceD);
                cycles.AddRange(faceD);
                cycles.AddRange(faceD);
            }

            // Front face moves
            else if (singleMove == "F")
            {
                cycles.AddRange(faceF);
            }
            else if (singleMove == "F2")
            {
                cycles.AddRange(faceF);
                cycles.AddRange(faceF);
            }
            else if (singleMove == "F'")
            {
                cycles.AddRange(faceF);
                cycles.AddRange(faceF);
                cycles.AddRange(faceF);
            }

            // Back face moves
            else if (singleMove == "B")
            {
                cycles.AddRange(faceB);
            }
            else if (singleMove == "B2")
            {
                cycles.AddRange(faceB);
                cycles.AddRange(faceB);
            }
            else if (singleMove == "B'")
            {
                cycles.AddRange(faceB);
                cycles.AddRange(faceB);
                cycles.AddRange(faceB);
            }

            // M slice move
            else if (singleMove == "M")
            {
                cycles.AddRange(sliceM);
            }
            else if (singleMove == "M2")
            {
                cycles.AddRange(sliceM);
                cycles.AddRange(sliceM);
            }
            else if (singleMove == "M'")
            {
                cycles.AddRange(sliceM);
                cycles.AddRange(sliceM);
                cycles.AddRange(sliceM);
            }

            // E slice move
            else if (singleMove == "E")
            {
                cycles.AddRange(sliceE);

            }
            else if (singleMove == "E2")
            {
                cycles.AddRange(sliceE);
                cycles.AddRange(sliceE);

            }
            else if (singleMove == "E'")
            {
                cycles.AddRange(sliceE);
                cycles.AddRange(sliceE);
                cycles.AddRange(sliceE);

            }

            // S slice move
            else if (singleMove == "S")
            {
                cycles.AddRange(sliceS);
            }
            else if (singleMove == "S2")
            {
                cycles.AddRange(sliceS);
                cycles.AddRange(sliceS);

            }
            else if (singleMove == "S'")
            {
                cycles.AddRange(sliceS);
                cycles.AddRange(sliceS);
                cycles.AddRange(sliceS);
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
        
    }
}
