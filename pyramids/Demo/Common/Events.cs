using System;
using System.IO;
using System.Threading.Tasks;

namespace Common
{
    public static class Events
    {
        private static TextWriter _file = new Func<TextWriter>(() => 
        {
            File.Delete("log");
            return File.AppendText("log");
        }).Invoke();

        public static void Marker(string line) 
        {
            _file.WriteLine(line);
            _file.Flush();
        }
    }
}