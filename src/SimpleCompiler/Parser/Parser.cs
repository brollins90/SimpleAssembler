namespace SimpleCompiler.Parser
{
    using Lexer.LexTokens;
    using Expressions;
    using System;
    using System.Collections.Generic;
    using Simple;
    using Lexer;

    public class Parser : IParser
    {
        int KERNEL_SIZE = 0x3000;

        public List<uint> Kernel { get; private set; }
        public uint KernelIndex { get; private set; }
        public uint LineNumber { get; private set; }
        public Dictionary<string, uint> LabelTable { get; }

        public Dictionary<string, int> BinOpPrecedence { get; }

        public Parser()
        {
            BinOpPrecedence = new Dictionary<string, int>();
            BinOpPrecedence.Add("<", 10);
            BinOpPrecedence.Add("+", 20);
            BinOpPrecedence.Add("-", 20);
            BinOpPrecedence.Add("*", 40);
            //Kernel = new List<uint>(KERNEL_SIZE);
            //for (int i = 0; i < KERNEL_SIZE; i++) { Kernel.Add(0); }
            KernelIndex = 0;
            LineNumber = 1;
            LabelTable = new Dictionary<string, uint>();
        }

        public Expression Parse(string fileData)
        {
            // First round to construct the label table
            Lexer lexer = new Lexer(fileData);

            var output = ParseExpression(lexer);
            return output;
            //while (lexer.HasNext())
            //{
            //    //ParseInstruction(lexer, true);
            //}

            //// reset the stuff after the first go round that found the label locations
            //Kernel = new List<uint>(KERNEL_SIZE);
            //for (int i = 0; i < KERNEL_SIZE; i++) { Kernel.Add(0); }
            //lexer = new Lexer.Lexer(fileData);
            //KernelIndex = 0;
            //LineNumber = 1;

            //while (lexer.HasNext())
            //{
            //    //ParseInstruction(lexer, false);
            //}

            //return Kernel.ToArray();
        }


        public Expression ParseExpression(Lexer lexer)
        {
            var LHS = ParsePrimary(lexer);
            if (LHS == null)
            {
                return null;
            }
            return ParseBinaryOperationRHS(lexer, 0, LHS);
        }

        // IntegerExpression ::= {integer}
        public IntegerExpression ParseIntegerExpression(Lexer lexer)
        {
            var current = lexer.Next();
            if (current is NumberLexToken)
            {
                return new IntegerExpression((current as NumberLexToken).IntValue());
            }
            else
            {
                lexer.UnGet(current);
                return null;
            }
        }

        // parenexpression ::= '(' expression ')'
        public Expression ParseParenExpression(Lexer lexer)
        {
            return null;
        }

        public Expression ParsePrimary(Lexer lexer)
        {
            var current = lexer.Next();
            lexer.UnGet(current);

            if (current == null)
                throw new SyntaxException($"something is wrong here");
            else if (current is NumberLexToken)
                return ParseIntegerExpression(lexer);
            else
                return null;
        }

        // binaryoperationrhs ::= ('+' primary)*
        private Expression ParseBinaryOperationRHS(Lexer lexer, int previousPrecedence, Expression LHS)
        {
            while (true)
            {
                // get current token and find its precedence
                var currentToken = lexer.Next();
                int currentPrecedence = GetTokenPrecedence(currentToken);

                // if the current precedence is less that the previous, return LHS as it is
                if (currentPrecedence < previousPrecedence)
                    return LHS;


                var operation = (currentToken as BinaryOperatorLexToken).Value();
                // otherwise, parse out the RHS
                var RHS = ParsePrimary(lexer);
                if (RHS == null)
                    return null;

                // If BinOp binds less tightly with RHS than the operator after RHS, let
                // the pending operator take RHS as its LHS.
                var tokenAfterRHS = lexer.Next();
                lexer.UnGet(tokenAfterRHS);
                var afterRHSPrecendence = GetTokenPrecedence(tokenAfterRHS);
                if (currentPrecedence < afterRHSPrecendence)
                {
                    RHS = ParseBinaryOperationRHS(lexer, currentPrecedence + 1, RHS);
                }

                // merge LHS and RHS
                LHS = new BinaryOperatorExpression(operation, LHS, RHS);
            }
        }

        private int GetTokenPrecedence(LexToken current)
        {
            if (current is BinaryOperatorLexToken)
            {
                if (BinOpPrecedence.ContainsKey(current.Value()))
                    return BinOpPrecedence[current.Value()];
            }
            return -1;
        }
    }
}