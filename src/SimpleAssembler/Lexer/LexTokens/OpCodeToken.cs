namespace SimpleAssembler.Lexer.LexTokens
{
    using System;

    public class OpCodeToken : LexToken
    {
        public OperationType OperationType { get; }

        public OpCodeToken(string value)
            : base(value)
        {
            OperationType = ParseOperationType(value);
        }

        private OperationType ParseOperationType(string value)
        {
            if (value.Equals("mov", StringComparison.InvariantCultureIgnoreCase))
            {
                return OperationType.MOV;
            }
            throw new LexSyntaxException($"{value} is not a valid operation type");
        }
    }

    public enum OperationType
    {
        MOV
    }
}