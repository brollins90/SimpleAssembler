namespace SimpleAssembler.Tokenizer
{
    using Tokens;

    public interface ITokenizer
    {
        bool HasNext();
        Token Next();
    }
}