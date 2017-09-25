using System;
using MioLang.InputSource;
using MioLang.Lexing;
using MioLang.Parsing;
namespace MioLang
{
    class Program
    {
        InputStream stream;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello MioLang!");
            runtest(args);
        }
        static void runtest(string[] args)
        {
            Console.WriteLine(args);
            var inputStream = new InputStream();
            foreach (var filedata in args)
            {
                inputStream.AddSourceFile(filedata);
            }

            var lex = new Lexing.Lexer(inputStream);
            var par = new ParseContext(lex);
            var expr = Parser.Parse(par);

            Console.WriteLine(expr);
        }
    }
}
