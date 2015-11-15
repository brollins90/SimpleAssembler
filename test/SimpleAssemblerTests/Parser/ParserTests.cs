namespace SimpleAssemblerTests.Parser
{
    using SimpleAssembler.Tokenizer;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class ParserTests
    {

        [Fact]
        public void ParserReturnsIntArray()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();
            var result = parser.Parse("");

            Assert.IsType(typeof(int[]), result);
        }

        [Fact]
        public void ParserCanParseLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            int labelIndex = -1;
            ITokenStream tokenStream = new TokenStream("loop:");

            var result = parser.TryParseLabel(tokenStream, out labelIndex);

            Assert.True(result);
            Assert.Equal(0, labelIndex);
            Assert.False(tokenStream.HasNext());
        }

        [Fact]
        public void ParserTryParseLabelReturnsTokensToStream()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            int labelIndex = -1;
            ITokenStream tokenStream = new TokenStream("MOV r0, 0x3f000000");

            var result = parser.TryParseLabel(tokenStream, out labelIndex);

            Assert.False(result);
            Assert.Equal(-1, labelIndex);
            Assert.True(tokenStream.HasNext());

            var token1 = tokenStream.Next();
            Assert.IsType(typeof(AlphaNumToken), token1);
        }

        [Fact]
        public void ParserParseLabelReturnsCorrectIndex()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOV r0, 0x3f000000" + Environment.NewLine +
                "loop: MOV r1, 0x200000" + Environment.NewLine;

            int labelIndex = -1;
            ITokenStream tokenStream = new TokenStream(myProgram);

            int instruction1 = parser.ParseInstruction(tokenStream);

            var result = parser.TryParseLabel(tokenStream, out labelIndex);

            Assert.True(result);
            Assert.Equal(1, labelIndex);
        }

        [Fact]
        public void ParserParseMovParse0()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOV r0, 0x0" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            int instruction = parser.ParseInstruction(tokenStream);

            Assert.Equal(0xe3a00000, Convert.ToUInt32(instruction));
        }

        [Fact]
        public void ParserParseMovParse1()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOV r0, 0x01" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            int instruction = parser.ParseInstruction(tokenStream);

            Assert.Equal(0xe3a00001, Convert.ToUInt32(instruction));
        }
    }
}