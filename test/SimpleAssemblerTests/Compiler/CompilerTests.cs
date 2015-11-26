namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Tokenizer;
    using SimpleAssembler.Tokenizer.Tokens;
    using System;
    using Xunit;

    public class CompilerTests
    {
        //#region parser

        //[Fact]
        //public void ReturnsUIntArray()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();
        //    var result = parser.Parse("");

        //    Assert.IsType(typeof(uint[]), result);
        //}

        //[Fact]
        //public void CanCompileLabelAlphaNum()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    uint labelIndex = uint.MaxValue;
        //    ITokenStream tokenStream = new TokenStream("loop:");

        //    var result = parser.TryParseLabel(tokenStream, out labelIndex, true);

        //    Assert.True(result);
        //    Assert.Equal((uint)0, labelIndex);
        //    Assert.False(tokenStream.HasNext());
        //}

        //[Fact]
        //public void CanCompileLabelAlphaNumUnderscore()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    uint labelIndex = uint.MaxValue;
        //    ITokenStream tokenStream = new TokenStream("lo_op:");

        //    var result = parser.TryParseLabel(tokenStream, out labelIndex, true);

        //    Assert.True(result);
        //    Assert.Equal((uint)0, labelIndex);
        //    Assert.False(tokenStream.HasNext());
        //}

        ////[Fact]
        ////public void ParserTryParseLabelReturnsTokensToStream()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    uint labelIndex = uint.MaxValue;
        ////    ITokenStream tokenStream = new TokenStream("MOVT r0, 0x3f000000");

        ////    var result = parser.TryParseLabel(tokenStream, out labelIndex, true);

        ////    Assert.False(result);
        ////    Assert.Equal(uint.MaxValue, labelIndex);
        ////    Assert.True(tokenStream.HasNext());

        ////    var token1 = tokenStream.Next();
        ////    Assert.IsType(typeof(AlphaNumToken), token1);
        ////}

        //[Fact]
        //public void CanCompileLab4()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

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

        ////[Fact]
        ////public void ParserParseLabelReturnsCorrectIndex()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    var myProgram =
        ////        "MOVT r0, 0x3f20" + Environment.NewLine +
        ////        "loop: MOVW r0, 0x0" + Environment.NewLine;

        ////    parser.Parse(myProgram);
        ////    uint labelIndex = parser.KernelIndex;

        ////    // the loop branch should be at 1 making the current index 2
        ////    Assert.Equal((uint)2, labelIndex);
        ////}

        ////[Fact]
        ////public void ParserGetsCorrectLineNumberLine0()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();
        ////    uint lineNumber = parser.LineNumber;

        ////    Assert.Equal((uint)0, lineNumber);
        ////}

        ////[Fact]
        ////public void ParserGetsCorrectLineNumber1()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    var myProgram =
        ////        "MOVT r0, 0x3f20" + Environment.NewLine;

        ////    parser.Parse(myProgram);
        ////    uint lineNumber = parser.LineNumber;

        ////    Assert.Equal((uint)1, lineNumber);
        ////}

        ////[Fact]
        ////public void ParserGetsCorrectLineNumber2()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    var myProgram =
        ////        "MOVT r0, 0x3f20" + Environment.NewLine +
        ////        "loop: MOVW r0, 0x0" + Environment.NewLine;

        ////    parser.Parse(myProgram);
        ////    uint lineNumber = parser.LineNumber;

        ////    Assert.Equal((uint)2, lineNumber);
        ////}
        //#endregion

        ////#region Data Statements

        ////[Fact]
        ////public void ParserCanParseDataByteWordSize()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    uint data = uint.MaxValue;
        ////    ITokenStream tokenStream = new TokenStream("byte: 0x48, 0x69, 0x20, 0x00");

        ////    var result = parser.TryParseData(tokenStream, out data);

        ////    Assert.True(result);
        ////    Assert.Equal((uint)0x48692000, data);
        ////    Assert.False(tokenStream.HasNext());
        ////}

        ////[Fact]
        ////public void ParserCanParseDataByteLessThanWordSize()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    ITokenStream tokenStream = new TokenStream("byte: 0x48, 0x69, 0x20");

        ////    Assert.Throws(typeof(SyntaxException), () =>
        ////    {
        ////        uint dataIndex;
        ////        parser.TryParseData(tokenStream, out dataIndex);
        ////    });
        ////}

        ////[Fact]
        ////public void ParserCanParseDataByteGreaterThanWordSize()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    ITokenStream tokenStream = new TokenStream("byte: 0x48, 0x69, 0x20, 0x48, 0x69, 0x20");

        ////    Assert.Throws(typeof(SyntaxException), () =>
        ////    {
        ////        uint dataIndex;
        ////        parser.TryParseData(tokenStream, out dataIndex);
        ////    });
        ////}
        ////#endregion

        //#region ANDS

        //[Fact]
        //public void CompileANDS()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "ANDS a3, a3, 0x20" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    // ANDS $"{cond}21{Rn}{Rd}{imm12}"

        //    Assert.Equal(0xe2122020, instruction);
        //}
        //#endregion

        //#region BAL
        //[Fact]
        //public void CompileBALWithUndeclaredLabelWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "BAL loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void CompileBAL()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BAL loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal(0xeafffffd, output[1]);
        //}

        //[Fact]
        //public void CompileBALWithLabelDeclaredAfter()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "BAL loop" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0xeaffffff, output[0]);
        //    Assert.Equal(0xe3000000, output[1]);
        //}
        //#endregion

        //#region BL
        //[Fact]
        //public void CompileBLWithUndeclaredLabelWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "BL loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void CompileBL()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BL loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal(0xebfffffd, output[1]);
        //}

        //[Fact]
        //public void CompileBLWithLabelDeclaredAfter()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

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
        //public void CompileBNEWithUndeclaredLabelWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "BNE loop" + Environment.NewLine;

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        parser.Parse(myProgram);
        //    });
        //}

        //[Fact]
        //public void CompileBNE()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "loop: MOVW r0, 0x0" + Environment.NewLine +
        //        "BNE loop" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal(0xe3000000, output[0]);
        //    Assert.Equal((uint)0x1afffffd, output[1]);
        //}

        //[Fact]
        //public void CompileBNEWithLabelDeclaredAfter()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "BNE loop" + Environment.NewLine +
        //        "loop: MOVW r0, 0x0" + Environment.NewLine;

        //    var output = parser.Parse(myProgram);

        //    Assert.Equal((uint)0x1affffff, output[0]);
        //    Assert.Equal(0xe3000000, output[1]);
        //}
        //#endregion

        //#region LDR

        //[Fact]
        //public void CompileLDRPositive()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "LDR v4, sp, 0x8" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe59d7008, instruction);
        //}

        ////// TODO: finish the LDR and STR negative instructions
        ////[Fact]
        ////public void CompileLDRNegitive()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    var myProgram =
        ////        "LDR v4, sp, -0x4" + Environment.NewLine;

        ////    ITokenStream tokenStream = new TokenStream(myProgram);
        ////    uint instruction;
        ////    parser.TryParseInstruction(tokenStream, out instruction, false);

        ////    Assert.Equal(0xe51d7ffc, instruction);
        ////}
        //#endregion

        //#region MOV
        //[Fact]
        //public void CompileMOVR0R0()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOV r0, r0" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe1a00000, instruction);
        //}

        //[Fact]
        //public void CompileMOVR0R1()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOV r0, r1" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe1a00001, instruction);
        //}

        //[Fact]
        //public void CompileMOVWithInvalidRegisterWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOV r0, r18" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        uint instruction;
        //        parser.TryParseInstruction(tokenStream, out instruction, false);
        //    });
        //}
        //#endregion

        //#region MOVT

        //[Fact]
        //public void CompileMOVT0()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOVT r0, 0x0" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe3400000, instruction);
        //}

        //[Fact]
        //public void CompileMOVT1()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOVT r1, 0x01" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe3401001, instruction);
        //}

        //[Fact]
        //public void CompileMOVTWithNumberTooLargeWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOVT r0, 0x10000" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        uint instruction;
        //        parser.TryParseInstruction(tokenStream, out instruction, false);
        //    });
        //}
        //#endregion

        //#region MOVW

        //[Fact]
        //public void CompileMOVW0()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOVW r0, 0x0" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe3000000, instruction);
        //}

        //[Fact]
        //public void CompileMOVW1()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOVW r1, 0x01" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe3001001, instruction);
        //}

        //[Fact]
        //public void CompileMOVWWithNumberTooLargeWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "MOVW r0, 0x10000" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        uint instruction;
        //        parser.TryParseInstruction(tokenStream, out instruction, false);
        //    });
        //}
        //#endregion

        //#region POP

        //[Fact]
        //public void CompilePOP()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "POP r1" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe49d1004, instruction); // ????
        //}

        //[Fact]
        //public void CompilePOP1RegInRegList()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "POP {r1}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe49d1004, instruction);
        //}

        //[Fact]
        //public void CompilePOPR1HyphenR2()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "POP {r1-r2}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe8bd0006, instruction);
        //}

        ////// TODO: finish the registerlist with hyphen instructions
        ////[Fact]
        ////public void CompilePOPR1HyphenR10()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    var myProgram =
        ////        "POP {r1-r10}" + Environment.NewLine;

        ////    ITokenStream tokenStream = new TokenStream(myProgram);
        ////    uint instruction;
        ////    parser.TryParseInstruction(tokenStream, out instruction, false);

        ////    Assert.Equal(0xe8bd07fe, instruction);
        ////}

        //[Fact]
        //public void CompilePOPR1CommaR2()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "POP {r1,r2}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe8bd0006, instruction);
        //}

        //[Fact]
        //public void CompilePOPR1CommaR10()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "POP {r1,r10}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe8bd0402, instruction);
        //}
        //#endregion

        //#region PUSH

        //[Fact]
        //public void CompilePUSH()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "PUSH r1" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe52d1004, instruction);
        //}

        //[Fact]
        //public void CompilePUSH1RegInRegList()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "PUSH {r1}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe52d1004, instruction);
        //}

        //[Fact]
        //public void CompilePUSHR1HyphenR2()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "PUSH {r1-r2}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe92d0006, instruction);
        //}

        ////// TODO: finish the registerlist with hyphen instructions
        ////[Fact]
        ////public void CompilePUSHR1HyphenR10()
        ////{
        ////    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        ////    var myProgram =
        ////        "PUSH {r1-r10}" + Environment.NewLine;

        ////    ITokenStream tokenStream = new TokenStream(myProgram);
        ////    uint instruction;
        ////    parser.TryParseInstruction(tokenStream, out instruction, false);

        ////    Assert.Equal(0xe92d07fe, instruction);
        ////}

        //[Fact]
        //public void CompilePUSHR1CommaR2()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "PUSH {r1,r2}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe92d0006, instruction);
        //}

        //[Fact]
        //public void CompilePUSHR1CommaR10()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "PUSH {r1,r10}" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe92d0402, instruction);
        //}
        //#endregion

        //#region STR

        //[Fact]
        //public void CompileSTRParse10()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "STR r1, r0, 0x10" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe5001010, instruction);
        //}

        //[Fact]
        //public void CompileSTRTooLarge()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "STR r1, r2, 0x1000" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        uint instruction;
        //        parser.TryParseInstruction(tokenStream, out instruction, false);
        //    });
        //}
        //#endregion
        
        //#region SUBS

        //[Fact]
        //public void CompileSUBS1()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "SUBS r3, r3, 0x01" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);
        //    uint instruction;
        //    parser.TryParseInstruction(tokenStream, out instruction, false);

        //    Assert.Equal(0xe2533001, instruction);
        //}

        //[Fact]
        //public void CompileSUBSTooLargeWillThrow()
        //{
        //    SimpleAssembler.Parser.Parser parser = new SimpleAssembler.Parser.Parser();

        //    var myProgram =
        //        "SUBS r3, r3, 0x10000" + Environment.NewLine;

        //    ITokenStream tokenStream = new TokenStream(myProgram);

        //    Assert.Throws(typeof(SyntaxException), () =>
        //    {
        //        uint instruction;
        //        parser.TryParseInstruction(tokenStream, out instruction, false);
        //    });
        //}
        //#endregion

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
