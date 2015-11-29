namespace SimpleCompiler.Parser
{
    using Lexer.LexTokens;
    using Expressions;
    using System;
    using System.Collections.Generic;

    public class Parser : IParser
    {
        int KERNEL_SIZE = 0x3000;

        public List<uint> Kernel { get; private set; }
        public uint KernelIndex { get; private set; }
        public uint LineNumber { get; private set; }
        public Dictionary<string, uint> LabelTable { get; }

        public Parser()
        {
            //Kernel = new List<uint>(KERNEL_SIZE);
            //for (int i = 0; i < KERNEL_SIZE; i++) { Kernel.Add(0); }
            KernelIndex = 0;
            LineNumber = 1;
            LabelTable = new Dictionary<string, uint>();
        }

        public uint[] Parse(string fileData)
        {
            Kernel = new List<uint>(KERNEL_SIZE);
            for (int i = 0; i < KERNEL_SIZE; i++) { Kernel.Add(0); }

            // First round to construct the label table
            Lexer.Lexer lexer = new Lexer.Lexer(fileData);

            while (lexer.HasNext())
            {
                //ParseInstruction(lexer, true);
            }

            // reset the stuff after the first go round that found the label locations
            Kernel = new List<uint>(KERNEL_SIZE);
            for (int i = 0; i < KERNEL_SIZE; i++) { Kernel.Add(0); }
            lexer = new Lexer.Lexer(fileData);
            KernelIndex = 0;
            LineNumber = 1;

            while (lexer.HasNext())
            {
                //ParseInstruction(lexer, false);
            }

            return Kernel.ToArray();
        }

        public Expression ParsePrimary(Lexer.Lexer lexer)
        {
            return null;
        }

        //public void ParseInstruction(Lexer.Lexer lexer, bool buildingLabelTable = false)
        //{
        //    var operation = lexer.Next();
        //    lexer.UnGet(operation);

        //    if (operation is AddressDataStatementLexToken)
        //    {
        //        ParseAddressDataStatement(lexer);
        //    }
        //    else if (operation is ByteDataStatementLexToken)
        //    {
        //        ParseByteDataStatement(lexer);
        //    }
        //    else if (operation is IfStatementLexToken)
        //    {
        //        ParseIfStatement(lexer, buildingLabelTable);
        //    }
        //    else if (operation is LabelDeclarationLexToken)
        //    {
        //        ParseLabelDeclaration(lexer, buildingLabelTable);
        //    }
        //    else if (operation is NewLineLexToken)
        //    {
        //        ParseNewLine(lexer);
        //    }
        //    else if (operation is OpCodeLexToken)
        //    {
        //        ParseOperation(lexer, buildingLabelTable);
        //    }
        //    else if (operation is WordDataStatementLexToken)
        //    {
        //        ParseWordDataStatement(lexer);
        //    }
        //    else
        //    {
        //        throw new SyntaxException($"Unknown instruction: {operation.Value()} on line {LineNumber}");
        //    }
        //}


        //public void ParseAddressDataStatement(Lexer.Lexer lexer)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is AddressDataStatementLexToken)
        //    {
        //        var address = lexer.Next();

        //        if (address != null
        //            && address is NumberLexToken)
        //        {
        //            KernelIndex = (uint)(address as NumberLexToken).IntValue() / 4; // TODO make a uintvalue method
        //        }
        //    }
        //}

        //private NumberLexToken GetNextByte(Lexer.Lexer lexer)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is NumberLexToken)
        //    {
        //        if ((token as NumberLexToken).IntValue() > 0xff
        //            || (token as NumberLexToken).IntValue() < 0x0)
        //        {
        //            throw new SyntaxException($"{token.Value()} is not the correct size of a byte");
        //        }
        //    }
        //    else
        //    {
        //        lexer.UnGet(token);
        //        token = new NumberLexToken("0x0");
        //    }
        //    return (NumberLexToken)token;
        //}

        //public void ParseByteDataStatement(Lexer.Lexer lexer)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is ByteDataStatementLexToken)
        //    {
        //        var a1 = lexer.Next();
        //        while (a1 != null
        //            && a1 is NumberLexToken
        //            //&& (a1 as NumberLexToken).IntValue() != 0
        //            )
        //        {
        //            if ((a1 as NumberLexToken).IntValue() > 0xff
        //                || (a1 as NumberLexToken).IntValue() < 0x0)
        //            {
        //                throw new SyntaxException($"{a1.Value()} is not the correct size of a byte");
        //            }
        //            var a2 = GetNextByte(lexer);
        //            var a3 = GetNextByte(lexer);
        //            var a4 = GetNextByte(lexer);

        //            var data = $"{a4.Value().Substring(2).PadLeft(2, '0')}{a3.Value().Substring(2).PadLeft(2, '0')}{a2.Value().Substring(2).PadLeft(2, '0')}{a1.Value().Substring(2).PadLeft(2, '0')}";
        //            var encoded = Convert.ToUInt32(data, 16);
        //            WriteInstructionToKernel(encoded);

        //            a1 = lexer.Next();
        //        }
        //        lexer.UnGet(a1);
        //    }
        //}

        //public void ParseIfStatement(Lexer.Lexer lexer, bool buildingLabelTable)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is IfStatementLexToken)
        //    {
        //        var operand1 = (lexer.Next() as RegisterLexToken);
        //        var op = (lexer.Next() as OperatorLexToken);
        //        var operand2 = lexer.Next();
        //        var thenStatement = lexer.Next();
        //        var thenLabel = (lexer.Next() as LabelReferenceLexToken);
        //        var elseStatement = lexer.Next();
        //        var elseLabel = (lexer.Next() as LabelReferenceLexToken);

        //        bool op2IsNumber = (operand2 is NumberLexToken);

        //        //if (op2IsNumber)
        //        WriteInstructionToKernel(EncodeCMPIInstruction(operand1.Value(), (operand2 as NumberLexToken).IntValue()));
        //        //else
        //        //    EncodeCMPInstruction(operand1.Value(), operand2.Value());

        //        WriteInstructionToKernel(EncodeBranchInstruction(op.Condition, "a", thenLabel.Value(), buildingLabelTable));
        //        WriteInstructionToKernel(EncodeBranchInstruction("e", "a", elseLabel.Value(), buildingLabelTable));
        //    }
        //}

        //public void ParseLabelDeclaration(Lexer.Lexer lexer, bool buildingLabelTable)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is LabelDeclarationLexToken)
        //    {
        //        if (buildingLabelTable
        //            && !LabelTable.ContainsKey(token.Value()))
        //        {
        //            LabelTable.Add(token.Value(), KernelIndex);
        //        }
        //    }
        //}

        //public void ParseNewLine(Lexer.Lexer lexer)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is NewLineLexToken)
        //    {
        //        LineNumber++;
        //    }
        //}

        //public uint ParseOperation(Lexer.Lexer lexer, bool buildLabelTable)
        //{
        //    uint encodedInstruction = 0;

        //    var token = lexer.Next();
        //    if (token != null
        //        && token is OpCodeLexToken)
        //    {
        //        var operation = token as OpCodeLexToken;

        //        switch (operation.OperationType)
        //        {
        //            case OperationType.ADDI:
        //                encodedInstruction = EncodeADDIInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // imm12 
        //                break;

        //            case OperationType.ANDS:
        //                encodedInstruction = EncodeANDSInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // imm12 
        //                break;

        //            case OperationType.BAL:
        //                encodedInstruction = EncodeBranchInstruction(
        //                    "e",                                            // Always
        //                    "a",                                            // No Link
        //                    lexer.Next().Value(),                           // Label
        //                    buildLabelTable);
        //                break;

        //            case OperationType.BEQ:
        //                encodedInstruction = EncodeBranchInstruction(
        //                    "0",                                            // Equal
        //                    "a",                                            // No Link
        //                    lexer.Next().Value(),                           // Label
        //                    buildLabelTable);
        //                break;

        //            case OperationType.BGE:
        //                encodedInstruction = EncodeBranchInstruction(
        //                    "a",                                            // Greater than or equal
        //                    "a",                                            // No Link
        //                    lexer.Next().Value(),                           // Label
        //                    buildLabelTable);
        //                break;

        //            case OperationType.BL:
        //                encodedInstruction = EncodeBranchInstruction(
        //                    "e",                                            // Always
        //                    "b",                                            // Link
        //                    lexer.Next().Value(),                           // Label
        //                    buildLabelTable);
        //                break;

        //            case OperationType.BNE:
        //                encodedInstruction = EncodeBranchInstruction(
        //                    "1",                                            // Not Equal
        //                    "a",                                            // No Link
        //                    lexer.Next().Value(),                           // Label
        //                    buildLabelTable);
        //                break;

        //            case OperationType.CMPI:
        //                encodedInstruction = EncodeCMPIInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // imm16 
        //                break;

        //            case OperationType.LDR:
        //                encodedInstruction = EncodeLDRInstruction(
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    lexer.Next().Value(),                           // baseRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // offset 
        //                break;

        //            case OperationType.LDRB:
        //                encodedInstruction = EncodeLDRBInstruction(
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    lexer.Next().Value(),                           // baseRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // offset 
        //                break;

        //            case OperationType.MOV:
        //                encodedInstruction = EncodeMOVInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    lexer.Next().Value());                          // sourceRegister
        //                break;

        //            case OperationType.MOVT:
        //                encodedInstruction = EncodeMOVTInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // imm16 
        //                break;

        //            case OperationType.MOVW:
        //                encodedInstruction = EncodeMOVWInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // imm16 
        //                break;

        //            case OperationType.POP:
        //                encodedInstruction = EncodePOPInstruction(
        //                    lexer.Next().Value());                           // destinationRegister
        //                break;

        //            case OperationType.PUSH:
        //                encodedInstruction = EncodePUSHInstruction(
        //                    lexer.Next().Value());                           // sourceRegister
        //                break;

        //            case OperationType.ROR:
        //                encodedInstruction = EncodeRORInstruction(
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    lexer.Next().Value(),                           // baseRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // shift 
        //                break;

        //            case OperationType.STR:
        //                encodedInstruction = EncodeSTRInstruction(
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    lexer.Next().Value(),                           // baseRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // offset 
        //                break;

        //            case OperationType.SUBS:
        //                encodedInstruction = EncodeSUBSInstruction(
        //                    lexer.Next().Value(),                           // destinationRegister
        //                    lexer.Next().Value(),                           // sourceRegister
        //                    (lexer.Next() as NumberLexToken).IntValue());   // imm12 
        //                break;
        //        }
        //    }

        //    WriteInstructionToKernel(encodedInstruction);
        //    return encodedInstruction;
        //}

        //public void ParseWordDataStatement(Lexer.Lexer lexer)
        //{
        //    var token = lexer.Next();
        //    if (token != null
        //        && token is WordDataStatementLexToken)
        //    {
        //        var a1 = lexer.Next();
        //        while (a1 != null
        //            && a1 is NumberLexToken
        //            && (a1 as NumberLexToken).IntValue() != 0)
        //        {
        //            var encoded = Convert.ToUInt32(a1.Value(), 16);
        //            WriteInstructionToKernel(encoded);

        //            a1 = lexer.Next();
        //        }
        //        lexer.UnGet(a1);
        //    }
        //}


        //public uint EncodeADDIInstruction(string destinationRegister, string sourceRegister, int imm12)
        //{
        //    if (imm12 > 0xfff)
        //    {
        //        throw new SyntaxException("On ADDI, op2 cannot be larger than 0xFFF");
        //    }

        //    string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

        //    string instruction = $"e28{destinationRegister}{sourceRegister}{imm12String}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeANDSInstruction(string destinationRegister, string sourceRegister, int imm12)
        //{
        //    if (imm12 > 0xfff)
        //    {
        //        throw new SyntaxException("On ANDS, op2 cannot be larger than 0xFFF");
        //    }

        //    string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

        //    string instruction = $"e21{destinationRegister}{sourceRegister}{imm12String}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeBranchInstruction(string condition, string withLink, string label, bool buildLabelTable)
        //{
        //    uint labelLocation = 0;
        //    if (LabelTable.ContainsKey(label))
        //    {
        //        labelLocation = LabelTable[label];
        //    }
        //    else
        //    {
        //        if (!buildLabelTable)
        //        {
        //            throw new SyntaxException($"Label {label} not in label table");
        //        }
        //    }

        //    uint offset = labelLocation - KernelIndex;
        //    offset = offset - 2;

        //    string offsetString = Convert.ToString(offset, 16);
        //    if (offset > 0xfffffff) offsetString = offsetString.Substring(1);
        //    if (offset > 0xffffff) offsetString = offsetString.Substring(1);
        //    else offsetString = offsetString.PadLeft(6, '0');
        //    //offsetString = (offset > 0xffffff) ? Convert.ToString(offset, 16) : $"{("" + offset).PadLeft(12, '0')}";
        //    //offsetString = offsetString.Substring(2);

        //    string instruction = $"{condition}{withLink}{offsetString}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeCMPIInstruction(string destinationRegister, int imm16)
        //{
        //    if (imm16 > 0xfff)
        //    {
        //        throw new SyntaxException("On CMPI, op2 cannot be larger than 0xFFF");
        //    }
            
        //    string imm12 = $"{IntToHexString(imm16, 2)}{IntToHexString(imm16, 1)}{IntToHexString(imm16, 0)}";

        //    string instruction = $"e35{destinationRegister}0{imm12}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeLDRInstruction(string sourceRegister, string baseRegister, int imm12)
        //{
        //    if (imm12 > 0xfff)
        //    {
        //        throw new SyntaxException("On LDR, op2 cannot be larger than 0xFFF");
        //    }

        //    // p 0 = post, 1 = pre (when using immediate, p=1
        //    // u 0 = down, 1 = up, (is imm12 positive or negative)
        //    // b 0 = word, 1 = byte
        //    // w 0 = no write back, 1 = write back (when using immediate, w=0)
        //    // l 0 = store, 1 = load

        //    // A8.8.204
        //    // {cond}010{P}{U}0{W}1{Rn}{Rt}{imm12}
        //    // 1110  0101   1 0 0 1 
        //    // 1110  0101   1001    rn rt imm12
        //    // e     5      9     
        //    //
        //    // e59{rn}{rt}{imm12}  // when imm12 is positive
        //    // e51{rn}{rt}{imm12}  // when imm12 is negitive

        //    string posNeg = (imm12 < 0) ? "1" : "9";

        //    string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

        //    string instruction = $"e5{posNeg}{baseRegister}{sourceRegister}{imm12String}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeLDRBInstruction(string sourceRegister, string baseRegister, int imm12)
        //{
        //    if (imm12 > 0xfff)
        //    {
        //        throw new SyntaxException("On LDRB, op2 cannot be larger than 0xFFF");
        //    }

        //    string posNeg = (imm12 < 0) ? "5" : "d";

        //    string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

        //    string instruction = $"e5{posNeg}{baseRegister}{sourceRegister}{imm12String}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeMOVInstruction(string destinationRegister, string sourceRegister)
        //{
        //    string instruction = $"e1a0{destinationRegister}00{sourceRegister}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeMOVTInstruction(string destinationRegister, int imm16)
        //{
        //    if (imm16 > 0xffff)
        //    {
        //        throw new SyntaxException("On MOVT, op2 cannot be larger than 0xFFFF");
        //    }

        //    string imm4 = IntToHexString(imm16, 3);
        //    string imm12 = $"{IntToHexString(imm16, 2)}{IntToHexString(imm16, 1)}{IntToHexString(imm16, 0)}";

        //    string instruction = $"e34{imm4}{destinationRegister}{imm12}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeMOVWInstruction(string destinationRegister, int imm16)
        //{
        //    if (imm16 > 0xffff)
        //    {
        //        throw new SyntaxException("On MOVW, op2 cannot be larger than 0xFFFF");
        //    }

        //    string imm4 = IntToHexString(imm16, 3);
        //    string imm12 = $"{IntToHexString(imm16, 2)}{IntToHexString(imm16, 1)}{IntToHexString(imm16, 0)}";

        //    string instruction = $"e30{imm4}{destinationRegister}{imm12}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodePOPInstruction(string destinationRegister)
        //{

        //    string instruction = $"e49d{destinationRegister}004";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodePUSHInstruction(string sourceRegister)
        //{

        //    string instruction = $"e52d{sourceRegister}004";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeRORInstruction(string destinationRegister, string sourceRegister, int imm5)
        //{
        //    if (imm5 > 0x1f)
        //    {
        //        throw new SyntaxException("On ROR, op2 cannot be larger than 0x1F");
        //    }

        //    string imm5bin = Convert.ToString(imm5, 2) + "110";
        //    int imm5int = Convert.ToInt32(imm5bin, 2);
        //    string imm5String = $"{IntToHexString(imm5int, 1)}{IntToHexString(imm5int, 0)}";

        //    string instruction = $"e1a0{destinationRegister}{imm5String}{sourceRegister}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeSTRInstruction(string sourceRegister, string baseRegister, int imm12)
        //{
        //    if (imm12 > 0xfff)
        //    {
        //        throw new SyntaxException("On STR, op2 cannot be larger than 0xFFF");
        //    }

        //    // p 0 = post, 1 = pre
        //    // u 0 = down, 1 = up,
        //    // b 0 = word, 1 = byte
        //    // w 0 = no write back, 1 = write back
        //    // l 0 = store, 1 = load

        //    // A8.8.204
        //    // {cond}010{P}{U}0{W}0{Rn}{Rt}{imm12}
        //    // 1110  0101   0000    rn rt imm12
        //    // e     5      0     

        //    string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

        //    string instruction = $"e50{baseRegister}{sourceRegister}{imm12String}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //public uint EncodeSUBSInstruction(string destinationRegister, string sourceRegister, int imm12)
        //{
        //    if (imm12 > 0xfff)
        //    {
        //        throw new SyntaxException("On SUBS, op2 cannot be larger than 0xFFF");
        //    }

        //    string imm12String = $"{IntToHexString(imm12, 2)}{IntToHexString(imm12, 1)}{IntToHexString(imm12, 0)}";

        //    string instruction = $"e25{destinationRegister}{sourceRegister}{imm12String}";
        //    uint encodedOperation = Convert.ToUInt32(instruction, 16);
        //    return encodedOperation;
        //}

        //private string IntToHexString(int i, int nibble)
        //{
        //    int ret = (i >> (4 * nibble)) & 0xF;
        //    return Convert.ToString(ret, 16);
        //}

        //private void WriteInstructionToKernel(uint instruction)
        //{
        //    Console.WriteLine($"{KernelIndex}: {Convert.ToString(instruction, 16)}");
        //    Kernel[(int)KernelIndex++] = instruction;
        //}
    }
}