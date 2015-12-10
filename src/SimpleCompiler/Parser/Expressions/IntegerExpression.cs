namespace SimpleCompiler.Parser.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class IntegerExpression : Expression
    {
        public int Value { get; }

        public IntegerExpression(int value)
        {
            Value = value;
        }

        public override string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"MOVW a1, 0x{Convert.ToString((Value & 0xffff), 16)}");
            if (Value > 0xffff)
            {
                string upperBits = Convert.ToString(((Value & 0xffff0000) >> 16), 16);
                sb.AppendLine($"MOVT a1, 0x{upperBits}");
            }
            sb.AppendLine("PUSH a1");
            return sb.ToString();
        }
    }
}