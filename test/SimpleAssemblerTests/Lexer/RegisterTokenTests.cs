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
        public void R0Lower()
        {
            RegisterToken token = new RegisterToken("r0");

            Assert.Equal("0", token.Value());
        }

        [Fact]
        public void R0Upper()
        {
            RegisterToken token = new RegisterToken("R0");

            Assert.Equal("0", token.Value());
        }

        [Fact]
        public void PCUpper()
        {
            RegisterToken token = new RegisterToken("PC");

            Assert.Equal("f", token.Value());
        }

        [Fact]
        public void Registers()
        {
            Assert.Equal("0", new RegisterToken("r0").Value());
            Assert.Equal("1", new RegisterToken("r1").Value());
            Assert.Equal("2", new RegisterToken("r2").Value());
            Assert.Equal("3", new RegisterToken("r3").Value());
            Assert.Equal("4", new RegisterToken("r4").Value());
            Assert.Equal("5", new RegisterToken("r5").Value());
            Assert.Equal("6", new RegisterToken("r6").Value());
            Assert.Equal("7", new RegisterToken("r7").Value());
            Assert.Equal("8", new RegisterToken("r8").Value());
            Assert.Equal("9", new RegisterToken("r9").Value());
            Assert.Equal("a", new RegisterToken("r10").Value());
            Assert.Equal("b", new RegisterToken("r11").Value());
            Assert.Equal("c", new RegisterToken("r12").Value());
            Assert.Equal("d", new RegisterToken("r13").Value());
            Assert.Equal("e", new RegisterToken("r14").Value());
            Assert.Equal("f", new RegisterToken("r15").Value());

            Assert.Equal("0", new RegisterToken("a1").Value());
            Assert.Equal("1", new RegisterToken("a2").Value());
            Assert.Equal("2", new RegisterToken("a3").Value());
            Assert.Equal("3", new RegisterToken("a4").Value());

            Assert.Equal("4", new RegisterToken("v1").Value());
            Assert.Equal("5", new RegisterToken("v2").Value());
            Assert.Equal("6", new RegisterToken("v3").Value());
            Assert.Equal("7", new RegisterToken("v4").Value());
            Assert.Equal("8", new RegisterToken("v5").Value());
            Assert.Equal("9", new RegisterToken("v6").Value());
            Assert.Equal("a", new RegisterToken("v7").Value());
            Assert.Equal("b", new RegisterToken("v8").Value());

            Assert.Equal("9", new RegisterToken("sb").Value());
            Assert.Equal("a", new RegisterToken("sl").Value());
            Assert.Equal("b", new RegisterToken("fp").Value());
            Assert.Equal("c", new RegisterToken("ip").Value());
            Assert.Equal("d", new RegisterToken("sp").Value());
            Assert.Equal("e", new RegisterToken("lr").Value());
            Assert.Equal("f", new RegisterToken("pc").Value());
        }
    }
}