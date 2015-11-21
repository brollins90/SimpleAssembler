namespace SimpleAssembler.Tokenizer.Tokens
{
    public abstract class SpecialToken : Token
    {
        public SpecialToken(string value)
            : base(value)
        {
        }
    }
}