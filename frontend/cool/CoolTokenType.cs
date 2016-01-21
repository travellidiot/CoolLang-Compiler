using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.frontend.cool
{
    internal enum TokenType
    {
        // Operators
        DArraw,     // "=>"
        Equal,      // "="
        LT,         // "<"
        Assign,     // "<-"
        LE,         // "<="
        GT,         // ">"
        GE,         // ">="
        Slash,      // "/"
        Add,        // "+"
        Minus,      // "-"
        Star,       // "*"
        RC,         // "*)"
        LP,         // "("
        LC,         // "(*"
        RP,         // ")"
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
        Error,
        Comment,
    }

    internal class CoolTokenType : ITokenType
    {
        private readonly TokenType _type;
        public string Text => _type.ToString().ToLower();

        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        #region
        {
            {"=>", TokenType.DArraw},
            {"=", TokenType.Equal},
            {"<", TokenType.LT},
            {"<-", TokenType.Assign},
            {"<=", TokenType.LE},
            {">", TokenType.GT},
            {">=", TokenType.GE},
            {"/", TokenType.Slash},
            {"+", TokenType.Add},
            {"-", TokenType.Minus},
            {"*", TokenType.Star},
            {"*)", TokenType.RC},
            {"(", TokenType.LP},
            {"(*", TokenType.LC},
            {")", TokenType.RP},
            {".", TokenType.Dot},
            {"~", TokenType.Anti},
            {",", TokenType.Comma},
            {";", TokenType.Semic},
            {":", TokenType.Colon},
            {"@", TokenType.Dispatch},
            {"{", TokenType.LB},
            {"}", TokenType.RB}
        };
        #endregion


        internal CoolTokenType(TokenType type)
        {
            this._type = type;
        }

    }
}
