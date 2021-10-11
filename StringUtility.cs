using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class StringUtility
    {
        private static readonly char HORIZONTAL_SEP = '│';
        private static readonly char VERTICAL_SEP = '─';

        private static readonly char TOP_LEFT  = '┌';
        private static readonly char TOP_RIGHT = '┐';

        private static readonly char BOT_LEFT  = '└';
        private static readonly char BOT_RIGHT = '┘';

        private static readonly char TWO_WAY_TOP = '┬';
        private static readonly char TWO_WAY_BOT = '┴';
        private static readonly char TWO_WAY_LEFT = '├';
        private static readonly char TWO_WAY_RIGHT = '┤';

        private static readonly char FOUR_WAY = '┼';

        public static string MakeGrid(params string[] strings)
        {
            int blockCount = strings.Length;
            int maxWidth = Console.WindowWidth;

            for( int rows = 1; rows <= strings.Length; rows++)
            {
                int cols = (int)Math.Ceiling((double)blockCount / rows);
                int size = rows * cols;

                string[] copy = new string[size];
                for( int i = 0; i < size; i++ )
                {
                    copy[i] = "";
                }

                Array.Copy(strings, copy, strings.Length);

                string candidate = MakeGrid(rows, cols, 1, copy);

                int width = candidate.IndexOf('\n');

                if( width <= maxWidth )
                {
                    return candidate;
                }
            }
            return "";
        }

        public static string MakeGrid(int rows, int cols, int padding, params string[] strings)
        {
            int count = strings.Length;

            if( count != rows * cols )
            {
                throw new Exception();
                // Throw an error
            }

            string rv = "";

            List<int> columnWidths = new List<int>(cols);
            for (int col = 0; col < cols; col++)
                columnWidths.Add(0);

            List<int> rowHeights = new List<int>(rows);
            for (int row = 0; row < rows; row++)
                rowHeights.Add(0);


            // Find the widths for each column
            // and heights for each row
            // By iterating over each string
            for( int row = 0; row < rows; row++)
            {
                for( int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;

                    string thisBlock = strings[index];

                    var size = DetermineSize(thisBlock);

                    int thisBlockRows = size.Item1;
                    int thisBlockCols = size.Item2 + 2*padding;

                    if (thisBlockRows > rowHeights[row])
                        rowHeights[row] = thisBlockRows;

                    if (thisBlockCols > columnWidths[col])
                        columnWidths[col] = thisBlockCols;
                }
            }

            rv += GenerateSeparatorLine(columnWidths, TOP_LEFT, TWO_WAY_TOP, TOP_RIGHT);

            rv += '\n';


            // Iterate over the blocks and print them
            for ( int gridRow = 0; gridRow < rows; gridRow++)
            {
                // Iterate over all block rows
                for( int blockRowIndex = 0; blockRowIndex < rowHeights[gridRow]; blockRowIndex++ )
                {
                    // Print top of block items
                    rv += HORIZONTAL_SEP;
                    //rv += GenerateSeparatorLine(columnWidths);


                    // Print main grid content
                    for (int gridCol = 0; gridCol < cols; gridCol++)
                    {
                        int index = gridRow * cols + gridCol;
                        string block = strings[index];

                        var blockRows = block.Split('\n');

                        if( blockRowIndex < blockRows.Length )
                        {
                            rv += (new string(' ', padding)+blockRows[blockRowIndex]).PadRight(columnWidths[gridCol]);
                            rv += HORIZONTAL_SEP;
                        }
                        else
                        {
                            rv += (new string(' ', padding) + "").PadRight(columnWidths[gridCol]);
                            rv += HORIZONTAL_SEP;

                        }
                    }
                    rv += "\n";

                }
                //Print bottom of block content
                if (gridRow != rows - 1)
                {
                    rv += GenerateSeparatorLine(columnWidths, TWO_WAY_LEFT, FOUR_WAY, TWO_WAY_RIGHT);
                    rv += "\n";
                }
                else
                {
                    rv += GenerateSeparatorLine(columnWidths, BOT_LEFT, TWO_WAY_BOT, BOT_RIGHT);
                    rv += "\n";
                }
            }
            ;
            return rv;
        }

        private static string GenerateSeparatorLine(List<int> columnWidths, char leftEndChar, char middleChar, char rightEndChar)
        {
            string rv = "";
            rv += leftEndChar;
            for( int index = 0; index < columnWidths.Count; index++ )
            {
                rv += new string(VERTICAL_SEP, columnWidths[index]);

                if( index != columnWidths.Count - 1)
                {
                    rv += middleChar;
                }
                else
                {
                    rv += rightEndChar;
                }
            }
            return rv;
        }

        public static (int,int) DetermineSize(string str)
        {
            if( string.IsNullOrEmpty(str))
            {
                return (0, 0);
            }
            int longestRowLength = 0;
            int row = 1;
            int col = 0;

            foreach( char ch in str.ToCharArray() )
            {
                if( ch == '\n')
                {
                    row++;
                    //if (col > longestRowLength)
                    //    longestRowLength = col;
                    col = 0;
                }
                else
                {
                    col++;
                }
                if (col > longestRowLength)
                    longestRowLength = col;
            }
            return (row, longestRowLength);
        }
    }
}
