namespace SimpleAssembler.Compiler
{
    using System;
    using System.Collections.Generic;
    using Tokenizer;
    using Tokenizer.Tokens;

    public class Compiler : ICompiler
    {
        private uint _kernelIndex;
        private Dictionary<string, uint> _labelTable;
        private uint _lineNumber;

        public Compiler()
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
            TryParseNewLine(tokenStream);
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
                (
                    label.GetType() == typeof(AlphaNumToken) ||
                    label.GetType() == typeof(AlphaNumUnderscoreToken)
                ))
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
                if (offset > 0xfffffff) offsetString = offsetString.Substring(1);
                if (offset > 0xffffff) offsetString = offsetString.Substring(1);
                else offsetString = offsetString.PadLeft(6, '0');
                //offsetString = (offset > 0xffffff) ? Convert.ToString(offset, 16) : $"{("" + offset).PadLeft(12, '0')}";
                //offsetString = offsetString.Substring(2);
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
                operation.Value().ToLowerInvariant().Equals("ands"))
            {
                parseResult = TryEncodeANDS(tokenStream, out encodedOperation);
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("mov"))
            {
                parseResult = TryEncodeMOV(tokenStream, out encodedOperation);
            }
            else if (operation != null &&
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
                operation.Value().ToLowerInvariant().Equals("ldr"))
            {
                parseResult = TryEncodeLDR(tokenStream, out encodedOperation);
            }
            //else if (operation != null &&
            //    operation.Value().ToLowerInvariant().Equals("ldiia"))
            //{
            //    parseResult = TryEncodeLDIIA(tokenStream, out encodedOperation);
            //}
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("pop"))
            {
                parseResult = TryEncodePOP(tokenStream, out encodedOperation);
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("push"))
            {
                parseResult = TryEncodePUSH(tokenStream, out encodedOperation);
            }
            else if (operation != null &&
                operation.Value().ToLowerInvariant().Equals("str"))
            {
                parseResult = TryEncodeSTR(tokenStream, out encodedOperation);
            }
            //else if (operation != null &&
            //    operation.Value().ToLowerInvariant().Equals("strdb"))
            //{
            //    parseResult = TryEncodeSTRDB(tokenStream, out encodedOperation);
            //}
            else
            {
                tokenStream.UnGet(operation);
                parseResult = false;
            }

            return parseResult;
        }

        public bool TryEncodeANDS(ITokenStream tokenStream, out uint encodedOperation)
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
                    throw new SyntaxException("On ANDS, op2 cannot be larger than 0xFFF");
                }

                string rn = RegisterToHex(arg1);
                string rd = RegisterToHex(arg2);
                string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e21{rn}{rd}{imm12}";
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

        public bool TryEncodeMOV(ITokenStream tokenStream, out uint encodedOperation)
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
                arg2.GetType() == typeof(AlphaNumToken))
            {
                string rd = RegisterToHex(arg1);
                string rm = RegisterToHex(arg2);

                string opString = $"e1a0{rd}00{rm}";
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

        public bool TryEncodeLDR(ITokenStream tokenStream, out uint encodedOperation)
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
                    throw new SyntaxException("On LDR, op2 cannot be larger than 0xFFF");
                }
                // p 0 = post, 1 = pre (when using immediate, p=1
                // u 0 = down, 1 = up, (is imm12 positive or negative)
                // b 0 = word, 1 = byte
                // w 0 = no write back, 1 = write back (when using immediate, w=0)
                // l 0 = store, 1 = load

                // A8.8.204
                // {cond}010{P}{U}0{W}1{Rn}{Rt}{imm12}
                // 1110  0101   1 0 0 1 
                // 1110  0101   1001    rn rt imm12
                // e     5      9     
                //
                // e59{rn}{rt}{imm12}  // when imm12 is positive
                // e51{rn}{rt}{imm12}  // when imm12 is negitive

                string posNeg = ((arg3 as NumberToken).IsNegative()) ? "1" : "9";
                string rt = RegisterToHex(arg1);
                string rn = RegisterToHex(arg2);
                string offset = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e5{posNeg}{rn}{rt}{offset}";
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

        //public bool TryEncodeLDIIA(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();
        //    var comma1 = tokenStream.Next();
        //    var arg2 = tokenStream.Next();
        //    var comma2 = tokenStream.Next();
        //    var arg3 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken) &&
        //        comma1 != null &&
        //        comma1.GetType() == typeof(CommaToken) &&
        //        arg2 != null &&
        //        arg2.GetType() == typeof(AlphaNumToken) &&
        //        comma2 != null &&
        //        comma2.GetType() == typeof(CommaToken) &&
        //        arg3 != null &&
        //        arg3.GetType() == typeof(NumberToken))
        //    {
        //        int intVal = (arg3 as NumberToken).IntValue();
        //        if (intVal > 0xfff)
        //        {
        //            throw new SyntaxException("On LDIIA, op2 cannot be larger than 0xFFF");
        //        }
        //        // p 0 = post, 1 = pre
        //        // u 0 = down, 1 = up,
        //        // b 0 = word, 1 = byte
        //        // w 0 = no write back, 1 = write back
        //        // l 0 = store, 1 = load

        //        // A8.8.204
        //        // {cond}010{P}{U}0{W}1{Rn}{Rt}{imm12}
        //        // 1110  0100   1011    rn rt imm12
        //        // e     4      b     

        //        string rd = RegisterToHex(arg1);
        //        string rn = RegisterToHex(arg2);
        //        string offset = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e4b{rn}{rd}{offset}";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg3);
        //        tokenStream.UnGet(comma2);
        //        tokenStream.UnGet(arg2);
        //        tokenStream.UnGet(comma1);
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }
        //    return parseResult;
        //}

        public bool TryEncodePOP(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            var arg1 = tokenStream.Next();

            if (arg1 != null &&
                arg1.GetType() == typeof(AlphaNumToken))
            {
                string rt = RegisterToHex(arg1);

                //0xe49d{rt}004

                string opString = $"e49d{rt}004";
                encodedOperation = Convert.ToUInt32(opString, 16);
                parseResult = true;
            }
            else if (arg1 != null &&
                arg1.GetType() == typeof(LeftCurlyToken))

            {
                var reg1 = tokenStream.Next();
                List<Token> registerList = new List<Token>();
                registerList.Add(reg1);

                var next = tokenStream.Next();
                while (next.GetType() != typeof(RightCurlyToken))
                {
                    registerList.Add(next);
                    next = tokenStream.Next();
                }

                if (registerList.Count == 1)
                {
                    string rt = RegisterToHex(reg1);

                    //0xe49d{rt}004

                    string opString = $"e49d{rt}004";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
                else
                {
                    int registerInt = 0;
                    for (int i = 0; i < registerList.Count; i++)
                    {
                        var token = registerList[i];
                        if (token.GetType() == typeof(AlphaNumToken))
                        {
                            string regNumString = RegisterToHex(token);
                            int regNum = Convert.ToInt32(regNumString, 16);
                            int bit = (1 << regNum);
                            registerInt |= bit;
                        }
                    }
                    string registerIntToString = Convert.ToString(registerInt, 16).PadLeft(4, '0');

                    //0xe49d{rt}004

                    string opString = $"e8bd{registerIntToString}";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
            }
            else
            {
                tokenStream.UnGet(arg1);
                parseResult = false;
            }
            return parseResult;
        }

        public bool TryEncodePUSH(ITokenStream tokenStream, out uint encodedOperation)
        {
            var parseResult = true;
            encodedOperation = 0;

            var arg1 = tokenStream.Next();

            if (arg1 != null &&
                arg1.GetType() == typeof(AlphaNumToken))
            {
                string rt = RegisterToHex(arg1);

                //0xe52d{rt}004

                string opString = $"e52d{rt}004";
                encodedOperation = Convert.ToUInt32(opString, 16);
                parseResult = true;
            }
            else if (arg1 != null &&
                arg1.GetType() == typeof(LeftCurlyToken))

            {
                var reg1 = tokenStream.Next();

                List<Token> registerList = new List<Token>();
                registerList.Add(reg1);

                var next = tokenStream.Next();
                while (next.GetType() != typeof(RightCurlyToken))
                {
                    registerList.Add(next);
                    next = tokenStream.Next();
                }

                if (registerList.Count == 1)
                {
                    string rt = RegisterToHex(reg1);

                    //0xe52d{rt}004

                    string opString = $"e52d{rt}004";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
                else
                {
                    int registerInt = 0;
                    for (int i = 0; i < registerList.Count; i++)
                    {
                        var token = registerList[i];
                        if (token.GetType() == typeof(AlphaNumToken))
                        {
                            string regNumString = RegisterToHex(token);
                            int regNum = Convert.ToInt32(regNumString, 16);
                            int bit = (1 << regNum);
                            registerInt |= bit;
                        }
                    }
                    string registerIntToString = Convert.ToString(registerInt, 16).PadLeft(4, '0');

                    //0xe49d{rt}004

                    string opString = $"e92d{registerIntToString}";
                    encodedOperation = Convert.ToUInt32(opString, 16);
                    parseResult = true;
                }
            }
            else
            {
                tokenStream.UnGet(arg1);
                parseResult = false;
            }
            return parseResult;
        }

        //public bool TryEncodeSTRDB(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();
        //    var comma1 = tokenStream.Next();
        //    var arg2 = tokenStream.Next();
        //    var comma2 = tokenStream.Next();
        //    var arg3 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken) &&
        //        comma1 != null &&
        //        comma1.GetType() == typeof(CommaToken) &&
        //        arg2 != null &&
        //        arg2.GetType() == typeof(AlphaNumToken) &&
        //        comma2 != null &&
        //        comma2.GetType() == typeof(CommaToken) &&
        //        arg3 != null &&
        //        arg3.GetType() == typeof(NumberToken))
        //    {
        //        int intVal = (arg3 as NumberToken).IntValue();
        //        if (intVal > 0xfff)
        //        {
        //            throw new SyntaxException("On STRDB, op2 cannot be larger than 0xFFF");
        //        }
        //        // p 0 = post, 1 = pre
        //        // u 0 = down, 1 = up,
        //        // b 0 = word, 1 = byte
        //        // w 0 = no write back, 1 = write back
        //        // l 0 = store, 1 = load

        //        // A8.8.204
        //        // {cond}010{P}{U}0{W}0{Rn}{Rt}{imm12}
        //        // 1110  0101   0010    rn rt imm12
        //        // e     5      2     

        //        string rt = RegisterToHex(arg1);
        //        string rn = RegisterToHex(arg2);
        //        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e52{rn}{rt}{imm12}";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg3);
        //        tokenStream.UnGet(comma2);
        //        tokenStream.UnGet(arg2);
        //        tokenStream.UnGet(comma1);
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }
        //    return parseResult;
        //}

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
                // p 0 = post, 1 = pre
                // u 0 = down, 1 = up,
                // b 0 = word, 1 = byte
                // w 0 = no write back, 1 = write back
                // l 0 = store, 1 = load

                // A8.8.204
                // {cond}010{P}{U}0{W}0{Rn}{Rt}{imm12}
                // 1110  0101   0000    rn rt imm12
                // e     5      0     

                string rt = RegisterToHex(arg1);
                string rn = RegisterToHex(arg2);
                string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

                string opString = $"e50{rn}{rt}{imm12}";
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
                (
                label.GetType() == typeof(AlphaNumToken) ||
                label.GetType() == typeof(AlphaNumUnderscoreToken)) &&
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