namespace SimpleCompiler
{
    using CommandLine;
    using CommandLine.Text;

    public class Options
    {
        [Option('b', "source", Required = true,
        HelpText = "Input file to be processed.",
        DefaultValue = "C:\\_pi-class\\9\\lab9.b")]
        public string SourceFile { get; set; }

        [Option('s', "Assembly", Required = true,
        HelpText = "Assembly output file.",
        DefaultValue = "C:\\_pi-class\\9\\lab9.s")]
        public string AssemblyFile { get; set; }

        [Option('o', "output", Required = true,
        HelpText = "Output image file to be written.",
        DefaultValue = "C:\\_pi-class\\kernel7.img")]
        public string OutputFile { get; set; }

        [Option('v', "verbose", DefaultValue = true,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}