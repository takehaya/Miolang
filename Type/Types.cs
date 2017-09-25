using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Linq;
using MioLang.Utils;

namespace MioLang.Type
{
    /// <summary>
    /// type of abstract class
    /// 構文木を構成する項を定義
    /// </summary>
    abstract class MType{}

    class MTypeVar:MType
    {
        static int VarID = 0;
        public int Id {get; private set;}
        public MType Var{get; set;}

        public MTypeVar(){
            Id = VarID++;
            Var = null;
        }
        public override string ToString() => string.Format("<{0}>",Id);
    }

    class MIntType:MType
    {
        public static MType type = new MIntType();
        public static MType Type => type;

        public override string ToString() => "Int";
    }

    class MFunType : MType{
        public MType ArgType {get; private set;}
        public MType RetType {get; private set;}

        public MFunType(MType argtype, MType rettype){
            ArgType = argtype;
            RetType = rettype;
        }
        public override string ToString() => string.Format("({0}->{1})",ArgType,RetType);
    }
}