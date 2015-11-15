namespace SimpleAssemblerTests.Parser
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class ParserTests
    {

        [Fact]
        public void ParserReturnsIntArray()
        {
            SimpleAssembler.Parser parser = new SimpleAssembler.Parser("");
            var result = parser.Parse();

            Assert.IsType(typeof(int[]), result);
        }
    }
}