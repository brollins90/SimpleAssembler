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
            Assert.IsType(typeof(BinaryOperatorLexToken), token2);
            Assert.Equal("+", token2.Value());
            Assert.IsType(typeof(NumberLexToken), token3);
            Assert.Equal("0x2", token3.Value());
        }
    }
}