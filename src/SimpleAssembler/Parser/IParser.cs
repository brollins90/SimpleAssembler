namespace SimpleAssembler.Parser
{
    using System.Collections.Generic;

    public interface IParser
    {
        List<uint> Kernel { get; }
        uint KernelIndex { get; }
        Dictionary<string, uint> LabelTable { get; }
        uint LineNumber { get; }

        uint[] Parse(string fileData);
    }
}