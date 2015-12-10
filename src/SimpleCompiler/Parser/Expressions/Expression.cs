namespace SimpleCompiler.Parser.Expressions
{
    using System;
    using System.Collections.Generic;

    public class Expression
    {
        public virtual string GenerateCode()
        {
            return "0";
        }
    }
}