using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.message;

namespace Compiler.frontend
{
    public class Source : IMessageProducer
    {
        public static char Eol => '\n';
        public static char Eof => unchecked((char)-1);
        public int LineNum { get; private set; }
        public int CurrentPos { get; private set; }

        public event EventHandler<Message> MessageHandler;
        private readonly StreamReader _reader;
        private string _line;

        public Source(StreamReader reader)
        {
            this._reader = reader;
            LineNum = 0;
            CurrentPos = -2;
        }

        public char CurrentChar()
        {
            // First time
            if (CurrentPos == -2)
            {
                ReadLine();
                return NextChar();
            }
            // At the end of file
            else if (_line == null)
            {
                return Eof;
            }
            // At the end of line
            else if ((CurrentPos == -1) || (CurrentPos == _line.Length))
            {
                return Eol;
            }
            // Need to read the next line
            else if (CurrentPos > _line.Length)
            {
                ReadLine();
                return NextChar();
            }
            // Return the character at the current position.
            else
            {
                return _line[CurrentPos];
            }
        }

        public char NextChar()
        {
            ++CurrentPos;
            return CurrentChar();
        }

        public char PeekChar()
        {
            CurrentChar();
            if (_line == null)
            {
                return Eof;
            }
            var nextPos = CurrentPos + 1;
            return nextPos < _line.Length ? _line[nextPos] : Eol;
        }

        private void ReadLine()
        {
            _line = _reader.ReadLine();
            CurrentPos = -1;

            if (_line != null)
            {
                LineNum++;
            }
        }

        public void Close()
        {
            if (_reader == null) return;
            try
            {
                _reader.Close();
            }
            catch (IOException ex)
            {
                Console.Write(ex.StackTrace);
                throw ex;
            }
        }

        public void SendMessage(Message message)
        {
            MessageHandler?.Invoke(this, message);
        }
    }
}
