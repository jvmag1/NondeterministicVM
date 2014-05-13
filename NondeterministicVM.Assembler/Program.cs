using System;
using System.IO;

namespace NondeterministicVM.Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: {0} <input_file_name> <output_file_name>", System.AppDomain.CurrentDomain.FriendlyName);

                Environment.Exit(1);
            }

            try
            {
                using (var compiler = new AsmFileCompiler(args[0], args[1]))
                {
                    compiler.Compile();
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File {0} was not found", args[0]);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
            }
        }
    }
}
