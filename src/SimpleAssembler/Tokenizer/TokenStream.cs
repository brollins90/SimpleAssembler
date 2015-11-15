namespace SimpleAssembler.Tokenizer
{
    using System;
    using System.Collections.Generic;

    public class TokenStream : ITokenStream
    {
        Stack<Token> _tokenStack;
        ITokenizer _tokenizer;

        public TokenStream()
        {
            _tokenStack = new Stack<Token>();
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

        public void Load(string s)
        {
            if (_tokenizer.HasNext())
            {
                throw new InvalidOperationException();
            }
            _tokenizer = new Tokenizer(s);
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