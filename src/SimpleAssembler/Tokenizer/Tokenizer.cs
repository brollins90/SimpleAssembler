namespace SimpleAssembler.Tokenizer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Tokenizer : ITokenizer
    {
        private string _inputString;
        private int _index;


        public Tokenizer(string input)
        {
            _inputString = input;
            _index = 0;
        }

        public Token Next()
        {
            string tokenString = "";
            bool stillReading = true;
            ReadState state = ReadState.None;

            while (_index < _inputString.Length && stillReading)
            {
                char current = _inputString[_index];

                if ((current >= 'A' && current <= 'Z')
                    || (current >= 'a' && current <= 'z')
                    || (current >= '0' && current <= '9'))
                {
                    if (state == ReadState.None || state == ReadState.AlphaNum)
                    {
                        state = ReadState.AlphaNum;
                        tokenString += current;
                        _index++;
                    }
                    else
                    {
                        stillReading = false;
                    }
                }
                else if (current == ' ' || current == '\t')
                {
                    if (state == ReadState.None)
                    {
                        state = ReadState.None;
                        _index++;
                    }
                    else
                    {
                        // eat whitespace and return
                        stillReading = false;
                        _index++;
                    }
                }
                else if (current == ':')
                {
                    if (state == ReadState.None)
                    {
                        state = ReadState.Colon;
                        tokenString += current;
                        stillReading = false;
                        _index++;
                    }
                    else if (state == ReadState.AlphaNum)
                    {
                        stillReading = false;
                        // do not increment
                    }
                    else
                    {
                        throw new SyntaxException();
                    }
                }
                else if (current == ',')
                {
                    if (state == ReadState.None)
                    {
                        state = ReadState.Comma;
                        tokenString += current;
                        stillReading = false;
                        _index++;
                    }
                    else if (state == ReadState.AlphaNum)
                    {
                        stillReading = false;
                        // do not increment
                    }
                    else
                    {
                        throw new SyntaxException();
                    }
                }
            }

            switch (state)
            {
                case ReadState.AlphaNum:
                    return new AlphaNumToken(tokenString);
                case ReadState.None:
                    break;
                case ReadState.Colon:
                    return new ColonToken(tokenString);
                case ReadState.Comma:
                    return new CommaToken(tokenString);
            }

            return null;
        }

        public bool HasNext()
        {
            return (_index + 1) < _inputString.Length;
        }
    }

    public enum ReadState
    {
        None,
        AlphaNum,
        Colon,
        Comma
    }
}