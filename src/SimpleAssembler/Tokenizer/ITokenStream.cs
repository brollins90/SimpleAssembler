namespace SimpleAssembler.Tokenizer
{	
    public interface ITokenStream : ITokenizer
    {
        void Load(string s);
        void UnGet(Token token);
    }
}