namespace SimpleAssembler.Tokenizer
{
    using System;
    using System.Collections.Generic;
    using Tokens;

    public class TokenStream : ITokenStream
    {
        Stack<Token> _tokenStack;
        ITokenizer _tokenizer;

        public TokenStream(string fileData)
        {
            _tokenStack = new Stack<Token>();
            _tokenizer = new Tokenizer(fileData);
        }

        public bool HasNext()
        {
            if (_tokenizer == null)
            {
                return false;
            }
            else
            {
                return _tokenizer.HasNext();
            }
        }

        public Token Next()
        {
            if (_tokenStack.Count > 0)
            {
                return _tokenStack.Pop();
            }
            if (_tokenizer == null)
            {
                throw new InvalidOperationException();
            }
            return _tokenizer.Next();
        }

        public void UnGet(Token token)
        {
            _tokenStack.Push(token);
        }
    }
}