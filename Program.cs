using System;
using MioLang.InputSource;
namespace MioLang
{
    class Program
    {
        InputStream stream;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello MioLang!");
        }
        void runtest(string[] args)
        {
            var InputStream = new InputStream();
            foreach (var filedata in args)
            {
                InputStream.AddSourceFile(filedata);
            }
        }
    }
}
