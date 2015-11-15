namespace SimpleAssembler.Parser
{
    using Tokenizer;
    using System;
    using System.Collections.Generic;

    public class Parser
    {
        //private string _fileString;
        //private List<int> _kernelImgList;
        private int _kernelIndex;
        private Dictionary<string, int> _labelTable;

        public Parser()
        {
            _kernelIndex = 0;
            _labelTable = new Dictionary<string, int>();
        }

        //public Parser(string fileString)
        //{
        //    _fileString = fileString;
        //    _kernelIndex = 0;
        //    _kernelImgList = new List<int>();
        //    
        //}

        public int[] Parse(string fileData)
        {
            List<int> imageList = new List<int>();
            ITokenStream tokenStream = new TokenStream(fileData);

            while (tokenStream.HasNext())
            {
                int instruction = ParseInstruction(tokenStream);
                imageList.Add(instruction);
            }

            int[] outputArray = imageList.ToArray();
            return outputArray;
        }

        public int ParseInstruction(ITokenStream tokenStream)
        {
            int instruction = 0;
            if (TryParseLabel(tokenStream, out instruction)) { return instruction; }
            //else if (TryParseOperation(tokenStream, out instruction)) { return instruction; }
            else { throw new SyntaxException("Unable to parse instruction"); }
        }

        //public void ParseOperation(ITokenStream tokenStream)
        //{
        //    var operation = tokenStream.Next();
        //    int argCount = GetArgCount(operation);

        //    if (TryParseBranch(tokenStream)) { }
        //    else if (TryParseDataProc(tokenStream)) { }
        //    else if (TryParseLoadStore(tokenStream)) { }
        //    else { throw new SyntaxException(); }
        //}

        //public bool TryParseBranch(ITokenStream tokenStream)
        //{
        //    var operation = tokenStream.Next();
        //    var label = tokenStream.Next();
        //    var parseResult = true;

        //    if (operation.GetType() == typeof(AlphaNumToken)
        //        && IsValidBranch(operation)
        //        && label.GetType() == typeof(AlphaNumToken)
        //        && _labelTable.ContainsKey(label.Value()))
        //    {
        //        // encode operation
        //        // append op to kernel img
        //    }
        //    else { parseResult = false; }

        //    return parseResult;
        //}

        //public bool TryParseDataProc(ITokenStream tokenStream)
        //{
        //    var operation = tokenStream.Next();
        //    int argCount = GetArgCount(operation);
        //    var parseResult = true;

        //    parseResult = false;

        //    return parseResult;
        //}

        public bool TryParseLabel(ITokenStream tokenStream, out int labelIndex)
        {
            labelIndex = -1;
            var label = tokenStream.Next();
            var colon = tokenStream.Next();
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

        //public bool TryParseLoadStore(ITokenStream tokenStream)
        //{
        //    var parseResult = true;

        //    parseResult = false;

        //    return parseResult;
        //}

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

        //public bool IsValidBranch(Token token)
        //{
        //    var isValid = true;

        //    if (token.Value().Equals("BAL", StringComparison.InvariantCultureIgnoreCase)
        //        && token.Value().Equals("BNE", StringComparison.InvariantCultureIgnoreCase))
        //    { }
        //    else { isValid = false; }

        //    return isValid;
        //}

        //public bool IsValidDataProc(Token token)
        //{
        //    var isValid = true;

        //    if (token.Value().Equals("MOV", StringComparison.InvariantCultureIgnoreCase)
        //        && token.Value().Equals("ORR", StringComparison.InvariantCultureIgnoreCase))
        //    { }
        //    else { isValid = false; }

        //    return isValid;
        //}
    }
}