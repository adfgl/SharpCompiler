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
    }
}
