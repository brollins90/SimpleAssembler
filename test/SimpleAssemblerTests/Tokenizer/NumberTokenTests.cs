namespace SimpleAssemblerTests.Tokenizer
{
    using Simple.Tokenizer.Tokens;
    using Xunit;

    public class NumberTokenTests
    {
        [Fact]
        public void NumberTokenParsesFromDecString()
        {
            NumberToken token = new NumberToken("100");

            Assert.Equal("0x64", token.Value());
            Assert.Equal(100, token.IntValue());
        }

        [Fact]
        public void NumberTokenParsesFromHexString()
        {
            NumberToken token = new NumberToken("0x10");

            Assert.Equal("0x10", token.Value());
            Assert.Equal(16, token.IntValue());
        }

        [Fact]
        public void NumberTokenToBinaryString()
        {
            NumberToken token = new NumberToken("0x10");

            Assert.Equal("0x10", token.Value());
            Assert.Equal("10000", token.BinString());
        }

        [Fact]
        public void NumberTokenToOctString()
        {
            NumberToken token = new NumberToken("0x10");

            Assert.Equal("0x10", token.Value());
            Assert.Equal("20", token.OctString());
        }
    }
}