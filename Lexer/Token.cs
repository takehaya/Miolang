namespace MioLang.Lexer
{
    
    public class Token{
        public TokenType Type{get; set;}
        public int IntVal{get; private set;}
        public char CharVal{get;private set;}
        public double DoubleVal{get;private set;}
        public string StrVal{get; private set;}
        public string Pos {get; private set;}

        public Token(TokenType _type) {
            Type = _type;
        }
        public Token(string _pos,TokenType _type){
            Pos = _pos;
            Type = _type;
        }
        public Token(string _pos,TokenType _type,int _intVal):this(_pos,_type){
            IntVal = _intVal;
        }
         public Token(string _pos,TokenType _type,string _strVal):this(_pos,_type){
            StrVal = _strVal;
        }
         public Token(string _pos,TokenType _type,char _charVal):this(_pos,_type){
            CharVal = _charVal;
        }
        public Token(string _pos,TokenType _type,double _doubleVal):this(_pos,_type){
            DoubleVal = _doubleVal;
        }
    }
    public enum TokenType
    {
        ID, INT, DBL, STR, CHAR,

        LP, RP,

        PLS, MNS, AST, SLS,

        BS, ARROW, EQ, EQEQ,

        COM, BAR, COL, SMC, LT, GT ,QUE, LBK, RBK,

        //特定キーワード
        TRUE, FALSE,
        TYPE, AND, DO,
        PRINT, LET, FUN,
        IN, AS,
        IF, THEN, ELSE, GUADE, REC,

        //終端記号
        EOF
    }
}