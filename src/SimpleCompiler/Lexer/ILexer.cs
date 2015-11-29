namespace SimpleCompiler.Lexer
{
    using LexTokens;

    public interface ILexer
    {
        bool HasNext();
        LexToken Next();
        void UnGet(LexToken token);
    }
}