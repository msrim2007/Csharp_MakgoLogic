class Score {
    private List<Card> gwangList = new();
    private List<Card> paintList = new();
    private List<Card> lineList = new();
    private List<Card> bloodList = new();

    private int score = 0;
    private int goScore = 0;
    private int shakeCount = 0;
    private int stealCount = 0;

    public void addGwang(Card card) { this.gwangList.Add(new Card(card)); }
    public void addPaint(Card card) { this.paintList.Add(new Card(card)); }
    public void addLine(Card card) { this.lineList.Add(new Card(card)); }
    public void addBlood(Card card) { this.bloodList.Add(new Card(card)); }

    public void steal() { ++this.stealCount; }
    public void shake() { ++this.shakeCount; }

    public int getSteal() { return this.stealCount; }
    public int getShakeCount() { return this.shakeCount; }
    
    public List<Card> getBloodList() { return this.bloodList; }
    public List<Card> getGwangList() { return this.gwangList; }
    public List<Card> getLineList() { return this.lineList; }
    public List<Card> getPaintList() { return this.paintList; }
    
    public Card getBlood() {
        bool removed = false;
        Card card = new(0, 0, 0);

        if (bloodList.Count == 1) {
            card = new Card(bloodList.ElementAt(0));
            bloodList.RemoveAt(0);
            removed = true;
        } else {
            for (int i = bloodList.Count - 1; i >= 0; --i) {
                if (bloodList.ElementAt(i).getCardOption() == 0) {
                    card = new Card(bloodList.ElementAt(i));
                    bloodList.RemoveAt(i);
                    removed = true;
                    break;
                }
            }
            if (!removed) {
                for (int i = bloodList.Count - 1; i >= 0; --i) {
                    if (bloodList.ElementAt(i).getCardOption() == 2) {
                        card = new Card(bloodList.ElementAt(i));
                        bloodList.RemoveAt(i);
                        removed = true;
                        break;
                    }
                }
                if (!removed) {
                    card = new Card(bloodList.ElementAt(0));
                    bloodList.RemoveAt(0);
                    removed = true;
                }
            }
        }

        return card;
    }

    public void addCard(Card card) { 
        CardType type = (CardType) card.getCardType();
        Card addCard = new Card(card);
        addCard.setOpen(true);
        if (type.Equals(CardType.Blood)) bloodList.Add(addCard);
        else if (type.Equals(CardType.Line)) lineList.Add(addCard);
        else if (type.Equals(CardType.Paint)) paintList.Add(addCard);
        else  gwangList.Add(addCard);
    }

    public override string ToString() {
        string str = "";

        this.gwangList.ForEach(gwang => {
            str += gwang.ToString();
        });
        if (this.gwangList.Count > 0) str += "\n";

        this.paintList.ForEach(paint => {
            str += paint.ToString();
        });
        if (this.paintList.Count > 0) str += "\n";  

        this.lineList.ForEach(line => {
            str += line.ToString();
        });
        if (this.lineList.Count > 0) str += "\n";

        this.bloodList.ForEach(blood => {
            str += blood.ToString();
        });
        if (this.bloodList.Count > 0) str += "\n";

        return str;
    }
}