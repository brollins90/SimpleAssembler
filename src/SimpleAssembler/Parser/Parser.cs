namespace SimpleAssembler.Parser
{
    using Lexer.LexTokens;
    using System;
    using System.Collections.Generic;
    using Tokenizer;
    using Tokenizer.Tokens;

    public class Parser : IParser
    {
        public List<uint> Kernel { get; }
        public uint KernelIndex { get; private set; }
        public uint LineNumber { get; private set; }
        public Dictionary<string, uint> LabelTable { get; }

        public Parser()
        {
            Kernel = new List<uint>(64000000);
            KernelIndex = 0;
            LineNumber = 1;
            LabelTable = new Dictionary<string, uint>();
        }

        public uint[] Parse(string fileData)
        {
            // First round to construct the label table
            Lexer.Lexer lexer = new Lexer.Lexer(fileData);

            while (lexer.HasNext())
            {
                ParseInstruction(lexer, true);
            }

            // reset the stuff after the first go round that found the label locations
            lexer = new Lexer.Lexer(fileData);
            KernelIndex = 0;
            LineNumber = 1;

            while (lexer.HasNext())
            {
                ParseInstruction(lexer, false);
                //uint instruction;
                //if (ParseInstruction(lexer, out instruction))
                //{
                //    Console.WriteLine($"{KernelIndex}: {Convert.ToString(instruction, 16)}");
                //    Kernel.Insert((int)KernelIndex, instruction); // ++
                //}
            }

            return Kernel.ToArray();
        }

        public void ParseInstruction(Lexer.Lexer lexer, bool buildingLabelTable = false)
        {
            var operation = lexer.Next();
            lexer.UnGet(operation);

            if (operation is AddressDataStatementLexToken)
            {
                ParseAddressDataStatement(lexer);
            }
            else if (operation is ByteDataStatementLexToken)
            {
                ParseByteDataStatement(lexer);
            }
            else if (operation is LabelDeclarationLexToken)
            {
                ParseLabelDeclaration(lexer, buildingLabelTable);
            }
            else if (operation is NewLineLexToken)
            {
                ParseNewLine(lexer);
            }
            else if (operation is OpCodeLexToken)
            {
                var encodedOperation = ParseOperation(lexer, buildingLabelTable); // do i want a return here
            }
            else if (operation is WordDataStatementLexToken)
            {
                ParseWordDataStatement(lexer);
            }
            else
            {
                throw new SyntaxException($"Unknown instruction: {operation.Value()}");
            }
        }


        public void ParseAddressDataStatement(Lexer.Lexer lexer)
        {
            var token = lexer.Next();
            if (token != null
                && token is AddressDataStatementLexToken)
            {
                var address = lexer.Next();

                if (address != null
                    && address is NumberLexToken)
                {
                    KernelIndex = (uint)(address as NumberLexToken).IntValue(); // TODO make a uintvalue method
                }
            }
        }

        public void ParseByteDataStatement(Lexer.Lexer lexer)
        {
            var token = lexer.Next();
            if (token != null
                && token is ByteDataStatementLexToken)
            {
            }
        }

        public void ParseLabelDeclaration(Lexer.Lexer lexer, bool buildingLabelTable)
        {
            var token = lexer.Next();
            if (token != null
                && token is LabelDeclarationLexToken)
            {
                if (buildingLabelTable
                    && !LabelTable.ContainsKey(token.Value()))
                {
                    LabelTable.Add(token.Value(), KernelIndex);
                }
            }
        }

        public void ParseNewLine(Lexer.Lexer lexer)
        {
            var token = lexer.Next();
            if (token != null
                && token is NewLineLexToken)
            {
                LineNumber++;
            }
        }

        public uint ParseOperation(Lexer.Lexer lexer, bool buildLabelTable)
        {
            uint encodedInstruction = 0;

            var token = lexer.Next();
            if (token != null
                && token is OpCodeLexToken)
            {
                var operation = token as OpCodeLexToken;

                switch (operation.OperationType)
                {
                    case OperationType.ANDS:
                        encodedInstruction = EncodeANDSInstruction(
                            lexer.Next().Value(),                           // destinationRegister
                            lexer.Next().Value(),                           // sourceRegister
                            (lexer.Next() as NumberLexToken).IntValue());   // imm12 
                        break;

                    case OperationType.BAL:
                        encodedInstruction = EncodeBranchInstruction(
                            "e",                                            // Always
                            "a",                                            // No Link
                            lexer.Next().Value(),                           // Label
                            buildLabelTable);
                        break;

                    case OperationType.BL:
                        encodedInstruction = EncodeBranchInstruction(
                            "e",                                            // Always
                            "b",                                            // Link
                            lexer.Next().Value(),                           // Label
                            buildLabelTable);
                        break;

                    case OperationType.BNE:
                        encodedInstruction = EncodeBranchInstruction(
                            "1",                                            // Not Equal
                            "a",                                            // No Link
                            lexer.Next().Value(),                           // Label
                            buildLabelTable);
                        break;

                    case OperationType.LDR:
                        encodedInstruction = EncodeLDRInstruction(
                            lexer.Next().Value(),                           // sourceRegister
                            lexer.Next().Value(),                           // baseRegister
                            (lexer.Next() as NumberLexToken).IntValue());   // offset 
                        break;

                    case OperationType.MOV:
                        encodedInstruction = EncodeMOVInstruction(
                            lexer.Next().Value(),                           // destinationRegister
                            lexer.Next().Value());                          // sourceRegister
                        break;

                    case OperationType.MOVT:
                        encodedInstruction = EncodeMOVTInstruction(
                            lexer.Next().Value(),                           // destinationRegister
                            (lexer.Next() as NumberLexToken).IntValue());   // imm16 
                        break;

                    case OperationType.MOVW:
                        encodedInstruction = EncodeMOVWInstruction(
                            lexer.Next().Value(),                           // destinationRegister
                            (lexer.Next() as NumberLexToken).IntValue());   // imm16 
                        break;

                    case OperationType.POP:
                        encodedInstruction = EncodePOPInstruction(
                            lexer.Next().Value());                           // destinationRegister
                        break;

                    case OperationType.PUSH:
                        encodedInstruction = EncodePUSHInstruction(
                            lexer.Next().Value());                           // sourceRegister
                        break;

                    case OperationType.STR:
                        encodedInstruction = EncodeSTRInstruction(
                            lexer.Next().Value(),                           // sourceRegister
                            lexer.Next().Value(),                           // baseRegister
                            (lexer.Next() as NumberLexToken).IntValue());   // offset 
                        break;

                    case OperationType.SUBS:
                        encodedInstruction = EncodeSUBSInstruction(
                            lexer.Next().Value(),                           // destinationRegister
                            lexer.Next().Value(),                           // sourceRegister
                            (lexer.Next() as NumberLexToken).IntValue());   // imm12 
                        break;
                }
            }

            WriteInstructionToKernel(encodedInstruction);
            return encodedInstruction;
        }

        private void WriteInstructionToKernel(uint instruction)
        {
            Console.WriteLine($"{KernelIndex}: {Convert.ToString(instruction, 16)}");
            Kernel.Insert((int)KernelIndex++, instruction);
        }

        public void ParseWordDataStatement(Lexer.Lexer lexer)
        {
            var token = lexer.Next();
            if (token != null
                && token is WordDataStatementLexToken)
            {
            }
        }

        public uint EncodeANDSInstruction(string destinationRegister, string sourceRegister, int imm12)
        {
            if (imm12 > 0xffff)
            {
                throw new SyntaxException("On ANDS, op2 cannot be larger than 0xFFF");
            }

            string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

            string instruction = $"e21{destinationRegister}{sourceRegister}{imm12String}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeBranchInstruction(string condition, string withLink, string label, bool buildLabelTable)
        {
            uint labelLocation = 0;
            if (LabelTable.ContainsKey(label))
            {
                labelLocation = LabelTable[label];
            }
            else
            {
                if (!buildLabelTable)
                {
                    throw new SyntaxException($"Label {label} not in label table");
                }
            }

            uint offset = labelLocation - KernelIndex;
            offset = offset - 2;

            string offsetString = Convert.ToString(offset, 16);
            if (offset > 0xfffffff) offsetString = offsetString.Substring(1);
            if (offset > 0xffffff) offsetString = offsetString.Substring(1);
            else offsetString = offsetString.PadLeft(6, '0');
            //offsetString = (offset > 0xffffff) ? Convert.ToString(offset, 16) : $"{("" + offset).PadLeft(12, '0')}";
            //offsetString = offsetString.Substring(2);

            string instruction = $"{condition}{withLink}{offsetString}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeLDRInstruction(string sourceRegister, string baseRegister, int imm12)
        {
            if (imm12 > 0xffff)
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

            string posNeg = (imm12 < 0) ? "1" : "9";

            string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

            string instruction = $"e5{posNeg}{baseRegister}{sourceRegister}{imm12String}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeMOVInstruction(string destinationRegister, string sourceRegister)
        {
            string instruction = $"e1a0{destinationRegister}00{sourceRegister}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeMOVTInstruction(string destinationRegister, int imm16)
        {
            if (imm16 > 0xffff)
            {
                throw new SyntaxException("On MOVT, op2 cannot be larger than 0xFFFF");
            }

            string imm4 = IntToHexString(imm16, 3);
            string imm12 = $"{IntToHexString(imm16, 2)}{IntToHexString(imm16, 1)}{IntToHexString(imm16, 0)}";

            string instruction = $"e34{imm4}{destinationRegister}{imm12}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeMOVWInstruction(string destinationRegister, int imm16)
        {
            if (imm16 > 0xffff)
            {
                throw new SyntaxException("On MOVW, op2 cannot be larger than 0xFFFF");
            }

            string imm4 = IntToHexString(imm16, 3);
            string imm12 = $"{IntToHexString(imm16, 2)}{IntToHexString(imm16, 1)}{IntToHexString(imm16, 0)}";

            string instruction = $"e30{imm4}{destinationRegister}{imm12}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodePOPInstruction(string destinationRegister)
        {

            string instruction = $"e49d{destinationRegister}004";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodePUSHInstruction(string sourceRegister)
        {

            string instruction = $"e52d{sourceRegister}004";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeSTRInstruction(string sourceRegister, string baseRegister, int imm12)
        {
            if (imm12 > 0xfff)
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

            string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

            string instruction = $"e50{baseRegister}{sourceRegister}{imm12String}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        public uint EncodeSUBSInstruction(string destinationRegister, string sourceRegister, int imm12)
        {
            if (imm12 > 0xfff)
            {
                throw new SyntaxException("On SUBS, op2 cannot be larger than 0xFFF");
            }

            string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

            string instruction = $"e25{destinationRegister}{sourceRegister}{imm12String}";
            uint encodedOperation = Convert.ToUInt32(instruction, 16);
            return encodedOperation;
        }

        //public bool TryParseDataProc(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var operation = tokenStream.Next();
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("ands"))
        //    {
        //        parseResult = TryEncodeANDS(tokenStream, out encodedOperation);
        //    }
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("mov"))
        //    {
        //        parseResult = TryEncodeMOV(tokenStream, out encodedOperation);
        //    }
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("movt"))
        //    {
        //        parseResult = TryEncodeMOVT(tokenStream, out encodedOperation);
        //    }
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("movw"))
        //    {
        //        parseResult = TryEncodeMOVW(tokenStream, out encodedOperation);
        //    }
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("subs"))
        //    {
        //        parseResult = TryEncodeSUBS(tokenStream, out encodedOperation);
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(operation);
        //        parseResult = false;
        //    }

        //    return parseResult;
        //}

        //public bool TryParseLoadStore(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var operation = tokenStream.Next();
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("ldr"))
        //    {
        //        parseResult = TryEncodeLDR(tokenStream, out encodedOperation);
        //    }
        //    //else if (operation != null &&
        //    //    operation.Value().ToLowerInvariant().Equals("ldiia"))
        //    //{
        //    //    parseResult = TryEncodeLDIIA(tokenStream, out encodedOperation);
        //    //}
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("pop"))
        //    {
        //        parseResult = TryEncodePOP(tokenStream, out encodedOperation);
        //    }
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("push"))
        //    {
        //        parseResult = TryEncodePUSH(tokenStream, out encodedOperation);
        //    }
        //    else if (operation != null &&
        //        operation.Value().ToLowerInvariant().Equals("str"))
        //    {
        //        parseResult = TryEncodeSTR(tokenStream, out encodedOperation);
        //    }
        //    //else if (operation != null &&
        //    //    operation.Value().ToLowerInvariant().Equals("strdb"))
        //    //{
        //    //    parseResult = TryEncodeSTRDB(tokenStream, out encodedOperation);
        //    //}
        //    else
        //    {
        //        tokenStream.UnGet(operation);
        //        parseResult = false;
        //    }

        //    return parseResult;
        //}

        //public bool TryEncodeANDS(ITokenStream tokenStream, out uint encodedOperation)
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
        //        if (intVal > 0xffff)
        //        {
        //            throw new SyntaxException("On ANDS, op2 cannot be larger than 0xFFF");
        //        }

        //        string rn = RegisterToHex(arg1);
        //        string rd = RegisterToHex(arg2);
        //        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e21{rn}{rd}{imm12}";
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

        //public bool TryEncodeMOV(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();
        //    var comma1 = tokenStream.Next();
        //    var arg2 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken) &&
        //        comma1 != null &&
        //        comma1.GetType() == typeof(CommaToken) &&
        //        arg2 != null &&
        //        arg2.GetType() == typeof(AlphaNumToken))
        //    {
        //        string rd = RegisterToHex(arg1);
        //        string rm = RegisterToHex(arg2);

        //        string opString = $"e1a0{rd}00{rm}";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg2);
        //        tokenStream.UnGet(comma1);
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }

        //    return parseResult;
        //}

        //public bool TryEncodeMOVT(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();
        //    var comma1 = tokenStream.Next();
        //    var arg2 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken) &&
        //        comma1 != null &&
        //        comma1.GetType() == typeof(CommaToken) &&
        //        arg2 != null &&
        //        arg2.GetType() == typeof(NumberToken))
        //    {

        //        int intVal = (arg2 as NumberToken).IntValue();
        //        if (intVal > 0xffff)
        //        {
        //            throw new SyntaxException("On MOVT, op2 cannot be larger than 0xFFFF");
        //        }

        //        string imm4 = IntToHexString(intVal, 3);
        //        string rd = RegisterToHex(arg1);
        //        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e34{imm4}{rd}{imm12}";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg2);
        //        tokenStream.UnGet(comma1);
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }

        //    return parseResult;
        //}

        //public bool TryEncodeMOVW(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();
        //    var comma1 = tokenStream.Next();
        //    var arg2 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken) &&
        //        comma1 != null &&
        //        comma1.GetType() == typeof(CommaToken) &&
        //        arg2 != null &&
        //        arg2.GetType() == typeof(NumberToken))
        //    {

        //        int intVal = (arg2 as NumberToken).IntValue();
        //        if (intVal > 0xffff)
        //        {
        //            throw new SyntaxException("On MOVW, op2 cannot be larger than 0xFFFF");
        //        }

        //        string imm4 = IntToHexString(intVal, 3);
        //        string rd = RegisterToHex(arg1);
        //        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e30{imm4}{rd}{imm12}";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg2);
        //        tokenStream.UnGet(comma1);
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }
        //    return parseResult;
        //}

        //public bool TryEncodeLDR(ITokenStream tokenStream, out uint encodedOperation)
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
        //            throw new SyntaxException("On LDR, op2 cannot be larger than 0xFFF");
        //        }
        //        // p 0 = post, 1 = pre (when using immediate, p=1
        //        // u 0 = down, 1 = up, (is imm12 positive or negative)
        //        // b 0 = word, 1 = byte
        //        // w 0 = no write back, 1 = write back (when using immediate, w=0)
        //        // l 0 = store, 1 = load

        //        // A8.8.204
        //        // {cond}010{P}{U}0{W}1{Rn}{Rt}{imm12}
        //        // 1110  0101   1 0 0 1 
        //        // 1110  0101   1001    rn rt imm12
        //        // e     5      9     
        //        //
        //        // e59{rn}{rt}{imm12}  // when imm12 is positive
        //        // e51{rn}{rt}{imm12}  // when imm12 is negitive

        //        string posNeg = ((arg3 as NumberToken).IsNegative()) ? "1" : "9";
        //        string rt = RegisterToHex(arg1);
        //        string rn = RegisterToHex(arg2);
        //        string offset = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e5{posNeg}{rn}{rt}{offset}";
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

        ////public bool TryEncodeLDIIA(ITokenStream tokenStream, out uint encodedOperation)
        ////{
        ////    var parseResult = true;
        ////    encodedOperation = 0;

        ////    var arg1 = tokenStream.Next();
        ////    var comma1 = tokenStream.Next();
        ////    var arg2 = tokenStream.Next();
        ////    var comma2 = tokenStream.Next();
        ////    var arg3 = tokenStream.Next();

        ////    if (arg1 != null &&
        ////        arg1.GetType() == typeof(AlphaNumToken) &&
        ////        comma1 != null &&
        ////        comma1.GetType() == typeof(CommaToken) &&
        ////        arg2 != null &&
        ////        arg2.GetType() == typeof(AlphaNumToken) &&
        ////        comma2 != null &&
        ////        comma2.GetType() == typeof(CommaToken) &&
        ////        arg3 != null &&
        ////        arg3.GetType() == typeof(NumberToken))
        ////    {
        ////        int intVal = (arg3 as NumberToken).IntValue();
        ////        if (intVal > 0xfff)
        ////        {
        ////            throw new SyntaxException("On LDIIA, op2 cannot be larger than 0xFFF");
        ////        }
        ////        // p 0 = post, 1 = pre
        ////        // u 0 = down, 1 = up,
        ////        // b 0 = word, 1 = byte
        ////        // w 0 = no write back, 1 = write back
        ////        // l 0 = store, 1 = load

        ////        // A8.8.204
        ////        // {cond}010{P}{U}0{W}1{Rn}{Rt}{imm12}
        ////        // 1110  0100   1011    rn rt imm12
        ////        // e     4      b     

        ////        string rd = RegisterToHex(arg1);
        ////        string rn = RegisterToHex(arg2);
        ////        string offset = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        ////        string opString = $"e4b{rn}{rd}{offset}";
        ////        encodedOperation = Convert.ToUInt32(opString, 16);
        ////        parseResult = true;
        ////    }
        ////    else
        ////    {
        ////        tokenStream.UnGet(arg3);
        ////        tokenStream.UnGet(comma2);
        ////        tokenStream.UnGet(arg2);
        ////        tokenStream.UnGet(comma1);
        ////        tokenStream.UnGet(arg1);
        ////        parseResult = false;
        ////    }
        ////    return parseResult;
        ////}

        //public bool TryEncodePOP(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken))
        //    {
        //        string rt = RegisterToHex(arg1);

        //        //0xe49d{rt}004

        //        string opString = $"e49d{rt}004";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else if (arg1 != null &&
        //        arg1.GetType() == typeof(LeftCurlyToken))

        //    {
        //        var reg1 = tokenStream.Next();
        //        List<Token> registerList = new List<Token>();
        //        registerList.Add(reg1);

        //        var next = tokenStream.Next();
        //        while (next.GetType() != typeof(RightCurlyToken))
        //        {
        //            registerList.Add(next);
        //            next = tokenStream.Next();
        //        }

        //        if (registerList.Count == 1)
        //        {
        //            string rt = RegisterToHex(reg1);

        //            //0xe49d{rt}004

        //            string opString = $"e49d{rt}004";
        //            encodedOperation = Convert.ToUInt32(opString, 16);
        //            parseResult = true;
        //        }
        //        else
        //        {
        //            int registerInt = 0;
        //            for (int i = 0; i < registerList.Count; i++)
        //            {
        //                var token = registerList[i];
        //                if (token.GetType() == typeof(AlphaNumToken))
        //                {
        //                    string regNumString = RegisterToHex(token);
        //                    int regNum = Convert.ToInt32(regNumString, 16);
        //                    int bit = (1 << regNum);
        //                    registerInt |= bit;
        //                }
        //            }
        //            string registerIntToString = Convert.ToString(registerInt, 16).PadLeft(4, '0');

        //            //0xe49d{rt}004

        //            string opString = $"e8bd{registerIntToString}";
        //            encodedOperation = Convert.ToUInt32(opString, 16);
        //            parseResult = true;
        //        }
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }
        //    return parseResult;
        //}

        //public bool TryEncodePUSH(ITokenStream tokenStream, out uint encodedOperation)
        //{
        //    var parseResult = true;
        //    encodedOperation = 0;

        //    var arg1 = tokenStream.Next();

        //    if (arg1 != null &&
        //        arg1.GetType() == typeof(AlphaNumToken))
        //    {
        //        string rt = RegisterToHex(arg1);

        //        //0xe52d{rt}004

        //        string opString = $"e52d{rt}004";
        //        encodedOperation = Convert.ToUInt32(opString, 16);
        //        parseResult = true;
        //    }
        //    else if (arg1 != null &&
        //        arg1.GetType() == typeof(LeftCurlyToken))

        //    {
        //        var reg1 = tokenStream.Next();

        //        List<Token> registerList = new List<Token>();
        //        registerList.Add(reg1);

        //        var next = tokenStream.Next();
        //        while (next.GetType() != typeof(RightCurlyToken))
        //        {
        //            registerList.Add(next);
        //            next = tokenStream.Next();
        //        }

        //        if (registerList.Count == 1)
        //        {
        //            string rt = RegisterToHex(reg1);

        //            //0xe52d{rt}004

        //            string opString = $"e52d{rt}004";
        //            encodedOperation = Convert.ToUInt32(opString, 16);
        //            parseResult = true;
        //        }
        //        else
        //        {
        //            int registerInt = 0;
        //            for (int i = 0; i < registerList.Count; i++)
        //            {
        //                var token = registerList[i];
        //                if (token.GetType() == typeof(AlphaNumToken))
        //                {
        //                    string regNumString = RegisterToHex(token);
        //                    int regNum = Convert.ToInt32(regNumString, 16);
        //                    int bit = (1 << regNum);
        //                    registerInt |= bit;
        //                }
        //            }
        //            string registerIntToString = Convert.ToString(registerInt, 16).PadLeft(4, '0');

        //            //0xe49d{rt}004

        //            string opString = $"e92d{registerIntToString}";
        //            encodedOperation = Convert.ToUInt32(opString, 16);
        //            parseResult = true;
        //        }
        //    }
        //    else
        //    {
        //        tokenStream.UnGet(arg1);
        //        parseResult = false;
        //    }
        //    return parseResult;
        //}

        ////public bool TryEncodeSTRDB(ITokenStream tokenStream, out uint encodedOperation)
        ////{
        ////    var parseResult = true;
        ////    encodedOperation = 0;

        ////    var arg1 = tokenStream.Next();
        ////    var comma1 = tokenStream.Next();
        ////    var arg2 = tokenStream.Next();
        ////    var comma2 = tokenStream.Next();
        ////    var arg3 = tokenStream.Next();

        ////    if (arg1 != null &&
        ////        arg1.GetType() == typeof(AlphaNumToken) &&
        ////        comma1 != null &&
        ////        comma1.GetType() == typeof(CommaToken) &&
        ////        arg2 != null &&
        ////        arg2.GetType() == typeof(AlphaNumToken) &&
        ////        comma2 != null &&
        ////        comma2.GetType() == typeof(CommaToken) &&
        ////        arg3 != null &&
        ////        arg3.GetType() == typeof(NumberToken))
        ////    {
        ////        int intVal = (arg3 as NumberToken).IntValue();
        ////        if (intVal > 0xfff)
        ////        {
        ////            throw new SyntaxException("On STRDB, op2 cannot be larger than 0xFFF");
        ////        }
        ////        // p 0 = post, 1 = pre
        ////        // u 0 = down, 1 = up,
        ////        // b 0 = word, 1 = byte
        ////        // w 0 = no write back, 1 = write back
        ////        // l 0 = store, 1 = load

        ////        // A8.8.204
        ////        // {cond}010{P}{U}0{W}0{Rn}{Rt}{imm12}
        ////        // 1110  0101   0010    rn rt imm12
        ////        // e     5      2     

        ////        string rt = RegisterToHex(arg1);
        ////        string rn = RegisterToHex(arg2);
        ////        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        ////        string opString = $"e52{rn}{rt}{imm12}";
        ////        encodedOperation = Convert.ToUInt32(opString, 16);
        ////        parseResult = true;
        ////    }
        ////    else
        ////    {
        ////        tokenStream.UnGet(arg3);
        ////        tokenStream.UnGet(comma2);
        ////        tokenStream.UnGet(arg2);
        ////        tokenStream.UnGet(comma1);
        ////        tokenStream.UnGet(arg1);
        ////        parseResult = false;
        ////    }
        ////    return parseResult;
        ////}

        //public bool TryEncodeSTR(ITokenStream tokenStream, out uint encodedOperation)
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
        //            throw new SyntaxException("On STR, op2 cannot be larger than 0xFFF");
        //        }
        //        // p 0 = post, 1 = pre
        //        // u 0 = down, 1 = up,
        //        // b 0 = word, 1 = byte
        //        // w 0 = no write back, 1 = write back
        //        // l 0 = store, 1 = load

        //        // A8.8.204
        //        // {cond}010{P}{U}0{W}0{Rn}{Rt}{imm12}
        //        // 1110  0101   0000    rn rt imm12
        //        // e     5      0     

        //        string rt = RegisterToHex(arg1);
        //        string rn = RegisterToHex(arg2);
        //        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e50{rn}{rt}{imm12}";
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

        //public bool TryEncodeSUBS(ITokenStream tokenStream, out uint encodedOperation)
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
        //        if (intVal > 0xffff)
        //        {
        //            throw new SyntaxException("On SUBS, op2 cannot be larger than 0xFFF");
        //        }

        //        string rn = RegisterToHex(arg1);
        //        string rd = RegisterToHex(arg2);
        //        string imm12 = $"{IntToHexString(intVal, 2)}{IntToHexString(intVal, 1)}{IntToHexString(intVal, 0)}";

        //        string opString = $"e25{rn}{rd}{imm12}";
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