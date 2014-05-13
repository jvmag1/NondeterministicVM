using System;
using System.IO;

namespace NondeterministicVM.BLL
{
    public class VirtualMachine
    {
        private readonly CPU _cpu;
        private readonly Memory _memory;
        private readonly Interpreter _interpreter;

        public VirtualMachine(CPU cpu, Memory memory, Random random, IOutput output)
        {
            _cpu = cpu;
            _memory = memory;
            _interpreter = new Interpreter(_cpu, _memory, random, output);
        }

        public void LoadFromFile(string fileName)
        {
            byte[] buffer = File.ReadAllBytes(fileName);

            uint memAddr = 0;

            for (int i = 0; i < buffer.Length; i += 4)
            {
                var reverseBuffer = new byte[4];

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

                _memory.WriteWord(memAddr, baitas);

                memAddr += 4;
            }
        }

        public void NextStep()
        {
            var instruction = Instruction.FromWord(_memory.ReadWord(_cpu.PC), (uint) _memory.ReadWord(_cpu.PC + sizeof (uint)));

            _interpreter.ExecuteInstruction(instruction);
        }
    }
}
