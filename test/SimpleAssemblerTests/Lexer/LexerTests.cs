namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Lexer;
    using SimpleAssembler.Lexer.LexTokens;
    using System;
    using Xunit;

    public class LexerTests
    {
        [Fact]
        public void LexerLabelDeclaration()
        {
            var lexer = new Lexer("loop:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationToken), token1);
            Assert.Equal("loop", token1.Value());
        }

        [Fact]
        public void LexerLabelDeclarationUpper()
        {
            var lexer = new Lexer("LOOP:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationToken), token1);
            Assert.Equal("loop", token1.Value());
        }

        [Fact]
        public void LexerLabelDeclarationMixedCase()
        {
            var lexer = new Lexer("LooP:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationToken), token1);
            Assert.Equal("loop", token1.Value());
        }

        [Fact]
        public void LexerLabelDeclarationWithNum()
        {
            var lexer = new Lexer("loop1:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationToken), token1);
            Assert.Equal("loop1", token1.Value());
        }

        [Fact]
        public void LexerLabelDeclarationWithUnderscore()
        {
            var lexer = new Lexer("lo_op:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationToken), token1);
            Assert.Equal("lo_op", token1.Value());
        }

        [Fact]
        public void LexerBLWithUnderscore()
        {
            var lexer = new Lexer("BL initialize_uart");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(LabelReferenceToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
        }

        [Fact]
        public void LexerBLWithUnderscoreAndNewLine()
        {
            var lexer = new Lexer("BL initialize_uart\r\n");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart
            var token3 = lexer.Next(); // \r\n
            var token4 = lexer.Next(); // null

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(LabelReferenceToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineToken), token3);
            Assert.Null(token4);
        }

        [Fact]
        public void LexerBLWithUnderscoreAndCommentAndNewLine()
        {
            var lexer = new Lexer($"BL initialize_uart //this is a comment{Environment.NewLine}");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart
            var token3 = lexer.Next(); // \r\n
            var token4 = lexer.Next(); // null

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(LabelReferenceToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineToken), token3);
            Assert.Null(token4);
        }

        [Fact]
        public void LexerBLWithUnderscoreAndCommentAndSlashRSlashN()
        {
            var lexer = new Lexer($"BL initialize_uart //this is a comment\r\n");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart
            var token3 = lexer.Next(); // \r\n
            var token4 = lexer.Next(); // null

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(LabelReferenceToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineToken), token3);
            Assert.Null(token4);
        }

        [Fact]
        public void LexerMovInstruction()
        {
            var lexer = new Lexer("MOV pc, lr");
            var token1 = lexer.Next();
            var token2 = lexer.Next();
            var token3 = lexer.Next();

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.MOV, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(RegisterToken), token2);
            Assert.Equal("f", token2.Value());
            Assert.IsType(typeof(RegisterToken), token3);
            Assert.Equal("e", token3.Value());
        }

        [Fact]
        public void LexerMOVWInstruction0x0()
        {
            var lexer = new Lexer("MOVW a1, 0x0");
            var token1 = lexer.Next(); // MOVW
            var token2 = lexer.Next(); // a1
            var token3 = lexer.Next(); // 0x0

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.MOVW, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(RegisterToken), token2);
            Assert.Equal("0", token2.Value());
            Assert.IsType(typeof(NumberToken), token3);
            Assert.Equal("0x0", token3.Value());
        }

        [Fact]
        public void LexerMOVWInstruction0x9000()
        {
            var lexer = new Lexer("MOVW sp, 0x9000");
            var token1 = lexer.Next(); // MOVW
            var token2 = lexer.Next(); // sp
            var token3 = lexer.Next(); // 0x9000

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.MOVW, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(RegisterToken), token2);
            Assert.Equal("d", token2.Value());
            Assert.IsType(typeof(NumberToken), token3);
            Assert.Equal("0x9000", token3.Value());
        }

        [Fact]
        public void LexerMOVTInstruction0x3f20()
        {
            var lexer = new Lexer("MOVT a1, 0x3f20");
            var token1 = lexer.Next(); // MOVT
            var token2 = lexer.Next(); // a1
            var token3 = lexer.Next(); // 0x3f20

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.MOVT, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(RegisterToken), token2);
            Assert.Equal("0", token2.Value());
            Assert.IsType(typeof(NumberToken), token3);
            Assert.Equal("0x3f20", token3.Value());
        }

        [Fact]
        public void LexerPOPInstruction1Register()
        {
            var lexer = new Lexer("POP a2");
            var token1 = lexer.Next(); // POP
            var token2 = lexer.Next(); // a2

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.POP, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(RegisterToken), token2);
            Assert.Equal("1", token2.Value());
        }

        [Fact]
        public void LexerPUSHInstruction1Register()
        {
            var lexer = new Lexer("PUSH a3");
            var token1 = lexer.Next();
            var token2 = lexer.Next();

            Assert.IsType(typeof(OpCodeToken), token1);
            Assert.Equal(OperationType.PUSH, (token1 as OpCodeToken).OperationType);
            Assert.IsType(typeof(RegisterToken), token2);
            Assert.Equal("2", token2.Value());
        }
        //[Fact]
        //public void TokenizerMovUndIsAlphaNumUnd()
        //{
        //    var tokenizer = new Tokenizer("mov_mov");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(AlphaNumUnderscoreToken), token);
        //    Assert.Equal("mov_mov", token.Value());
        //}

        //[Fact]
        //public void TokenizerColonIsColon()
        //{
        //    var tokenizer = new Tokenizer(":");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(ColonToken), token);
        //    Assert.Equal(":", token.Value());
        //}

        //[Fact]
        //public void TokenizerCommaIsComma()
        //{
        //    var tokenizer = new Tokenizer(",");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(CommaToken), token);
        //    Assert.Equal(",", token.Value());
        //}

        //[Fact]
        //public void TokenizerLeftCurlyIsLeftCurly()
        //{
        //    var tokenizer = new Tokenizer("{");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(LeftCurlyToken), token);
        //    Assert.Equal("{", token.Value());
        //}

        //[Fact]
        //public void TokenizerNewLineIsNewLine()
        //{
        //    var tokenizer = new Tokenizer("\r\n");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(NewLineToken), token);
        //    Assert.Equal("\r\n", token.Value());
        //}

        //[Fact]
        //public void TokenizerHexDigitIsNumberToken()
        //{
        //    var tokenizer = new Tokenizer("0x10");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(NumberToken), token);
        //    Assert.Equal("0x10", token.Value());
        //    Assert.Equal(16, (token as NumberToken).IntValue());
        //}

        //[Fact]
        //public void TokenizerDecimalDigitIsNumberToken()
        //{
        //    var tokenizer = new Tokenizer("#4");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(NumberToken), token);
        //    Assert.Equal("0x4", token.Value());
        //    Assert.Equal(4, (token as NumberToken).IntValue());
        //}

        //[Fact]
        //public void TokenizerRightCurlyIsRightCurly()
        //{
        //    var tokenizer = new Tokenizer("}");
        //    var token = tokenizer.Next();

        //    Assert.IsType(typeof(RightCurlyToken), token);
        //    Assert.Equal("}", token.Value());
        //}

        //[Fact]
        //public void TokenizerWordAndColonIsAlphaNumAndColon()
        //{
        //    var tokenizer = new Tokenizer("loop:");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();

        //    Assert.IsType(typeof(AlphaNumToken), token1);
        //    Assert.Equal("loop", token1.Value());

        //    Assert.IsType(typeof(ColonToken), token2);
        //    Assert.Equal(":", token2.Value());
        //}

        //[Fact]
        //public void TokenizerWordUndAndColonIsAlphaNumUndAndColon()
        //{
        //    var tokenizer = new Tokenizer("loop_loop:");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();

        //    Assert.IsType(typeof(AlphaNumUnderscoreToken), token1);
        //    Assert.Equal("loop_loop", token1.Value());

        //    Assert.IsType(typeof(ColonToken), token2);
        //    Assert.Equal(":", token2.Value());
        //}

        //[Fact]
        //public void TokenizerWordAndCommaIsAlphaNumAndComma()
        //{
        //    var tokenizer = new Tokenizer("r1,");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();

        //    Assert.IsType(typeof(AlphaNumToken), token1);
        //    Assert.Equal("r1", token1.Value());

        //    Assert.IsType(typeof(CommaToken), token2);
        //    Assert.Equal(",", token2.Value());
        //}

        //[Fact]
        //public void TokenizerInstructionWithTwoParams()
        //{
        //    var tokenizer = new Tokenizer("mov r1, #0");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();
        //    var token3 = tokenizer.Next();
        //    var token4 = tokenizer.Next();

        //    Assert.IsType(typeof(AlphaNumToken), token1);
        //    Assert.Equal("mov", token1.Value());

        //    Assert.IsType(typeof(AlphaNumToken), token2);
        //    Assert.Equal("r1", token2.Value());

        //    Assert.IsType(typeof(CommaToken), token3);
        //    Assert.Equal(",", token3.Value());

        //    Assert.IsType(typeof(NumberToken), token4);
        //    Assert.Equal("0x0", token4.Value());
        //    Assert.Equal(0, (token4 as NumberToken).IntValue());
        //}

        //[Fact]
        //public void TokenizerRegisterListOneRegister()
        //{
        //    var tokenizer = new Tokenizer("{r1}");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();
        //    var token3 = tokenizer.Next();

        //    Assert.IsType(typeof(LeftCurlyToken), token1);
        //    Assert.Equal("{", token1.Value());

        //    Assert.IsType(typeof(AlphaNumToken), token2);
        //    Assert.Equal("r1", token2.Value());

        //    Assert.IsType(typeof(RightCurlyToken), token3);
        //    Assert.Equal("}", token3.Value());
        //}

        //[Fact]
        //public void TokenizerRegisterListTwoRegistersComma()
        //{
        //    var tokenizer = new Tokenizer("{r1,r2}");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();
        //    var token3 = tokenizer.Next();
        //    var token4 = tokenizer.Next();
        //    var token5 = tokenizer.Next();

        //    Assert.IsType(typeof(LeftCurlyToken), token1);
        //    Assert.Equal("{", token1.Value());

        //    Assert.IsType(typeof(AlphaNumToken), token2);
        //    Assert.Equal("r1", token2.Value());

        //    Assert.IsType(typeof(CommaToken), token3);
        //    Assert.Equal(",", token3.Value());

        //    Assert.IsType(typeof(AlphaNumToken), token4);
        //    Assert.Equal("r2", token4.Value());

        //    Assert.IsType(typeof(RightCurlyToken), token5);
        //    Assert.Equal("}", token5.Value());
        //}

        //[Fact]
        //public void TokenizerRegisterListTwoRegistersHyphen()
        //{
        //    var tokenizer = new Tokenizer("{r1-r2}");
        //    var token1 = tokenizer.Next();
        //    var token2 = tokenizer.Next();
        //    var token3 = tokenizer.Next();
        //    var token4 = tokenizer.Next();
        //    var token5 = tokenizer.Next();

        //    Assert.IsType(typeof(LeftCurlyToken), token1);
        //    Assert.Equal("{", token1.Value());

        //    Assert.IsType(typeof(AlphaNumToken), token2);
        //    Assert.Equal("r1", token2.Value());

        //    Assert.IsType(typeof(HyphenToken), token3);
        //    Assert.Equal("-", token3.Value());

        //    Assert.IsType(typeof(AlphaNumToken), token4);
        //    Assert.Equal("r2", token4.Value());

        //    Assert.IsType(typeof(RightCurlyToken), token5);
        //    Assert.Equal("}", token5.Value());
        //}
    }
}
