namespace SimpleAssembler
{
    using Tokenizer;
    using System;
    using System.Collections.Generic;

    public class Parser
    {
        private string _fileString;
        private int _kernelIndex;
        private List<int> _kernelImgList;
        private Dictionary<string, int> _labelTable;

        public Parser(string fileString)
        {
            _fileString = fileString;
            _kernelIndex = 0;
            _kernelImgList = new List<int>();
            _labelTable = new Dictionary<string, int>();
        }

        public int[] Parse()
        {
            ITokenStream tokenStream = new TokenStream();
            tokenStream.Load(_fileString);

            while (tokenStream.HasNext())
            {
                ParseInstruction(tokenStream);
            }

            int[] outputArray = _kernelImgList.ToArray();
            return outputArray;
        }

        private void ParseInstruction(ITokenStream tokenStream)
        {
            TryParseLabel(tokenStream);
            ParseOperation(tokenStream);
        }

        private void ParseOperation(ITokenStream tokenStream)
        {
            if (TryParseBranch(tokenStream)) { }
            else if (TryParseDataProc(tokenStream)) { }
            else if (TryParseLoadStore(tokenStream)) { }
            else { throw new SyntaxException(); }
        }

        private bool TryParseBranch(ITokenStream tokenStream)
        {
            var operation = tokenStream.Next();
            var label = tokenStream.Next();
            var parseResult = true;

            if (operation.GetType() == typeof(AlphaNumToken)
                && IsValidBranch(operation)
                && label.GetType() == typeof(AlphaNumToken)
                && _labelTable.ContainsKey(label.Value()))
            {
                // encode operation
                // append op to kernel img
            }
            else { parseResult = false; }

            return parseResult;
        }

        private bool TryParseDataProc(ITokenStream tokenStream)
        {
            var parseResult = true;

            parseResult = false;

            return parseResult;
        }

        private object TryParseLabel(ITokenStream tokenStream)
        {
            var label = tokenStream.Next();
            var colon = tokenStream.Next();
            var parseResult = true;

            if (label.GetType() == typeof(AlphaNumToken) &&
                colon.GetType() == typeof(ColonToken))
            {
                // add to label table
                _labelTable.Add(label.Value(), _kernelIndex);
            }
            else
            {
                tokenStream.UnGet(colon);
                tokenStream.UnGet(label);
                parseResult = false;
            }
            return parseResult;
        }

        private bool TryParseLoadStore(ITokenStream tokenStream)
        {
            var parseResult = true;

            parseResult = false;

            return parseResult;
        }

        private bool IsValidBranch(Token token)
        {
            var isValid = true;

            if (token.Value().Equals("BAL", StringComparison.InvariantCultureIgnoreCase)
                && token.Value().Equals("BNE", StringComparison.InvariantCultureIgnoreCase))
            { }
            else { isValid = false; }

            return isValid;
        }
    }
}