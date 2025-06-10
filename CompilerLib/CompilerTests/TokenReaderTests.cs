using CompilerLib;

namespace CompilerTests
{
    public class TokenReaderTests
    {
        [Fact]
        public void EOF_DetectedCorrectly()
        {
            string source = "";
            List<Token> tokens = new TokenReader(source).ReadAll();

            Assert.Single(tokens);
            Assert.Equal(ETokenType.EOF, tokens[0].type);
        }

        [Fact]
        public void Integer_DetectedCorrectly()
        {
            int integer = 123;
            string source = $" {integer} ";

            List<Token> tokens = new TokenReader(source).ReadAll();
            Assert.Equal(2, tokens.Count);

            Token token = tokens[0];

            Assert.Equal(ETokenType.Integer, token.type);
            Assert.Equal(integer, token.AsInteger());
        }
    }
}
