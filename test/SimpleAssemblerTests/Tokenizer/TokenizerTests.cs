namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Tokenizer;
    using SimpleAssembler.Tokenizer.Tokens;
    using Xunit;

    public class TokenizerTests
    {
        [Fact]
        public void TokenizerMovIsAlphaNum()
        {
            var tokenizer = new Tokenizer("MOV");
            var token = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token);
            Assert.Equal("MOV", token.Value());
        }

        [Fact]
        public void TokenizerColonIsColon()
        {
            var tokenizer = new Tokenizer(":");
            var token = tokenizer.Next();

            Assert.IsType(typeof(ColonToken), token);
            Assert.Equal(":", token.Value());
        }

        [Fact]
        public void TokenizerCommaIsComma()
        {
            var tokenizer = new Tokenizer(",");
            var token = tokenizer.Next();

            Assert.IsType(typeof(CommaToken), token);
            Assert.Equal(",", token.Value());
        }

        [Fact]
        public void TokenizerHexDigitIsNumberToken()
        {
            var tokenizer = new Tokenizer("0x10");
            var token = tokenizer.Next();

            Assert.IsType(typeof(NumberToken), token);
            Assert.Equal("0x10", token.Value());
            Assert.Equal(16, (token as NumberToken).IntValue());
        }

        [Fact]
        public void TokenizerDecimalDigitIsNumberToken()
        {
            var tokenizer = new Tokenizer("#4");
            var token = tokenizer.Next();

            Assert.IsType(typeof(NumberToken), token);
            Assert.Equal("0x4", token.Value());
            Assert.Equal(4, (token as NumberToken).IntValue());
        }

        [Fact]
        public void TokenizerWordAndColonIsAlphaNumAndColon()
        {
            var tokenizer = new Tokenizer("loop:");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token1);
            Assert.Equal("loop", token1.Value());

            Assert.IsType(typeof(ColonToken), token2);
            Assert.Equal(":", token2.Value());
        }

        [Fact]
        public void TokenizerWordAndCommaIsAlphaNumAndComma()
        {
            var tokenizer = new Tokenizer("r1,");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token1);
            Assert.Equal("r1", token1.Value());

            Assert.IsType(typeof(CommaToken), token2);
            Assert.Equal(",", token2.Value());
        }

        [Fact]
        public void TokenizerInstructionWithTwoParams()
        {
            var tokenizer = new Tokenizer("mov r1, #0");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();
            var token3 = tokenizer.Next();
            var token4 = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token1);
            Assert.Equal("mov", token1.Value());

            Assert.IsType(typeof(AlphaNumToken), token2);
            Assert.Equal("r1", token2.Value());

            Assert.IsType(typeof(CommaToken), token3);
            Assert.Equal(",", token3.Value());

            Assert.IsType(typeof(NumberToken), token4);
            Assert.Equal("0x0", token4.Value());
            Assert.Equal(0, (token4 as NumberToken).IntValue());
        }
    }
}
