class Deck {
    private List<Card> cardList = new();
    private Score score = new();
    private int type;
    private bool turn = false;
    private int pooCount = 0;
    private bool win = false;

    public Deck(int type) {
        if (type == (int) DeckType.Deck) this.cardList = FullDeck.GetFullCardList();
        this.type = type;
    }

    public void go() { this.score.go(); }
    public void setWin() { this.win = true; }
    public void foo() { ++this.pooCount; }
    public void setTurn(bool turn) { this.turn = turn; }
    public void addCard(Card card) { 
        this.cardList.Add(card); 
        this.cardList.Last().setOpen(true); 
    }
    public void removeCard(int index) { this.cardList.RemoveAt(index); }

    public void addScore(Card card) { this.score.addCard(card); }

    public bool isWin() { return this.win; }
    public bool isTurn() { return this.turn; }
    public DeckType getType() { return (DeckType) this.type; }
    public int getPooCount() { return this.pooCount; }
    public List<Card> getCardList() { return this.cardList; }
    public int getCount() { return this.cardList.Count; }
    public Card getCard(int index) { return this.cardList.ElementAt(index); }
    public Score getScore() { return this.score; }

    public void shake() { this.score.shake(); }
    public bool isShaked() { return this.score.isShaked(); }

    public void steal() { this.score.steal(); }
    public void setStealCount() { this.score.setStealCount(); }
    public int getStealCount() { return this.score.getStealCount(); }

    public int getCountByMonth(CardMonth month) {
        int count = 0;
        this.cardList.ForEach(card => {
            if (card.getMonth() == month) ++count;
        });
        return count;
    }
    public int getCountByMonth(int month) { return this.getCountByMonth((CardMonth) month); }

    public int[] getCardIndexByMonth(CardMonth month) {
        int[] indexs = new int[this.getCountByMonth(month)];
        int j = 0;
        for (int i = 0; i < this.getCount(); ++i) {
            if (this.getCard(i).getMonth() == month) indexs[j++] = i;
        }

        return indexs;
    }

    public List<Card> getListByMonth(CardMonth month) {
        List<Card> cards = new();
        this.cardList.ForEach(card => {
            if (card.getMonth() == month) cards.Add(card);
        });
        return cards;
    }

    public string toString() {
        string str = "";

        str += "[" + this.getType().ToString() + "][" + this.cardList.Count + "]";
        
        if (this.getType() != DeckType.Deck) {
            str += "\n";
            int cnt = 0;
            for (int i = 0; i < this.cardList.Count; ++i) {
                str += this.cardList.ElementAt(i).toString();
                if (this.getType() == DeckType.Field && ++cnt % 3 == 0) {
                    str += "\n";
                }
            }
        }

        return str;
    }
}