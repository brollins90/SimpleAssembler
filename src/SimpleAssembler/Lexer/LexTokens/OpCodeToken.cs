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
            if (value.Equals("ands", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.ANDS;
            else if (value.Equals("bal", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BAL;
            else if (value.Equals("bl", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BL;
            else if (value.Equals("bne", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BNE;
            else if (value.Equals("ldr", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.LDR;
            else if (value.Equals("mov", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.MOV;
            else if (value.Equals("movt", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.MOVT;
            else if (value.Equals("movw", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.MOVW;
            else if (value.Equals("pop", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.POP;
            else if (value.Equals("push", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.PUSH;
            else if (value.Equals("str", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.STR;
            else if (value.Equals("subs", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.SUBS;

            throw new LexSyntaxException($"{value} is not a valid operation type");
        }
    }

    public enum OperationType
    {
        ANDS,
        BAL,
        BL,
        BNE,
        LDR,
        MOV,
        MOVT,
        MOVW,
        POP,
        PUSH,
        STR,
        SUBS
    }
}