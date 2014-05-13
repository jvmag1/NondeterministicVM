namespace NondeterministicVM.BLL
{
    public enum OpCodes
    {
        // Lifecycle operations
        Halt = 0x00,

        // Arithmetic operations
        Add = 0x01,
        Subtract = 0x02,
        Multiply = 0x03,
        Divide = 0x04,
        Modulo = 0x05,
        Increase = 0x06,
        Decrease = 0x07,

        // Logical operations
        And = 0x10,
        Or = 0x11,
        Xor = 0x12,
        Negate = 0x13,

        // Comparison operations
        Compare = 0x20,

        // Flow control operations
        Jump = 0x30,
        JumpIfLess = 0x31,
        JumpIfEqual = 0x32,
        JumpIfNotEqual = 0x33,
        JumpIfGreater = 0x34,

        // I/O operations
        LoadByMem = 0x40,
        LoadByReg = 0x41,
        Store = 0x42,
        PrintString = 0x43,
        PrintNumber = 0x44,

        // Randomized operations
        RandomizedAdd = 0xA1,
        RandomizedSubtract = 0xA2,
        RandomizedMultiply = 0xA3,
        RandomizedDivide = 0xA4,
        RandomizedModulo = 0xA5
    }
}
