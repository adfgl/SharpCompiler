namespace CompilerLib
{
    public enum ETokenType
    {
        Undefined,

        EOF,
        LineBreak,

        Integer, Float, String, Identifier,

        Minus, Plus, Mult, Div,
        Equal, Tilda, Underscore, AtSign, BackSlash,

        OpenBracket, CloseBracket,
        OpenCurly, CloseCurly,
        OpenSquare, CloseSquare,

        Colon, SemiColon, Dot, Comma, Question, Exclamation, GreaterThan, LessThan,
    }
}
