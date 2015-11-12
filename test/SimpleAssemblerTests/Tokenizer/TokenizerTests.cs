namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Tokenizer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class TokenizerTests
    {
        [Fact]
        public void TokenizerMovIsAlphaNum()
        {
            var tokenizer = new Tokenizer("MOV");
            var token = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token);
            Assert.Equal("MOV", token.Value);
        }

        [Fact]
        public void TokenizerColonIsColon()
        {
            var tokenizer = new Tokenizer(":");
            var token = tokenizer.Next();

            Assert.IsType(typeof(ColonToken), token);
            Assert.Equal(":", token.Value);
        }

        [Fact]
        public void TokenizerCommaIsComma()
        {
            var tokenizer = new Tokenizer(",");
            var token = tokenizer.Next();

            Assert.IsType(typeof(CommaToken), token);
            Assert.Equal(",", token.Value);
        }

        [Fact]
        public void TokenizerWordAndColonIsAlphaNumAndColon()
        {
            var tokenizer = new Tokenizer("loop:");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token1);
            Assert.Equal("loop", token1.Value);

            Assert.IsType(typeof(ColonToken), token2);
            Assert.Equal(":", token2.Value);
        }

        [Fact]
        public void TokenizerWordAndCommaIsAlphaNumAndComma()
        {
            var tokenizer = new Tokenizer("r1,");
            var token1 = tokenizer.Next();
            var token2 = tokenizer.Next();

            Assert.IsType(typeof(AlphaNumToken), token1);
            Assert.Equal("r1", token1.Value);

            Assert.IsType(typeof(CommaToken), token2);
            Assert.Equal(",", token2.Value);
        }
    }
}
