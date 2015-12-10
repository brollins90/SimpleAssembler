namespace SimpleCompiler.Parser.Expressions
{
    using Simple;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class BinaryOperatorExpression : Expression
    {
        public BinaryOperatorExpression(string op, Expression lHS, Expression rHS)
        {
            Operation = op;
            LHS = lHS;
            RHS = rHS;
        }

        public string Operation { get; }
        public Expression LHS { get; }
        public Expression RHS { get; }

        public override string GenerateCode()
        {
            string left = LHS.GenerateCode();
            string right = RHS.GenerateCode();

            if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
            {
                throw new SyntaxException($"LHS or RHS of binop is invalid: {left}, {right}");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(left);
            sb.Append(right);
            sb.AppendLine($"POP a1");
            sb.AppendLine($"POP a2");

            if (Operation.Equals("+", StringComparison.InvariantCultureIgnoreCase))
            {
                sb.AppendLine("ADDRS a1, a1, a2");
            }
            else if (Operation.Equals("-", StringComparison.InvariantCultureIgnoreCase))
            {
                sb.AppendLine("SUBRS a1, a1, a2");
            }
            sb.AppendLine("PUSH a1");
            return sb.ToString();
        }
    }
}