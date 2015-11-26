namespace SimpleAssembler.Lexer
{
    using System.Collections.Generic;
    using LexTokens;
    using Tokenizer;
    using Tokenizer.Tokens;

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
            var t1 = _tokenStream.Next();

            if (t1 != null
                && t1 is SimpleAssembler.Tokenizer.Tokens.NewLineToken)
            {
                return new LexTokens.NewLineToken(t1.Value());
            }

            else if ((t1 is AlphaNumToken)
                || (t1 is AlphaNumUnderscoreToken))
            {
                var t2 = _tokenStream.Next();

                // t2 is colon
                if (t2 != null
                    && t2 is ColonToken)
                {
                    // throw away t2
                    return new LabelDeclarationToken(t1.Value());
                }

                // t2 is comma
                else if (t2 != null
                  && t2 is CommaToken
                  && t1 != null
                  && (t1 as AlphaNumToken).IsRegister()) // todo: i dont think we get here ?? maybe do if i push back on the alpha alpha line
                {
                    // throw away t2
                    return new RegisterToken(t1.Value());
                }

                // t2 is alphanumund  ( could be a branch )
                else if (t2 != null
                  && (t2 is AlphaNumUnderscoreToken || t2 is AlphaNumToken)
                  && t1 != null
                  && (t1 as AlphaNumToken).IsBranchOpCode())
                {
                    _lexTokenStack.Push(new LabelReferenceToken(t2.Value()));
                    return new OpCodeToken(t1.Value());
                }

                //// t2 is alphanum
                //else if (t2 != null
                //  && t2 is AlphaNumToken
                //  && t1 != null
                //  && (t1 as AlphaNumToken).IsOpCode())
                //{
                //    return new OpCodeToken(t1.Value());
                //}

                else
                {
                    _tokenStream.UnGet(t2);
                }
            }
            else
            {
                _tokenStream.UnGet(t1);
            }

            throw new SyntaxException($"t1: {t1.Value()}");
        }

        //protected LexToken GetNext()
        //{
        //    List<Token> tokens = new List<Token>();
        //    Stack<LexToken> lexTokens = new Stack<LexToken>();
        //    bool stillReading = true;
        //    LexerState state = LexerState.None;

        //    while (_tokenStream.HasNext() && stillReading)
        //    {
        //        var current = _tokenStream.Next();
        //        tokens.Add(current);

        //        switch (state)
        //        {
        //            case LexerState.None:

        //                // alphanum
        //                if (current.GetType() == typeof(AlphaNumToken)
        //                    || current.GetType() == typeof(AlphaNumUnderscoreToken))
        //                {
        //                    state = LexerState.AlphaNum;
        //                }
        //                break;

        //            case LexerState.AlphaNum:

        //                if (current.GetType() == typeof(AlphaNumToken))
        //                {
        //                    // probably an op
        //                    var previous = tokens[tokens.Count - 2];

        //                    if ((current as AlphaNumToken).IsRegister())
        //                    {
        //                        // registers have to be followed by an opcode
        //                        if ((previous as AlphaNumToken).IsOpCode())
        //                        {
        //                            lexTokens.Push(new OpCodeToken(previous.Value()));
        //                        }
        //                    }

        //                    if ((current as AlphaNumToken).IsOpCode())
        //                    {
        //                        // registers have to be followed by an opcode
        //                        if ((previous as AlphaNumToken).IsOpCode())
        //                        {
        //                            lexTokens.Push(new OpCodeToken(previous.Value()));
        //                        }
        //                    }
        //                }
        //                else if (current.GetType() == typeof(AlphaNumUnderscoreToken))
        //                {
        //                    // alpha followed by alphaUnd branch to label

        //                }
        //                else if (current.GetType() == typeof(ColonToken))
        //                {
        //                    // AlphaNum -> Colon = LabelDeclaration
        //                    // TODO: or address...
        //                    current = tokens.Pop(); // current is current
        //                    current = tokens.Pop(); // current is previous
        //                    // if current is address...
        //                    return new LabelDeclarationToken(current.Value());
        //                }
        //                break;
        //        }


        //    }
        //    //_tokenStream.Next()
        //    return null;
        //}
    }

    public enum LexerState
    {
        None,
        AlphaNum
    }
}