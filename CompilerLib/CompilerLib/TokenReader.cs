using System.Data.Common;
using System.Runtime.CompilerServices;

namespace CompilerLib
{
    public class TokenReader
    {
        const char NO_CHAR = '\0';

        readonly StringBuffer m_buffer;

        readonly int _length;
        readonly string _source;
        int _current = 0;
        int _line = 0;
        int _column = 0;

        public TokenReader(string source)
        {
            m_buffer = new StringBuffer(1024) { AllowResize = true };
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
            _column++;
            char ch = _source[_current++];
            if (IsWhiteSpace(ch))
            {
                _column = 0;
                _line++;
            }
            return ch;
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
                return Number();
            }

            if (ch == '_' || IsLetter(ch))
            {
                return IdentifierOrKeyword();
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

                case '\"':
                    return String();

                case '\n':
                    type = ETokenType.LineBreak;
                    if (Peek(1) == '\r')
                    {
                        length++;
                    }
            
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

                case '(':
                    type = ETokenType.OpenBracket;
                    break;

                case '[':
                    type = ETokenType.OpenSquare;
                    break;

                case '{':
                    type = ETokenType.OpenCurly;
                    break;

                case ')':
                    type = ETokenType.CloseBracket;
                    break;

                case ']':
                    type = ETokenType.CloseSquare;
                    break;

                case '}':
                    type = ETokenType.CloseCurly;
                    break;

                default:
                    type = ETokenType.Undefined;
                    throw new Exception($"Unexpected character '{ch}'.");
            }

            m_buffer.Reset();
            for (int i = 0; i < length; i++)
            {
                m_buffer.Push(Consume());
            }
            return new Token(type, m_buffer.ToString(), line, column);
        }

        Token IdentifierOrKeyword()
        {
            int start = _current;
            int line = _line;
            int column = _column;

            m_buffer.Reset();
            while (true)
            {
                char ch = Peek();
                if (IsLetter(ch) || IsDigit(ch) || ch == '_')
                {
                    m_buffer.Push(Consume());
                }
                else
                {
                    break;
                }
            }
            string value = m_buffer.ToString();
            return new Token(ETokenType.Identifier, value, line, column);
        }

        Token String()
        {
            int start = _current;
            int line = _line;
            int column = _column;

            char ch = Consume();
            if (ch != '\"')
            {
                throw new Exception();
            }

            m_buffer.Reset();
            while (true)
            {
                if (Peek() == '\"')
                {
                    Consume();
                    break;
                }
                m_buffer.Push(Consume());
            }
            return new Token(ETokenType.String, m_buffer.ToString(), line, column);
        }

        Token Number()
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
