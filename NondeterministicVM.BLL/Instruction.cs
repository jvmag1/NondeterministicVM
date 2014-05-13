using System;

namespace NondeterministicVM.BLL
{
    public struct Instruction
    {
        public OpCodes OpCode;
        public int FirstRegister;
        public int SecondRegister;
        public int ThirdRegister;
        public int ExecutionPossibility;
        public uint MemoryAddress;

        public static Instruction FromWord(int firstWord, uint secondWord = 0)
        {
            return new Instruction
            {
                OpCode = (OpCodes)BitConverter.GetBytes(firstWord)[3],
                FirstRegister = BitConverter.GetBytes(firstWord)[2] & 0x0F,
                SecondRegister = (BitConverter.GetBytes(firstWord)[1] & 0xF0) >> 4,
                ThirdRegister = BitConverter.GetBytes(firstWord)[1] & 0x0F,
                ExecutionPossibility = BitConverter.GetBytes(firstWord)[0],
                MemoryAddress = secondWord
            };
        }

        public bool Equals(Instruction other)
        {
            return OpCode == other.OpCode &&
                   FirstRegister == other.FirstRegister &&
                   SecondRegister == other.SecondRegister &&
                   ThirdRegister == other.ThirdRegister &&
                   ExecutionPossibility == other.ExecutionPossibility &&
                   MemoryAddress == other.MemoryAddress;
        }
    }
}
