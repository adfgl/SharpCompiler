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

        public override string ToString()
        {
            return $"[{type}] {value}";
        }
    }
}
