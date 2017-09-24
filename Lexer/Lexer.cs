using System;
using System.Collections.Generic;
using System.Text;
using MioLang.InputSource;
using MioLang.Utils;

namespace MioLang.Lexer
{
    /// <summary>
    /// Lexer.
    /// </summary>
    public class Lexer
    {
        InputSource stream;
        Dictionary<string, TokenType> keywords;
        Dictionary<char, TokenType> symbols;

        public string Pos => stream.Pos;

        public Lexer(InputStream _stream){
            stream = _stream;
            InitSynbols();
            InitKeywords();
            stream.NextChar();
        }
        public Token NextToken(){
            stream.SkipSpace();

            if (stream.IsEof())
            {
                return new Token(TokenType.EOF);
            }else if(stream.IsDigit())
            {
                return NextNumToken();
            }else if(stream.IsIdStartChar()){
                return NextStrToken();
            }else{
                return NextSynbolToken();
            }
        }
        Token NextNumToken(){
            var sb = new StringBuilder();
            while(stream.IsDigit()){
                sb.Append(stream.Char);
                stream.NextChar();
            }
            return new Token(Pos,TokenType.INT,int.Parse(sb.ToString()));
        }

        Token NextStrToken(){
            var sb = new StringBuilder();
            while(stream.IsIdChar()){
                sb.Append(stream.Char);
                stream.NextChar();
            }
            var str = sb.ToString();
            if (keywords.ContainsKey(str))
            {
                return new Token(keywords[str]);
            }else{
                return new Token(str,TokenType.ID);
            }
        }
        Token NextSynbolToken(){
            if (symbols.ContainsKey(stream.Char))
            {
                var type = symbols[stream.Char];
                stream.NextChar();
                return new Token(type);
            }else
            {
                switch (stream.Char)
                {
                    case '-':
                        stream.NextChar();
                        if (stream.Char == '>')
                        {
                            stream.NextChar();
                            return new Token(TokenType.ARROW);
                        }else
                        {
                            return new Token(TokenType.MNS);
                        }
                    case '=':
                        stream.NextChar();
                        if(stream.Char == '='){
                            stream.NextChar();
                            return new Token(TokenType.EQEQ);
                        }else
                        {
                            return new Token(TokenType.EQ);
                        }
                    default:
                        throw new Error("Syntaxs error");
                }
            }
        }
        void InitKeywords()
        {
            keywords = new Dictionary<string, TokenType>()
            {
                {"print", TokenType.PRINT},

                {"if", TokenType.IF},
                {"then", TokenType.THEN},
                {"else", TokenType.ELSE},
                {"guard", TokenType.GUADE},

                {"true", TokenType.TRUE},
                {"false", TokenType.FALSE},

                {"let", TokenType.LET},
                {"fun", TokenType.FUN},
                {"in", TokenType.IN},
                {"as", TokenType.AS},
                {"rec", TokenType.REC},
                { "type", TokenType.TYPE },
                { "and", TokenType.AND },
                { "do", TokenType.DO },
            };
        }
        void InitSynbols()
        {
            symbols = new Dictionary<char, TokenType>()
            {
                {'(', TokenType.LP},
                {')', TokenType.RP},

                {'+', TokenType.PLS},
                //.MNSはアローと比較の時に生成するので除外
                {'=', TokenType.EQ},
                {'*', TokenType.AST},
                {'/', TokenType.SLS},

                {'\\', TokenType.BS},
                {'[', TokenType.LBK},
                {']', TokenType.RBK},
                {'?', TokenType.QUE},

                {'<', TokenType.LT},
                {'>', TokenType.GT},
                {'|', TokenType.BAR},
                {'&', TokenType.AND},

                {',', TokenType.COM},
                {':', TokenType.COL},
                {';', TokenType.SMC},
            };
        }
    }
}