namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Lexer;
    using SimpleAssembler.Lexer.LexTokens;
    using System;
    using Xunit;

    public class LexerTests
    {
        [Fact]
        public void LabelDeclarationIsAlphaFollowedByColon()
        {
            var lexer = new Lexer("loop:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationLexToken), token1);
            Assert.Equal("loop", token1.Value());
        }

        [Fact]
        public void LabelDeclarationIsAlphaFollowedByColonUpperCase()
        {
            var lexer = new Lexer("LOOP:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationLexToken), token1);
            Assert.Equal("loop", token1.Value());
        }

        [Fact]
        public void LabelDeclarationIsAlphaFollowedByColonMixedCase()
        {
            var lexer = new Lexer("LooP:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationLexToken), token1);
            Assert.Equal("loop", token1.Value());
        }

        [Fact]
        public void LabelDeclarationIsAlphaNumFollowedByColon()
        {
            var lexer = new Lexer("loop1:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationLexToken), token1);
            Assert.Equal("loop1", token1.Value());
        }

        [Fact]
        public void LabelDeclarationIsAlphaNumWithUnderscoreFollowedByColon()
        {
            var lexer = new Lexer("lo_op:");
            var token1 = lexer.Next();

            Assert.IsType(typeof(LabelDeclarationLexToken), token1);
            Assert.Equal("lo_op", token1.Value());
        }

        [Fact]
        public void BLToAlpha()
        {
            var lexer = new Lexer("BL loop");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(LabelReferenceLexToken), token2);
            Assert.Equal("loop", token2.Value());
        }

        [Fact]
        public void BLToAlphaWithUnderscore()
        {
            var lexer = new Lexer("BL initialize_uart");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(LabelReferenceLexToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
        }

        [Fact]
        public void BLToAlphaWithUnderscoreAndSlashRSlashN()
        {
            var lexer = new Lexer("BL initialize_uart\r\n");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart
            var token3 = lexer.Next(); // \r\n
            var token4 = lexer.Next(); // null

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(LabelReferenceLexToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineLexToken), token3);
            Assert.Null(token4);
        }

        [Fact]
        public void BLToAlphaWithUnderscoreAndNewLine()
        {
            var lexer = new Lexer($"BL initialize_uart{Environment.NewLine}");
            var token1 = lexer.Next(); // BL
            var token2 = lexer.Next(); // initialize_uart
            var token3 = lexer.Next(); // \r\n
            var token4 = lexer.Next(); // null

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(LabelReferenceLexToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineLexToken), token3);
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

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(LabelReferenceLexToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineLexToken), token3);
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

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.BL, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(LabelReferenceLexToken), token2);
            Assert.Equal("initialize_uart", token2.Value());
            Assert.IsType(typeof(NewLineLexToken), token3);
            Assert.Null(token4);
        }

        [Fact]
        public void LexerMovInvalidArg2()
        {
            var lexer = new Lexer("MOV pc, 0x0");

            Assert.Throws<LexSyntaxException>(() =>
            {
                var token1 = lexer.Next();
                var token2 = lexer.Next();
                var token3 = lexer.Next();
            });
        }

        [Fact]
        public void LexerMovInvalidArg1_1()
        {
            var lexer = new Lexer("MOV 0x0");

            Assert.Throws<LexSyntaxException>(() =>
            {
                var token1 = lexer.Next();
                var token2 = lexer.Next();
                var token3 = lexer.Next();
            });
        }

        [Fact]
        public void LexerMovInvalidArg1_2()
        {
            var lexer = new Lexer("MOV 0x0, 0x0");

            Assert.Throws<LexSyntaxException>(() =>
            {
                var token1 = lexer.Next();
                var token2 = lexer.Next();
                var token3 = lexer.Next();
            });
        }

        [Fact]
        public void LexerMovInvalidArg1_3()
        {
            var lexer = new Lexer("MOV 0x0, r2");

            Assert.Throws<LexSyntaxException>(() =>
            {
                var token1 = lexer.Next();
                var token2 = lexer.Next();
                var token3 = lexer.Next();
            });
        }

        [Fact]
        public void LexerMovInstruction()
        {
            var lexer = new Lexer("MOV pc, lr");
            var token1 = lexer.Next();
            var token2 = lexer.Next();
            var token3 = lexer.Next();

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.MOV, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(RegisterLexToken), token2);
            Assert.Equal("f", token2.Value());
            Assert.IsType(typeof(RegisterLexToken), token3);
            Assert.Equal("e", token3.Value());
        }

        [Fact]
        public void LexerMOVWInstruction0x0()
        {
            var lexer = new Lexer("MOVW a1, 0x0");
            var token1 = lexer.Next(); // MOVW
            var token2 = lexer.Next(); // a1
            var token3 = lexer.Next(); // 0x0

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.MOVW, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(RegisterLexToken), token2);
            Assert.Equal("0", token2.Value());
            Assert.IsType(typeof(NumberLexToken), token3);
            Assert.Equal("0x0", token3.Value());
        }

        [Fact]
        public void LexerMOVWInstruction0x9000()
        {
            var lexer = new Lexer("MOVW sp, 0x9000");
            var token1 = lexer.Next(); // MOVW
            var token2 = lexer.Next(); // sp
            var token3 = lexer.Next(); // 0x9000

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.MOVW, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(RegisterLexToken), token2);
            Assert.Equal("d", token2.Value());
            Assert.IsType(typeof(NumberLexToken), token3);
            Assert.Equal("0x9000", token3.Value());
        }

        [Fact]
        public void LexerMOVTInstruction0x3f20()
        {
            var lexer = new Lexer("MOVT a1, 0x3f20");
            var token1 = lexer.Next(); // MOVT
            var token2 = lexer.Next(); // a1
            var token3 = lexer.Next(); // 0x3f20

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.MOVT, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(RegisterLexToken), token2);
            Assert.Equal("0", token2.Value());
            Assert.IsType(typeof(NumberLexToken), token3);
            Assert.Equal("0x3f20", token3.Value());
        }

        [Fact]
        public void LexerPOPInstruction1Register()
        {
            var lexer = new Lexer("POP a2");
            var token1 = lexer.Next(); // POP
            var token2 = lexer.Next(); // a2

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.POP, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(RegisterLexToken), token2);
            Assert.Equal("1", token2.Value());
        }

        [Fact]
        public void LexerPUSHInstruction1Register()
        {
            var lexer = new Lexer("PUSH a3");
            var token1 = lexer.Next();
            var token2 = lexer.Next();

            Assert.IsType(typeof(OpCodeLexToken), token1);
            Assert.Equal(OperationType.PUSH, (token1 as OpCodeLexToken).OperationType);
            Assert.IsType(typeof(RegisterLexToken), token2);
            Assert.Equal("2", token2.Value());
        }

        [Fact]
        public void LexerAddressDataStatement()
        {
            var lexer = new Lexer("ADDRESS: 0x9000");
            var token1 = lexer.Next();
            var token2 = lexer.Next();

            Assert.IsType(typeof(AddressDataStatementLexToken), token1);
            Assert.IsType(typeof(NumberLexToken), token2);
            Assert.Equal("0x9000", token2.Value());
        }

        [Fact]
        public void LexerByteDataStatement()
        {
            var lexer = new Lexer("BYTE: 0x48, 0x69, 0x20, 0x0");
            var token1 = lexer.Next();
            var token2 = lexer.Next();
            var token3 = lexer.Next();
            var token4 = lexer.Next();
            var token5 = lexer.Next();

            Assert.IsType(typeof(ByteDataStatementLexToken), token1);
            Assert.IsType(typeof(NumberLexToken), token2);
            Assert.Equal("0x48", token2.Value());
            Assert.IsType(typeof(NumberLexToken), token3);
            Assert.Equal("0x69", token3.Value());
            Assert.IsType(typeof(NumberLexToken), token4);
            Assert.Equal("0x20", token4.Value());
            Assert.IsType(typeof(NumberLexToken), token5);
            Assert.Equal("0x0", token5.Value());
        }

        [Fact]
        public void LexerWordDataStatement()
        {
            var lexer = new Lexer("WORD: 0x48692000");
            var token1 = lexer.Next();
            var token2 = lexer.Next();

            Assert.IsType(typeof(WordDataStatementLexToken), token1);
            Assert.IsType(typeof(NumberLexToken), token2);
            Assert.Equal("0x48692000", token2.Value());
        }
    }
}
