namespace SimpleAssembler.Tokenizer
{
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
                string currentLower = $"{current}".ToLowerInvariant();
                current = currentLower[0];

                switch (state)
                {
                    case ReadState.None:

                        // alphanum
                        if (current >= 'a' && current <= 'z')
                        {
                            state = ReadState.AlphaNum;
                            tokenString += current;
                            _index++;
                        }
                        else if (current == ' ' || current == '\t')
                        {
                            // eat whitespace
                            _index++;
                        }

                        // starting comment
                        else if (current == '/')
                        {
                            state = ReadState.Comment1;
                            tokenString += current;
                            _index++;
                        }

                        // newline
                        else if (current == '\r')
                        {
                            state = ReadState.NewLineR;
                            tokenString += current;
                            _index++;
                        }
                        else if (current == '\n')
                        {
                            state = ReadState.NewLine;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }

                        // number
                        else if (current == '#')
                        {
                            state = ReadState.DecimalNumber;
                            // dont add the # to the string
                            _index++;
                        }
                        else if (current == '0')
                        {
                            state = ReadState.Hex0;
                            tokenString += current;
                            _index++;
                        }

                        // specials
                        else if (current == ':')
                        {
                            state = ReadState.Colon;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }
                        else if (current == ',')
                        {
                            state = ReadState.Comma;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }

                        // Reigster list
                        else if (current == '{')
                        {
                            state = ReadState.RegisterListStart;
                            tokenString += current;
                            _index++;
                        }

                        // else
                        else
                        {
                            throw new SyntaxException($"Cannot start an instruction with a '{current}'");
                        }

                        break;

                    case ReadState.AlphaNum:

                        if ((current >= 'a' && current <= 'z')
                            || (current >= '0' && current <= '9'))
                        {
                            tokenString += current;
                            _index++;
                        }
                        else if (current == ':' || current == ',' || current == ' ' || current == '\t')
                        {
                            stillReading = false;
                        }
                        else if (current == '\r' || current == '\n')
                        {
                            stillReading = false;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.Comment1:
                        if (current == '/')
                        {
                            state = ReadState.Comment2;
                            tokenString += current;
                            _index++;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.Comment2:
                        if (current == '\r' || current == '\n')
                        {
                            // if we get to the end of a comment, we can reset the state
                            tokenString = "";
                            state = ReadState.None;
                        }
                        else
                        {
                            tokenString += current;
                            _index++;
                        }
                        break;

                    case ReadState.DecimalNumber:
                        if (current >= '0' && current <= '9')
                        {
                            tokenString += current;
                            _index++;
                        }
                        else if (current == '\r' || current == '\n' || current == ' ' || current == '\t')
                        {
                            stillReading = false;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.HexNumber:
                        if ((current >= '0' && current <= '9')
                            || (current >= 'a' && current <= 'f'))
                        {
                            tokenString += current;
                            _index++;
                        }
                        else if (current == '\r' || current == '\n' || current == ' ' || current == '\t')
                        {
                            stillReading = false;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.Hex0:
                        if (current == 'x')
                        {
                            state = ReadState.HexNumber;
                            tokenString += current;
                            _index++;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.NewLineR:
                        if (current == '\n')
                        {
                            state = ReadState.NewLine;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.RegisterListStart:
                        if (current == '\r' || current == '\n')
                        {
                            throw new SyntaxException($"Cannot end a line before the register list is closed");
                        }
                        else if (current == '}')
                        {
                            state = ReadState.RegisterList;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }
                        else
                        {
                            tokenString += current;
                            _index++;
                        }
                        break;
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
                case ReadState.NewLine:
                    return new NewLineToken(tokenString);
                case ReadState.Hex0:
                    throw new SyntaxException($"A '{state}' is not a valid state");
                case ReadState.RegisterList:
                    return new RegisterListToken(tokenString);
                case ReadState.None:
                    return null;
            }
            throw new SyntaxException($"Unknown tokenizer state.  (tokenString is {tokenString})");
            //return null;
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
        HexNumber,
        NewLineR,
        NewLine,
        RegisterListStart,
        RegisterList
    }
}