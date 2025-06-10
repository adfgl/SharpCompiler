namespace CompilerLib
{
    public enum ETokenType
    {
        Undefined,

        EOF,
        LineBreak,

        Integer, Float, String, Identifier,

        Minus, Plus, Mult, Div,
    }
}
