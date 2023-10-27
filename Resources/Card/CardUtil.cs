class CardUtil {
    private CardUtil() {}

    // for simple check for card
    public static bool IsSep(Card card) { return card.getMonth() == CardMonth.Sep; }
    public static bool IsBoom(Card card) { return card.getType() == CardType.Boom; }
    public static bool IsLine(Card card) { return card.getType() == CardType.Line; }
    public static bool IsBlood(Card card) { return card.getType() == CardType.Blood; }
    public static bool IsPaint(Card card) { return card.getType() == CardType.Paint; }
    public static bool IsKwang(Card card) { return card.getType() == CardType.Kwang; }
    public static bool IsBird(Card card) { return card.getOption() == CardOption.Bird; }
    public static bool IsDouble(Card card) { return card.getOption() == CardOption.Double; }
    public static bool IsTriple(Card card) { return card.getOption() == CardOption.Triple; }
    public static bool IsRedLine(Card card) { return card.getOption() == CardOption.RedLine; }
    public static bool IsBlueLine(Card card) { return card.getOption() == CardOption.BlueLine; }
    public static bool IsRainKwang(Card card) { return card.getOption() == CardOption.RainKwang; }
}