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
            if (_tokenizer == null)
            {
                throw new InvalidOperationException();
            }
            Token token = (_tokenStack.Count > 0) ? _tokenStack.Pop() : _tokenizer.Next();

            return token;
        }

        public void UnGet(Token token)
        {
            if (token != null)
            {
                _tokenStack.Push(token);
            }
        }
    }
}