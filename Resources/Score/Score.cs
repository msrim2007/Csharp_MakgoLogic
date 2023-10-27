class Score {
    private List<Card>[] cardListByType = { new(), new(), new(), new() };
    private bool isShake = false;
    private int goCount = 0;
    private int goScore = 0;
    private int reverseGoCount = 0;
    private int stealCount = 0;
    private int totalScore = 0;

    public List<Card>[] getScoreList() { return this.cardListByType; }


    public void setStealCount() { this.stealCount = 0; }
    public void steal() { ++this.stealCount; }
    public int getStealCount() { return this.stealCount; }

    public void setTotalScore(int totalScore) { this.totalScore = totalScore; }
    public int getTotalScore() { return this.totalScore; }

    public void go() { 
        ++this.goCount; 
        this.goScore = this.totalScore; 
    }
    public int getGoScore() { return this.goScore; }

    public void reserveGo() { ++this.reverseGoCount; }
    public int getReverseGoCount() { return this.reverseGoCount; }
    public int getGoCount() { return this.goCount; }

    public void shake() { this.isShake = true; }
    public bool isShaked() { return this.isShake; }

    public void addCard(Card card) {
        if (card.getType() == CardType.Blood) {
            this.cardListByType[0].Add(new Card(card));
            this.cardListByType[0].Last().setOpen(true);
        } else if (card.getType() == CardType.Kwang) {
            this.cardListByType[1].Add(new Card(card));
            this.cardListByType[1].Last().setOpen(true);
        } else if (card.getType() == CardType.Line) {
            this.cardListByType[2].Add(new Card(card));
            this.cardListByType[2].Last().setOpen(true);
        } else if (card.getType() == CardType.Paint) {
            this.cardListByType[3].Add(new Card(card));
            this.cardListByType[3].Last().setOpen(true);
        }
    }

    public string toString() {
        string str = "";
        if (this.cardListByType[0].Count != 0) {
            this.cardListByType[0].ForEach(card => {
                str += card.toString();
            });
            str += "\n";
        } 
        if (this.cardListByType[1].Count != 0) {
            this.cardListByType[1].ForEach(card => {
                str += card.toString();
            });
            str += "\n";
        }
        if (this.cardListByType[2].Count != 0) {
            this.cardListByType[2].ForEach(card => {
                str += card.toString();
            });
            str += "\n";
        }
        if (this.cardListByType[3].Count != 0) {
            this.cardListByType[3].ForEach(card => {
                str += card.toString();
            });
            str += "\n";
        }

        return str;
    }
}