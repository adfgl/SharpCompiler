namespace CompilerLib
{
    public readonly struct Token
    {
        public readonly ETokenType type;
        public readonly string value;
        public readonly int line, column;

        public Token(ETokenType type, string value, int line, int column)
        {
            this.type = type;
            this.value = value;
            this.line = line;
            this.column = column;
        }

        public int AsInteger()
        {
            if (type != ETokenType.Integer)
            {
                throw new ArgumentException($"Token of type '{type}' cannot be cast to {ETokenType.Integer}.");
            }
            return int.Parse(value);
        }

        public override string ToString()
        {
            return $"(l{line} c{column}) [{type}] {value}";
        }
    }
}
