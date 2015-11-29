namespace Simple.Tokenizer
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
                            state = ReadState.ForwardSlash1;
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

                        // Left Curly
                        else if (current == '{')
                        {
                            state = ReadState.LeftCurly;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }

                        // Right Curly
                        else if (current == '}')
                        {
                            state = ReadState.RightCurly;
                            tokenString += current;
                            _index++;
                            stillReading = false;
                        }

                        // Hypen Curly
                        else if (current == '-')
                        {
                            state = ReadState.Hyphen;
                            tokenString += current;
                            _index++;
                            stillReading = false;
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
                        else if (current == '=' || current == '<' || current == '>' || current == '!')
                        {
                            state = ReadState.Operation;
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
                    case ReadState.AlphaNumUnd:

                        if ((current >= 'a' && current <= 'z')
                            || (current >= '0' && current <= '9'))
                        {
                            tokenString += current;
                            _index++;
                        }
                        else if (current == '}' || current == '-' || current == ':' || current == ',' || current == ' ' || current == '\t')
                        {
                            stillReading = false;
                        }
                        else if (current == '\r' || current == '\n')
                        {
                            stillReading = false;
                        }
                        else if (current == '_')
                        {
                            state = ReadState.AlphaNumUnd;
                            tokenString += current;
                            _index++;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.ForwardSlash1:
                        if (current == '/')
                        {
                            state = ReadState.CommentLine;
                            tokenString += current;
                            _index++;
                        }
                        else if (current == '*')
                        {
                            state = ReadState.CommentMultiLineNotClosed;
                            tokenString += current;
                            _index++;
                        }
                        else
                        {
                            throw new SyntaxException($"Cannot add a '{current}' to a '{state}' token");
                        }
                        break;

                    case ReadState.CommentMultiLineNotClosed:
                        if (current == '*')
                        {
                            state = ReadState.CommentMultiLineClosing;
                        }
                        tokenString += current;
                        _index++;
                        break;

                    case ReadState.CommentLine:
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

                    case ReadState.CommentMultiLineClosing:
                        if (current == '/')
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
                        else if (current == ',')
                        {
                            // TODO::: there is no valid commands that take a value after a hex number.  Should I throw here?  
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

                    case ReadState.LeftCurly:

                        if ((current >= 'a' && current <= 'z')
                            || (current >= '0' && current <= '9')
                            || current == ','
                            || current == '-'
                            || current == ' '
                            || current == '\t')
                        {
                            tokenString += current;
                            _index++;
                        }
                        else if (current == '}')
                        {
                            state = ReadState.CurlyBlockFinished;
                            tokenString += current;
                            _index++;
                            stillReading = false;
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

                    case ReadState.Operation:
                        if (current == '=' || current == '<' || current == '>')
                        {
                            tokenString += current;
                            _index++;
                        }
                        else
                        {
                            stillReading = false;
                        }
                        break;
                }
            }

            switch (state)
            {
                case ReadState.AlphaNum:
                    return new AlphaNumToken(tokenString);
                case ReadState.AlphaNumUnd:
                    return new AlphaNumUnderscoreToken(tokenString);
                case ReadState.Colon:
                    return new ColonToken(tokenString);
                case ReadState.Comma:
                    return new CommaToken(tokenString);
                case ReadState.Hyphen:
                    return new HyphenToken(tokenString);
                case ReadState.LeftCurly:
                    return new LeftCurlyToken(tokenString);
                case ReadState.HexNumber:
                case ReadState.DecimalNumber:
                    return new NumberToken(tokenString);
                case ReadState.NewLine:
                    return new NewLineToken(tokenString);
                case ReadState.RightCurly:
                    return new RightCurlyToken(tokenString);
                case ReadState.Hex0:
                    throw new SyntaxException($"A '{state}' is not a valid state");
                case ReadState.Operation:
                    return new SpecialToken(tokenString);
                case ReadState.None:
                    return null;
            }
            throw new SyntaxException("Unknown state");
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
        AlphaNumUnd,
        Colon,
        Comma,
        CommentLine,
        CommentMultiLineClosing,
        CommentMultiLineNotClosed,
        CurlyBlockFinished,
        DecimalNumber,
        ForwardSlash1,
        Hex0,
        HexNumber,
        Hyphen,
        LeftCurly,
        NewLineR,
        NewLine,
        Operation,
        RightCurly
    }
}