using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAssembler
{
    public class Program
    {
        public void Main(string[] args)
        {
            Program p = new Program();
            p.Go();
        }

        public void Go()
        {
            //var myProgram = 
            //    "MOVI R0 , 0x3f000000" + Environment.NewLine +
            //    "MOVI R1 , 0x200000" + Environment.NewLine +
            //    "ORR R0, R0 , R1 , 0" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "STRI R1, R0 , 0x10" + Environment.NewLine +
            //    "MOVI R1 , 0x80000" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "loop: STRI R1 , R0 , 0x20" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "MOVI R2 , 0x0" + Environment.NewLine +
            //    "wait1: ADDI R2 , R2 , 0x1" + Environment.NewLine +
            //    "CMPI R2 , 0x400000" + Environment.NewLine +
            //    "BNE wait1" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "STRI R1 , R0 , 0x2c" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "MOVI R2 , 0x0" + Environment.NewLine +
            //    "wait2: ADDI R2 , R2 , 0x1" + Environment.NewLine +
            //    "CMPI R2 , 0x400000" + Environment.NewLine +
            //    "BNE wait2" + Environment.NewLine +
            //    "" + Environment.NewLine +
            //    "BAL loop" + Environment.NewLine;
        }
    }
}
