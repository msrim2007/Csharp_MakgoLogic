class Card {
    // member
    private int month;
    private int type;
    private int option;
    private bool open = false;
    private int pooMonth;

    // generator
    public Card(int month, int type, int option) {
        this.month = month;
        this.type = type;
        this.option = option;
    }
    public Card(Card card): this((int)card.getMonth(), (int)card.getType(), (int)card.getOption()) {}

    // setter
    public void setType(int type) { this.type = type; }
    public void setOption(int option) { this.option = option; } 
    public void setOpen(bool open) { this.open = true; }
    public void setPooMonth(CardMonth month) { this.pooMonth = (int) month; }

    // getter
    public CardMonth getMonth() { return (CardMonth) this.month; }
    public CardType getType() { return (CardType) this.type; }
    public CardOption getOption() { return (CardOption) this.option; }
    public CardMonth getPooMonth() { return (CardMonth) this.pooMonth; }

    public bool isOpen() { return this.open; }

    // toString
    public string toString() {
        if (!this.isOpen()) {
            return "[ ? ]";
        }
        string str = "";

        str += "[";
        
        if (this.getMonth() == CardMonth.Bonus) str += "보너스";
        else {
            str += (int) this.getMonth() + "월 " + this.getType().ToString();
        }

        if (this.getOption() != CardOption.Normal) str += "(" + this.getOption().ToString() + ")";

        str += "]";

        return str;
    }
}