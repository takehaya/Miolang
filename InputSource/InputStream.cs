using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using MioLang.Utils;

namespace MioLang.InputSource
{
    /// <summary>
    /// Input Stream.
    /// sourcefile bundle
    /// </summary>
    public class InputStream
    {
        Queue<SourceFile> sourceQueue;
        Stack<SourceFile> sourceStack;
        SourceFile currentSrcfile;

        //実行パスの取得
        static string execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        int ch = -1;

        public InputStream(){
            sourceQueue = new Queue<SourceFile>();
            sourceStack = new Stack<SourceFile>();
        }

         public void AddSourceFile(string fname){
            if (File.Exists(fname))
            {
                var file = new FileStream(fname,FileMode.Open);
                var tr = new StreamReader(file);
                var srcfile = new SourceFile(tr,fname);
                sourceQueue.Enqueue(srcfile);
            }else
            {
                throw new Error("sourcefile："+fname+"not found :(");
            }
        }
        public void IncludeSourceFile(string fname){
            if (File.Exists(fname))
            {
                sourceStack.Push(currentSrcfile);
                var file = new FileStream(fname,FileMode.Open);
                var tr = new StreamReader(file);
                currentSrcfile = new SourceFile(tr,fname);
            }
            var path = Path.Combine(execPath,fname);
            if(File.Exists(path)){
                sourceStack.Push(currentSrcfile);
                var file = new FileStream(fname,FileMode.Open);
                var tr = new StreamReader(file);
            }else{
                throw new Error("sourcefile："+fname+"not found :(");
            }
        }

        public char Char => (ch == -1 ? 
            throw new Error("SyntaxError -- Unexpected end of file")
            :(char)ch
        );
      
        public string Pos => currentSrcfile.ToString();
        public bool IsEof() => ch == -1;
        public bool IsDigit() => !IsEof() && char.IsDigit(Char);
        public bool IsWhiteSpace() => !IsEof() && char.IsWhiteSpace(Char);
        public bool IsIdStartChar() => !IsEof() && char.IsLetter(Char);
        public bool IsIdChar() => !IsEof() && char.IsLetterOrDigit(Char);
        
        public void NextChar(){
            while (true)
            {
                if (currentSrcfile != null)
                {
                    ch = currentSrcfile.Read();
                }
                if (ch == -1)
                {
                    //read on new filedata,keep to continue
                    if (0<sourceStack.Count)
                    {
                        currentSrcfile = sourceStack.Pop();
                        continue;
                    }else if (0<sourceQueue.Count){
                        currentSrcfile = sourceStack.Pop();
                        continue;
                    }else{
                        break;
                    }
                }else{
                    break;
                }
            }
        }
        public void SkipSpace(){
            while (IsWhiteSpace())
            {
                NextChar();
            }
        }
    }
}