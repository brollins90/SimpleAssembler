namespace SimpleCompiler.Lexer
{
    using LexTokens;
    using Simple.Tokenizer;
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
            var t1 = _tokenStream.Next(); // t1 is the first token we care about
            return null;
            //if (t1 != null)
            //{
            //    // NewLine is still a NewLine
            //    if (t1 is NewLineToken)
            //    {
            //        return new NewLineLexToken(t1.Value());
            //    }

            //    // AlphaNumUnderscore can only be a label (and only if t2 is a colon)
            //    else if (t1 is AlphaNumUnderscoreToken)
            //    {
            //        var t2 = _tokenStream.Next();

            //        if (t2 != null)
            //        {
            //            if (t2 is ColonToken)
            //            {
            //                return new IdentifierLexToken(t1.Value()); // throw away the colon
            //            }
            //            else
            //            {
            //                //_tokenStream.UnGet(t2); // put the 'not'colon back
            //                throw new SyntaxException($"The only valid token after an AlphaNumUnderscoreToken is a colon.  current is {t2.GetType()}");
            //            }
            //        }
            //        else // t2 IS null
            //        {
            //            throw new SyntaxException("The token after an AlphaNumUnderscoreToken cannot be null");
            //        }

            //    }

            //    // AlphaNum can be a label, opcode, or special (address, byte, word)
            //    else if (t1 is AlphaNumToken)
            //    {
            //        var t1Val = (t1 as AlphaNumToken).Value().ToLowerInvariant();

            //        if (t1Val.Equals("if"))
            //        {
            //            var operand1 = _tokenStream.Next();
            //            var operation = _tokenStream.Next();
            //            var operand2 = _tokenStream.Next();
            //            var thenStatement = _tokenStream.Next();
            //            var thenLabel = _tokenStream.Next();
            //            var elseStatement = _tokenStream.Next();
            //            var elseLabel = _tokenStream.Next();

            //            if (operand1 != null
            //                && operand1 is AlphaNumToken
            //                && (operand1 as AlphaNumToken).IsRegister()
            //                && operation != null
            //                && operation is SpecialToken
            //                && (operation as SpecialToken).IsOperation()
            //                && operand2 != null
            //                && ((operand2 is AlphaNumToken && (operand2 as AlphaNumToken).IsRegister())
            //                  || operand2 is NumberToken)
            //                && thenStatement != null
            //                && thenStatement is AlphaNumToken
            //                && thenLabel != null
            //                && (thenLabel is AlphaNumToken || thenLabel is AlphaNumUnderscoreToken)
            //                && elseStatement != null
            //                && (elseStatement is AlphaNumToken || elseStatement is AlphaNumUnderscoreToken)
            //                && elseLabel != null
            //                && (elseLabel is AlphaNumToken || elseLabel is AlphaNumUnderscoreToken))
            //            {
            //                _lexTokenStack.Push(new IdentifierLexToken(elseLabel.Value()));
            //                _lexTokenStack.Push(new ElseStatementLexToken(elseStatement.Value()));
            //                _lexTokenStack.Push(new IdentifierLexToken(thenLabel.Value()));
            //                _lexTokenStack.Push(new ThenStatementLexToken(thenStatement.Value()));
            //                if (operand2 is NumberToken)
            //                {
            //                    _lexTokenStack.Push(new NumberLexToken(operand2.Value()));
            //                }
            //                else
            //                {
            //                    _lexTokenStack.Push(new IdentifierLexToken(operand2.Value()));
            //                }
            //                _lexTokenStack.Push(new BinaryOperationLexToken(operation.Value()));
            //                _lexTokenStack.Push(new IdentifierLexToken(operand1.Value()));
            //                return new IfStatementLexToken(t1.Value()); // return op
            //            }
            //            else
            //            {
            //                throw new SyntaxException($"Something is wrong with the if statement");
            //            }
            //        }

            //        // here t1 is an alphanum
            //        var t2 = _tokenStream.Next();

            //        if (t2 != null)
            //        {
            //            // t2 is colon so t1 is a label TODO:::(or special)
            //            if (t2 is ColonToken)
            //            {
            //                if (t1Val.Equals("address"))
            //                {
            //                    var next = _tokenStream.Next();
            //                    if (next is NumberToken)
            //                    {
            //                        _lexTokenStack.Push(new NumberLexToken(next.Value()));
            //                    }
            //                    else
            //                    {
            //                        throw new SyntaxException($"address: needs to be followed by a number");
            //                    }
            //                    return new IdentifierLexToken(t1Val);
            //                }
            //                else if (t1Val.Equals("word")
            //                    || t1Val.Equals("byte"))
            //                {
            //                    Stack<NumberToken> numbers = new Stack<NumberToken>();
            //                    var next = _tokenStream.Next();
            //                    while (next != null
            //                        && (next.GetType() == typeof(NumberToken) || next.GetType() == typeof(CommaToken)))
            //                    {
            //                        if (next is NumberToken)
            //                        {
            //                            numbers.Push(next as NumberToken);
            //                        }
            //                        next = _tokenStream.Next();
            //                    }

            //                    while (numbers.Count > 0)
            //                    {
            //                        var current = numbers.Pop();
            //                        _lexTokenStack.Push(new NumberLexToken(current.Value()));
            //                    }

            //                    if (t1Val.Equals("word"))
            //                    {
            //                        return new IdentifierLexToken(t1Val);
            //                    }
            //                    else // its a byte
            //                    {
            //                        return new IdentifierLexToken(t1Val);
            //                    }
            //                }
            //                else // it was a label
            //                {

            //                    return new IdentifierLexToken(t1.Value()); // throw away the colon
            //                }
            //            }

            //            // if t1 is alphanum and t2 is AlphaNumUnderscore, it has to be a branch to label
            //            if (t2 is AlphaNumUnderscoreToken)
            //            {
            //                _lexTokenStack.Push(new IdentifierLexToken(t2.Value())); // keep t2
            //                return new IdentifierLexToken(t1.Value()); // return op
            //            }

            //            // if t1 is alphanum and t2 is alpha num, it could be a branch to label
            //            if (t2 is AlphaNumToken)
            //                // if t1 is opcode
            //                if ((t1 as AlphaNumToken).IsOpCode())
            //                {

            //                    // if instuction is: $"{op} {reg}, {reg}, {imm12}"
            //                    if ((t1Val.Equals("addi")
            //                        || t1Val.Equals("ands")
            //                        || t1Val.Equals("ldr")
            //                        || t1Val.Equals("ldrb")
            //                        || t1Val.Equals("ror")
            //                        || t1Val.Equals("str")
            //                        || t1Val.Equals("subs"))
            //                        && (t2 as AlphaNumToken).IsRegister())
            //                    {
            //                        var comma = _tokenStream.Next();
            //                        var reg2 = _tokenStream.Next();
            //                        var comma2 = _tokenStream.Next();
            //                        var num = _tokenStream.Next();

            //                        if (comma != null
            //                            && comma is CommaToken
            //                            && reg2 != null
            //                            && reg2 is AlphaNumToken
            //                            && (reg2 as AlphaNumToken).IsRegister()
            //                            && comma2 != null
            //                            && comma2 is CommaToken
            //                            && num != null
            //                            && num is NumberToken)
            //                        {
            //                            _lexTokenStack.Push(new NumberLexToken(num.Value())); // keep num
            //                            _lexTokenStack.Push(new IdentifierLexToken(reg2.Value())); // keep reg2
            //                            _lexTokenStack.Push(new IdentifierLexToken(t2.Value())); // keep t2
            //                            return new IdentifierLexToken(t1.Value()); // return op
            //                        }
            //                        else
            //                        {
            //                            throw new SyntaxException($"{t1Val} should be followed by a register, a comma, a register, a comma, and a number");
            //                        }
            //                    }

            //                    // if instuction is: $"{op} {reg}, {reg2}"
            //                    else if ((t1Val.Equals("mov"))
            //                        && (t2 as AlphaNumToken).IsRegister())
            //                    {
            //                        var comma = _tokenStream.Next();
            //                        var reg2 = _tokenStream.Next();

            //                        if (comma != null
            //                            && comma is CommaToken
            //                            && reg2 != null
            //                            && reg2 is AlphaNumToken
            //                            && (reg2 as AlphaNumToken).IsRegister())
            //                        {
            //                            _lexTokenStack.Push(new IdentifierLexToken(reg2.Value())); // keep reg2
            //                            _lexTokenStack.Push(new IdentifierLexToken(t2.Value())); // keep t2
            //                            return new IdentifierLexToken(t1.Value()); // return op
            //                        }
            //                        else
            //                        {
            //                            throw new SyntaxException($"{t1Val} should be followed by a register, a comma, and a register");
            //                        }
            //                    }

            //                    // if instuction is: $"{op} {reg}, {imm16}"
            //                    else if ((t1Val.Equals("cmpi")
            //                        || t1Val.Equals("movt")
            //                        || t1Val.Equals("movw"))
            //                        && (t2 as AlphaNumToken).IsRegister())
            //                    {
            //                        var comma = _tokenStream.Next();
            //                        var num = _tokenStream.Next();

            //                        if (comma != null
            //                            && comma is CommaToken
            //                            && num != null
            //                            && num is NumberToken)
            //                        {
            //                            _lexTokenStack.Push(new NumberLexToken(num.Value())); // keep num
            //                            _lexTokenStack.Push(new IdentifierLexToken(t2.Value())); // keep t2
            //                            return new IdentifierLexToken(t1.Value()); // return op
            //                        }
            //                        else
            //                        {
            //                            throw new SyntaxException($"{t1Val} should be followed by a register, a comma, and a number.");
            //                        }
            //                    }

            //                    // if instuction is: $"{op} {reg}"
            //                    else if ((t1Val.Equals("pop")
            //                        || t1Val.Equals("push"))
            //                        && (t2 as AlphaNumToken).IsRegister())
            //                    {
            //                        _lexTokenStack.Push(new IdentifierLexToken(t2.Value())); // keep t2
            //                        return new IdentifierLexToken(t1.Value()); // return op
            //                    }

            //                    // if instuction is: $"{op} {label}"
            //                    else if ((t1Val.Equals("bal")
            //                        || t1Val.Equals("beq")
            //                        || t1Val.Equals("bge")
            //                        || t1Val.Equals("bl")
            //                        || t1Val.Equals("bne"))
            //                        && (t2 is AlphaNumToken || t2 is AlphaNumUnderscoreToken))
            //                    {
            //                        _lexTokenStack.Push(new IdentifierLexToken(t2.Value())); // keep t2
            //                        return new IdentifierLexToken(t1.Value()); // return op
            //                    }

            //                    // else 
            //                    else
            //                    {
            //                        throw new SyntaxException($"Unknown lex instuction t2: {t2.Value()}");
            //                    }
            //                }

            //                // something else
            //                else
            //                {
            //                    _tokenStream.UnGet(t2);
            //                }
            //            throw new SyntaxException($"t1: {t1.Value()}, t2: {t2.Value()}");

            //        } // t2 is null
            //        throw new SyntaxException($"t1 is an AlphaNum ({t1.Value()}) but t2 is null");
            //    }

            //    else // t1 is not a valid type (not a NewLine, AlphaNumUnderscore, AlphaNum)
            //    {
            //        _tokenStream.UnGet(t1);
            //        throw new SyntaxException($"t1 is not a valid type (not a NewLine, AlphaNumUnderscore, AlphaNum). (t1={t1.Value()}");
            //    }

            //    throw new SyntaxException($"t1: {t1.Value()}");

            //} // t1 is null
            //return null;
        }
    }

    public enum LexerState
    {
        None,
        AlphaNum
    }
}