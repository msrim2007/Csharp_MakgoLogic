class Game {
    private Deck deck = new Deck((int) DeckType.Dummy);
    private Deck field = new Deck((int) DeckType.Dummy);
    private Deck player = new Deck((int) DeckType.Dummy);
    private Deck ai = new Deck((int) DeckType.Dummy);
    private Deck dummy = new Deck((int) DeckType.Dummy);
    private Deck turn = new Deck((int) DeckType.Dummy);

    public Game() {}
    public Game(Deck deck, Deck field, Deck player, Deck ai, Deck dummy) {
        this.deck = deck;
        this.field = field;
        this.player = player;
        this.ai = ai;
        this.dummy = dummy;
    }

    public void setTurnDeck(Deck deck) { this.turn = deck; }
    public void setDummyDeck(Deck deck) { this.dummy = deck; }

    public Deck getDeck() { return this.deck; }
    public Deck getField() { return this.field; }
    public Deck getPlayer() { return this.player; }
    public Deck getAi() { return this.ai; }
    public Deck getDummy() { return this.dummy; }
    public Deck getTurn() { return this.turn; }

    public string toString() {
        string str = "";

        str += this.ai.toString();
        str += "\n[ AI SCORE FIELD ]" + this.ai.getScore().getTotalScore() + " 점\n";
        str += this.ai.getScore().toString();

        str += this.deck.toString();
        str += "\n\n";

        str += this.field.toString();
        str += "\n\n";

        str += "[ PLAYER SCORE FIELD ]" + this.player.getScore().getTotalScore() + " 점\n";
        str += this.player.getScore().toString();
        str += this.player.toString() + "\n";

        return str;
    }
}