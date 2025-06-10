using System.Data.Common;
using System.Runtime.CompilerServices;

namespace CompilerLib
{
    public class TokenReader
    {
        const char NO_CHAR = '\0';

        readonly int _length;
        readonly string _source;
        int _current = 0;

        public TokenReader(string source)
        {
            _source = source;
            _length = source.Length;
        }

        public string Source => _source;
        public int Length => _length;
        public int Current => _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLineBreak(char ch) => ch == '\n';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(char ch) => ch == ' ' || ch == '\t' || ch == '\r';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEOF(char ch) => ch == NO_CHAR;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigit(char ch) => ch >= '0' && ch <= '9';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLetter(char ch) => ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z';

        char Consume()
        {
            return _source[_current++];
        }

        char Peek()
        {
            int pos = _current;
            if (pos >= _length)
            {
                return NO_CHAR;
            }
            return _source[pos];
        }

        Token s_prevToken = new Token(ETokenType.Undefined, String.Empty);

        public List<Token> ReadAll()
        {
            List<Token> tokens = new List<Token>();
            Token token;
            do
            {
                token = Next();
                tokens.Add(token);
            } while (token.type != ETokenType.EOF);
            return tokens;
        }

        public Token Read() => s_prevToken = Next();

        Token Next()
        {
            char ch = Peek();
            if (IsDigit(ch))
            {
                return NumberLiteral();
            }

            ETokenType type;
            int length = 0;
            switch (ch)
            {
                case NO_CHAR:
                    type = ETokenType.EOF;
                    break;

                default:
                    type = ETokenType.Undefined;
                    throw new Exception($"Unexpected character '{ch}'.");
            }

            char[] value = new char[length];
            for (int i = 0; i < length; i++)
            {
                value[i] = Consume();
            }
            return new Token(type, new string(value));
        }


        Token NumberLiteral()
        {
            int start = _current;
            while (true)
            {
                char ch = Peek();
                if (IsDigit(ch))
                {
                    Consume();
                }
                else
                {
                    break;  
                }
            }

            string value = _source.Substring(start, _current - start);
            return new Token(ETokenType.Integer, new string(value.ToArray()));
        }
    }
}
