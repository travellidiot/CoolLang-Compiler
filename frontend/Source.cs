using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compiler.frontend
{
    public class Source
    {
        static char EOL { get { return '\n'; } }
        static char EOF { get { return '\0'; } }

        private StreamReader reader;
        private string line;
        private int lineNum = 0;
        private int currentPos = -2;

        public Source(StreamReader reader)
        {
            this.reader = reader;
        }

        public char currentChar()
        {
            if (currentPos == -2)
            {
                readLine();
                return nextChar();
            }
            else if (line == null)
            {
                return EOF;
            }
            else if ((currentPos == -1) || (currentPos == line.Length))
            {
                return EOL;
            }
            else if (currentPos > line.Length)
            {
                readLine();
                return nextChar();
            }
            else
            {
                return line[currentPos];
            }
        }

        public char nextChar()
        {
            ++currentPos;
            return currentChar();
        }

        public char peekChar()
        {
            currentChar();
            if (line == null)
            {
                return EOF;
            }
            int nextPos = currentPos + 1;
            return nextPos < line.Length ? line[nextPos] : EOL;
        }

        private void readLine()
        {
            line = reader.ReadLine();
            currentPos = -1;

            if (line != null)
            {
                lineNum++;
            }
        }

        private void close()
        {
            if (reader != null)
            {
                try
                {
                    reader.Close();
                }
                catch (IOException ex)
                {
                    Console.Write(ex.StackTrace);
                    throw ex;
                }
            }
        }
    }
}
