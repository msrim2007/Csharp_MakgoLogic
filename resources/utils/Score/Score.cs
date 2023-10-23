class Score {
    private List<Card> gwangList = new();
    private List<Card> paintList = new();
    private List<Card> lineList = new();
    private List<Card> bloodList = new();

    private int score = 0;
    private int goCount = 0;
    private int goScore = 0;
    private bool isShake = false;
    private int stealCount = 0;

    public void addGwang(Card card) { this.gwangList.Add(new Card(card)); }
    public void addPaint(Card card) { this.paintList.Add(new Card(card)); }
    public void addLine(Card card) { this.lineList.Add(new Card(card)); }
    public void addBlood(Card card) { this.bloodList.Add(new Card(card)); }

    public void steal() { ++this.stealCount; }
    public void shake() { this.isShake = true; }
    public void go() {
        ++this.goCount;        
        this.goScore = this.score;
    }

    public void setSteal() { this.stealCount = 0; }
    public int getSteal() { return this.stealCount; }
    public bool shaked() { return this.isShake; }
    public int getScore() { return this.score; }
    public int getGoScore() { return this.goScore; }
    public int getGoCount() { return this.goCount; }
    
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

    public void scoreCheck() {
        this.score = 0;

        // 광 점수
        if (this.gwangList.Count >= 3) {
            if (this.gwangList.Count == 3) {
                this.gwangList.ForEach(gwang => {
                    if (gwang.getCardOption() == (int) CardOption.Rain) this.score += 2;
                });
            } else {
                this.score += this.gwangList.Count;
            }
        }

        // 열끗
        if (this.paintList.Count >= 5) {
            this.score += this.paintList.Count - 4;
        }
        
        // 고도리
        int godoryCount = 0;
        this.paintList.ForEach(paint => {
            if (paint.getCardOption() == (int) CardOption.Bird) ++godoryCount;
        });
        if (godoryCount == 3) this.score += 5;

        // 띠
        if (this.lineList.Count >= 5) {
            this.score += this.lineList.Count - 4;
        }

        // 청단, 홍단, 초단
        int blue = 0;
        int red = 0;
        int grass = 0;
        this.lineList.ForEach(line => {
            if (line.getCardOption() == (int) CardOption.Blue) ++blue;
            else if (line.getCardOption() == (int) CardOption.Red) ++red;
            else if (line.getCardOption() == (int) CardOption.Grass) ++grass;
        });

        if (blue == 3) this.score += 3;
        if (red == 3) this.score += 3;
        if (grass == 3) this.score += 3;
        
        // 피
        int bloodScore = 0;
        this.bloodList.ForEach(blood => {
            if (blood.getCardOption() == (int) CardOption.Double) bloodScore += 2;
            else if (blood.getCardOption() == (int) CardOption.Triple) bloodScore += 3;
            else ++bloodScore;
        });
        if (bloodScore >= 10) {
            this.score += bloodScore - 9;
        }

        // 흔듬
        if (this.isShake) this.score *= 2;

        // GO
        if (this.goCount > 0) this.score *= ((int) Math.Pow(2, this.goCount));
    }

    public override string ToString() {
        string str = "";

        if (this.goCount > 0) str += this.goCount + "고\n";
        if (this.goCount > 0) str += this.goCount + "흔듬\n";

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