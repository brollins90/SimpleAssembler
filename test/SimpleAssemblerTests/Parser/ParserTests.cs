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

            var result = parser.TryParseLabel(tokenStream, out labelIndex, true);

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

            var result = parser.TryParseLabel(tokenStream, out labelIndex, true);

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
                "MOVW r0, 0x0" + Environment.NewLine +
                "MOVT r0, 0x3f20" + Environment.NewLine +
                "MOVW r1, 0x0" + Environment.NewLine +
                "MOVT r1, 0x20" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r1, r0, 0x10" + Environment.NewLine +
                "MOVW r2, 0x8000" + Environment.NewLine +
                "" + Environment.NewLine +
                "loop: STR r2, r0, 0x20" + Environment.NewLine +
                "MOVW r3, 0x0" + Environment.NewLine +
                "MOVT r3, 0x10" + Environment.NewLine +
                "wait1: SUBS r3, r3, 0x01" + Environment.NewLine +
                "  BNE wait1" + Environment.NewLine +
                "" + Environment.NewLine +
                "STR r2, r0, 0x2c" + Environment.NewLine +
                "" + Environment.NewLine +
                "MOVW r3, 0x0" + Environment.NewLine +
                "MOVT r3, 0x10" + Environment.NewLine +
                "wait2: SUBS r3, r3, 0x01" + Environment.NewLine +
                "  BNE wait2" + Environment.NewLine +
                "" + Environment.NewLine +
                "BAL loop" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal(0xe3000000, output[0]);
            Assert.Equal(0xe3430f20, output[1]);
            Assert.Equal(0xe3001000, output[2]);
            Assert.Equal(0xe3401020, output[3]);
            Assert.Equal(0xe5801010, output[4]);
            Assert.Equal(0xe3082000, output[5]);
            Assert.Equal(0xe5802020, output[6]);
            Assert.Equal(0xe3003000, output[7]);
            Assert.Equal(0xe3403010, output[8]);
            Assert.Equal(0xe2533001, output[9]);
            Assert.Equal((uint)0x1afffffd, output[10]);
            Assert.Equal(0xe580202c, output[11]);
            Assert.Equal(0xe3003000, output[12]);
            Assert.Equal(0xe3403010, output[13]);
            Assert.Equal(0xe2533001, output[14]);
            Assert.Equal((uint)0x1afffffd, output[15]);
            Assert.Equal(0xeafffff4, output[16]);
        }

        [Fact]
        public void ParserParseLabelReturnsCorrectIndex()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x3f20" + Environment.NewLine +
                "loop: MOVW r0, 0x0" + Environment.NewLine;

            parser.Parse(myProgram);
            int labelIndex = parser.GetCurrentKernelIndex();

            // the loop branch should be at 1 making the current index 2
            Assert.Equal(2, labelIndex);
        }

        [Fact]
        public void ParserGetsCorrectLineNumberLine0()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();
            int lineNumber = parser.GetCurrentLineNumber();

            Assert.Equal(0, lineNumber);
        }

        [Fact]
        public void ParserGetsCorrectLineNumber1()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x3f20" + Environment.NewLine;

            parser.Parse(myProgram);
            int lineNumber = parser.GetCurrentLineNumber();

            Assert.Equal(1, lineNumber);
        }

        [Fact]
        public void ParserGetsCorrectLineNumber2()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x3f20" + Environment.NewLine +
                "loop: MOVW r0, 0x0" + Environment.NewLine;

            parser.Parse(myProgram);
            int lineNumber = parser.GetCurrentLineNumber();

            Assert.Equal(2, lineNumber);
        }

        #region MOV
        [Fact]
        public void ParserParseMOVParser0r0()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOV r0, r0" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction, false);

            Assert.Equal(0xe1a00000, instruction);
        }

        [Fact]
        public void ParserParseMOVParser0r1()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOV r0, r1" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction, false);

            Assert.Equal(0xe1a00001, instruction);
        }

        [Fact]
        public void ParserParseMOVNotRegr18()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOV r0, r18" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);

            Assert.Throws(typeof(SyntaxException), () =>
            {
                uint instruction;
                parser.TryParseInstruction(tokenStream, out instruction, false);
            });
        }
        #endregion

        #region MOVT
        [Fact]
        public void ParserParseMOVTParse0()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "MOVT r0, 0x0" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);
            uint instruction;
            parser.TryParseInstruction(tokenStream, out instruction, false);

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
            parser.TryParseInstruction(tokenStream, out instruction, false);

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
                parser.TryParseInstruction(tokenStream, out instruction, false);
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
            parser.TryParseInstruction(tokenStream, out instruction, false);

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
            parser.TryParseInstruction(tokenStream, out instruction, false);

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
                parser.TryParseInstruction(tokenStream, out instruction, false);
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
            parser.TryParseInstruction(tokenStream, out instruction, false);

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
                parser.TryParseInstruction(tokenStream, out instruction, false);
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
            parser.TryParseInstruction(tokenStream, out instruction, false);

            Assert.Equal(0xe2533001, instruction);
        }

        [Fact]
        public void ParserParseSUBSTooLarge()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "SUBS r3, r3, 0x10000" + Environment.NewLine;

            ITokenStream tokenStream = new TokenStream(myProgram);

            Assert.Throws(typeof(SyntaxException), () =>
            {
                uint instruction;
                parser.TryParseInstruction(tokenStream, out instruction, false);
            });
        }
        #endregion
        #region BAL
        [Fact]
        public void ParserParseBALParseNoLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "BAL loop" + Environment.NewLine;

            Assert.Throws(typeof(SyntaxException), () =>
            {
                parser.Parse(myProgram);
            });
        }

        [Fact]
        public void ParserParseBALParseWithLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "loop: MOVW r0, 0x0" + Environment.NewLine +
                "BAL loop" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal(0xe3000000, output[0]);
            Assert.Equal(0xeafffffd, output[1]);
        }

        [Fact]
        public void ParserParseBALParseWithLabelAfter()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "BAL loop" + Environment.NewLine +
                "loop: MOVW r0, 0x0" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal((uint)0xeaffffff, output[0]);
            Assert.Equal(0xe3000000, output[1]);
        }
        #endregion
        #region BNE
        [Fact]
        public void ParserParseBNEParseNoLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "BNE loop" + Environment.NewLine;

            Assert.Throws(typeof(SyntaxException), () =>
            {
                parser.Parse(myProgram);
            });
        }

        [Fact]
        public void ParserParseBNEParseWithLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "loop: MOVW r0, 0x0" + Environment.NewLine +
                "BNE loop" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal(0xe3000000, output[0]);
            Assert.Equal((uint)0x1afffffd, output[1]);
        }

        [Fact]
        public void ParserParseBNEParseWithLabelAfter()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "BNE loop" + Environment.NewLine +
                "loop: MOVW r0, 0x0" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal((uint)0x1affffff, output[0]);
            Assert.Equal(0xe3000000, output[1]);
        }
        #endregion
        #region BL
        [Fact]
        public void ParserParseBLParseNoLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "BL loop" + Environment.NewLine;

            Assert.Throws(typeof(SyntaxException), () =>
            {
                parser.Parse(myProgram);
            });
        }

        [Fact]
        public void ParserParseBLParseWithLabel()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "loop: MOVW r0, 0x0" + Environment.NewLine +
                "BL loop" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal(0xe3000000, output[0]);
            Assert.Equal(0xebfffffd, output[1]);
        }

        [Fact]
        public void ParserParseBLParseWithLabelAfter()
        {
            SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

            var myProgram =
                "BL loop" + Environment.NewLine +
                "loop: MOVW r0, 0x0" + Environment.NewLine;

            var output = parser.Parse(myProgram);

            Assert.Equal(0xebffffff, output[0]);
            Assert.Equal(0xe3000000, output[1]);
        }
        #endregion
    }
}