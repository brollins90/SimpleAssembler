namespace SimpleAssembler.Tokenizer
{
    public class Token
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
    }
}