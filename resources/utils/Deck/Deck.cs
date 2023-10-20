using System.ComponentModel;

class Deck {
    private List<Card> cardList = new();
    private List<Card>[] listByMonth = Array.Empty<List<Card>>();
    public Score scoreList = new();
    private int deckType = 0;
    private bool turn = false;
    private bool win = false;
    private int stealCount = 0;

    public Deck(int type) {
        this.deckType = type;
        if (this.deckType == (int) DeckType.Deck) {
            this.cardList = DeckUtil.InitCardList();
            this.Shuffle();
        }
    }
    public Deck(DeckType type) : this((int) type) {}

    public Card getCard(int index) { return this.cardList.ElementAt(index); }
    public List<Card> getCardList() { return this.cardList; }
    public void addCard(Card card) { this.cardList.Add(new Card(card)); }
    public Card getLastCard() { return this.cardList.Last(); }
    public void removeLastCard() { this.cardList.RemoveAt(this.cardList.Count - 1); }
    public int getLength() { return this.cardList.Count; }
    public void setTurn(bool turn) { this.turn = turn; }
    public bool getTurn() { return this.turn; }
    public void setWin(bool win) { this.win = win; }
    public bool getWin() { return this.win; }
    public void removeCard(Card card) { this.cardList.Remove(card); }
    public void removeCard(int index) { this.cardList.RemoveAt(index); }
    public void addListByMonth(int month, Card card) { this.listByMonth[month].Add(card); }
    public List<Card> getListByMonth(int month) { return this.listByMonth[month]; }
    public List<Card>[] getListByMonth() { return this.listByMonth; }
    public void steal() { ++this.stealCount; }
    public void setSteal() { this.stealCount = 0; }

    private void Shuffle() {
        Random rand = new();
        int n = this.cardList.Count;
        while(n > 1) {
            --n;
            int k = rand.Next(n + 1);
            (this.cardList[n], this.cardList[k]) = (this.cardList[k], this.cardList[n]);
        }
    }

    public override string ToString() {
        string str = "[ " + ((DeckType) this.deckType).ToString() + " ]";
        if (this.deckType == (int) DeckType.Deck) str += "[ " + this.cardList.Count + " ]";
        else if (this.deckType == (int) DeckType.Field) {
            str += "\n";
            int cnt = 0;
            this.cardList.ForEach(card => {
                str += card.ToString();
                if (++cnt % 3 == 0) str += "\n";
            });
        } else {
            str += "\n";
            this.cardList.ForEach(card => {
                str += card.ToString();
            });
        }

        return str;
    }
}