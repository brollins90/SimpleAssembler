namespace SimpleAssembler.Lexer.LexTokens
{
    public class OperatorLexToken : LexToken
    {
        public OperatorLexToken(string value)
            : base(value)
        { }

        public string Condition
        {
            get
            {
                if (Value().Equals("=="))
                    return "0";
                else if (Value().Equals("!="))
                    return "1";
                else if (Value().Equals(">="))
                    return "a";
                else if (Value().Equals("<"))
                    return "b";
                else if (Value().Equals(">"))
                    return "c";
                else if (Value().Equals("<="))
                    return "d";

                return "e";
            }
        }
    }
}