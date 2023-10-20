/* Class For Card */
class Card {
    private int cardMonth;
    private int cardType; // 종류
    private int cardOption; // 특수패 (Normal, Rain, Double, Triple, Bird, Red, Blue)
    private bool isTake = false;
    private bool isOpen = false;
    private bool shaked = false;
    
    public Card(int month , int type, int option) {
        if (month < 0 || month > 12) return;
        if (type < 0 || type > 3) return;
        if (option < 0 || option > 7) return;

        this.cardMonth = month;
        this.cardType = type;
        this.cardOption = option;
    }
    public Card(CardMonth month, CardType type, CardOption option): this((int) month, (int) type, (int) option) {}
    public Card(Card card): this(card.cardMonth, card.cardType, card.cardOption) {}

    public int getCardMonth() { return this.cardMonth; }
    public int getCardType() { return this.cardType; }
    public int getCardOption() { return this.cardOption; }
    public void setOpen(bool open) { this.isOpen = open; }
    public void setShake(bool shake) { this.shaked = shake; }
    public bool isShaked() { return this.shaked; }
    public bool getIsTake() { return this.isTake; }
    public void setIsTake(bool take) { this.isTake = take; }

    public override string ToString() {
        string str = "[";
        if (!this.isOpen) {
            str += " ? ";
        } else {
            str += this.cardMonth == (int) CardMonth.Bonus ? "보너스 " : (this.cardMonth + "월 ");
            if (this.cardType == (int) CardType.Light) {
                if (this.cardOption == (int) CardOption.Rain) str += "비";
                str += "광";
            } else if (this.cardType == (int) CardType.Paint) {
                str += "열끗";
                if (this.cardOption == (int) CardOption.Bird) str += "(고도리)";
            } else if (this.cardType == (int) CardType.Line) {
                str += "띠";
                if (this.cardOption == (int) CardOption.Red) str += "(홍단)";
                else if (this.cardOption == (int) CardOption.Blue) str += "(청단)";
                else if (this.cardOption == (int) CardOption.Grass) str += "(초단)";
            } else { // Blood
                if (this.cardOption == (int) CardOption.Double) str += "쌍";
                else if (this.cardOption == (int) CardOption.Triple) str += "3";
                str += "피";
            }
        }
        str += "]";
        return str;
    }
}
