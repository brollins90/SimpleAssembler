namespace SimpleCompiler.Parser.Expressions
{
    using System;
    using System.Collections.Generic;
	
    public class BinaryOperatorExpression : Expression
    {
        public string Operation { get; set; }
        public Expression LHS { get; set; }
        public Expression RHS { get; set; }
    }
}