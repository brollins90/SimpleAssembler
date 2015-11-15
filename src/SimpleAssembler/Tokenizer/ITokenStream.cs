namespace SimpleAssembler.Tokenizer
{	
    public interface ITokenStream : ITokenizer
    {
        void UnGet(Token token);
    }
}