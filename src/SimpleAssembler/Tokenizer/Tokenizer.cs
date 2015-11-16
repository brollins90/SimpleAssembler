namespace SimpleAssembler.Tokenizer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Tokens;

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

                if (state == ReadState.Comment2)
                {
                    if (/*current == '\r' || */current == '\n')
                    {
                        state = ReadState.Comment2;
                        _index++;
                        // if we get to the end of a comment, we can reset the state
                        tokenString = "";
                        state = ReadState.None;
                    }
                    else
                    {
                        state = ReadState.Comment2;
                        tokenString += current;
                        _index++;
                    }
                }

                else if ((current >= 'A' && current <= 'Z')
                    || (current >= 'a' && current <= 'z'))
                {
                    if (state == ReadState.None || state == ReadState.AlphaNum)
                    {
                        state = ReadState.AlphaNum;
                        tokenString += current;
                        _index++;
                    }
                    else if (state == ReadState.Hex0)
                    {
                        if (current == 'x' || current == 'X')
                        {
                            state = ReadState.Hex0x;
                            tokenString += current;
                            _index++;
                        }
                        else
                        {
                            throw new SyntaxException();
                        }
                    }
                    else if (state == ReadState.Hex0x || state == ReadState.HexNumber)
                    {
                        state = ReadState.HexNumber;
                        tokenString += current;
                        _index++;
                    }
                    else
                    {
                        throw new SyntaxException();
                        //stillReading = false;
                    }
                }
                else if (current >= '0' && current <= '9')
                {
                    if (state == ReadState.None)
                    {
                        if (current == '0')
                        {
                            state = ReadState.Hex0;
                            tokenString += current;
                            _index++;
                        }
                    }
                    else if (state == ReadState.AlphaNum)
                    {
                        state = ReadState.AlphaNum;
                        tokenString += current;
                        _index++;
                    }
                    else if (state == ReadState.DecimalNumber)
                    {
                        state = ReadState.DecimalNumber;
                        tokenString += current;
                        _index++;
                    }
                    else if (state == ReadState.Hex0x)
                    {
                        state = ReadState.HexNumber;
                        tokenString += current;
                        _index++;
                    }
                    else if (state == ReadState.HexNumber)
                    {
                        state = ReadState.HexNumber;
                        tokenString += current;
                        _index++;
                    }
                    else
                    {
                        throw new SyntaxException();
                        //stillReading = false;
                    }
                }
                else if (current == ' ' || current == '\t' || current == '\r' || current == '\n')
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
                        // do not increment but return
                        stillReading = false;
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
                        _index++;
                        stillReading = false;
                    }
                    else if (state == ReadState.AlphaNum
                          || state == ReadState.HexNumber)
                    {
                        // do not increment but return
                        stillReading = false;
                    }
                    else
                    {
                        throw new SyntaxException();
                    }
                }
                else if (current == '#')
                {
                    if (state == ReadState.None)
                    {
                        state = ReadState.DecimalNumber;
                        // dont add the # to the string
                        _index++;
                    }
                    else
                    {
                        throw new SyntaxException();
                    }
                }
                else if (current == '/')
                {
                    if (state == ReadState.None)
                    {
                        state = ReadState.Comment1;
                        tokenString += current;
                        _index++;
                    }
                    if (state == ReadState.Comment1)
                    {
                        state = ReadState.Comment2;
                        tokenString += current;
                        _index++;
                    }
                    else
                    {
                        throw new SyntaxException();
                    }
                }
                else
                {
                    throw new SyntaxException();
                }
            }

            switch (state)
            {
                case ReadState.AlphaNum:
                    return new AlphaNumToken(tokenString);
                case ReadState.Colon:
                    return new ColonToken(tokenString);
                case ReadState.Comma:
                    return new CommaToken(tokenString);
                case ReadState.HexNumber:
                case ReadState.DecimalNumber:
                    return new NumberToken(tokenString);
                case ReadState.Hex0:
                case ReadState.Hex0x:
                case ReadState.None:
                    //throw new SyntaxException();
                    break;
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
        Comma,
        Comment1,
        Comment2,
        DecimalNumber,
        Hex0,
        Hex0x,
        HexNumber
    }
}