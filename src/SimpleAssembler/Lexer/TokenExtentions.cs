﻿namespace Simple.Tokenizer.Tokens
{
    public static class TokenExtentions
    {
        public static bool IsRegister(this AlphaNumToken token)
        {
            var lower = token.Value().ToLowerInvariant();

            if (lower.Equals("r0")
                || lower.Equals("r1")
                || lower.Equals("r2")
                || lower.Equals("r3")
                || lower.Equals("r4")
                || lower.Equals("r5")
                || lower.Equals("r6")
                || lower.Equals("r7")
                || lower.Equals("r8")
                || lower.Equals("r9")
                || lower.Equals("r10")
                || lower.Equals("r11")
                || lower.Equals("r12")
                || lower.Equals("r13")
                || lower.Equals("r14")
                || lower.Equals("r15")
                || lower.Equals("a1")
                || lower.Equals("a2")
                || lower.Equals("a3")
                || lower.Equals("a4")
                || lower.Equals("v1")
                || lower.Equals("v2")
                || lower.Equals("v3")
                || lower.Equals("v4")
                || lower.Equals("v5")
                || lower.Equals("v6")
                || lower.Equals("v7")
                || lower.Equals("v8")
                || lower.Equals("sb")
                || lower.Equals("sl")
                || lower.Equals("fp")
                || lower.Equals("ip")
                || lower.Equals("sp")
                || lower.Equals("lr")
                || lower.Equals("pc"))
            {
                return true;
            }
            return false;
        }

        public static bool IsOpCode(this AlphaNumToken token)
        {
            var lower = token.Value().ToLowerInvariant();

            if (lower.Equals("addi")
                || lower.Equals("ands")
                || lower.Equals("andrs")
                || lower.Equals("bal")
                || lower.Equals("beq")
                || lower.Equals("bge")
                || lower.Equals("bl")
                || lower.Equals("bne")
                || lower.Equals("cmpi")
                || lower.Equals("cps")
                || lower.Equals("cpsid")
                || lower.Equals("cpsie")
                || lower.Equals("ldr")
                || lower.Equals("ldrb")
                || lower.Equals("mov")
                || lower.Equals("movt")
                || lower.Equals("movw")
                || lower.Equals("mulrs")
                || lower.Equals("orrs")
                || lower.Equals("orrrs")
                || lower.Equals("pop")
                || lower.Equals("push")
                || lower.Equals("ror")
                || lower.Equals("str")
                || lower.Equals("subs"))
            {
                return true;
            }
            return false;
        }

        public static bool IsOperation(this SpecialToken token)
        {
            var lower = token.Value().ToLowerInvariant();

            if (lower.Equals("<")
                || lower.Equals(">")
                || lower.Equals("==")
                || lower.Equals("<=")
                || lower.Equals(">=")
                || lower.Equals("!="))
            {
                return true;
            }
            return false;
        }
    }
}