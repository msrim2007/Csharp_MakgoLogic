class PlayMG {
    public Deck deck;
    public Deck fieldDeck;
    public Deck playerDeck;
    public Deck aiDeck;
    public bool stop = false;

    public PlayMG(Deck deck, Deck fieldDeck, Deck playerDeck, Deck aiDeck) {
        this.deck = deck;
        this.fieldDeck = fieldDeck;
        this.playerDeck = playerDeck;
        this.aiDeck = aiDeck;
    }
}