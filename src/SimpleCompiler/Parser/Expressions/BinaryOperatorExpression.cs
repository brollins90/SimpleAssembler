namespace SimpleCompiler.Parser.Expressions
{
    using System;
    using System.Collections.Generic;

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
    }
}