using System;
using System.Text;
using MioLang.Utils;

namespace MioLang.AST
{
    abstract class SExpr
    {
        public string Pos {get; set;}
    }

    /// <summary>
    /// 定数
    /// </summary>
    class SInt:SExpr
    {
        public int Value{get;set;}

        public SInt(int value){
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }

    /// <summary>
    /// 二項演算子
    /// </summary>
    abstract class SBinop : SExpr
    {
        public SExpr Lhs{ get; private set;}
        public SExpr Rhs{ get; private set;}
        public SBinop(SExpr lhs, SExpr rhs){
            Lhs = lhs;
        }
    }
    class SAdd:SBinop
    {
        public SAdd(SExpr lhs,SExpr rhs):base(lhs,rhs){}

        public override string ToString() => string.Format("({0}+{1})",Lhs,Rhs);
    }

    class SSub:SBinop
    {
        public SSub(SExpr lhs, SExpr rhs):base(lhs,rhs){}

        public override string ToString()=>string.Format("({0}-{1})",Lhs,Rhs);
    }

    class SMul:SBinop
    {
        public SMul(SExpr lhs,SExpr rhs):base(lhs,rhs){}

        public override string ToString()=>string.Format("({0}*{1})",Lhs,Rhs);
    }

    class SDiv:SBinop
    {
        public SDiv(SExpr lhs, SExpr rhs):base(lhs,rhs){}
        public override string ToString()=>string.Format("({0}/{1})",Lhs,Rhs);
    }
    class SEq:SBinop
    {
        public SEq(SExpr lhs,SExpr rhs):base(lhs,rhs){}
        public override string ToString()=>string.Format("({0}=={1})",Lhs,Rhs);       
    }

    class SVar:SExpr{
        public string VarName{get;private set;}
        
        public SVar(string name){
            VarName = name;
        }

        public override string ToString() =>string.Format("({0})",VarName);
    }

    class SFunc :SExpr
    {
        public string ArgName{get; private set;}
        public SExpr Body{get; private set;}

        public SFunc(string argname, SExpr body){
            ArgName = argname;
            Body = body;
        }

        public override string ToString()=>string.Format("(\\{0}->{1})",ArgName,Body);
    }

    class SApp:SExpr{
        public SExpr FuncExpr{get; private set;}
        public SExpr ArgExpr{get; private set;}

        public SApp(SExpr func_expr,SExpr arg_expr){
            FuncExpr = func_expr;
            ArgExpr = arg_expr;
        }
        public override string ToString() => string.Format("({0}{1})",FuncExpr,ArgExpr);
    }

    class SLet:SExpr{
        public string VarName{get; private set;}
        public SExpr E1 {get; private set;}
        public SExpr E2 {get; private set;}

        public SLet(string var_name,SExpr e1, SExpr e2){
            VarName = var_name;
            E1 = e1;
            E2 = e2;
        }

        public override string ToString() => string.Format("(let {0}={1}in{2})",VarName,E1,E2);
    }

    class SRec:SExpr{
          public string VarName{get; private set;}
        public SExpr E1 {get; private set;}
        public SExpr E2 {get; private set;}

        public SRec(string var_name,SExpr e1, SExpr e2){
            VarName = var_name;
            E1 = e1;
            E2 = e2;
        }

        public override string ToString() => string.Format("(rec{0}:{1}in{2})",VarName,E1,E2);

    }

   
    /// <summary>
    /// IF式
    /// </summary>
    class SIf : SExpr
    {
        public SExpr CondExpr { get; private set; }
        public SExpr ThenExpr { get; private set; }
        public SExpr ElseExpr { get; private set; }

        public SIf(SExpr cond_expr, SExpr then_expr, SExpr else_expr)
        {
            CondExpr = cond_expr;
            ThenExpr = then_expr;
            ElseExpr = else_expr;
        }

        public override string ToString()
        {
            return string.Format("(if {0} then {1} then {2})", CondExpr, ThenExpr, ElseExpr);
        }
    }

    /// <summary>
    /// print expr
    /// </summary>
    class SPrint : SExpr
    {
        public SExpr Body { get; private set; }

        public SPrint(SExpr body)
        {
            Body = body;
        }

        public override string ToString()
        {
            return string.Format("(print {0})", Body);
        }
    }


}