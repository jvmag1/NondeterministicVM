using System;
using NondeterministicVM.BLL;
using NondeterministicVM.Tests.Helpers;
using NUnit.Framework;

namespace NondeterministicVM.Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        [Test]
        public void TestComparison()
        {
            // Arrange
            var cpu = new CPU();

            cpu.C = 0;
            cpu.R[2] = 100;
            cpu.R[3] = 200;

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 2,
                MemoryAddress = 0,
                OpCode = OpCodes.Compare,
                SecondRegister = 3,
                ThirdRegister = 0
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert
            Assert.AreEqual(-1, cpu.C);
        }

        [Test]
        [ExpectedException("NondeterministicVM.BLL.VmHaltException")]
        public void TestHalt()
        {
            // Arrange
            var cpu = new CPU();

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 0,
                MemoryAddress = 0,
                OpCode = OpCodes.Halt,
                SecondRegister = 0,
                ThirdRegister = 0
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert            
            // Expected exception specified in method attribute
        }

        [Test]
        public void TestJumpIfGreater()
        {
            // Arrange
            var cpu = new CPU();

            cpu.C = 1;
            cpu.PC = 0;

            unchecked
            {
                cpu.R[0] = (int) 0x98513691;
            }

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 0,
                MemoryAddress = 0,
                OpCode = OpCodes.JumpIfGreater,
                SecondRegister = 0,
                ThirdRegister = 0
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert
            Assert.AreEqual(0x98513691, cpu.PC);
        }

        [Test]
        public void TestLogicalOr()
        {
            // Arrange
            var cpu = new CPU();

            cpu.R[10] = 0;
            cpu.R[11] = 0x02040608;
            cpu.R[12] = 0x10305070;

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 10,
                MemoryAddress = 0,
                OpCode = OpCodes.Or,
                SecondRegister = 11,
                ThirdRegister = 12
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert
            Assert.AreEqual(0x12345678, cpu.R[10]);
        }

        [Test]
        public void TestLogicalOrWithZeroProbability()
        {
            // Arrange
            var cpu = new CPU();

            cpu.R[10] = 0;
            cpu.R[11] = 0x02040608;
            cpu.R[12] = 0x10305070;

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 0,
                FirstRegister = 10,
                MemoryAddress = 0,
                OpCode = OpCodes.Or,
                SecondRegister = 11,
                ThirdRegister = 12
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert
            Assert.AreEqual(0, cpu.R[10]);
        }

        [Test]
        public void TestMultiplication()
        {
            // Arrange
            var cpu = new CPU();

            cpu.R[0] = 0;
            cpu.R[1] = 200;
            cpu.R[2] = 300;

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 0,
                MemoryAddress = 0,
                OpCode = OpCodes.Multiply,
                SecondRegister = 1,
                ThirdRegister = 2
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert
            Assert.AreEqual(60000, cpu.R[0]);
        }

        [Test]
        public void TestRandomizedAdd()
        {
            // Arrange
            var cpu = new CPU();

            cpu.R[0] = 0;
            cpu.R[1] = 200;
            cpu.R[2] = 300;

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 0,
                MemoryAddress = 0,
                OpCode = OpCodes.RandomizedAdd,
                SecondRegister = 1,
                ThirdRegister = 2
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            // Assert
            Assert.That(cpu.R[0], Is.EqualTo(500).Within(1).Percent);
        }

        [Test]
        public void TestStoreByMem()
        {
            // Arrange
            var cpu = new CPU();

            cpu.R[0] = 0x0A0B0C0D;

            var random = new Random();

            var memory = new Memory(random);

            var consoleOutput = new ConsoleOutput();

            var interpreter = new Interpreter(cpu, memory, random, consoleOutput);

            var instruction = new Instruction
            {
                ExecutionPossibility = 100,
                FirstRegister = 0,
                MemoryAddress = 0x87654320,
                OpCode = OpCodes.StoreByMem,
                SecondRegister = 1,
                ThirdRegister = 2                
            };

            // Act
            interpreter.ExecuteInstruction(instruction);

            var valueInMemory = memory.ReadWord(0x87654320);

            // Assert
            Assert.AreEqual(0x0A0B0C0D, valueInMemory);
        }
    }
}
