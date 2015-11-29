namespace SimpleCompilerTests.Lexer
{
    using SimpleCompiler.Lexer;
    using SimpleCompiler.Lexer.LexTokens;
    using Xunit;

    public class LexerTests
    {
        [Fact]
        public void SingleInt()
        {
            ILexer lexer = new Lexer("1");
            var token1 = lexer.Next();

            Assert.IsType(typeof(NumberLexToken), token1);
            Assert.Equal("0x1", token1.Value());
        }

        [Fact]
        public void BasicMath()
        {
            ILexer lexer = new Lexer("1 + 2");
            var token1 = lexer.Next();
            var token2 = lexer.Next();
            var token3 = lexer.Next();

            Assert.IsType(typeof(NumberLexToken), token1);
            Assert.Equal("0x1", token1.Value());
            Assert.IsType(typeof(BinaryOperationLexToken), token1);
            Assert.Equal("+", token1.Value());
            Assert.IsType(typeof(NumberLexToken), token1);
            Assert.Equal("0x10", token1.Value());
        }
    }
}