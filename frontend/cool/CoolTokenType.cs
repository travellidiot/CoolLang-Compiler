using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.frontend.cool
{
    public enum TokenType
    {
        // Operators
        DArraw,         // "=>"
        Equal,          // "="
        LessThan,       // "<"
        Assign,         // "<-"
        LessEqual,      // "<="
        GreatThan,      // ">"
        GreatEqual,     // ">="
        Slash,          // "/"
        Add,            // "+"
        Minus,          // "-"
        Star,           // "*"
        RightComment,   // "*)"
        LeftParen,      // "("
        LeftComment,    // "(*"
        RightParen,     // ")"
        Dot,            // "."
        Anti,           // "~"
        Comma,          // ","
        Semic,          // ";"
        Colon,          // ":"
        Dispatch,       // "@"
        LeftBracket,    // "{"
        RightBracket,   // "}"

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

    public class CoolTokenType : ITokenType
    {
        public TokenType CoolType { get; }
        public string Text => CoolType.ToString().ToLower();

        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        #region
        {
            {"=>", TokenType.DArraw},
            {"=", TokenType.Equal},
            {"<", TokenType.LessThan},
            {"<-", TokenType.Assign},
            {"<=", TokenType.LessEqual},
            {">", TokenType.GreatThan},
            {">=", TokenType.GreatEqual},
            {"/", TokenType.Slash},
            {"+", TokenType.Add},
            {"-", TokenType.Minus},
            {"*", TokenType.Star},
            {"*)", TokenType.RightComment},
            {"(", TokenType.LeftParen},
            {"(*", TokenType.LeftComment},
            {")", TokenType.RightParen},
            {".", TokenType.Dot},
            {"~", TokenType.Anti},
            {",", TokenType.Comma},
            {";", TokenType.Semic},
            {":", TokenType.Colon},
            {"@", TokenType.Dispatch},
            {"{", TokenType.LeftBracket},
            {"}", TokenType.RightBracket}
        };
        #endregion

        public static Dictionary<string, TokenType> KeyWords = new Dictionary<string, TokenType>()
        #region
        {
            {"class", TokenType.Class},
            {"else", TokenType.Else},
            {"if", TokenType.If},
            {"fi", TokenType.Fi},
            {"in", TokenType.In},
            {"inherits", TokenType.Inherits},
            {"let", TokenType.Let},
            {"loop", TokenType.Loop},
            {"pool", TokenType.Pool},
            {"then", TokenType.Then},
            {"while", TokenType.While},
            {"case", TokenType.Case},
            {"esac", TokenType.Esac},
            {"of", TokenType.Of},
            {"new", TokenType.New},
            {"isvoid", TokenType.Isvoid},
            {"not", TokenType.Not},
            {"false", TokenType.BoolConst},
            {"true", TokenType.BoolConst}
        };
        #endregion

        internal CoolTokenType(TokenType coolType)
        {
            this.CoolType = coolType;
        }

        public bool Equals(ITokenType other)
        {
            CoolTokenType otherToken = other as CoolTokenType;
            return this.CoolType == otherToken?.CoolType;
        }

        public bool Is(TokenType type)
        {
            return this.CoolType == type;
        }
    }
}
