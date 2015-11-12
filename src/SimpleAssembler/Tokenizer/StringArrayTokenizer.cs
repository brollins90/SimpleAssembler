//namespace SimpleAssembler.Tokenizer
//{
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.Text.RegularExpressions;

//    public class StringArrayTokenizer : ITokenizer
//    {
//        private string[] _stringArray;
//        private int _index;


//        public StringArrayTokenizer(string input)
//        {
//            _stringArray = input.Split(' ');
//            _index = -1;
//        }

//        private bool TryParseToken(string value, out Token outToken)
//        {
//            outToken = null;

//            if (value.Equals(":"))
//            {
//                outToken = new ColonToken(value);
//            }
//            else if (value.Equals(","))
//            {
//                outToken = new CommaToken(value);
//            }
//            else if (Regex.IsMatch(value, @"^[a-zA-Z0-9]+$"))
//            {
//                outToken = new AlphaNumToken(value);
//            }

//            //string tokenString = "";
//            //bool stillReading = true;
//            //ReadState state = ReadState.None;

//            //while (_index < _inputString.Length && stillReading)
//            //{
//            //    char current = _inputString[_index++];

//            //    if ((current >= 'A' && current <= 'Z')
//            //        || (current >= 'a' && current <= 'z')
//            //        || (current >= 0 && current <= 9))
//            //    {
//            //        if (state == ReadState.None || state == ReadState.AlphaNum)
//            //        {
//            //            state = ReadState.AlphaNum;
//            //            tokenString += current;
//            //        }
//            //        else
//            //        {
//            //            stillReading = false;
//            //        }
//            //    }
//            //    else if (current == ' ')
//            //    {
//            //        // eat whitespace and continue
//            //    }
//            //    else if (current == ':')
//            //    {
//            //        if (state == ReadState.None)
//            //        {
//            //            state = ReadState.Colon;
//            //            stillReading = false;
//            //        }
//            //        else
//            //        {
//            //            throw new SyntaxException();
//            //        }
//            //    }
//            //}

//            //if (tokenString.Equals(":"))
//            //{

//            //}
//            return outToken != null;
//        }

//        public bool HasNext()
//        {
//            return (_index + 1) < _stringArray.Length;
//        }

//        public Token Next()
//        {
//            Token outToken;
//            if (TryParseToken(_stringArray[_index++], out outToken))
//            {
//                return outToken;
//            }
//            throw new SyntaxException();
//        }
//    }

//    public enum ReadState
//    {
//        None,
//        AlphaNum,
//        Colon
//    }
//}