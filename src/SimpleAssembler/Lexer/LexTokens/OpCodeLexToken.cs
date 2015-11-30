namespace SimpleAssembler.Lexer.LexTokens
{
    using Simple;
    using System;

    public class OpCodeLexToken : LexToken
    {
        public OperationType OperationType { get; }

        public OpCodeLexToken(string value)
            : base(value)
        {
            OperationType = ParseOperationType(value);
        }

        private OperationType ParseOperationType(string value)
        {
            if (value.Equals("addi", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.ADDI;
            else if (value.Equals("ands", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.ANDS;
            else if (value.Equals("bal", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BAL;
            else if (value.Equals("beq", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BEQ;
            else if (value.Equals("bge", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BGE;
            else if (value.Equals("bl", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BL;
            else if (value.Equals("bne", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.BNE;
            else if (value.Equals("cmpi", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.CMPI;
            else if (value.Equals("cps", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.CPS;
            else if (value.Equals("cpsid", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.CPSID;
            else if (value.Equals("cpsie", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.CPSIE;
            else if (value.Equals("ldr", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.LDR;
            else if (value.Equals("ldrb", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.LDRB;
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
            else if (value.Equals("ror", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.ROR;
            else if (value.Equals("str", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.STR;
            else if (value.Equals("subs", StringComparison.InvariantCultureIgnoreCase))
                return OperationType.SUBS;

            throw new SyntaxException($"{value} is not a valid operation type");
        }
    }

    public enum OperationType
    {
        ADDI,
        ANDS,
        BAL,
        BEQ,
        BGE,
        BL,
        BNE,
        CMPI,
        CPS,
        CPSID,
        CPSIE,
        LDR,
        LDRB,
        MOV,
        MOVT,
        MOVW,
        POP,
        PUSH,
        ROR,
        STR,
        SUBS
    }
}