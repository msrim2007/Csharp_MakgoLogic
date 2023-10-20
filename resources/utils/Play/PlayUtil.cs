class PlayUtil {
    private PlayUtil() {}

    public static void Deal(Deck deck, Deck field, Deck player1, Deck player2) {
        for (int i = 0; i < 2; ++i) {
            // 플레이어에게 5장 씩
            for (int p1 = 0; p1 < 5; ++p1) {
                player1.addCard(deck.getLastCard());
                deck.removeLastCard();
            }
            for (int p2 = 0; p2 < 5; ++p2) {
                player2.addCard(deck.getLastCard());
                deck.removeLastCard();
                player2.getLastCard().setOpen(true);
            }

            // 필드에 4장
            for (int f = 0; f < 4; ++f) {
                field.addCard(deck.getLastCard());
                deck.removeLastCard();
            }
        }
    }
}