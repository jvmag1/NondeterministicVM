using NondeterministicVM.BLL;
using NUnit.Framework;

namespace NondeterministicVM.Tests
{
    [TestFixture]
    public class InstructionTests
    {
        [Test]
        public void TestParsingFromWord()
        {
            // Arrange
            const int word = 0x2003B05F;

            var targetResult = new Instruction
            {
                OpCode = OpCodes.Compare,
                FirstRegister = 3,
                SecondRegister = 11,
                ThirdRegister = 0,
                ExecutionPossibility = 95
            };

            // Act
            var instruction = Instruction.FromWord(word);

            // Assert
            Assert.AreEqual(targetResult, instruction);
        }

        [Test]
        public void TestParsingFromWordWithUnusedBits()
        {
            // Arrange
            const int word = 0x20F3B05F;

            var targetResult = new Instruction
            {
                OpCode = OpCodes.Compare,
                FirstRegister = 3,
                SecondRegister = 11,
                ThirdRegister = 0,
                ExecutionPossibility = 95
            };

            // Act
            var instruction = Instruction.FromWord(word);

            // Assert
            Assert.AreEqual(targetResult, instruction);
        }

        [Test]
        public void TestParsingTwoWordInstruction()
        {
            // Arrange
            const int word1 = 0x40050064;
            const int word2 = 0x12345678;

            var targetResult = new Instruction
            {
                OpCode = OpCodes.LoadByMem,
                FirstRegister = 5,
                SecondRegister = 0,
                ThirdRegister = 0,
                ExecutionPossibility = 100,
                MemoryAddress = 0x12345678
            };

            // Act
            var instruction = Instruction.FromWord(word1, word2);

            // Assert
            Assert.AreEqual(targetResult, instruction);
        }
    }
}
