class DeckUtil {
    private DeckUtil() {}

    public static Card GetLastCardForMove(Deck deck) {
        Card card = new Card(deck.getCardList().Last());
        deck.getCardList().Remove(deck.getCardList().Last());
        return card;
    }

    public static Card GetCardForMove(Deck deck, int index) {
        Card card = new Card(deck.getCardList().ElementAt(index));
        deck.getCardList().RemoveAt(index);
        return card;
    }

    public static void CardListSortByMonth (List<Card> deck) {
        deck.Sort((card1, card2) => ((int)card1.getMonth()).CompareTo((int)card2.getMonth()));
    }
}