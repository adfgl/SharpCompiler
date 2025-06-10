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
        int _line = 0;
        int _column = 0;

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

        char Peek(int offset = 0)
        {
            int pos = _current + offset;
            if (pos >= _length)
            {
                return NO_CHAR;
            }
            return _source[pos];
        }

        public List<Token> ReadAll(bool ignoreLineBreaks = false)
        {
            List<Token> tokens = new List<Token>();
            Token token;
            do
            {
                token = Next();
                if (token.type == ETokenType.LineBreak && ignoreLineBreaks)
                {
                    continue;
                }
                tokens.Add(token);
            } while (token.type != ETokenType.EOF);
            return tokens;
        }

        public Token Read() => Next();

        void SkipWhiteSpaces()
        {
            while (IsWhiteSpace(Peek()))
            {
                Consume();
            }
        }

        Token Next()
        {
            SkipWhiteSpaces();

            char ch = Peek();
            if (IsDigit(ch))
            {
                return NumberLiteral();
            }

            ETokenType type;
            int length = 1;
            int line = _line;
            int column = _column;
            switch (ch)
            {
                case NO_CHAR:
                    type = ETokenType.EOF;
                    length = 0;
                    break;

                case '\n':
                    type = ETokenType.LineBreak;
                    if (Peek(1) == '\r')
                    {
                        length++;
                    }
                    _column = 0;
                    _line++;
                    break;

                case '-':
                    type = ETokenType.Minus;
                    break;

                case '+':
                    type = ETokenType.Plus;
                    break;

                case '*':
                    type = ETokenType.Mult;
                    break;

                case '/':
                    type = ETokenType.Div;
                    break;

                case ':':
                    type = ETokenType.Colon;
                    break;

                case ';':
                    type = ETokenType.SemiColon;
                    break;

                case '?':
                    type = ETokenType.Question;
                    break;

                case '!':
                    type = ETokenType.Exclamation;
                    break;

                case '<':
                    type = ETokenType.LessThan;
                    break;

                case '>':
                    type = ETokenType.GreaterThan;
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
            return new Token(type, new string(value), line, column);
        }

        Token NumberLiteral()
        {
            int start = _current;
            int line = _line;
            int column = _column;
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
            return new Token(ETokenType.Integer, value, line, column);
        }
    }
}
