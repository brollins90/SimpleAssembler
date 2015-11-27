namespace SimpleAssembler.Tokenizer.Tokens
{
    public abstract class Token
    {
        private string _value;

        public Token(string value)
        {
            _value = value;
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