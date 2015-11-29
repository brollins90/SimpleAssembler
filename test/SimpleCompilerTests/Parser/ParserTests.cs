namespace SimpleCompilerTests.Parser
{
    using SimpleCompiler;
    using SimpleCompiler.Parser;
    using System;
    using Xunit;

    public class ParserTests
    {
        //#region parser

        [Fact]
        public void ReturnsParseTree()
        {
            IParser parser = new Parser();
            var result = parser.Parse("");

            Assert.IsType(typeof(uint[]), result);
        }

        [Fact]
        public void CanParseSingelInt()
        {
            IParser parser = new Parser();

            var myProgram = "1";

            var output = parser.Parse(myProgram);

            Assert.Contains("loop", parser.LabelTable.Keys);
            Assert.Equal((uint)0, parser.LabelTable["loop"]);
        }

        [Fact]
        public void CanParseAddition()
        {
            IParser parser = new Parser();

            var myProgram = "1 + 2";

            var output = parser.Parse(myProgram);

            Assert.Contains("loop", parser.LabelTable.Keys);
            Assert.Equal((uint)0, parser.LabelTable["loop"]);
        }

        //[Fact]
        //public void ParserCanParseLabelAlphaNumUnderscore()
        //{
        //    IParser parser = new Parser();

        //    var myProgram = "lo_op:";

        //    var output = parser.Parse(myProgram);

        //    Assert.Contains("lo_op", parser.LabelTable.Keys);
        //    Assert.Equal((uint)0, parser.LabelTable["lo_op"]);
        //}

        //[Fact]
        //public void ParserParseLab4()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVW r0, 0x0" + Environment.NewLine +
        //        "MOVT r0, 0x3f20" + Environment.NewLine +
        //        "MOVW r1, 0x0" + Environment.NewLine +
        //        "MOVT r1, 0x20" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "STR r1, r0, 0x10" + Environment.NewLine +
        //        "MOVW r2, 0x8000" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "loop: STR r2, r0, 0x20" + Environment.NewLine +
        //        "MOVW r3, 0x0" + Environment.NewLine +
        //        "MOVT r3, 0x10" + Environment.NewLine +
        //        "wait1: SUBS r3, r3, 0x01" + Environment.NewLine +
        //        "  BNE wait1" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "STR r2, r0, 0x2c" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "MOVW r3, 0x0" + Environment.NewLine +
        //        "MOVT r3, 0x10" + Environment.NewLine +
        //        "wait2: SUBS r3, r3, 0x01" + Environment.NewLine +
        //        "  BNE wait2" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "BAL loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal(0xe3430f20, output[1]);
        //    Assert.Equal(0xe3001000, output[2]);
        //    Assert.Equal(0xe3401020, output[3]);
        //    Assert.Equal(0xe5001010, output[4]);
        //    Assert.Equal(0xe3082000, output[5]);
        //    Assert.Equal(0xe5002020, output[6]);
        //    Assert.Equal(0xe3003000, output[7]);
        //    Assert.Equal(0xe3403010, output[8]);
        //    Assert.Equal(0xe2533001, output[9]);
        //    Assert.Equal((uint)0x1afffffd, output[10]);
        //    Assert.Equal(0xe500202c, output[11]);
        //    Assert.Equal(0xe3003000, output[12]);
        //    Assert.Equal(0xe3403010, output[13]);
        //    Assert.Equal(0xe2533001, output[14]);
        //    Assert.Equal((uint)0x1afffffd, output[15]);
        //    Assert.Equal(0xeafffff4, output[16]);
        //}

        //[Fact]
        //public void ParserParseLab7()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVW r0, 0x0" + Environment.NewLine +
        //        "MOVT r0, 0x3f20" + Environment.NewLine +
        //        "MOVW r1, 0x0" + Environment.NewLine +
        //        "MOVT r1, 0x20" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "STR r1, r0, 0x10" + Environment.NewLine +
        //        "MOVW r2, 0x8000" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "loop: STR r2, r0, 0x20" + Environment.NewLine +
        //        "MOVW r3, 0x0" + Environment.NewLine +
        //        "MOVT r3, 0x10" + Environment.NewLine +
        //        "wait1: SUBS r3, r3, 0x01" + Environment.NewLine +
        //        "  BNE wait1" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "STR r2, r0, 0x2c" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "MOVW r3, 0x0" + Environment.NewLine +
        //        "MOVT r3, 0x10" + Environment.NewLine +
        //        "wait2: SUBS r3, r3, 0x01" + Environment.NewLine +
        //        "  BNE wait2" + Environment.NewLine +
        //        "" + Environment.NewLine +
        //        "BAL loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal(0xe3430f20, output[1]);
        //    Assert.Equal(0xe3001000, output[2]);
        //    Assert.Equal(0xe3401020, output[3]);
        //    Assert.Equal(0xe5001010, output[4]);
        //    Assert.Equal(0xe3082000, output[5]);
        //    Assert.Equal(0xe5002020, output[6]);
        //    Assert.Equal(0xe3003000, output[7]);
        //    Assert.Equal(0xe3403010, output[8]);
        //    Assert.Equal(0xe2533001, output[9]);
        //    Assert.Equal((uint)0x1afffffd, output[10]);
        //    Assert.Equal(0xe500202c, output[11]);
        //    Assert.Equal(0xe3003000, output[12]);
        //    Assert.Equal(0xe3403010, output[13]);
        //    Assert.Equal(0xe2533001, output[14]);
        //    Assert.Equal((uint)0x1afffffd, output[15]);
        //    Assert.Equal(0xeafffff4, output[16]);
        //}

        //[Fact]
        //public void ParserParseLabelReturnsCorrectIndex()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVT r0, 0x3f20" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Contains("loop", parser.LabelTable.Keys);
        //    Assert.Equal((uint)1, parser.LabelTable["loop"]);
        //}

        //[Fact]
        //public void ParserStartsOnLine1()
        //{
        //    IParser parser = new Parser();
        //    uint lineNumber = parser.LineNumber;

        //    Assert.Equal((uint)1, lineNumber);
        //}

        //[Fact]
        //public void ParserGetsCorrectLineNumber2()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVT r0, 0x3f20" + Environment.NewLine;

        //    parser.Parse(myProgram);
        //    uint lineNumber = parser.LineNumber;

        //    Assert.Equal((uint)2, lineNumber);
        //}

        //[Fact]
        //public void ParserGetsCorrectLineNumber3()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVT r0, 0x3f20" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    parser.Parse(myProgram);
        //    uint lineNumber = parser.LineNumber;

        //    Assert.Equal((uint)3, lineNumber);
        //}
        //#endregion

        //#region Data Statements

        //[Fact]
        //public void ParserCanParseDataAddress()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "address: 0x48" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x12, parser.KernelIndex);
        //}

        //[Fact]
        //public void ParserWillThrowIfAddressIsNotFollowedByANumber()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "address:" + Environment.NewLine;

        //    Assert.Throws<SyntaxException>(() =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void ParserCanParseDataByteWordSize()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "byte: 0x48, 0x69, 0x20, 0x00" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x00206948, output[0]);
        //}

        //[Fact]
        //public void ParserCanParseDataByteLessThanWordSize()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "byte: 0x48, 0x69, 0x20" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x00206948, output[0]);
        //}

        //[Fact]
        //public void ParserCanParseDataByteGreaterThanWordSize()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "byte: 0x48, 0x69, 0x20, 0x48, 0x69, 0x20" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x48206948, output[0]);
        //    Assert.Equal((uint)0x00002069, output[1]);
        //}

        //[Fact]
        //public void ParserWillThrowIfByteValueIsTooBig()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "byte: 0x100, 0x50, 0x40, 0x0" + Environment.NewLine;

        //    Assert.Throws<SyntaxException>(() =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void ParserCanParseWord()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "word: 0x48692000" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x48692000, output[0]);
        //}

        //[Fact]
        //public void ParserCanParseWord2()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "word: 0x48692048, 0x69200000" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x48692048, output[0]);
        //    Assert.Equal((uint)0x69200000, output[1]);
        //}
        //#endregion

        //#region ADDI

        //[Fact]
        //public void ParserParseADDI()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "ADDI a3, a3, 0x4" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0xe2822004, output[0]);
        //}
        //#endregion

        //#region ANDS

        //[Fact]
        //public void ParserParseAND()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "ANDS a3, a3, 0x20" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe2122020, output[0]);
        //}
        //#endregion

        //#region BAL
        //[Fact]
        //public void ParserParseBALParseNoLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BAL loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void ParserParseBALParseWithLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BAL loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal(0xeafffffd, output[1]);
        //}

        //[Fact]
        //public void ParserParseBALParseWithLabelAfter()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BAL loop" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0xeaffffff, output[0]);
        //    Assert.Equal(0xe3000000, output[1]);
        //}
        //#endregion

        //#region BEQ
        //[Fact]
        //public void ParserParseBEQParseNoLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BEQ loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void ParserParseBEQParseWithLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BEQ loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal((uint)0x0afffffd, output[1]);
        //}

        //[Fact]
        //public void ParserParseBEQParseWithLabelAfter()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BEQ loop" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x0affffff, output[0]);
        //    Assert.Equal(0xe3000000, output[1]);
        //}
        //#endregion

        //#region BL
        //[Fact]
        //public void ParserParseBLParseNoLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BL loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void ParserParseBLParseWithLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BL loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal(0xebfffffd, output[1]);
        //}

        //[Fact]
        //public void ParserParseBLParseWithLabelAfter()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BL loop" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xebffffff, output[0]);
        //    Assert.Equal(0xe3000000, output[1]);
        //}
        //#endregion

        //#region BNE
        //[Fact]
        //public void ParserParseBNEParseNoLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BNE loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void ParserParseBNEParseWithLabel()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BNE loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal((uint)0x1afffffd, output[1]);
        //}

        //[Fact]
        //public void ParserParseBNEParseWithLabelAfter()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "BNE loop" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x1affffff, output[0]);
        //    Assert.Equal(0xe3000000, output[1]);
        //}
        //#endregion

        //#region CMPI

        //[Fact]
        //public void ParserParseCMPI()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "CMPI a1, 0x4" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3500004, output[0]);
        //}
        //#endregion

        //#region LDR

        //[Fact]
        //public void ParserParseLDRPositive()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "LDR v4, sp, 0x8" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe59d7008, output[0]);
        //}

        ////// TODO: finish the LDR and STR negative instructions
        ////[Fact]
        ////public void ParserParseLDRNegitive()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "LDR v4, sp, -0x4" + Environment.NewLine;

        ////    ITokenStream tokenStream = new TokenStream(myProgram);
        ////    uint instruction;
        ////    parser.TryParseInstruction(tokenStream, out instruction, false);

        ////    Assert.Equal(0xe51d7ffc, instruction);
        ////}
        //#endregion

        //#region LDRB

        //[Fact]
        //public void ParserParseLDRBPositive()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "LDRB a1, v1, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe5d40000, output[0]);
        //}
        //#endregion

        //#region LDIIA

        ////[Fact]
        ////public void ParserParseLDIIAv4v80x0()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "LDIIA v4, sp, 0x0" + Environment.NewLine;

        ////    ITokenStream tokenStream = new TokenStream(myProgram);
        ////    uint instruction;
        ////    parser.TryParseInstruction(tokenStream, out instruction, false);

        ////    Assert.Equal(0xe4bd7000, instruction);
        ////}
        //#endregion

        //#region MOV
        //[Fact]
        //public void ParserParseMOVParser0r0()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOV r0, r0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe1a00000, output[0]);
        //}

        //[Fact]
        //public void ParserParseMOVParser0r1()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOV r0, r1" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe1a00001, output[0]);
        //}

        //[Fact]
        //public void ParserParseMOVNotRegr18()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOV r0, r18" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}
        //#endregion

        //#region MOVT
        //[Fact]
        //public void ParserParseMOVTParse0()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVT r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3400000, output[0]);
        //}

        //[Fact]
        //public void ParserParseMOVTParse1()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVT r1, 0x01" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3401001, output[0]);
        //}

        //[Fact]
        //public void ParserParseMOVTTooLarge()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVT r0, 0x10000" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}
        //#endregion

        //#region MOVW

        //[Fact]
        //public void ParserParseMOVWParse0()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //}

        //[Fact]
        //public void ParserParseMOVWParse1()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVW r1, 0x01" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3001001, output[0]);
        //}

        //[Fact]
        //public void ParserParseMOVWTooLarge()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "MOVW r0, 0x10000" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}
        //#endregion

        //#region POP

        //[Fact]
        //public void ParserParsePOPR1()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "POP r1" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe49d1004, output[0]);
        //}

        ////[Fact]
        ////public void ParserParsePOPR1InList()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "POP {r1}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe49d1004, output[0]);
        ////}

        ////[Fact]
        ////public void ParserParsePOPR1HyphenR2()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "POP {r1-r2}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe8bd0006, output[0]);
        ////}

        ////// TODO: finish the registerlist with hyphen instructions
        ////[Fact]
        ////public void ParserParsePOPR1HyphenR10()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "POP {r1-r10}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe8bd07fe, output[0]);
        ////}

        ////[Fact]
        ////public void ParserParsePOPR1CommaR2()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "POP {r1,r2}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe8bd0006, output[0]);
        ////}

        ////[Fact]
        ////public void ParserParsePOPR1CommaR10()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "POP {r1,r10}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe8bd0402, output[0]);
        ////}
        //#endregion

        //#region PUSH

        //[Fact]
        //public void ParserParsePUSHR1()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "PUSH r1" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe52d1004, output[0]);
        //}

        ////[Fact]
        ////public void ParserParsePUSHR1InList()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "PUSH {r1}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe52d1004, output[0]);
        ////}

        ////[Fact]
        ////public void ParserParsePUSHR1HyphenR2()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "PUSH {r1-r2}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe92d0006, output[0]);
        ////}

        ////// TODO: finish the registerlist with hyphen instructions
        ////[Fact]
        ////public void ParserParsePUSHR1HyphenR10()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "PUSH {r1-r10}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe92d07fe, output[0]);
        ////}

        ////[Fact]
        ////public void ParserParsePUSHR1CommaR2()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "PUSH {r1,r2}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe92d0006, output[0]);
        ////}

        ////[Fact]
        ////public void ParserParsePUSHR1CommaR10()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "PUSH {r1,r10}" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe92d0402, output[0]);
        ////}
        //#endregion

        //#region ROR

        //[Fact]
        //public void ParserParseROR()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "ROR r1, r2, #28" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe1a01e62, output[0]);
        //}
        //#endregion

        //#region STR

        //[Fact]
        //public void ParserParseSTRParse10()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "STR r1, r0, 0x10" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe5001010, output[0]);
        //}

        //[Fact]
        //public void ParserParseSTRTooLarge()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "STR r1, r2, 0x1000" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}
        //#endregion

        //#region STRDB

        ////[Fact]
        ////public void ParserParseSTRDBv4v80x0()
        ////{
        ////    IParser parser = new Parser();

        ////    var myProgram =
        ////        "STRDB v4, v8, 0x0" + Environment.NewLine;

        ////    var output = parser.Parse(myProgram);

        ////    Assert.Equal(0xe52b7000, output[0]);
        ////}
        //#endregion

        //#region SUBS

        //[Fact]
        //public void ParserParseSUBSParse01()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "SUBS r3, r3, 0x01" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe2533001, output[0]);
        //}

        //[Fact]
        //public void ParserParseSUBSTooLarge()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "SUBS r3, r3, 0x10000" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        var output = parser.Parse(myProgram);
        //    });
        //}
        //#endregion


        //[Fact]
        //public void ParserIf()
        //{
        //    IParser parser = new Parser();

        //    var myProgram =
        //        "IF r1 == 0x4 THEN yes ELSE no" + Environment.NewLine +
        //        "yes:" + Environment.NewLine +
        //        "MOVW r0, 0x0" + Environment.NewLine +
        //        "no:" + Environment.NewLine +
        //        "MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3510004, output[0]);
        //    Assert.Equal((uint)0x0a000000, output[1]);
        //    Assert.Equal(0xea000000, output[2]);
        //    Assert.Equal(0xe3000000, output[3]);
        //    Assert.Equal(0xe3000000, output[4]);
        //}

        //// Data-processing (immediate)
        //// $"{cond[4]}001{op[5]}{Rn[4]}{imm16}
        ////
        //// page 199
        //// AND  $"{cond}20{Rn}{imm16}" (Bitwise AND)
        //// ANDS $"{cond}21{Rn}{imm16}"
        //// EOR  $"{cond}22{Rn}{imm16}" (Bitwise Exclusive OR)
        //// EORS $"{cond}23{Rn}{imm16}"
        //// SUB  $"{cond}24{Rn}{imm16}" (Subtract)
        //// SUBS $"{cond}25{Rn}{imm16}"
        //// RSB  $"{cond}26{Rn}{imm16}" (Reverse Subtract)
        //// RSBS $"{cond}27{Rn}{imm16}"
        //// ADD  $"{cond}28{Rn}{imm16}" (Add)
        //// ADDS $"{cond}29{Rn}{imm16}"
        //// ADC  $"{cond}2a{Rn}{imm16}" (Add with Carry)
        //// ADCS $"{cond}2b{Rn}{imm16}"
        //// SBC  $"{cond}2a{Rn}{imm16}" (Subtract with Carry)
        //// SBCS $"{cond}2b{Rn}{imm16}"
        //// RSC  $"{cond}2c{Rn}{imm16}" (Reverse Subtract with Carry)
        //// RSCS $"{cond}2d{Rn}{imm16}"
        //// TST  $"{cond}31{Rn}{imm16}" (Test)
        //// TEQ  $"{cond}33{Rn}{imm16}" (Test Equivalence)
        //// CMP  $"{cond}35{Rn}{imm16}" (Compare)
        //// CMN  $"{cond}37{Rn}{imm16}" (Compare Negative)
        //// ORR  $"{cond}38{Rn}{imm16}" (Bitwise OR)
        //// ORRS $"{cond}39{Rn}{imm16}"
        //// MOV  $"{cond}3a{Rn}{imm16}" (Move)
        //// MOVS $"{cond}3b{Rn}{imm16}"
        //// BIC  $"{cond}3c{Rn}{imm16}" (Bitwise Bit Clear)
        //// BICS $"{cond}3d{Rn}{imm16}"
        //// MVN  $"{cond}3e{Rn}{imm16}" (Bitwise Not)
        //// MVNS $"{cond}3f{Rn}{imm16}"
        ////
        ////
        //// Load/store
        //// $"{cond[4]}01{A}{op1[5]}{Rn[4]}{imm11}{B}{imm4}
        ////
        //// p 0 = post, 1 = pre
        //// u 0 = down, 1 = up, (is imm12 positive or negative)
        //// b 0 = word, 1 = byte
        //// w 0 = no write back, 1 = write back
        //// l 0 = store, 1 = load
        ////
        //// page 208
        //// STR    $"{cond}010{P}{U}0{W}0{Rn}{Rt}{imm12}" (Store Immediate)
        //// STR    $"{cond}4     {U}000  {Rn}{Rt}{imm12}"
        ////         "e40{Rn}{Rt}{+imm12}"   // positive offset
        ////         "e48{Rn}{Rt}{-imm12}"   // negative offset

        //// i dont know if after this is correct
        //// STR    $"{cond}011{P}{U}0{W}0{Rn}{Rt}{imm5}{type[2]}0{Rm}" (Store Register)
        //// STR    $"{cond}6     {U}000  {Rn}{Rt}{imm5}{type[2]}0{Rm}"
        ////         "e60{Rn}{Rt}{+imm12}"
        ////         "e68{Rn}{Rt}{-imm12}"
        //// STRDB  $"{cond}6     {U}010  {Rn}{Rt}{imm12}" (Store Immediate Decrement Before)

        //// LDI    $"{cond}010{P}{U}0{W}1{Rn}{Rt}{imm12}" (Load Immediate)
        //// LDI    $"{cond}4     {U}001  {Rn}{Rt}{imm12}"
        ////         "e41{Rn}{Rt}{+imm12}"
        ////         "e49{Rn}{Rt}{-imm12}"

        //// LDIIA  $"{cond}4     {U}011  {Rn}{Rt}{imm12}" (Load Immediate Increment After)
        ////        $"e43{Rn}{Rt}{imm12}"
        ////        $"e4b{Rn}{Rt}{imm12}"


        //// LDR    $"{cond}011{P}{U}0{W}1{Rn}{Rt}{imm5}{type[2]}0{Rm}" (Load Register)
        //// LDR    $"{cond}6     {U}001  {Rn}{Rt}{imm5}{type[2]}0{Rm}"
        ////         "e61{Rn}{Rt}{+imm12}"
        ////         "e69{Rn}{Rt}{-imm12}"
        ////
        ////
        //// page 536
        //// POP A1 reg list is larger than 1
        //// POP A1     $"{cond}100010111101{register_list}"
        ////             "e8bd{register_list}"
        ////
        //// POP A2     $"{cond}010010011101{rt[4]}000000000100"
        ////             "e49d{rt}004"

        //// page 539
        //// PUSH A1 means reg list has more than one reg
        //// PUSH (A1)  $"{cond}100100101101{register_list}"
        ////             "e92d{register_list}"
        ////
        //// PUSH A2 means reg list has one reg
        //// PUSH (A2)  $"{cond}010100101101{rt[4]}000000000100"
        ////             "e52d{rt}004"
    }
}