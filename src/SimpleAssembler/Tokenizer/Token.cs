namespace SimpleAssembler.Tokenizer
{
    public class Token
    {
        private string _value;

        public Token(string value)
        {
            _value = value;
        }

        public string Value()
        {
            return _value;
        }
    }
}