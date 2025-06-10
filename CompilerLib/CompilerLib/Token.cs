namespace CompilerLib
{
    public readonly struct Token
    {
        public readonly ETokenType type;
        public readonly string value;

        public Token(ETokenType type, string value)
        {
            this.type = type;
            this.value = value;
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
            return $"[{type}] {value}";
        }
    }
}
