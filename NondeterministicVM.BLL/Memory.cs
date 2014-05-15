using System;
using System.Collections.Generic;

namespace NondeterministicVM.BLL
{
    public class Memory
    {
        // We do not want to map 4GB of memory at once, so we are going to allocate memory word by word on demand
        public readonly Dictionary<uint, int> _dataStore = new Dictionary<uint, int>();

        private readonly Random _random;

        public Memory(Random random)
        {
            _random = random;
        }

        public int ReadWord(uint address)
        {
            CheckAddress(address);

            if (! _dataStore.ContainsKey(address))
            {
                if (address < 0xC0000000) // 3GB
                {
                    _dataStore.Add(address, 0);
                }
                else
                {
                    return _random.Next();
                }
            }

            return _dataStore[address];
        }

        public void WriteWord(uint address, int word)
        {
            CheckAddress(address);

            if (! _dataStore.ContainsKey(address))
            {
                _dataStore.Add(address, 0);
            }

            if (address < 0xC0000000) // 3GB
            {
                _dataStore[address] = word;
            }
            else
            {
                var randomBit = _random.Next(0, 31);

                _dataStore[address] = word ^ (1 << randomBit);
            }
        }

        private void CheckAddress(uint address)
        {
            if (address % sizeof(uint) != 0)
            {
                throw new NotSupportedException("Memory access should be aligned by word size");
            }
        }
    }
}
