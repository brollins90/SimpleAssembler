namespace SimpleAssembler.Lexer.LexTokens
{
    public abstract class LexToken
    {
        private string _value;

        public LexToken(string value)
        {
            _value = value.ToLowerInvariant();
        }

        public virtual string Value()
        {
            return _value;
        }

        public override string ToString()
        {
            return Value();
        }
    }
}