namespace SimpleCompiler.Compiler
{
    using System;
    using System.Collections.Generic;

    public interface ICompiler
    {
        uint[] Compile(string program);
    }
}