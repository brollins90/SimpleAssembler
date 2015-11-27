namespace SimpleAssemblerTests.Lexer
{
    using SimpleAssembler;
    using SimpleAssembler.Lexer.LexTokens;
    using Xunit;

    public class OpCodeTokenTests
    {
        [Fact]
        public void OperationTypeNONE()
        {
            Assert.Throws<SyntaxException>(() =>
            {
                OpCodeLexToken token = new OpCodeLexToken("NONE");
            });
        }

        [Fact]
        public void OperationTypeANDS()
        {
            OpCodeLexToken token = new OpCodeLexToken("ANDS");

            Assert.Equal("ands", token.Value());
            Assert.Equal(OperationType.ANDS, token.OperationType);
        }

        [Fact]
        public void OperationTypeADDI()
        {
            OpCodeLexToken token = new OpCodeLexToken("ADDI");

            Assert.Equal("addi", token.Value());
            Assert.Equal(OperationType.ADDI, token.OperationType);
        }

        [Fact]
        public void OperationTypeBAL()
        {
            OpCodeLexToken token = new OpCodeLexToken("BAL");

            Assert.Equal("bal", token.Value());
            Assert.Equal(OperationType.BAL, token.OperationType);
        }

        [Fact]
        public void OperationTypeBEQ()
        {
            OpCodeLexToken token = new OpCodeLexToken("BEQ");

            Assert.Equal("beq", token.Value());
            Assert.Equal(OperationType.BEQ, token.OperationType);
        }

        [Fact]
        public void OperationTypeBL()
        {
            OpCodeLexToken token = new OpCodeLexToken("BL");

            Assert.Equal("bl", token.Value());
            Assert.Equal(OperationType.BL, token.OperationType);
        }

        [Fact]
        public void OperationTypeBNE()
        {
            OpCodeLexToken token = new OpCodeLexToken("BNE");

            Assert.Equal("bne", token.Value());
            Assert.Equal(OperationType.BNE, token.OperationType);
        }

        [Fact]
        public void OperationTypeCMPI()
        {
            OpCodeLexToken token = new OpCodeLexToken("CMPI");

            Assert.Equal("cmpi", token.Value());
            Assert.Equal(OperationType.CMPI, token.OperationType);
        }

        [Fact]
        public void OperationTypeLDR()
        {
            OpCodeLexToken token = new OpCodeLexToken("LDR");

            Assert.Equal("ldr", token.Value());
            Assert.Equal(OperationType.LDR, token.OperationType);
        }

        [Fact]
        public void OperationTypeMOV()
        {
            OpCodeLexToken token = new OpCodeLexToken("MOV");

            Assert.Equal("mov", token.Value());
            Assert.Equal(OperationType.MOV, token.OperationType);
        }

        [Fact]
        public void OperationTypeMOVT()
        {
            OpCodeLexToken token = new OpCodeLexToken("MOVT");

            Assert.Equal("movt", token.Value());
            Assert.Equal(OperationType.MOVT, token.OperationType);
        }

        [Fact]
        public void OperationTypeMOVW()
        {
            OpCodeLexToken token = new OpCodeLexToken("MOVW");

            Assert.Equal("movw", token.Value());
            Assert.Equal(OperationType.MOVW, token.OperationType);
        }

        [Fact]
        public void OperationTypePOP()
        {
            OpCodeLexToken token = new OpCodeLexToken("POP");

            Assert.Equal("pop", token.Value());
            Assert.Equal(OperationType.POP, token.OperationType);
        }

        [Fact]
        public void OperationTypePUSH()
        {
            OpCodeLexToken token = new OpCodeLexToken("PUSH");

            Assert.Equal("push", token.Value());
            Assert.Equal(OperationType.PUSH, token.OperationType);
        }

        [Fact]
        public void OperationTypeSTR()
        {
            OpCodeLexToken token = new OpCodeLexToken("STR");

            Assert.Equal("str", token.Value());
            Assert.Equal(OperationType.STR, token.OperationType);
        }

        [Fact]
        public void OperationTypeSUBS()
        {
            OpCodeLexToken token = new OpCodeLexToken("SUBS");

            Assert.Equal("subs", token.Value());
            Assert.Equal(OperationType.SUBS, token.OperationType);
        }
    }
}