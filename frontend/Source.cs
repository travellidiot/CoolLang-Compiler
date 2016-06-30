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

        private readonly List<IMessageListener> _listeners = new List<IMessageListener>();
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
            if (_line == null)
            {
                return Eof;
            }
            // At the end of line
            if ((CurrentPos == -1) || (CurrentPos == _line.Length))
            {
                return Eol;
            }
            // Need to read the next line
            if (CurrentPos > _line.Length)
            {
                ReadLine();
                return NextChar();
            }
            // Return the character at the current position.
            return _line[CurrentPos];
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

        public void AddListener(IMessageListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IMessageListener listener)
        {
            _listeners.Remove(listener);
        }

        public void SendMessage(Message message)
        {
            foreach (var listener in _listeners)
            {
                listener.MessageReceived(message);
            }
        }
    }
}
