using CompilerLib;

namespace CompilerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string source = File.ReadAllText(@"text.txt");
            foreach (Token item in new TokenReader(source).ReadAll())
            {
                Console.WriteLine(item);
            }
        }
    }
}
