namespace SimpleCompilerTests.Parser
{
    using SimpleCompiler.Parser;
    using SimpleCompiler.Parser.Expressions;
    using Xunit;

    public class ParserTests
    {

        [Fact]
        public void ReturnsParseTree()
        {
            IParser parser = new Parser();

            var myProgram = "2";

            var output = parser.Parse(myProgram);

            Assert.IsType(typeof(IntegerExpression), output);
        }

        [Fact]
        public void SingleInt()
        {
            IParser parser = new Parser();

            var myProgram = "1";

            var output = parser.Parse(myProgram);

            Assert.IsType(typeof(IntegerExpression), output);
        }

        [Fact]
        public void BasicMath()
        {
            IParser parser = new Parser();

            var myProgram = "1 + 2";

            var output = parser.Parse(myProgram);

            Assert.IsType(typeof(BinaryOperatorExpression), output);
            Assert.Equal("+", (output as BinaryOperatorExpression).Operation);

            Assert.IsType(typeof(IntegerExpression), (output as BinaryOperatorExpression).LHS);
            Assert.Equal(1, ((output as BinaryOperatorExpression).LHS as IntegerExpression).Value);

            Assert.IsType(typeof(IntegerExpression), (output as BinaryOperatorExpression).RHS);
            Assert.Equal(2, ((output as BinaryOperatorExpression).RHS as IntegerExpression).Value);
        }

        [Fact]
        public void BasicMath2()
        {
            IParser parser = new Parser();

            var myProgram = "1 + 2 + 3";

            var output = parser.Parse(myProgram);

            Assert.IsType(typeof(BinaryOperatorExpression), output);
            Assert.Equal("+", (output as BinaryOperatorExpression).Operation);

            Assert.IsType(typeof(BinaryOperatorExpression), (output as BinaryOperatorExpression).LHS);

            Assert.IsType(typeof(IntegerExpression), ((output as BinaryOperatorExpression).LHS as BinaryOperatorExpression).LHS);
            Assert.Equal(1, (((output as BinaryOperatorExpression).LHS as BinaryOperatorExpression).LHS as IntegerExpression).Value);

            Assert.IsType(typeof(IntegerExpression), ((output as BinaryOperatorExpression).LHS as BinaryOperatorExpression).RHS);
            Assert.Equal(2, (((output as BinaryOperatorExpression).LHS as BinaryOperatorExpression).RHS as IntegerExpression).Value);

            Assert.IsType(typeof(IntegerExpression), (output as BinaryOperatorExpression).RHS);
            Assert.Equal(3, ((output as BinaryOperatorExpression).RHS as IntegerExpression).Value);

            Assert.IsType(typeof(IntegerExpression), (output as BinaryOperatorExpression).RHS);
            Assert.Equal(3, ((output as BinaryOperatorExpression).RHS as IntegerExpression).Value);
        }
    }
}