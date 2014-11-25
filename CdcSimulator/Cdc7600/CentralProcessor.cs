namespace CdcSimulator.Cdc7600
{
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class CentralProcessor
    {
        public ArithmeticLogicUnit Alu = new ArithmeticLogicUnit();
        public Dictionary<OpCode, int> TimingMap = new Dictionary<OpCode, int>
        {
            
        };

        public Register U1Register = new Register();
        public Register U2Register = new Register();
        public Register U3Register = new Register();
    }
}
