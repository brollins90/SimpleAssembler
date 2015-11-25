namespace SimpleAssembler.Tokenizer.Tokens
{
    using System;

    public class NumberToken : Token
    {
        private bool _isNegative;
        private int _intValue;
        private string _overrideValue;

        public NumberToken(string value)
            : base(value)
        {
            _isNegative = false;
            if (value.StartsWith("0x", StringComparison.Ordinal)) // hex
            {
                _intValue = Convert.ToInt32(value.Substring(2), 16);
                _overrideValue = $"0x{HexString()}";
            }
            else if (value.StartsWith("-", StringComparison.Ordinal)) // negative, after hex because no neg hex
            {
                _isNegative = true;
                _intValue = Convert.ToInt32(value);
                _overrideValue = $"0x{HexString()}";
            }
            else
            {
                _intValue = Convert.ToInt32(value, 10);
                _overrideValue = $"0x{HexString()}";
            }
        }

        public override string Value()
        {
            return _overrideValue;
        }

        public bool IsNegative()
        {
            return _isNegative;
        }

        public int IntValue()
        {
            return _intValue;
        }

        public string BinString()
        {
            return Convert.ToString(_intValue, 2);
        }

        public string DecString()
        {
            return Convert.ToString(_intValue, 10);
        }

        public string HexString()
        {
            return Convert.ToString(_intValue, 16);
        }

        public string OctString()
        {
            return Convert.ToString(_intValue, 8);
        }
    }
}