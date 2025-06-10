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

        [Fact]
        public void MathOperators_DetectedCorrectly()
        {
            List<string> ops = ["+", "-", "*", "/"];
            string source = String.Join(" \n ", ops);

            List<Token> tokens = new TokenReader(source).ReadAll(true);
            Assert.Equal(ops.Count + 1, tokens.Count);

            for (int i = 0; i < ops.Count; i++)
            {
                ETokenType expected;
                string op = ops[i];
                switch (op)
                {
                    case "+":
                        expected = ETokenType.Plus;
                        break;

                    case "-":
                        expected = ETokenType.Minus;
                        break;

                    case "*":
                        expected = ETokenType.Mult;
                        break;

                    case "/":
                        expected = ETokenType.Div;
                        break;

                    default:
                        throw new Exception($"Unknonwn operator '{op}'.");
                }

                Token t = tokens[i];
                Assert.Equal(op, t.value);
                Assert.Equal(expected, t.type);
            }
        }
    }
}
