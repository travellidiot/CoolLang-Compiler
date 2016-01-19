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
        Error,
    }

    internal class CoolTokenType : ITokenType
    {
        private readonly TokenType _type;
        public string Text => _type.ToString().ToLower();

        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        #region
        {
            {"=>", TokenType.DArraw},
            {"<-", TokenType.Assign},
            {"<=", TokenType.LE},
            {">=", TokenType.GE},
            {"/", TokenType.Slash},
            {"+", TokenType.Add},
            {"-", TokenType.Minus},
            {"*", TokenType.Star},
            {"(", TokenType.Lp},
            {")", TokenType.Rp},
            {"=", TokenType.Equal},
            {"<", TokenType.LT},
            {">", TokenType.GT},
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
