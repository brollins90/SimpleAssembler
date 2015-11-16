namespace SimpleAssembler.Parser
{
    using System;
    using System.Collections.Generic;
    using Tokenizer;
    using Tokenizer.Tokens;

    public class Parser
    {
        private uint _kernelIndex;
        private Dictionary<string, uint> _labelTable;

        public Parser()
        {
            _kernelIndex = 0;
            _labelTable = new Dictionary<string, uint>();
        }

        public int GetCurrentIndex()
        {
            return (int)_kernelIndex;
        }

        public uint[] Parse(string fileData)
        {
            List<uint> imageList = new List<uint>();
            ITokenStream tokenStream = new TokenStream(fileData);

            while (tokenStream.HasNext())
            {
                uint instruction;
                if (TryParseInstruction(tokenStream, out instruction))
                {
                    imageList.Insert((int)_kernelIndex++, instruction);
                }
            }

            uint[] outputArray = imageList.ToArray();
            return outputArray;
        }

        public bool TryParseInstruction(ITokenStream tokenStream, out uint instruction)
        {
            uint labelIndex = uint.MaxValue;
            if (TryParseLabel(tokenStream, out labelIndex))
            {
                instruction = labelIndex;
                return false;
            }

            instruction = ParseOperation(tokenStream);

            if (instruction == uint.MaxValue)
            {
                throw new SyntaxException("Unable to parse instruction");
            }
            return true;
        }

        public uint ParseOperation(ITokenStream tokenStream)
        {
            uint encodedOperation = 0;
            var operation = tokenStream.Next();
            tokenStream.UnGet(operation);
            //int argCount = GetArgCount(operation);

            if (TryParseBranch(tokenStream, out encodedOperation)) { }
            else if (TryParseDataProc(tokenStream, out encodedOperation)) { }
            else if (TryParseLoadStore(tokenStream, out encodedOperation)) { }
            else
            {
                throw new SyntaxException($"Unknown instruction: {operation.Value()}");
            }

            return encodedOperation;
        }

        public bool TryParseBranch(ITokenStream tokenStream, out uint encodedOperation)
        {
            var operation = tokenStream.Next();
            var label = tokenStream.Next();
            var parseResult = true;
            encodedOperation = 0;

            string condition = null;

            if (operation.Value().ToLowerInvariant().Equals("bne"))
            {
                condition = "1";
            }
            else if (operation.Value().ToLowerInvariant().Equals("bal"))
            {
                condition = "e";
            }

            if (condition != null
                && operation.GetType() == typeof(AlphaNumToken)
                && label.GetType() == typeof(AlphaNumToken))
            {
                if (!_labelTable.ContainsKey(label.Value()))
                {
                    throw new SyntaxException($"Label {label.Value()} not in label table");
                }

                //string condition = "e";
                string op = "a";

                uint offset = _labelTable[label.Value()] - _kernelIndex;
                offset = offset - 2;

                //string imm4 = IntToHexString(intVal, 3);
                //string rd = RegisterToHex(arg1);
                //string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";
                string offsetString = Convert.ToString(offset, 16);
                offsetString = offsetString.Substring(2);
                string opString = $"{condition}{op}{offsetString}";
                encodedOperation = Convert.ToUInt32(opString, 16);
                parseResult = true;
            }
            else
            {
                tokenStream.UnGet(label);
                tokenStream.UnGet(operation);
                parseResult = false;
            }

            return parseResult;
        }

        public bool TryParseDataProc(ITokenStream tokenStream, out uint encodedOperation)
        {
            var operation = tokenStream.Next();
            var parseResult = true;
            encodedOperation = 0;

            if (operation.Value().ToLowerInvariant().Equals("movt"))
            {
                parseResult = TryEncodeMOVT(tokenStream, out encodedOperation);
            }
            else if (operation.Value().ToLowerInvariant().Equals("movw"))
            {
                parseResult = TryEncodeMOVW(tokenStream, out encodedOperation);
            }
            else if (operation.Value().ToLowerInvariant().Equals("subs"))
            {
                parseResult = TryEncodeSUBS(tokenStream, out encodedOperation);
            }
            else
            {
                tokenStream.UnGet(operation);
                parseResult = false;
            }

            return parseResult;
        }

        public bool TryParseLoadStore(ITokenStream tokenStream, out uint encodedOperation)
        {
            var operation = tokenStream.Next();
            var parseResult = true;
            encodedOperation = 0;

            if (operation.Value().ToLowerInvariant().Equals("str"))
            {
                parseResult = TryEncodeSTR(tokenStream, out encodedOperation);
            }
            else
            {
                tokenStream.UnGet(operation);
                parseResult = false;
            }

            return parseResult;
        }

        public bool TryEncodeMOVT(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            try
            {
                var arg1 = tokenStream.Next();
                var comma1 = tokenStream.Next();
                var arg2 = tokenStream.Next();

                if (arg1.GetType() == typeof(AlphaNumToken) &&
                    comma1.GetType() == typeof(CommaToken) &&
                    arg2.GetType() == typeof(NumberToken))
                {

                    int intVal = (arg2 as NumberToken).IntValue();
                    if (intVal > 0xffff)
                    {
                        throw new SyntaxException("On MOVT, op2 cannot be larger than 0xFFFF");
                    }

                    string condition = "e";
                    string op = "34";
                    string imm4 = IntToHexString(intVal, 3);
                    string rd = RegisterToHex(arg1);
                    string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                    string opString = $"{condition}{op}{imm4}{rd}{imm12}";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
                else
                {
                    tokenStream.UnGet(arg2);
                    tokenStream.UnGet(comma1);
                    tokenStream.UnGet(arg1);
                    parseResult = false;
                }
            }
            catch (SyntaxException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            return parseResult;
        }

        public bool TryEncodeMOVW(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            try
            {
                var arg1 = tokenStream.Next();
                var comma1 = tokenStream.Next();
                var arg2 = tokenStream.Next();

                if (arg1.GetType() == typeof(AlphaNumToken) &&
                    comma1.GetType() == typeof(CommaToken) &&
                    arg2.GetType() == typeof(NumberToken))
                {

                    int intVal = (arg2 as NumberToken).IntValue();
                    if (intVal > 0xffff)
                    {
                        throw new SyntaxException("On MOVW, op2 cannot be larger than 0xFFFF");
                    }

                    string condition = "e";
                    string op = "30";
                    string imm4 = IntToHexString(intVal, 3);
                    string rd = RegisterToHex(arg1);
                    string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                    string opString = $"{condition}{op}{imm4}{rd}{imm12}";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
                else
                {
                    tokenStream.UnGet(arg2);
                    tokenStream.UnGet(comma1);
                    tokenStream.UnGet(arg1);
                    parseResult = false;
                }
            }
            catch (SyntaxException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            return parseResult;
        }

        public bool TryEncodeSTR(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            try
            {
                var arg1 = tokenStream.Next();
                var comma1 = tokenStream.Next();
                var arg2 = tokenStream.Next();
                var comma2 = tokenStream.Next();
                var arg3 = tokenStream.Next();

                if (arg1.GetType() == typeof(AlphaNumToken) &&
                    comma1.GetType() == typeof(CommaToken) &&
                    arg2.GetType() == typeof(AlphaNumToken) &&
                    comma2.GetType() == typeof(CommaToken) &&
                    arg3.GetType() == typeof(NumberToken))
                {
                    int intVal = (arg3 as NumberToken).IntValue();
                    if (intVal > 0xfff)
                    {
                        throw new SyntaxException("On STR, op2 cannot be larger than 0xFFF");
                    }

                    string condition = "e";
                    string op = "58";
                    string rt = RegisterToHex(arg1);
                    string rn = RegisterToHex(arg2);
                    string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                    string opString = $"{condition}{op}{rn}{rt}{imm12}";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
                else
                {
                    tokenStream.UnGet(arg3);
                    tokenStream.UnGet(comma2);
                    tokenStream.UnGet(arg2);
                    tokenStream.UnGet(comma1);
                    tokenStream.UnGet(arg1);
                    parseResult = false;
                }
            }
            catch (SyntaxException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            return parseResult;
        }

        public bool TryEncodeSUBS(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            try
            {
                var arg1 = tokenStream.Next();
                var comma1 = tokenStream.Next();
                var arg2 = tokenStream.Next();
                var comma2 = tokenStream.Next();
                var arg3 = tokenStream.Next();

                if (arg1.GetType() == typeof(AlphaNumToken) &&
                    comma1.GetType() == typeof(CommaToken) &&
                    arg2.GetType() == typeof(AlphaNumToken) &&
                    comma2.GetType() == typeof(CommaToken) &&
                    arg3.GetType() == typeof(NumberToken))
                {

                    int intVal = (arg3 as NumberToken).IntValue();
                    if (intVal > 0xffff)
                    {
                        throw new SyntaxException("On SUBS, op2 cannot be larger than 0xFFF");
                    }

                    string condition = "e";
                    string op = "25";
                    string rn = RegisterToHex(arg1);
                    string rd = RegisterToHex(arg2);
                    string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                    string opString = $"{condition}{op}{rn}{rd}{imm12}";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
                else
                {
                    tokenStream.UnGet(arg3);
                    tokenStream.UnGet(comma2);
                    tokenStream.UnGet(arg2);
                    tokenStream.UnGet(comma1);
                    tokenStream.UnGet(arg1);
                    parseResult = false;
                }
            }
            catch (SyntaxException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            return parseResult;
        }

        private string IntToHexString(int i, int nibble)
        {
            int ret = (i >> (4 * nibble)) & 0xF;
            return Convert.ToString(ret, 16);
        }

        private string RegisterToHex(Token arg)
        {
            if (arg.GetType() == typeof(AlphaNumToken) &&
                arg.Value().StartsWith("r", StringComparison.InvariantCultureIgnoreCase))
            {
                string decimalString = arg.Value().Substring(1);
                int i = Convert.ToInt32(decimalString);
                return IntToHexString(i, 0);
            }
            throw new SyntaxException($"{arg.Value()} is not a register");
        }

        public bool TryParseLabel(ITokenStream tokenStream, out uint labelIndex)
        {
            labelIndex = uint.MaxValue;
            var label = tokenStream.Next();
            var colon = tokenStream.Next();
            if (label == null || colon == null)
            {
                Console.WriteLine();
            }

            var parseResult = true;

            if (label.GetType() == typeof(AlphaNumToken) &&
                colon.GetType() == typeof(ColonToken))
            {
                // add to label table
                _labelTable.Add(label.Value(), _kernelIndex);
                labelIndex = _kernelIndex;
            }
            else
            {
                tokenStream.UnGet(colon);
                tokenStream.UnGet(label);
                parseResult = false;
            }
            return parseResult;
        }

        //public int GetArgCount(Token operation)
        //{
        //    if (operation.GetType() != typeof(AlphaNumToken))
        //    {
        //        throw new SyntaxException();
        //    }

        //    var lower = operation.Value().ToLowerInvariant();

        //    if (lower.Equals("bal", StringComparison.InvariantCultureIgnoreCase)) { return 1; }
        //    else if (lower.Equals("bne", StringComparison.InvariantCultureIgnoreCase)) { return 1; }
        //    else if (lower.Equals("movi", StringComparison.InvariantCultureIgnoreCase)) { return 2; }
        //    else { return 0; }
        //}

        //public bool ValidateOperation(Token operation, params Token[] args)
        //{
        //    if (operation.GetType() != typeof(AlphaNumToken))
        //    {
        //        throw new SyntaxException();
        //    }

        //    var lower = operation.Value().ToLowerInvariant();

        //    if (lower.Equals("bal", StringComparison.InvariantCultureIgnoreCase)
        //        || lower.Equals("bne", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        if (args.Length == 1
        //            && args[0].GetType() == typeof(AlphaNumToken))
        //        {
        //            if (_labelTable.ContainsKey(args[0].Value().ToLowerInvariant()))
        //            {
        //                return true;
        //            }
        //            throw new SyntaxException("Label not in label table");
        //        }
        //    }

        //    else if (lower.Equals("movi", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        if (args.Length == 3
        //            && args[0].GetType() == typeof(AlphaNumToken)
        //            && args[1].GetType() == typeof(CommaToken)
        //            && args[2].GetType() == typeof(NumberToken))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;

        //}
    }
}