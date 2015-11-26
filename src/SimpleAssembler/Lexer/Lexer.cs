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

            if (t1 != null)
            {
                // NewLine is still a NewLine
                if (t1 is SimpleAssembler.Tokenizer.Tokens.NewLineToken)
                {
                    return new LexTokens.NewLineToken(t1.Value());
                }

                // AlphaNumUnderscore can only be a label
                if (t1 is AlphaNumUnderscoreToken)
                {
                    var t2 = _tokenStream.Next();

                    if (t2 != null
                        && t2 is ColonToken)
                    {
                        // throw away t2
                        return new LabelDeclarationToken(t1.Value());
                    }
                    else
                    {
                        _tokenStream.UnGet(t2);
                    }
                }

                // AlphaNum can be a label, opcode, or special (address, byte, word)
                if (t1 is AlphaNumToken)
                {
                    var t2 = _tokenStream.Next();

                    if (t2 != null)
                    {
                        // t2 is colon so t1 is a label
                        if (t2 is ColonToken)
                        {
                            // throw away t2
                            return new LabelDeclarationToken(t1.Value());
                        }

                        if ((t1 as AlphaNumToken).IsOpCode())
                        {
                            var t1Val = (t1 as AlphaNumToken).Value().ToLowerInvariant();
                            if ((t1Val.Equals("ands") // $"{op} {reg}, {reg}, {imm12}"
                                || t1Val.Equals("ldr")
                                || t1Val.Equals("str")
                                || t1Val.Equals("subs"))
                                && (t2 as AlphaNumToken).IsRegister())
                            {
                                var comma = _tokenStream.Next();
                                var reg2 = _tokenStream.Next();
                                var comma2 = _tokenStream.Next();
                                var num = _tokenStream.Next();

                                if (comma != null
                                    && comma is CommaToken
                                    && reg2 != null
                                    && (reg2 as AlphaNumToken).IsRegister()
                                    && comma2 != null
                                    && comma2 is CommaToken
                                    && num != null
                                    && num is SimpleAssembler.Tokenizer.Tokens.NumberToken)
                                {
                                    _lexTokenStack.Push(new LexTokens.NumberToken(num.Value())); // keep num
                                    _lexTokenStack.Push(new RegisterToken(reg2.Value())); // keep reg2
                                    _lexTokenStack.Push(new RegisterToken(t2.Value())); // keep t2
                                    return new OpCodeToken(t1.Value()); // return op
                                }
                            }
                            else if ((t1Val.Equals("mov"))
                                && (t2 as AlphaNumToken).IsRegister()) // $"{op} {reg}, {reg2}"
                            {
                                var comma = _tokenStream.Next();
                                var reg2 = _tokenStream.Next();

                                if (comma != null
                                    && comma is CommaToken
                                    && reg2 != null
                                    && (reg2 as AlphaNumToken).IsRegister())
                                {
                                    _lexTokenStack.Push(new RegisterToken(reg2.Value())); // keep reg2
                                    _lexTokenStack.Push(new RegisterToken(t2.Value())); // keep t2
                                    return new OpCodeToken(t1.Value()); // return op
                                }
                            }
                            else if ((t1Val.Equals("movt") // $"{op} {reg}, {imm16}"
                                || t1Val.Equals("movw"))
                                && (t2 as AlphaNumToken).IsRegister())
                            {
                                var comma = _tokenStream.Next();
                                var num = _tokenStream.Next();

                                if (comma != null
                                    && comma is CommaToken
                                    && num != null
                                    && num is SimpleAssembler.Tokenizer.Tokens.NumberToken)
                                {
                                    _lexTokenStack.Push(new LexTokens.NumberToken(num.Value())); // keep num
                                    _lexTokenStack.Push(new RegisterToken(t2.Value())); // keep t2
                                    return new OpCodeToken(t1.Value()); // return op
                                }
                            }
                            else if ((t1Val.Equals("pop") // $"{op} {reg}"
                                || t1Val.Equals("push"))
                                && (t2 as AlphaNumToken).IsRegister())
                            {
                                _lexTokenStack.Push(new RegisterToken(t2.Value())); // keep t2
                                return new OpCodeToken(t1.Value()); // return op
                            }
                            else if ((t1Val.Equals("bal") // $"{op} {label}"
                                || t1Val.Equals("bl")
                                || t1Val.Equals("bne"))
                                && (t2 is AlphaNumToken || t2 is AlphaNumUnderscoreToken))
                            {
                                _lexTokenStack.Push(new LabelReferenceToken(t2.Value())); // keep t2
                                return new OpCodeToken(t1.Value()); // return op
                            }
                        }

                        //// t2 is comma
                        //if (t2 is CommaToken
                        //  && (t1 as AlphaNumToken).IsRegister()) ///// // todo: i dont think we get here ?? maybe do if i push back on the alpha alpha line
                        //{
                        //    // throw away t2
                        //    return new RegisterToken(t1.Value());
                        //}

                        //// t2 is AlphaNum || AlphaNumUnderscore ( could be a branch )
                        //if ((t2 is AlphaNumToken || t2 is AlphaNumUnderscoreToken)
                        //  && (t1 as AlphaNumToken).IsBranchOpCode())
                        //{
                        //    _lexTokenStack.Push(new LabelReferenceToken(t2.Value())); // keep t2
                        //    return new OpCodeToken(t1.Value()); // return branch op
                        //}

                        //// t2 is alphanum
                        //if (t2 is AlphaNumToken
                        //  && (t2 as AlphaNumToken).IsRegister() // we might miss a valid branch instruction because of this, but it should have been found earlier
                        //  && (t1 as AlphaNumToken).IsOpCode())
                        //{
                        //    var t3 = _tokenStream.Next();
                        //    if (t3 != null)
                        //    {
                        //        if (t3 is CommaToken)
                        //        {
                        //            _tokenStream.UnGet(t3); // put the comma back
                        //            _tokenStream.UnGet(t2); // put the register
                        //        }
                        //        else
                        //        {
                        //            _tokenStream.UnGet(t3);
                        //            _lexTokenStack.Push(new RegisterToken(t2.Value()));
                        //        }
                        //    }
                        //    else
                        //    {
                        //        _lexTokenStack.Push(new RegisterToken(t2.Value()));
                        //    }
                        //    return new OpCodeToken(t1.Value());
                        //}

                        // something else
                        else
                        {
                            _tokenStream.UnGet(t2);
                        }
                        throw new LexSyntaxException($"t1: {t1.Value()}, t2: {t2.Value()}");

                    } // t2 is null
                    throw new LexSyntaxException($"t2 was null, but we couldnt figure out what an alone t1 is. t1: {t1.Value()}");
                }
                else
                {
                    _tokenStream.UnGet(t1);
                }

                throw new LexSyntaxException($"t1: {t1.Value()}");

            } // t1 is null
            return null;
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