namespace SimpleAssembler.Lexer
{
    using LexTokens;
    using System.Collections.Generic;
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
            var t1 = _tokenStream.Next(); // t1 is the first token we care about

            if (t1 != null)
            {
                // NewLine is still a NewLine
                if (t1 is NewLineToken)
                {
                    return new NewLineLexToken(t1.Value());
                }

                // AlphaNumUnderscore can only be a label (and only if t2 is a colon)
                else if (t1 is AlphaNumUnderscoreToken)
                {
                    var t2 = _tokenStream.Next();

                    if (t2 != null)
                    {
                        if (t2 is ColonToken)
                        {
                            return new LabelDeclarationLexToken(t1.Value()); // throw away the colon
                        }
                        else
                        {
                            //_tokenStream.UnGet(t2); // put the 'not'colon back
                            throw new LexSyntaxException($"The only valid token after an AlphaNumUnderscoreToken is a colon.  current is {t2.GetType()}");
                        }
                    }
                    else // t2 IS null
                    {
                        throw new LexSyntaxException("The token after an AlphaNumUnderscoreToken cannot be null");
                    }

                }

                // AlphaNum can be a label, opcode, or special (address, byte, word)
                else if (t1 is AlphaNumToken)
                {
                    var t1Val = (t1 as AlphaNumToken).Value().ToLowerInvariant();

                    // here t1 is an alphanum
                    var t2 = _tokenStream.Next();

                    if (t2 != null)
                    {
                        // t2 is colon so t1 is a label TODO:::(or special)
                        if (t2 is ColonToken)
                        {
                            if (t1Val.Equals("address"))
                            {
                                var next = _tokenStream.Next();
                                if (next is NumberToken)
                                {
                                    _lexTokenStack.Push(new NumberLexToken(next.Value()));
                                }
                                else
                                {
                                    throw new LexSyntaxException($"address: needs to be followed by a number");
                                }
                                return new AddressDataStatementLexToken(t1Val);
                            }
                            else if (t1Val.Equals("word")
                                || t1Val.Equals("byte"))
                            {
                                Stack<NumberToken> numbers = new Stack<NumberToken>();
                                var next = _tokenStream.Next();
                                while (next != null 
                                    && (next.GetType() == typeof(NumberToken) || next.GetType() == typeof(CommaToken)))
                                {
                                    if (next is NumberToken)
                                    {
                                        numbers.Push(next as NumberToken);
                                    }
                                    next = _tokenStream.Next();
                                }

                                while (numbers.Count > 0)
                                {
                                    var current = numbers.Pop();
                                    _lexTokenStack.Push(new NumberLexToken(current.Value()));
                                }

                                if (t1Val.Equals("word"))
                                {
                                    return new WordDataStatementLexToken(t1Val);
                                }
                                else // its a byte
                                {
                                    return new ByteDataStatementLexToken(t1Val);
                                }
                            }
                            else // it was a label
                            {

                                return new LabelDeclarationLexToken(t1.Value()); // throw away the colon
                            }
                        }

                        // if t1 is alphanum and t2 is AlphaNumUnderscore, it has to be a branch to label
                        if (t2 is AlphaNumUnderscoreToken)
                        {
                            _lexTokenStack.Push(new LabelReferenceLexToken(t2.Value())); // keep t2
                            return new OpCodeLexToken(t1.Value()); // return op
                        }

                        // if t1 is alphanum and t2 is alpha num, it could be a branch to label
                        if (t2 is AlphaNumToken)
                            // if t1 is opcode
                            if ((t1 as AlphaNumToken).IsOpCode())
                            {

                                // if instuction is: $"{op} {reg}, {reg}, {imm12}"
                                if ((t1Val.Equals("ands")
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
                                        && reg2 is AlphaNumToken
                                        && (reg2 as AlphaNumToken).IsRegister()
                                        && comma2 != null
                                        && comma2 is CommaToken
                                        && num != null
                                        && num is NumberToken)
                                    {
                                        _lexTokenStack.Push(new NumberLexToken(num.Value())); // keep num
                                        _lexTokenStack.Push(new RegisterLexToken(reg2.Value())); // keep reg2
                                        _lexTokenStack.Push(new RegisterLexToken(t2.Value())); // keep t2
                                        return new OpCodeLexToken(t1.Value()); // return op
                                    }
                                    else
                                    {
                                        throw new LexSyntaxException($"{t1Val} should be followed by a register, a comma, a register, a comma, and a number");
                                    }
                                }

                                // if instuction is: $"{op} {reg}, {reg2}"
                                else if ((t1Val.Equals("mov"))
                                    && (t2 as AlphaNumToken).IsRegister())
                                {
                                    var comma = _tokenStream.Next();
                                    var reg2 = _tokenStream.Next();

                                    if (comma != null
                                        && comma is CommaToken
                                        && reg2 != null
                                        && reg2 is AlphaNumToken
                                        && (reg2 as AlphaNumToken).IsRegister())
                                    {
                                        _lexTokenStack.Push(new RegisterLexToken(reg2.Value())); // keep reg2
                                        _lexTokenStack.Push(new RegisterLexToken(t2.Value())); // keep t2
                                        return new OpCodeLexToken(t1.Value()); // return op
                                    }
                                    else
                                    {
                                        throw new LexSyntaxException($"{t1Val} should be followed by a register, a comma, and a register");
                                    }
                                }

                                // if instuction is: $"{op} {reg}, {imm16}"
                                else if ((t1Val.Equals("movt")
                                    || t1Val.Equals("movw"))
                                    && (t2 as AlphaNumToken).IsRegister())
                                {
                                    var comma = _tokenStream.Next();
                                    var num = _tokenStream.Next();

                                    if (comma != null
                                        && comma is CommaToken
                                        && num != null
                                        && num is NumberToken)
                                    {
                                        _lexTokenStack.Push(new NumberLexToken(num.Value())); // keep num
                                        _lexTokenStack.Push(new RegisterLexToken(t2.Value())); // keep t2
                                        return new OpCodeLexToken(t1.Value()); // return op
                                    }
                                    else
                                    {
                                        throw new LexSyntaxException($"{t1Val} should be followed by a register, a comma, and a number.");
                                    }
                                }

                                // if instuction is: $"{op} {reg}"
                                else if ((t1Val.Equals("pop")
                                    || t1Val.Equals("push"))
                                    && (t2 as AlphaNumToken).IsRegister())
                                {
                                    _lexTokenStack.Push(new RegisterLexToken(t2.Value())); // keep t2
                                    return new OpCodeLexToken(t1.Value()); // return op
                                }

                                // if instuction is: $"{op} {label}"
                                else if ((t1Val.Equals("bal")
                                    || t1Val.Equals("bl")
                                    || t1Val.Equals("bne"))
                                    && (t2 is AlphaNumToken || t2 is AlphaNumUnderscoreToken))
                                {
                                    _lexTokenStack.Push(new LabelReferenceLexToken(t2.Value())); // keep t2
                                    return new OpCodeLexToken(t1.Value()); // return op
                                }

                                // else 
                                else
                                {
                                    throw new LexSyntaxException($"Unknown lex instuction t2: {t2.Value()}");
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
                    throw new LexSyntaxException($"t1 is an AlphaNum ({t1.Value()}) but t2 is null");
                }

                else // t1 is not a valid type (not a NewLine, AlphaNumUnderscore, AlphaNum)
                {
                    _tokenStream.UnGet(t1);
                    throw new LexSyntaxException($"t1 is not a valid type (not a NewLine, AlphaNumUnderscore, AlphaNum). (t1={t1.Value()}");
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