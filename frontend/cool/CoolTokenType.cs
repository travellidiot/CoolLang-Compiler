using System.Collections.Generic;

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
        Dispatch,       // "."
        Anti,           // "~"
        Comma,          // ","
        Semic,          // ";"
        Colon,          // ":"
        At,             // "@"
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
        // operators
        public static readonly CoolTokenType DArraw         = new CoolTokenType(TokenType.DArraw);
        public static readonly CoolTokenType Equal          = new CoolTokenType(TokenType.Equal);
        public static readonly CoolTokenType LessThan       = new CoolTokenType(TokenType.LessThan);
        public static readonly CoolTokenType Assign         = new CoolTokenType(TokenType.Assign);
        public static readonly CoolTokenType LessEqual      = new CoolTokenType(TokenType.LessEqual);
        public static readonly CoolTokenType GreatThan      = new CoolTokenType(TokenType.GreatThan);
        public static readonly CoolTokenType GreatEqual     = new CoolTokenType(TokenType.GreatEqual);
        public static readonly CoolTokenType Slash          = new CoolTokenType(TokenType.Slash);
        public static readonly CoolTokenType Add            = new CoolTokenType(TokenType.Add);
        public static readonly CoolTokenType Minus          = new CoolTokenType(TokenType.Minus);
        public static readonly CoolTokenType Star           = new CoolTokenType(TokenType.Star);
        public static readonly CoolTokenType RightComment   = new CoolTokenType(TokenType.RightComment);
        public static readonly CoolTokenType LeftParen      = new CoolTokenType(TokenType.LeftParen);
        public static readonly CoolTokenType LeftComment    = new CoolTokenType(TokenType.LeftComment);
        public static readonly CoolTokenType RightParen     = new CoolTokenType(TokenType.RightParen);
        public static readonly CoolTokenType Dispatch       = new CoolTokenType(TokenType.Dispatch);
        public static readonly CoolTokenType Anti           = new CoolTokenType(TokenType.Anti);
        public static readonly CoolTokenType Comma          = new CoolTokenType(TokenType.Comma);
        public static readonly CoolTokenType Semic          = new CoolTokenType(TokenType.Semic);
        public static readonly CoolTokenType Colon          = new CoolTokenType(TokenType.Colon);
        public static readonly CoolTokenType At             = new CoolTokenType(TokenType.At);
        public static readonly CoolTokenType LeftBracket    = new CoolTokenType(TokenType.LeftBracket);
        public static readonly CoolTokenType RightBracket   = new CoolTokenType(TokenType.RightBracket);
        // keywords
        public static readonly CoolTokenType Class          = new CoolTokenType(TokenType.Class);
        public static readonly CoolTokenType Else           = new CoolTokenType(TokenType.Else);
        public static readonly CoolTokenType If             = new CoolTokenType(TokenType.If);
        public static readonly CoolTokenType Fi             = new CoolTokenType(TokenType.Fi);
        public static readonly CoolTokenType In             = new CoolTokenType(TokenType.In);
        public static readonly CoolTokenType Inherits       = new CoolTokenType(TokenType.Inherits);
        public static readonly CoolTokenType Let            = new CoolTokenType(TokenType.Let);
        public static readonly CoolTokenType Loop           = new CoolTokenType(TokenType.Loop);
        public static readonly CoolTokenType Pool           = new CoolTokenType(TokenType.Pool);
        public static readonly CoolTokenType Then           = new CoolTokenType(TokenType.Then);
        public static readonly CoolTokenType While          = new CoolTokenType(TokenType.While);
        public static readonly CoolTokenType Case           = new CoolTokenType(TokenType.Case);
        public static readonly CoolTokenType Esac           = new CoolTokenType(TokenType.Esac);
        public static readonly CoolTokenType Of             = new CoolTokenType(TokenType.Of);
        public static readonly CoolTokenType New            = new CoolTokenType(TokenType.New);
        public static readonly CoolTokenType Isvoid         = new CoolTokenType(TokenType.Isvoid);
        public static readonly CoolTokenType Not            = new CoolTokenType(TokenType.Not);
        //
        public static readonly CoolTokenType BoolConst      = new CoolTokenType(TokenType.BoolConst);
        public static readonly CoolTokenType IntConst       = new CoolTokenType(TokenType.IntConst);
        public static readonly CoolTokenType StringConst    = new CoolTokenType(TokenType.StringConst);
        public static readonly CoolTokenType TypeId         = new CoolTokenType(TokenType.TypeId);
        public static readonly CoolTokenType ObjectId       = new CoolTokenType(TokenType.ObjectId);
        public static readonly CoolTokenType Error          = new CoolTokenType(TokenType.Error);
        public static readonly CoolTokenType Comment        = new CoolTokenType(TokenType.Comment);



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
            {".", TokenType.Dispatch},
            {"~", TokenType.Anti},
            {",", TokenType.Comma},
            {";", TokenType.Semic},
            {":", TokenType.Colon},
            {"@", TokenType.At},
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
            CoolType = coolType;
        }

        public bool Is(TokenType type)
        {
            return CoolType == type;
        }

        public int CompareTo(ITokenType other)
        {
            var otherToken = other as CoolTokenType;
            return CoolType.CompareTo(otherToken?.CoolType);
        }

        public override bool Equals(object obj)
        {
            var otherToken = obj as CoolTokenType;
            return CoolType == otherToken?.CoolType;
        }

        protected bool Equals(CoolTokenType other)
        {
            return CoolType == other.CoolType;
        }

        public override int GetHashCode()
        {
            return (int) CoolType;
        }
    }
}
