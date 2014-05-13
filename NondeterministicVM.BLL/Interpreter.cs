using System;
using System.Text;

namespace NondeterministicVM.BLL
{
    public class Interpreter
    {
        private readonly CPU _cpu;
        private readonly Memory _memory;
        private readonly Random _random;
        private readonly IOutput _output;

        public Interpreter(CPU cpu, Memory memory, Random random, IOutput output)
        {
            _cpu = cpu;
            _memory = memory;
            _random = random;
            _output = output;
        }

        public void ExecuteInstruction(Instruction instruction)
        {
            _cpu.AdvancePC();

            if (instruction.OpCode == OpCodes.LoadByMem || instruction.OpCode == OpCodes.Store)
            {
                _cpu.AdvancePC();                
            }

            if (instruction.OpCode == OpCodes.Halt)
            {
                throw new VmHaltException();
            }
            else if ((instruction.ExecutionPossibility == 0) || (_random.Next(0, 100) > instruction.ExecutionPossibility))
            {
                return;
            }

            switch (instruction.OpCode)
            {
                case OpCodes.Add:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] + _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.And:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] & _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.Compare:
                    int subtractionResult = _cpu.R[instruction.FirstRegister] - _cpu.R[instruction.SecondRegister];
                    _cpu.C = (sbyte) Math.Sign(subtractionResult);
                    break;

                case OpCodes.Decrease:
                    _cpu.R[instruction.FirstRegister]--;
                    break;

                case OpCodes.Divide:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] / _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.Increase:
                    _cpu.R[instruction.FirstRegister]++;
                    break;

                case OpCodes.Jump:
                    _cpu.PC = (uint) _cpu.R[instruction.FirstRegister];
                    break;

                case OpCodes.JumpIfEqual:
                    if (_cpu.C == 0)
                    {
                        _cpu.PC = (uint) _cpu.R[instruction.FirstRegister];
                    }
                    break;

                case OpCodes.JumpIfGreater:
                    if (_cpu.C == 1)
                    {
                        _cpu.PC = (uint) _cpu.R[instruction.FirstRegister];
                    }
                    break;

                case OpCodes.JumpIfLess:
                    if (_cpu.C == -1)
                    {
                        _cpu.PC = (uint) _cpu.R[instruction.FirstRegister];
                    }
                    break;

                case OpCodes.JumpIfNotEqual:
                    if (_cpu.C != 0)
                    {
                        _cpu.PC = (uint) _cpu.R[instruction.FirstRegister];
                    }
                    break;

                case OpCodes.LoadByMem:
                    _cpu.R[instruction.FirstRegister] = _memory.ReadWord(instruction.MemoryAddress);
                    break;

                case OpCodes.LoadByReg:
                    _cpu.R[instruction.FirstRegister] = _memory.ReadWord((uint) _cpu.R[instruction.SecondRegister]);
                    break;

                case OpCodes.Modulo:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] % _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.Multiply:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] * _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.Negate:
                    _cpu.R[instruction.FirstRegister] = ~ _cpu.R[instruction.SecondRegister];
                    break;

                case OpCodes.Or:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] | _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.PrintNumber:
                    _output.Write(_cpu.R[instruction.FirstRegister].ToString());
                    break;

                case OpCodes.PrintString:
                    var bytes = BitConverter.GetBytes(_cpu.R[instruction.FirstRegister]);
                    Array.Reverse(bytes);
                    var text = Encoding.ASCII.GetString(bytes);
                    _output.Write(text);
                    break;

                case OpCodes.RandomizedAdd:
                    _cpu.R[instruction.FirstRegister] = Randomize(_cpu.R[instruction.SecondRegister] + _cpu.R[instruction.ThirdRegister]);                    
                    break;

                case OpCodes.RandomizedDivide:
                    _cpu.R[instruction.FirstRegister] = Randomize(_cpu.R[instruction.SecondRegister] / _cpu.R[instruction.ThirdRegister]);                    
                    break;

                case OpCodes.RandomizedModulo:
                    _cpu.R[instruction.FirstRegister] = Randomize(_cpu.R[instruction.SecondRegister] % _cpu.R[instruction.ThirdRegister]);                    
                    break;

                case OpCodes.RandomizedMultiply:
                    _cpu.R[instruction.FirstRegister] = Randomize(_cpu.R[instruction.SecondRegister] * _cpu.R[instruction.ThirdRegister]);                    
                    break;

                case OpCodes.RandomizedSubtract:
                    _cpu.R[instruction.FirstRegister] = Randomize(_cpu.R[instruction.SecondRegister] - _cpu.R[instruction.ThirdRegister]);
                    break;

                case OpCodes.Store:
                    _memory.WriteWord(instruction.MemoryAddress, _cpu.R[instruction.FirstRegister]);
                    break;

                case OpCodes.Subtract:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] - _cpu.R[instruction.ThirdRegister];
                    break;

                case OpCodes.Xor:
                    _cpu.R[instruction.FirstRegister] = _cpu.R[instruction.SecondRegister] ^ _cpu.R[instruction.ThirdRegister];
                    break;                    
            }
        }

        private int Randomize(int value)
        {
            int percentOfValue = value / 100;

            value += _random.Next(-percentOfValue, +percentOfValue);

            return value;
        }
    }
}
