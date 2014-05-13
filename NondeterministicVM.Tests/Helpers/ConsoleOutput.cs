using System;
using NondeterministicVM.BLL;

namespace NondeterministicVM.Tests.Helpers
{
    public class ConsoleOutput : IOutput
    {
        public void Write(string text)
        {
            Console.Write(text);
        }
    }
}
