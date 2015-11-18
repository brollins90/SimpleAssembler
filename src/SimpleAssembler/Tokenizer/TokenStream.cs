namespace SimpleAssembler.Tokenizer
{
    using System.Collections.Generic;
    using Tokens;

    public class TokenStream : ITokenStream
    {
        private Stack<Token> _tokenStack;
        private ITokenizer _tokenizer;

        public TokenStream(string fileData)
        {
            _tokenStack = new Stack<Token>();
            _tokenizer = new Tokenizer(fileData);
        }

        public bool HasNext()
        {
            return (_tokenizer == null) ? false : _tokenizer.HasNext();
        }

        public Token Next()
        {
            return (_tokenStack.Count > 0) ? _tokenStack.Pop() : _tokenizer.Next();
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