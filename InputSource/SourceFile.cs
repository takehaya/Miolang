using System;
using System.IO;
namespace MioLang.InputSource
{
    /// <summary>
    /// Source Files
    /// オープンしたファイルから読み出す
    /// </summary>
    public class SourceFile
    {
        string name;
        int line;
        TextReader fileReader;
        
        public SourceFile(TextReader _tr,string _name){
            name = _name;
            line = 1;
            fileReader = _tr;

        }
        public int Read(){
            var ch = fileReader.Read();
            if (ch == '\n')
            {
                line++;
            }
            return ch;
        }

        public void Close(){
            fileReader.Dispose();
        }

        public override string ToString(){
            return string.Format("{0}{1}",name,line);
        }
       }
}