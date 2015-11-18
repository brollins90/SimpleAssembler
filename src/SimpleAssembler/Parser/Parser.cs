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
        private uint _lineNumber;

        public Parser()
        {
            _kernelIndex = 0;
            _labelTable = new Dictionary<string, uint>();
            _lineNumber = 0;
        }

        public int GetCurrentKernelIndex()
        {
            return (int)_kernelIndex;
        }

        public int GetCurrentLineNumber()
        {
            return (int)_lineNumber;
        }

        public uint[] Parse(string fileData)
        {
            // First round to construct the label table
            ITokenStream tokenStream = new TokenStream(fileData);

            while (tokenStream.HasNext())
            {
                uint instruction;
                if (TryParseInstruction(tokenStream, out instruction, false))
                {
                    // dont save anything, just increment the index so we can populate the label table
                    _kernelIndex++;
                }
            }

            List<uint> imageList = new List<uint>();
            // reset the stuff after the first go round that found the label locations
            tokenStream = new TokenStream(fileData);
            _kernelIndex = 0;
            _lineNumber = 0;

            while (tokenStream.HasNext())
            {
                uint instruction;
                if (TryParseInstruction(tokenStream, out instruction))
                {
                    Console.WriteLine($"{_kernelIndex}: {Convert.ToString(instruction, 16)}");
                    imageList.Insert((int)_kernelIndex++, instruction);
                }
            }

            return imageList.ToArray();
        }

        public bool TryParseInstruction(ITokenStream tokenStream, out uint instruction, bool throwOnLabelNotFound = true)
        {
            uint labelIndex = uint.MaxValue;
            if (TryParseLabel(tokenStream, out labelIndex, throwOnLabelNotFound))
            {
                instruction = labelIndex;
                return false;
            }
            instruction = ParseOperation(tokenStream, throwOnLabelNotFound);

            if (instruction == uint.MaxValue || instruction == 0)
            {
                throw new SyntaxException("Unable to parse instruction");
            }

            TryParseNewLine(tokenStream);
            return true;
        }

        public void TryParseNewLine(ITokenStream tokenStream)
        {
            // try to clean the new lines
            if (tokenStream.HasNext())
            {
                var token = tokenStream.Next();

                if (token != null &&
                    token.GetType() == typeof(NewLineToken))
                {
                    _lineNumber++;
                    TryParseNewLine(tokenStream);
                }
                else
                {
                    tokenStream.UnGet(token);
                }
            }
        }

        public uint ParseOperation(ITokenStream tokenStream, bool throwOnLabelNotFound)
        {
            uint encodedOperation = 0;
            var operation = tokenStream.Next();
            tokenStream.UnGet(operation);

            if (operation != null)
            {
                if (TryParseBranch(tokenStream, out encodedOperation, throwOnLabelNotFound)) { }
                else if (TryParseDataProc(tokenStream, out encodedOperation)) { }
                else if (TryParseLoadStore(tokenStream, out encodedOperation)) { }
                else
                {
                    throw new SyntaxException($"Unknown instruction: {operation.Value()}");
                }
            }

            return encodedOperation;
        }

        public bool TryParseBranch(ITokenStream tokenStream, out uint encodedOperation, bool throwOnLabelNotFound)
        {
            var operation = tokenStream.Next();
            var label = tokenStream.Next();
            var parseResult = true;
            encodedOperation = 0;

            string condition = null;
            string opCode = null;

            if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("bal"))
            {
                condition = "e"; // Always
                opCode = "a"; // No Link
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("bne"))
            {
                condition = "1"; // Not Equal
                opCode = "a"; // No Link
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("bl"))
            {
                condition = "e"; // Always
                opCode = "b"; // Link
            }

            if (condition != null &&
                opCode != null &&
                operation != null &&
                operation.GetType() == typeof(AlphaNumToken) &&
                label != null &&
                label.GetType() == typeof(AlphaNumToken))
            {
                uint labelLocation = 0;
                if (_labelTable.ContainsKey(label.Value()))
                {
                    labelLocation = _labelTable[label.Value()];
                }
                else
                {
                    if (throwOnLabelNotFound)
                    {
                        throw new SyntaxException($"Label {label.Value()} not in label table");
                    }
                }

                uint offset = labelLocation - _kernelIndex;
                offset = offset - 2;

                string offsetString = Convert.ToString(offset, 16);
                offsetString = offsetString.Substring(2);
                string opString = $"{condition}{opCode}{offsetString}";
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

            if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("movt"))
            {
                parseResult = TryEncodeMOVT(tokenStream, out encodedOperation);
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("movw"))
            {
                parseResult = TryEncodeMOVW(tokenStream, out encodedOperation);
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("subs"))
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

            if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("str"))
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

            var arg1 = tokenStream.Next();
            var comma1 = tokenStream.Next();
            var arg2 = tokenStream.Next();

            if (arg1 != null &&
                arg1.GetType() == typeof(AlphaNumToken) &&
                comma1 != null &&
                comma1.GetType() == typeof(CommaToken) &&
                arg2 != null &&
                arg2.GetType() == typeof(NumberToken))
            {

                int intVal = (arg2 as NumberToken).IntValue();
                if (intVal > 0xffff)
                {
                    throw new SyntaxException("On MOVT, op2 cannot be larger than 0xFFFF");
                }

                string imm4 = IntToHexString(intVal, 3);
                string rd = RegisterToHex(arg1);
                string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e34{imm4}{rd}{imm12}";
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

            return parseResult;
        }

        public bool TryEncodeMOVW(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            var arg1 = tokenStream.Next();
            var comma1 = tokenStream.Next();
            var arg2 = tokenStream.Next();

            if (arg1 != null &&
                arg1.GetType() == typeof(AlphaNumToken) &&
                comma1 != null &&
                comma1.GetType() == typeof(CommaToken) &&
                arg2 != null &&
                arg2.GetType() == typeof(NumberToken))
            {

                int intVal = (arg2 as NumberToken).IntValue();
                if (intVal > 0xffff)
                {
                    throw new SyntaxException("On MOVW, op2 cannot be larger than 0xFFFF");
                }

                string imm4 = IntToHexString(intVal, 3);
                string rd = RegisterToHex(arg1);
                string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e30{imm4}{rd}{imm12}";
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
            return parseResult;
        }

        public bool TryEncodeSTR(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            var arg1 = tokenStream.Next();
            var comma1 = tokenStream.Next();
            var arg2 = tokenStream.Next();
            var comma2 = tokenStream.Next();
            var arg3 = tokenStream.Next();

            if (arg1 != null &&
                arg1.GetType() == typeof(AlphaNumToken) &&
                comma1 != null &&
                comma1.GetType() == typeof(CommaToken) &&
                arg2 != null &&
                arg2.GetType() == typeof(AlphaNumToken) &&
                comma2 != null &&
                comma2.GetType() == typeof(CommaToken) &&
                arg3 != null &&
                arg3.GetType() == typeof(NumberToken))
            {
                int intVal = (arg3 as NumberToken).IntValue();
                if (intVal > 0xfff)
                {
                    throw new SyntaxException("On STR, op2 cannot be larger than 0xFFF");
                }

                string rt = RegisterToHex(arg1);
                string rn = RegisterToHex(arg2);
                string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e58{rn}{rt}{imm12}";
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
            return parseResult;
        }

        public bool TryEncodeSUBS(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            var arg1 = tokenStream.Next();
            var comma1 = tokenStream.Next();
            var arg2 = tokenStream.Next();
            var comma2 = tokenStream.Next();
            var arg3 = tokenStream.Next();

            if (arg1 != null &&
                arg1.GetType() == typeof(AlphaNumToken) &&
                comma1 != null &&
                comma1.GetType() == typeof(CommaToken) &&
                arg2 != null &&
                arg2.GetType() == typeof(AlphaNumToken) &&
                comma2 != null &&
                comma2.GetType() == typeof(CommaToken) &&
                arg3 != null &&
                arg3.GetType() == typeof(NumberToken))
            {

                int intVal = (arg3 as NumberToken).IntValue();
                if (intVal > 0xffff)
                {
                    throw new SyntaxException("On SUBS, op2 cannot be larger than 0xFFF");
                }

                string rn = RegisterToHex(arg1);
                string rd = RegisterToHex(arg2);
                string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e25{rn}{rd}{imm12}";
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

            return parseResult;
        }

        public bool TryParseLabel(ITokenStream tokenStream, out uint labelIndex, bool throwOnLabelNotFound)
        {
            labelIndex = uint.MaxValue;
            var label = tokenStream.Next();
            var colon = tokenStream.Next();
            var parseResult = true;

            if (label != null &&
                label.GetType() == typeof(AlphaNumToken) &&
                colon != null &&
                colon.GetType() == typeof(ColonToken))
            {
                // add to label table
                if (throwOnLabelNotFound)
                {
                    //_labelTable.Add(label.Value(), _kernelIndex);
                    // dont add anything, it should be there already
                }
                else
                {
                    if (!_labelTable.ContainsKey(label.Value()))
                    {
                        _labelTable.Add(label.Value(), _kernelIndex);
                    }
                }
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

        private string IntToHexString(int i, int nibble)
        {
            int ret = (i >> (4 * nibble)) & 0xF;
            return Convert.ToString(ret, 16);
        }

        private string RegisterToHex(Token arg)
        {
            if (arg != null &&
                arg.GetType() == typeof(AlphaNumToken))
            {
                var register = arg.Value().ToLowerInvariant();

                if (register.Equals("r0"))
                    return "0";
                if (register.Equals("r1"))
                    return "1";
                if (register.Equals("r2"))
                    return "2";
                if (register.Equals("r3"))
                    return "3";
                if (register.Equals("r4"))
                    return "4";
                if (register.Equals("r5"))
                    return "5";
                if (register.Equals("r6"))
                    return "6";
                if (register.Equals("r7"))
                    return "7";
                if (register.Equals("r8"))
                    return "8";
                if (register.Equals("r9"))
                    return "9";
                if (register.Equals("r10"))
                    return "a";
                if (register.Equals("r11"))
                    return "b";
                if (register.Equals("r12"))
                    return "c";
                if (register.Equals("r13"))
                    return "d";
                if (register.Equals("r14"))
                    return "e";
                if (register.Equals("r15"))
                    return "f";
                if (register.Equals("a1"))
                    return "0";
                if (register.Equals("a2"))
                    return "1";
                if (register.Equals("a3"))
                    return "2";
                if (register.Equals("a4"))
                    return "3";
                if (register.Equals("v1"))
                    return "4";
                if (register.Equals("v2"))
                    return "5";
                if (register.Equals("v3"))
                    return "6";
                if (register.Equals("v4"))
                    return "7";
                if (register.Equals("v5"))
                    return "8";
                if (register.Equals("v6"))
                    return "9";
                if (register.Equals("v7"))
                    return "a";
                if (register.Equals("v8"))
                    return "b";
                if (register.Equals("sb"))
                    return "9";
                if (register.Equals("sl"))
                    return "a";
                if (register.Equals("fp"))
                    return "b";
                if (register.Equals("ip"))
                    return "c";
                if (register.Equals("sp"))
                    return "d";
                if (register.Equals("lr"))
                    return "e";
                if (register.Equals("pc"))
                    return "f";
            }
            //if (arg.GetType() == typeof(AlphaNumToken) &&
            //    arg.Value().StartsWith("r", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    string decimalString = arg.Value().Substring(1);
            //    int i = Convert.ToInt32(decimalString);
            //    return IntToHexString(i, 0);
            //}
            throw new SyntaxException($"{arg.Value()} is not a register");
        }
    }
}