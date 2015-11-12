namespace SimpleAssembler.Tokenizer
{
    public interface ITokenizer
    {
        bool HasNext();
        Token Next();
    }
}