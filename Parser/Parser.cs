using System;
using System.Linq.Expressions;
using MioLang.Lexing;
using MioLang.Utils;
using MioLang.AST;

namespace MioLang.Parsing
{
    static class Parser
    {
        /// <summary>
        /// Parse the specified context
        /// evaluation is method
        /// expr->func（formula）->cmp->add(sub)->mul(div)->app->facter
        /// </summary>
        /// <param name="ctx">ParserContext.</param>
        /// <returns></returns>
        public static SExpr Parse(ParseContext context)
        {
            SExpr expr = NewMethod(context);
            context.ReadToken(TokenType.EOF);
            return expr;
        }

        private static SExpr NewMethod(ParseContext context)
        {
            return ParseExpr(context);
        }

        static SExpr ParseExpr(ParseContext context)
        {
            return ParseFunExpr(context);
        }
        static SExpr ParseFunExpr(ParseContext context)
        {
            if(context.TokenData.Type == TokenType.BS){
                context.ReadToken(TokenType.BS);
                var arg_name = context.ReadStrToken(TokenType.ID);
                context.ReadToken(TokenType.ARROW);
                var body = ParseFunExpr(context);
                return new SFunc(arg_name, body);
            }else if(context.TokenData.Type == TokenType.IF){
                context.ReadToken(TokenType.IF);
                var cond_expr = ParseFunExpr(context);
                context.ReadToken(TokenType.THEN);
                var then_expr = ParseFunExpr(context);
                context.ReadToken(TokenType.ELSE);
                var else_expr = ParseFunExpr(context);
                return new SIf(cond_expr, then_expr, else_expr);
            }else if(context.TokenData.Type == TokenType.LET){
                context.ReadToken(TokenType.LET);
                var var_name = context.ReadStrToken(TokenType.ID);
                context.ReadToken(TokenType.EQ);
                var e1 = ParseFunExpr(context);
                context.ReadToken(TokenType.IN);
                var e2 = ParseFunExpr(context);
                return new SLet(var_name, e1, e2);
            }else if(context.TokenData.Type ==TokenType.REC){
                context.ReadToken(TokenType.REC);
                var var_name = context.ReadStrToken(TokenType.ID);
                context.ReadToken(TokenType.EQ);
                var e1 = ParseFunExpr(context);
                context.ReadToken(TokenType.IN);
                var e2 = ParseFunExpr(context);
                return new SRec(var_name, e1, e2);
            }else{
                return ParseCmpExpr(context);
            }
        }
        static SExpr ParseCmpExpr(ParseContext context)
        {
            var lhs = ParseAddExpr(context);

            while(context.TokenData.Type == TokenType.EQEQ)
            {
                var type = context.TokenData.Type;
                context.NextToken();
                var rhs = ParseAddExpr(context);
                if(type == TokenType.EQEQ){
                    lhs = new SEq(lhs, rhs);
                    break;
                }
            }

            return lhs;
        }
        static SExpr ParseAddExpr(ParseContext context)
        {
            var lhs = ParseMulExpr(context);
            while((context.TokenData.Type == TokenType.PLS)||
                  (context.TokenData.Type == TokenType.MNS)){
                var type = context.TokenData.Type;
                context.NextToken();
                var rhs = ParseMulExpr(context);
                if (type == TokenType.PLS)
                {
                    lhs = new SAdd(lhs, rhs);
                    break;
                }else if(type == TokenType.MNS){
                    lhs = new SSub(lhs, rhs);
                    break;
                }
            }
            return lhs;
        }
        static SExpr ParseMulExpr(ParseContext context)
        {
            var lhs = ParseAppExpr(context);
            while((context.TokenData.Type == TokenType.AST)||
                  (context.TokenData.Type == TokenType.SLS)){
                var type = context.TokenData.Type;
                context.NextToken();
                var rhs = ParseAppExpr(context);
                if(type == TokenType.AST)
                {
                    lhs = new SMul(lhs, rhs);
                    break;
                }else if(type == TokenType.SLS){
                    lhs = new SDiv(lhs, rhs);
                    break;
                }
            }
            return lhs;
        }
        static SExpr ParseAppExpr(ParseContext context)
        {
            if (context.TokenData.Type == TokenType.PRINT) 
            {
                context.ReadToken(TokenType.PRINT);
                var args = ParseFactor(context);
                return new SPrint(args);
            }else
            {
                var lhs = ParseFactor(context);

                while ((context.TokenData.Type == TokenType.LP) ||
                       (context.TokenData.Type == TokenType.ID) ||
                       (context.TokenData.Type == TokenType.INT))
                {

                    if (context.TokenData.Type == TokenType.PRINT)
                    {
                        context.ReadToken(TokenType.PRINT);
                        var args = ParseFactor(context);
                        lhs = new SPrint(args);
                    }
                    else
                    {
                        var rhs = ParseFactor(context);
                        lhs = new SApp(lhs, rhs);
                    }
                }
                return lhs;
            }
        }
        static SExpr ParseFactor(ParseContext context)
        {
            if (context.TokenData.Type == TokenType.LP)
            {
                context.ReadToken(TokenType.LP);
                var expr = ParseExpr(context);
                context.ReadToken(TokenType.RP);
                return expr;
            }else if (context.TokenData.Type == TokenType.INT)
            {
                var valuedata = context.ReadIntToken(TokenType.INT);
                return new SInt(valuedata);
            }else if (context.TokenData.Type == TokenType.ID)
            {
                var name = context.ReadStrToken(TokenType.ID);
                return new SVar(name);
            }else {
                context.SyntaxError();
                return null;
            }
        }
    }
}