namespace SimpleAssemblerTests.Parser
{
    using SimpleAssembler.Tokenizer;
    using SimpleAssembler.Tokenizer.Tokens;
    using System;
    using Xunit;

    public class ParserTests
    {

        [Fact]
        public void ParserReturnsUIntArray()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();
            var result = parser.Parse("");

            Assert.IsType(typeof(uint[]), result);
        }

        [Fact]
        public void ParserCanParseLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            uint labelIndex = uint.MaxValue;
            ITokenStream tokenStream = new TokenStream("loop:");

            var result = parser.TryParseLabel(tokenStream, out labelIndex);

            Assert.True(result);
            Assert.Equal((uint)0, labelIndex);
            Assert.False(tokenStream.HasNext());
        }

        [Fact]
        public void ParserTryParseLabelReturnsTokensToStream()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            uint labelIndex = uint.MaxValue;
            ITokenStream tokenStream = new TokenStream("MOVT r0, 0x3f000000");

            var result = parser.TryParseLabel(tokenStream, out labelIndex);

            Assert.False(result);
            Assert.Equal(uint.MaxValue, labelIndex);
            Assert.True(tokenStream.HasNext());

            var token1 = tokenStream.Next();
            Assert.IsType(typeof(AlphaNumToken), token1);
        }

        [Fact]
        public void ParserParseLabelReturnsCorrectIndex()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x3f20" + Environment.NewLine +
                "loop: MOVW r0, 0x0" + Environment.NewLine;

            parser.Parse(myProgram);
            int labelIndex = parser.GetCurrentIndex();
            
            // the loop branch should be at 1 making the current index 2
            Assert.Equal(2, labelIndex);
        }

        [Fact]
        public void ParserParseMOVTParse0()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x0" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction);

            Assert.Equal(0xe3400000, instruction);
        }

        [Fact]
        public void ParserParseMOVTParse1()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r1, 0x01" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction);

            Assert.Equal(0xe3401001, instruction);
        }

        [Fact]
        public void ParserParseMOVWParse0()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVW r0, 0x0" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction);

            Assert.Equal(0xe3000000, instruction);
        }

        [Fact]
        public void ParserParseMOVWParse1()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVW r1, 0x01" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction);

            Assert.Equal(0xe3001001, instruction);
        }
    }
}