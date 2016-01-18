using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.frontend.cool
{
    enum TokenType
    {
        // Operators
        DArraw,      // "=>"
        Assign,     // "<-"
        LE,         // "<="
        GE,         // ">="
        Slash,      // "/"
        Add,        // "+"
        Minus,      // "-"
        Star,       // "*"
        Lp,         // "("
        Rp,         // ")"
        Equal,      // "="
        LT,         // "<"
        GT,         // ">"
        Dot,        // "."
        Anti,       // "~"
        Comma,      // ","
        Semic,      // ";"
        Colon,      // ":"
        Dispatch,   // "@"
        LB,         // "{"
        RB,         // "}"

        // Keywords
        Class,
        Else,
        If,
        Fi,
        In,
        Inherits,
        Let,
        Loop,
        Pool,
        Then,
        While,
        Case,
        Esac,
        Of,
        New,
        Isvoid,
        Not,
        
        //
        BoolConst,
        IntConst,
        StringConst,
        TypeId,
        ObjectId,
    }

    class CoolTokenType
    {
        private readonly TokenType _type;
        public string Text => _type.ToString().ToLower();

        public static HashSet<string> ReservedWords = new HashSet<string>(
            Enum.GetValues(typeof (TokenType))
                .Cast<TokenType>()
                .Where(token => token <= TokenType.Class && token >= TokenType.Not)
                .Select(token => token.ToString()));


        public static Dictionary<string, TokenType> SpecialWords =
            Enum.GetValues(typeof (TokenType))
                .Cast<TokenType>()
                .Where(token => token >= TokenType.DArraw && token <= TokenType.RB)
                .ToDictionary(token => token.ToString().ToLower());


        CoolTokenType(TokenType type)
        {
            this._type = type;
        }

    }
}
