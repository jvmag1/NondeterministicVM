namespace NondeterministicVM.BLL
{
    public class CPU
    {
        public uint PC = 0;
        public int[] R = new int[16];
        public sbyte C = 0;

        public void AdvancePC()
        {
            PC += sizeof(uint);
        }
    }
}
