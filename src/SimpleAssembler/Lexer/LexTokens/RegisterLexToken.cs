namespace SimpleAssembler.Lexer.LexTokens
{
    using Simple;

    public class RegisterLexToken : LexToken
    {
        private string _value;

        public RegisterLexToken(string value)
            : base(value)
        {
            _value = ParseRegister(value);
        }

        public override string Value()
        {
            return _value;
        }

        private string ParseRegister(string value)
        {
            value = value.ToLowerInvariant();

            if (value.Equals("r0"))
                return "0";
            if (value.Equals("r1"))
                return "1";
            if (value.Equals("r2"))
                return "2";
            if (value.Equals("r3"))
                return "3";
            if (value.Equals("r4"))
                return "4";
            if (value.Equals("r5"))
                return "5";
            if (value.Equals("r6"))
                return "6";
            if (value.Equals("r7"))
                return "7";
            if (value.Equals("r8"))
                return "8";
            if (value.Equals("r9"))
                return "9";
            if (value.Equals("r10"))
                return "a";
            if (value.Equals("r11"))
                return "b";
            if (value.Equals("r12"))
                return "c";
            if (value.Equals("r13"))
                return "d";
            if (value.Equals("r14"))
                return "e";
            if (value.Equals("r15"))
                return "f";
            if (value.Equals("a1"))
                return "0";
            if (value.Equals("a2"))
                return "1";
            if (value.Equals("a3"))
                return "2";
            if (value.Equals("a4"))
                return "3";
            if (value.Equals("v1"))
                return "4";
            if (value.Equals("v2"))
                return "5";
            if (value.Equals("v3"))
                return "6";
            if (value.Equals("v4"))
                return "7";
            if (value.Equals("v5"))
                return "8";
            if (value.Equals("v6"))
                return "9";
            if (value.Equals("v7"))
                return "a";
            if (value.Equals("v8"))
                return "b";
            if (value.Equals("sb"))
                return "9";
            if (value.Equals("sl"))
                return "a";
            if (value.Equals("fp"))
                return "b";
            if (value.Equals("ip"))
                return "c";
            if (value.Equals("sp"))
                return "d";
            if (value.Equals("lr"))
                return "e";
            if (value.Equals("pc"))
                return "f";
            throw new SyntaxException($"{value} is not a valid register");
        }
    }
}