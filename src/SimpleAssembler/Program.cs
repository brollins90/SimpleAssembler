namespace SimpleAssembler
{
    using System;

    public class Program
    {
        public void Main(string[] args)
        {
            Program p = new Program();
            p.Go();
        }

        public void Go()
        {
            var myProgram =
                "MOV r0, 0x3f000000 //e3a0 043f" + Environment.NewLine +
                "MOV r1, 0x200000 //e3a0 1602" + Environment.NewLine +
                "ORR r0, r0, r1 //e180 0001" + Environment.NewLine +
                "STR r1, r0, 0x10 //e580 1010" + Environment.NewLine +
                "MOV r2, 0x80000 //e3a0 2a08" + Environment.NewLine +
                "" + Environment.NewLine +
                "loop: STR r2, r0, 0x20 //e580 2020" + Environment.NewLine +
                "wait1: MOVW r3, 0x0 //e300 3000" + Environment.NewLine +
                "  MOVT r3, 0x10 //e340 3010" + Environment.NewLine +
                "  SUBS r3, r3, 0x01 //e253 3001" + Environment.NewLine +
                "  BNE wait1 //1aff fffd - (BNE 0xfffffd[-3])" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r2, r0, 0x2c //e580 202c" + Environment.NewLine +
                "" + Environment.NewLine +
                "wait2: MOVW r3, 0x0 //e300 3000" + Environment.NewLine +
                "  MOVT r3, 0x10 //e340 3010" + Environment.NewLine +
                "  SUBS r3, r3, 0x01 //e253 3001" + Environment.NewLine +
                "  BNE wait2 //1aff fffd - (BNE 0xfffffd[-3])" + Environment.NewLine +
                "" + Environment.NewLine +
                "BAL loop //eaff fff4 - (BAL 0xfffff4[-12])" + Environment.NewLine;
        }
    }
}
