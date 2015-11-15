﻿namespace SimpleAssembler.Tokenizer
{
    using Tokens;

    public interface ITokenStream : ITokenizer
    {
        void UnGet(Token token);
    }
}