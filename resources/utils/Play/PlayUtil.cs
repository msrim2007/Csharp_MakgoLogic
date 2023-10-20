class PlayUtil {
    private PlayUtil() {}

    public static void Deal(PlayMG play) {
        for (int i = 0; i < 2; ++i) {
            // 플레이어에게 5장 씩
            for (int p1 = 0; p1 < 5; ++p1) {
                play.playerDeck.addCard(play.deck.getLastCard());
                play.playerDeck.getLastCard().setOpen(true);
                play.deck.removeLastCard();
            }
            for (int p2 = 0; p2 < 5; ++p2) {
                play.aiDeck.addCard(play.deck.getLastCard());
                play.deck.removeLastCard();
            }

            // 필드에 4장
            for (int f = 0; f < 4; ++f) {
                play.fieldDeck.addCard(play.deck.getLastCard());
                play.deck.removeLastCard();
            }
        }
    }

    public static int LeftCardLegnth(PlayMG play) {
        int cardLength = 0;
        cardLength += play.aiDeck.getLength();
        cardLength += play.playerDeck.getLength();
        return cardLength;
    }

    public static void Stop(PlayMG play, Deck stopPlayer) {
        play.stop = true;
        DeckUtil.Stop(stopPlayer);
    }

    public static bool IsEnd(PlayMG play) {
        bool end = false;

        if (play.stop) end = true;
        else if (LeftCardLegnth(play) == 0) end = true;
        if (DeckUtil.isPresident(play.aiDeck) || DeckUtil.isPresident(play.playerDeck)) {
            Console.WriteLine("[총통입니다.]");
            end = true;
            if (!(DeckUtil.isPresident(play.aiDeck) && DeckUtil.isPresident(play.playerDeck))) {
                if (DeckUtil.isPresident(play.aiDeck)) play.aiDeck.setWin(true);
                else play.playerDeck.setWin(true);
            }
        }

        return end;
    }

    public static Deck GetTurnDeck(PlayMG play) {
        if (play.aiDeck.getTurn()) return play.aiDeck;
        else return play.playerDeck;
    }
}