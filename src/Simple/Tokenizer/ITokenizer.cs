namespace Simple.Tokenizer
{
    using Tokens;

    public interface ITokenizer
    {
        bool HasNext();
        Token Next();
    }
}