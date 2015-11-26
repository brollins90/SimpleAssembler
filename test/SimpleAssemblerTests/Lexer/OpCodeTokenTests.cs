namespace SimpleAssemblerTests.Tokenizer
{
    using SimpleAssembler.Lexer;
    using SimpleAssembler.Lexer.LexTokens;
    using Xunit;

    public class OpCodeTokenTests
    {
        [Fact]
        public void OperationTypeNONE()
        {
            Assert.Throws<LexSyntaxException>(() =>
            {
                OpCodeToken token = new OpCodeToken("NONE");
            });
        }

        [Fact]
        public void OperationTypeANDS()
        {
            OpCodeToken token = new OpCodeToken("ANDS");

            Assert.Equal("ands", token.Value());
            Assert.Equal(OperationType.ANDS, token.OperationType);
        }

        [Fact]
        public void OperationTypeBAL()
        {
            OpCodeToken token = new OpCodeToken("BAL");

            Assert.Equal("bal", token.Value());
            Assert.Equal(OperationType.BAL, token.OperationType);
        }

        [Fact]
        public void OperationTypeBL()
        {
            OpCodeToken token = new OpCodeToken("BL");

            Assert.Equal("bl", token.Value());
            Assert.Equal(OperationType.BL, token.OperationType);
        }

        [Fact]
        public void OperationTypeBNE()
        {
            OpCodeToken token = new OpCodeToken("BNE");

            Assert.Equal("bne", token.Value());
            Assert.Equal(OperationType.BNE, token.OperationType);
        }

        [Fact]
        public void OperationTypeLDR()
        {
            OpCodeToken token = new OpCodeToken("LDR");

            Assert.Equal("ldr", token.Value());
            Assert.Equal(OperationType.LDR, token.OperationType);
        }

        [Fact]
        public void OperationTypeMOV()
        {
            OpCodeToken token = new OpCodeToken("MOV");

            Assert.Equal("mov", token.Value());
            Assert.Equal(OperationType.MOV, token.OperationType);
        }

        [Fact]
        public void OperationTypeMOVT()
        {
            OpCodeToken token = new OpCodeToken("MOVT");

            Assert.Equal("movt", token.Value());
            Assert.Equal(OperationType.MOVT, token.OperationType);
        }

        [Fact]
        public void OperationTypeMOVW()
        {
            OpCodeToken token = new OpCodeToken("MOVW");

            Assert.Equal("movw", token.Value());
            Assert.Equal(OperationType.MOVW, token.OperationType);
        }

        [Fact]
        public void OperationTypePOP()
        {
            OpCodeToken token = new OpCodeToken("POP");

            Assert.Equal("pop", token.Value());
            Assert.Equal(OperationType.POP, token.OperationType);
        }

        [Fact]
        public void OperationTypePUSH()
        {
            OpCodeToken token = new OpCodeToken("PUSH");

            Assert.Equal("push", token.Value());
            Assert.Equal(OperationType.PUSH, token.OperationType);
        }

        [Fact]
        public void OperationTypeSTR()
        {
            OpCodeToken token = new OpCodeToken("STR");

            Assert.Equal("str", token.Value());
            Assert.Equal(OperationType.STR, token.OperationType);
        }

        [Fact]
        public void OperationTypeSUBS()
        {
            OpCodeToken token = new OpCodeToken("SUBS");

            Assert.Equal("subs", token.Value());
            Assert.Equal(OperationType.SUBS, token.OperationType);
        }
    }
}