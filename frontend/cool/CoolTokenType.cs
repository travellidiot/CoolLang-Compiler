﻿using System;
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
        public TokenType CoolType { get; }
        public string Text => CoolType.ToString().ToLower();

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
    }
}
