namespace SimpleAssembler.Lexer.LexTokens
{
    using System;

    public class RegisterToken : LexToken
    {
        private string _value;

        public RegisterToken(string value)
            : base(value)
        {
            _value = ParseRegister(value);
        }

        public override string Value()
        {
            return _value;
        }

        private string ParseRegister(string value)
        {
            if (value.Equals("r0", StringComparison.InvariantCultureIgnoreCase))
            {
                return "0";
            }
            throw new LexSyntaxException($"{value} is not a valid register");
        }
    }
}