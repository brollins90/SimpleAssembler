namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Lexer;
    using SimpleAssembler.Lexer.LexTokens;
    using Xunit;

    public class RegisterTokenTests
    {
        [Fact]
        public void NonExistantRegister()
        {
            Assert.Throws<LexSyntaxException>(() =>
            {
                RegisterToken token = new RegisterToken("p3");
            });
        }

        [Fact]
        public void r0()
        {
            RegisterToken token = new RegisterToken("r0");

            Assert.Equal("0", token.Value());
        }
    }
}