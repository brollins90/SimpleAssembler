namespace SimpleAssemblerTests.Lexer
{
    using SimpleAssembler;
    using SimpleAssembler.Lexer.LexTokens;
    using Xunit;

    public class RegisterTokenTests
    {
        [Fact]
        public void NonExistantRegister()
        {
            Assert.Throws<SyntaxException>(() =>
            {
                RegisterLexToken token = new RegisterLexToken("p3");
            });
        }

        [Fact]
        public void R0Lower()
        {
            RegisterLexToken token = new RegisterLexToken("r0");
            Assert.Equal("0", token.Value());
        }

        [Fact]
        public void R0Upper()
        {
            RegisterLexToken token = new RegisterLexToken("R0");
            Assert.Equal("0", token.Value());
        }

        [Fact]
        public void PCUpper()
        {
            RegisterLexToken token = new RegisterLexToken("PC");
            Assert.Equal("f", token.Value());
        }

        [Fact]
        public void Registers()
        {
            Assert.Equal("0", new RegisterLexToken("r0").Value());
            Assert.Equal("1", new RegisterLexToken("r1").Value());
            Assert.Equal("2", new RegisterLexToken("r2").Value());
            Assert.Equal("3", new RegisterLexToken("r3").Value());
            Assert.Equal("4", new RegisterLexToken("r4").Value());
            Assert.Equal("5", new RegisterLexToken("r5").Value());
            Assert.Equal("6", new RegisterLexToken("r6").Value());
            Assert.Equal("7", new RegisterLexToken("r7").Value());
            Assert.Equal("8", new RegisterLexToken("r8").Value());
            Assert.Equal("9", new RegisterLexToken("r9").Value());
            Assert.Equal("a", new RegisterLexToken("r10").Value());
            Assert.Equal("b", new RegisterLexToken("r11").Value());
            Assert.Equal("c", new RegisterLexToken("r12").Value());
            Assert.Equal("d", new RegisterLexToken("r13").Value());
            Assert.Equal("e", new RegisterLexToken("r14").Value());
            Assert.Equal("f", new RegisterLexToken("r15").Value());

            Assert.Equal("0", new RegisterLexToken("a1").Value());
            Assert.Equal("1", new RegisterLexToken("a2").Value());
            Assert.Equal("2", new RegisterLexToken("a3").Value());
            Assert.Equal("3", new RegisterLexToken("a4").Value());

            Assert.Equal("4", new RegisterLexToken("v1").Value());
            Assert.Equal("5", new RegisterLexToken("v2").Value());
            Assert.Equal("6", new RegisterLexToken("v3").Value());
            Assert.Equal("7", new RegisterLexToken("v4").Value());
            Assert.Equal("8", new RegisterLexToken("v5").Value());
            Assert.Equal("9", new RegisterLexToken("v6").Value());
            Assert.Equal("a", new RegisterLexToken("v7").Value());
            Assert.Equal("b", new RegisterLexToken("v8").Value());

            Assert.Equal("9", new RegisterLexToken("sb").Value());
            Assert.Equal("a", new RegisterLexToken("sl").Value());
            Assert.Equal("b", new RegisterLexToken("fp").Value());
            Assert.Equal("c", new RegisterLexToken("ip").Value());
            Assert.Equal("d", new RegisterLexToken("sp").Value());
            Assert.Equal("e", new RegisterLexToken("lr").Value());
            Assert.Equal("f", new RegisterLexToken("pc").Value());
        }
    }
}