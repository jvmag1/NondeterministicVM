using System;
using System.IO;
using NondeterministicVM.BLL;
using NondeterministicVM.Tests.Helpers;
using NUnit.Framework;

namespace NondeterministicVM.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void TestVM()
        {
            var cpu = new CPU();
            var random = new Random();
            var memory = new Memory(random);

            byte[] buffer = File.ReadAllBytes(@"C:\Users\Julius\Documents\Visual Studio 2013\Projects\NondeterministicVM\NondeterministicVM.Assembler\bin\Debug\pi.hex");            

            uint memAddr = 0;

            for (int i = 0; i < buffer.Length; i += 4)
            {
                byte[] reverseBuffer = new byte[4];

                if (memAddr + 3 < buffer.Length)
                {
                    reverseBuffer[0] = buffer[memAddr + 3];
                }

                if (memAddr + 2 < buffer.Length)
                {
                    reverseBuffer[1] = buffer[memAddr + 2];
                }

                if (memAddr + 1 < buffer.Length)
                {
                    reverseBuffer[2] = buffer[memAddr + 1];
                }

                if (memAddr < buffer.Length)
                {
                    reverseBuffer[3] = buffer[memAddr];
                }

                int baitas = BitConverter.ToInt32(reverseBuffer, 0);

                memory.WriteWord(memAddr, baitas);

                memAddr += 4;
            }

            var consoleOutput = new ConsoleOutput();

            var vm = new VirtualMachine(cpu, memory, random, consoleOutput);

            try
            {
                while (true)
                {
                    vm.NextStep();
                }
            }
            catch (VmHaltException)
            {
            }
        }
    }
}
