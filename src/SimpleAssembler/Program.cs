namespace SimpleAssembler
{
    using System;
    using System.IO;

    public class Program
    {
        public void Main(string[] args)
        {
            Program p = new Program();
            p.Go();
        }

        public void Go()
        {
            var parser = new Parser.Parser();

            var myProgram =
                "MOVW r0, 0x0" + Environment.NewLine +
                "MOVT r0, 0x3f20" + Environment.NewLine +
                "MOVW r1, 0x0" + Environment.NewLine +
                "MOVT r1, 0x20" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r1, r0, 0x10" + Environment.NewLine +
                "MOVW r2, 0x8000" + Environment.NewLine +
                "" + Environment.NewLine +
                "loop: STR r2, r0, 0x20" + Environment.NewLine +
                "MOVW r3, 0x0" + Environment.NewLine +
                "MOVT r3, 0x10" + Environment.NewLine +
                "wait1: SUBS r3, r3, 0x01" + Environment.NewLine +
                "  BNE wait1" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r2, r0, 0x2c" + Environment.NewLine +
                "" + Environment.NewLine +
                "MOVW r3, 0x0" + Environment.NewLine +
                "MOVT r3, 0x10" + Environment.NewLine +
                "wait2: SUBS r3, r3, 0x01" + Environment.NewLine +
                "  BNE wait2" + Environment.NewLine +
                "" + Environment.NewLine +
                "BAL loop" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            using (BinaryWriter b = new BinaryWriter(File.Open("c:\\_\\kernel7.img", FileMode.Create)))
            {
                // Use foreach and write all 12 integers.
                foreach (int i in output)
                {
                    b.Write(i);
                    //b.Write((i >> (4 * 3)) & 0xF);
                    //b.Write((i >> (4 * 2)) & 0xF);
                    //b.Write((i >> (4 * 1)) & 0xF);
                    //b.Write((i >> (4 * 0)) & 0xF);
                }
            }

            //var myProgram =
            //    "MOVT r0, 0x3f20 //e343 0f20" + Environment.NewLine +
            //    "MOVW r0, 0x0 //e300 0000" + Environment.NewLine +
            //    "MOVT r1, 0x20 //e340 1020" + Environment.NewLine +
            //    "MOVW r1, 0x0 //e300 1000" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "STR r1, r0, 0x10 //e580 1010" + Environment.NewLine +
            //    "MOV r2, 0x80000 //e3a0 2a08" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "loop: STR r2, r0, 0x20 //e580 2020" + Environment.NewLine +
            //    "wait1: MOVW r3, 0x0 //e300 3000" + Environment.NewLine +
            //    "  MOVT r3, 0x10 //e340 3010" + Environment.NewLine +
            //    "  SUBS r3, r3, 0x01 //e253 3001" + Environment.NewLine +
            //    "  BNE wait1 //1aff fffd - (BNE 0xfffffd[-3])" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "STR r2, r0, 0x2c //e580 202c" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "wait2: MOVW r3, 0x0 //e300 3000" + Environment.NewLine +
            //    "  MOVT r3, 0x10 //e340 3010" + Environment.NewLine +
            //    "  SUBS r3, r3, 0x01 //e253 3001" + Environment.NewLine +
            //    "  BNE wait2 //1aff fffd - (BNE 0xfffffd[-3])" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "BAL loop //eaff fff4 - (BAL 0xfffff4[-12])" + Environment.NewLine;
        }
    }
}
