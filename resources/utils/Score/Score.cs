class Score {
    private List<Card> gwangList = new();
    private List<Card> paintList = new();
    private List<Card> lineList = new();
    private List<Card> bloodList = new();

    private int score = 0;
    private int goScore = 0;
    private int shakeCount = 0;

    public void shake() { ++this.shakeCount; }
    public int getShakeCount() { return this.shakeCount; }
    public int getBloodCnt() { return this.bloodList.Count; }
    
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

    
}