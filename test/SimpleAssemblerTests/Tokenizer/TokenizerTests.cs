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
            var tokenizer = new Tokenizer("mov");
            var token = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token);
            Assert.Equal("mov", token.Value());
        }
        [Fact]
        public void TokenizerMovUndIsAlphaNumUnd()
        {
            var tokenizer = new Tokenizer("mov_mov");
            var token = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumUnderscoreToken), token);
            Assert.Equal("mov_mov", token.Value());
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
        public void TokenizerLeftCurlyIsLeftCurly()
        {
            var tokenizer = new Tokenizer("{");
            var token = tokenizer.Next();

            Assert.IsType(typeof(LeftCurlyToken), token);
            Assert.Equal("{", token.Value());
        }

        [Fact]
        public void TokenizerNewLineIsNewLine()
        {
            var tokenizer = new Tokenizer("\r\n");
            var token = tokenizer.Next();

            Assert.IsType(typeof(NewLineToken), token);
            Assert.Equal("\r\n", token.Value());
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
        public void TokenizerRightCurlyIsRightCurly()
        {
            var tokenizer = new Tokenizer("}");
            var token = tokenizer.Next();

            Assert.IsType(typeof(RightCurlyToken), token);
            Assert.Equal("}", token.Value());
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
        public void TokenizerWordUndAndColonIsAlphaNumUndAndColon()
        {
            var tokenizer = new Tokenizer("loop_loop:");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumUnderscoreToken), token1);
            Assert.Equal("loop_loop", token1.Value());

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

        [Fact]
        public void TokenizerRegisterListOneRegister()
        {
            var tokenizer = new Tokenizer("{r1}");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();
            var token3 = tokenizer.Next();

            Assert.IsType(typeof(LeftCurlyToken), token1);
            Assert.Equal("{", token1.Value());

            Assert.IsType(typeof(AlphaNumToken), token2);
            Assert.Equal("r1", token2.Value());

            Assert.IsType(typeof(RightCurlyToken), token3);
            Assert.Equal("}", token3.Value());
        }

        [Fact]
        public void TokenizerRegisterListTwoRegistersComma()
        {
            var tokenizer = new Tokenizer("{r1,r2}");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();
            var token3 = tokenizer.Next();
            var token4 = tokenizer.Next();
            var token5 = tokenizer.Next();

            Assert.IsType(typeof(LeftCurlyToken), token1);
            Assert.Equal("{", token1.Value());

            Assert.IsType(typeof(AlphaNumToken), token2);
            Assert.Equal("r1", token2.Value());

            Assert.IsType(typeof(CommaToken), token3);
            Assert.Equal(",", token3.Value());

            Assert.IsType(typeof(AlphaNumToken), token4);
            Assert.Equal("r2", token4.Value());

            Assert.IsType(typeof(RightCurlyToken), token5);
            Assert.Equal("}", token5.Value());
        }

        [Fact]
        public void TokenizerRegisterListTwoRegistersHyphen()
        {
            var tokenizer = new Tokenizer("{r1-r2}");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();
            var token3 = tokenizer.Next();
            var token4 = tokenizer.Next();
            var token5 = tokenizer.Next();

            Assert.IsType(typeof(LeftCurlyToken), token1);
            Assert.Equal("{", token1.Value());

            Assert.IsType(typeof(AlphaNumToken), token2);
            Assert.Equal("r1", token2.Value());

            Assert.IsType(typeof(HyphenToken), token3);
            Assert.Equal("-", token3.Value());

            Assert.IsType(typeof(AlphaNumToken), token4);
            Assert.Equal("r2", token4.Value());

            Assert.IsType(typeof(RightCurlyToken), token5);
            Assert.Equal("}", token5.Value());
        }
    }
}
