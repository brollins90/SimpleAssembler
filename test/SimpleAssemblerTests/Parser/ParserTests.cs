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
        public void ParserParseLab4()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x3f20" + Environment.NewLine +
                "MOVW r0, 0x0" + Environment.NewLine +
                "MOVT r1, 0x20" + Environment.NewLine +
                "MOVW r1, 0x0" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r1, r0, 0x10" + Environment.NewLine +
                "MOVW r2, 0x8000" + Environment.NewLine +
                "" + Environment.NewLine +
                "loop: STR r2, r0, 0x20" + Environment.NewLine +
                "wait1: MOVW r3, 0x0" + Environment.NewLine +
                "  MOVT r3, 0x10" + Environment.NewLine +
                "  SUBS r3, r3, 0x01" + Environment.NewLine +
                "  BNE wait1 //1aff fffd - (BNE 0xfffffd[-3])" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r2, r0, 0x2c" + Environment.NewLine +
                "" + Environment.NewLine +
                "wait2: MOVW r3, 0x0" + Environment.NewLine +
                "  MOVT r3, 0x10" + Environment.NewLine +
                "  SUBS r3, r3, 0x01" + Environment.NewLine +
                "  BNE wait2 //1aff fffd - (BNE 0xfffffd[-3])" + Environment.NewLine +
                "" + Environment.NewLine +
                "BAL loop //eaff fff4 - (BAL 0xfffff4[-12])" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal(0xe3430f20, output[0]);
            Assert.Equal(0xe3000000, output[1]);
            Assert.Equal(0xe3401020, output[2]);
            Assert.Equal(0xe3001000, output[3]);
            Assert.Equal(0xe5801010, output[4]);
            Assert.Equal(0xe3082000, output[5]);
            Assert.Equal(0xe5802020, output[6]);
            Assert.Equal(0xe3003000, output[7]);
            Assert.Equal(0xe3403010, output[8]);
            // subs
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

        #region MOVT
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
        public void ParserParseMOVTTooLarge()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x10000" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);

            Assert.Throws(typeof(SyntaxException), () =>
            {
                uint instruction;
                parser.TryParseInstruction(tokenStream, out instruction);
            });
        }
        #endregion

        #region MOVW

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

        [Fact]
        public void ParserParseMOVWTooLarge()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVW r0, 0x10000" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);

            Assert.Throws(typeof(SyntaxException), () =>
            {
                uint instruction;
                parser.TryParseInstruction(tokenStream, out instruction);
            });
        }
        #endregion

        #region STR

        [Fact]
        public void ParserParseSTRParse10()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "STR r1, r0, 0x10" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction);

            Assert.Equal(0xe5801010, instruction);
        }

        [Fact]
        public void ParserParseSTRTooLarge()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "STR r1, r2, 0x1000" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);

            Assert.Throws(typeof(SyntaxException), () =>
            {
                uint instruction;
                parser.TryParseInstruction(tokenStream, out instruction);
            });
        }
        #endregion

        #region SUBS

        [Fact]
        public void ParserParseSUBSParse01()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "SUBS r3, r3, 0x01" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction);

            Assert.Equal(0xe2533001, instruction);
        }

        [Fact]
        public void ParserParseSUBSTooLarge()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "SUBS r3, r3, 0x1000" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);

            Assert.Throws(typeof(SyntaxException), () =>
            {
                uint instruction;
                parser.TryParseInstruction(tokenStream, out instruction);
            });
        }
        #endregion
    }
}