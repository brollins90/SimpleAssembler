namespace Simple.Tokenizer
{
    using Tokens;

    public interface ITokenStream : ITokenizer
    {
        void UnGet(Token token);
    }
}