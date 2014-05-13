using System;
using NondeterministicVM.BLL;
using NUnit.Framework;

namespace NondeterministicVM.Tests
{
    [TestFixture]
    public class MemoryTests
    {
        [Test]
        public void TestReadingLowerMemory()
        {
            // Arrange
            var random = new Random();
            var memory = new Memory(random);

            // Act
            var word1 = memory.ReadWord(0);
            var word2 = memory.ReadWord(4);
            var word3 = memory.ReadWord(8);

            // Assert
            Assert.AreEqual(0, word1);
            Assert.AreEqual(0, word2);
            Assert.AreEqual(0, word3);
        }

        [Test]
        public void TestWritingLowerMemory()
        {
            // Arrange
            var random = new Random();
            var memory = new Memory(random);

            // Act
            memory.WriteWord(100, 123);
            memory.WriteWord(104, 456);
            memory.WriteWord(108, 789);

            // Assert
            Assert.AreEqual(123, memory.ReadWord(100));
            Assert.AreEqual(456, memory.ReadWord(104));
            Assert.AreEqual(789, memory.ReadWord(108));
        }

        [Test]
        public void TestReadingUpperMemory()
        {
            // Arrange
            var random = new Random();
            var memory = new Memory(random);

            // Act
            var word1 = memory.ReadWord(0xC0000000);
            var word2 = memory.ReadWord(0xC0000004);
            var word3 = memory.ReadWord(0xC0000008);

            // Assert
            Assert.IsFalse(word1 == word2 && word2 == word3);            
        }

        [Test]
        public void TestWritingUpperMemory()
        {
            // Arrange
            var random = new Random();
            var memory = new Memory(random);

            // Act
            memory.WriteWord(0xC0000000, 123);
            memory.WriteWord(0xC0000004, 456);
            memory.WriteWord(0xC0000008, 789);

            // Assert
            Assert.IsTrue(SevenBitsEqual(123, memory.ReadWord(0xC0000000)));
            Assert.IsTrue(SevenBitsEqual(456, memory.ReadWord(0xC0000004)));
            Assert.IsTrue(SevenBitsEqual(789, memory.ReadWord(0xC0000008)));
        }

        private bool SevenBitsEqual(int value1, int value2)
        {
            int equalBits = 0;

            for (int i = 0; i < 8; i++)
            {
                int bit = 1 << i;

                if ((value1 & bit) == (value2 & bit))
                {
                    equalBits++;
                }
            }

            return equalBits == 7;
        }
    }
}
