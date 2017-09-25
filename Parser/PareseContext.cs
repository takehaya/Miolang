using System;
using MioLang.Lexing;
using MioLang.Utils;
namespace MioLang.Parsing
{
    class ParseContext
    {
        Lexer lexer;
        public Token TokenData{get; private set;}

        public ParseContext(Lexer _lexer){
            lexer = _lexer;
            NextToken();
        }

        public void NextToken(){
            TokenData = lexer.NextToken();
        }
        void EnsureToken(TokenType type){
            if (TokenData.Type  != type)
            {
                SyntaxError();
            }
        }

        public void ReadToken(TokenType type){
            EnsureToken(type);
			NextToken();
        }
        public int ReadIntToken(TokenType type)
		{
			EnsureToken(type);
			NextToken();
            return TokenData.IntVal;
		}
        public string ReadStrToken(TokenType type)
		{
			EnsureToken(type);
			NextToken();
            return TokenData.StrVal;
		}
        public void SyntaxError(){
            throw new Error(string.Format("{0}:構文エラー", lexer.Pos));
        }
    }
}