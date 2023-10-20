class DeckUtil {
    private DeckUtil() {}

    public static List<Card> InitCardList() {
        List<Card> cardList = new();

        List<int> cardInfo;
        for (int i = 0; i < cardInfos.Count; ++i) {
            cardInfo = cardInfos.ElementAt(i);
            cardList.Add(new Card(cardInfo.ElementAt(0), cardInfo.ElementAt(1), cardInfo.ElementAt(2)));
        }

        return cardList;
    }

    public static void InitListByMonth(Deck deck) {
        // (1월 ~ 12월)
        ResetListByMonth(deck.getListByMonth());
        deck.getCardList().ForEach(card => {
            deck.addListByMonth(card.getCardMonth(), card);
        });
    }

    public static int[] GetMonthCnt(Deck deck) {
        int[] monthCnt = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        deck.getCardList().ForEach(card => { ++monthCnt[card.getCardMonth()]; });
        return monthCnt;
    }

    public static void Stop(Deck deck) { deck.setWin(true); }

    public static bool isPresident(Deck deck) {
        bool president = false;
        int[] monthCnt = GetMonthCnt(deck);
        foreach (int i in monthCnt) {
            if (i == 4) {
                president = true;
                break;
            }
        }

        return president;
    }

    public static void Shake(Deck deck, int month) {
        deck.scoreList.shake();
        foreach (Card card in deck.getCardList()) {
            if (card.getCardMonth() == month) {
                card.setShake(true);
            }
        }
    }

    private static void ResetListByMonth(List<Card>[] arr) {
        for (int i = 0; i < arr.Length; ++i) {
            arr[i] = new();
        }
    }
    
    private static readonly List<List<int>> cardInfos = new() {
        new List<int> {0, 3, 2},
        new List<int> {0, 3, 2},
        new List<int> {0, 3, 3},
        new List<int> {0, 3, 3},
        new List<int> {1, 0, 0},
        new List<int> {1, 2, 5},
        new List<int> {1, 3, 0},
        new List<int> {1, 3, 0},
        new List<int> {2, 1, 4},
        new List<int> {2, 2, 5},
        new List<int> {2, 3, 0},
        new List<int> {2, 3, 0},
        new List<int> {3, 0, 0},
        new List<int> {3, 2, 5},
        new List<int> {3, 3, 0},
        new List<int> {3, 3, 0},
        new List<int> {4, 1, 4},
        new List<int> {4, 2, 7},
        new List<int> {4, 3, 0},
        new List<int> {4, 3, 0},
        new List<int> {5, 1, 0},
        new List<int> {5, 2, 7},
        new List<int> {5, 3, 0},
        new List<int> {5, 3, 0},
        new List<int> {6, 1, 0},
        new List<int> {6, 2, 6},
        new List<int> {6, 3, 0},
        new List<int> {6, 3, 0},
        new List<int> {7, 1, 0},
        new List<int> {7, 2, 7},
        new List<int> {7, 3, 0},
        new List<int> {7, 3, 0},
        new List<int> {8, 0, 0},
        new List<int> {8, 1, 4},
        new List<int> {8, 3, 0},
        new List<int> {8, 3, 0},
        new List<int> {9, 1, 2},
        new List<int> {9, 2, 6},
        new List<int> {9, 3, 0},
        new List<int> {9, 3, 0},
        new List<int> {10, 1, 0},
        new List<int> {10, 2, 6},
        new List<int> {10, 3, 0},
        new List<int> {10, 3, 0},
        new List<int> {11, 0, 0},
        new List<int> {11, 3, 2},
        new List<int> {11, 3, 0},
        new List<int> {11, 3, 0},
        new List<int> {12, 0, 1},
        new List<int> {12, 1, 0},
        new List<int> {12, 2, 0},
        new List<int> {12, 3, 2}
    };

}