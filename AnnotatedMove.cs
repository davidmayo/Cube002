using System;
using System.Collections.Generic;
using System.Text;

namespace Cube002
{
    class AnnotatedMove
    {
        public Move move;
        public string caption;
        public string detailedCaption;


        public AnnotatedMove( Move move, string caption, string detailedCaption)
        {
            this.move = move;
            this.caption = caption;
            this.detailedCaption = detailedCaption;
        }

        public AnnotatedMove(Move move, string caption) : this(move, caption, caption)
        {
        }

        public AnnotatedMove( Move move ) : this( move, "", "" )
        {
        }

        public AnnotatedMove() : this(new Move(""), "", "")
        {
        }
    }
}
