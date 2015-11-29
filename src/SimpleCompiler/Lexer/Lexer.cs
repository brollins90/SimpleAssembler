namespace SimpleCompiler.Lexer
{
    using LexTokens;
    using Simple;
    using Simple.Tokenizer;
    using Simple.Tokenizer.Tokens;
    using System.Collections.Generic;

    public class Lexer : ILexer
    {
        private string _inputString;
        private int _index;
        private Stack<char> _tokenStack;
        //private ITokenStream _tokenStream;

        public Lexer(string input)
        {
            _inputString = input;
            _index = 0;
            _tokenStack = new Stack<char>();
            //_tokenStream = new TokenStream(fileData);
        }

        public bool HasNext()
        {
            return (_index + 1) < _inputString.Length;
        }

        public LexToken Next()
        {
            return (_tokenStack.Count > 0) ? _tokenStack.Pop() : GetNext();
        }

        public void UnGet(char c)
        {
            _tokenStack.Push(c);
        }

        private char GetChar()
        {
            return _inputString[_index++];
        }

        private bool IsWhitespace(char c)
        {
            return (c == ' ' || c == '\t');
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z');
        }

        private bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        private bool IsHexDigit(char c)
        {
            return (c >= '0' && c <= '9')
                || (c >= 'a' && c <= 'f')
                || (c >= 'A' && c <= 'F');
        }

        private bool IsAlphaNum(char c)
        {
            return (IsAlpha(c) || IsDigit(c));
        }

        protected LexToken GetNext()
        {
            string tokenString = "";

            if (_index >= _inputString.Length)
            {
                throw new System.Exception($"i dont know if i need to watch for this");
            }
            char current = _inputString[_index];

            while (IsWhitespace(current))
            {
                current = GetChar();
            }

            if (IsAlpha(current))
            {
                do
                {
                    tokenString += current;
                    current = GetChar();
                } while (IsAlphaNum(current));

                if (tokenString.Equals("if", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    return new IfLexToken(tokenString);
                }
                return new IdentifierLexToken(tokenString);
            }

            if (current == '#')
            {
                current = GetChar();
                if (IsDigit(current) || current == '.')
                {
                    do
                    {
                        tokenString += current;
                        current = GetChar();
                    } while (IsDigit(current) || current == '.');
                    return new NumberLexToken(tokenString);
                }
            }

            if (current == '0')
            {
                current = GetChar();
                if (current == 'x' || current == 'X')
                {
                    current = GetChar();
                    if (IsHexDigit(current))
                    {
                        do
                        {
                            tokenString += current;
                            current = GetChar();
                        } while (IsHexDigit(current));
                        return new NumberLexToken($"0x{tokenString}");
                    }
                }
            }

            if (current == '/')
            {
                current = GetChar();
                if (current == '/')
                {
                    do
                    {
                        current = GetChar();
                    } while (current != '\r' && current != '\n');
                    current = GetChar();
                    if (current == '\n')
                    {
                        return new NewLineLexToken("");
                    }
                    else
                    {
                        throw new SyntaxException($"found a {current} after a \\ r");
                    }
                }
                else throw new SyntaxException($"found a {current} after a single /");
            }
            return null;
        }
    }

    public enum LexerState
    {
        None,
        AlphaNum
    }
}