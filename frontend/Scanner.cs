using System;
using System.Collections.Generic;

namespace Compiler.frontend
{
    public abstract class Scanner
    {
        protected Source Source;
        public Token CurrentToken { get; private set; }
        private readonly List<Token> _lookahead;
        private int _begin;

        public Scanner(Source source, int k)
        {
            Source = source;
            _lookahead = new List<Token>(k);
            _begin = 0;
            CurrentToken = ExtractToken();
            _lookahead.Add(CurrentToken);
            for (var i = _begin + 1; i < k; ++i)
            {
                _lookahead.Add(ExtractToken());
            }
        }

        public Token NextToken()
        {
            // No special processing for eof, becuase if already reached the end, the following
            // extracted token will keep being "EofToken"
            _lookahead[_begin] = ExtractToken();
            _begin = (_begin + 1)%_lookahead.Count;
            CurrentToken = _lookahead[_begin];

            return CurrentToken;
        }

        public Token Lookahead(int index)
        {
            if (index < 0 || index >= _lookahead.Count)
                throw new IndexOutOfRangeException();

            var idx = (_begin + index)%_lookahead.Count;
            return _lookahead[idx];
        }

        public List<Token> Lookahead(int begin, int end)
        {
            if (begin < 0 || end >= _lookahead.Count || begin >= end)
                throw new IndexOutOfRangeException();

            var ahead = new List<Token>();
            for (var i = begin; i < end; ++i)
            {
                var index = (_begin + i)%_lookahead.Count;
                ahead.Add(_lookahead[index]);
            }

            return ahead;
        }

        protected abstract Token ExtractToken();

        public char CurrentChar()
        {
            return Source.CurrentChar();
        }

        public char NextChar()
        {
            return Source.NextChar();
        }
    }
}