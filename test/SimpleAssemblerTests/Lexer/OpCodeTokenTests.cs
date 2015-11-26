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
        public void OperationTypeMOV()
        {
            OpCodeToken token = new OpCodeToken("MOV");

            Assert.Equal("mov", token.Value());
            Assert.Equal(OperationType.MOV, token.OperationType);
        }
    }
}