namespace SimpleCompiler.Lexer
{
    using LexTokens;
    using Simple.Tokenizer;
    using Simple.Tokenizer.Tokens;
    using System.Collections.Generic;

    public class Lexer : ILexer
    {
        private Stack<LexToken> _lexTokenStack;
        private ITokenStream _tokenStream;

        public Lexer(string fileData)
        {
            _lexTokenStack = new Stack<LexToken>();
            _tokenStream = new TokenStream(fileData);
        }

        public bool HasNext()
        {
            return (_tokenStream == null) ? false : _tokenStream.HasNext();
        }

        public LexToken Next()
        {
            return (_lexTokenStack.Count > 0) ? _lexTokenStack.Pop() : GetNext();
        }

        public void UnGet(LexToken lexToken)
        {
            if (lexToken != null)
            {
                _lexTokenStack.Push(lexToken);
            }
        }

        protected LexToken GetNext()
        {
            var current = _tokenStream.Next();

            if (current != null)
            {
                // alphanum
                // alphanumunderscore
                // colon
                // hypen ??
                // leftcurly

                // leftparan
                if (current is LeftParenToken)
                {
                    return new LeftParenLexToken(current.Value());
                }

                // new line
                else if (current is NewLineToken)
                {
                    return new NewLineLexToken(current.Value());
                }

                // number
                else if (current is NumberToken)
                {
                    return new NumberLexToken(current.Value());
                }

                // right curly
                // right paren
                else if (current is RightParenToken)
                {
                    return new RightParenLexToken(current.Value());
                }

                // special
                else if (current is SpecialToken)
                {
                    var special = current as SpecialToken;
                    if (special.Value().Equals("+", System.StringComparison.InvariantCultureIgnoreCase))
                        return new BinaryOperatorLexToken(special.Value());
                    else if (special.Value().Equals("-", System.StringComparison.InvariantCultureIgnoreCase))
                        return new BinaryOperatorLexToken(special.Value());
                    else if (special.Value().Equals("*", System.StringComparison.InvariantCultureIgnoreCase))
                        return new BinaryOperatorLexToken(special.Value());
                    else if (special.Value().Equals("/", System.StringComparison.InvariantCultureIgnoreCase))
                        return new BinaryOperatorLexToken(special.Value());
                    else if (special.Value().Equals("%", System.StringComparison.InvariantCultureIgnoreCase))
                        return new BinaryOperatorLexToken(special.Value());
                }
            }
            return null;
        }
    }
}