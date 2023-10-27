class FullDeck {
    private FullDeck () {}

    private static readonly List<int>[] fullCardInfo = {
        new List<int> {0, 0, 1},
        new List<int> {0, 0, 1},
        new List<int> {0, 0, 2},
        new List<int> {0, 0, 2},
        new List<int> {1, 0, 0},
        new List<int> {1, 0, 0},
        new List<int> {1, 1, 0},
        new List<int> {1, 2, 4},
        new List<int> {2, 0, 0},
        new List<int> {2, 0, 0},
        new List<int> {2, 2, 4},
        new List<int> {2, 3, 3},
        new List<int> {3, 0, 0},
        new List<int> {3, 0, 0},
        new List<int> {3, 1, 0},
        new List<int> {3, 2, 4},
        new List<int> {4, 0, 0},
        new List<int> {4, 0, 0},
        new List<int> {4, 2, 6},
        new List<int> {4, 3, 3},
        new List<int> {5, 0, 0},
        new List<int> {5, 0, 0},
        new List<int> {5, 2, 6},
        new List<int> {5, 3, 0},
        new List<int> {6, 0, 0},
        new List<int> {6, 0, 0},
        new List<int> {6, 2, 5},
        new List<int> {6, 3, 0},
        new List<int> {7, 0, 0},
        new List<int> {7, 0, 0},
        new List<int> {7, 2, 6},
        new List<int> {7, 3, 0},
        new List<int> {8, 0, 0},
        new List<int> {8, 0, 0},
        new List<int> {8, 1, 0},
        new List<int> {8, 3, 3},
        new List<int> {9, 0, 0},
        new List<int> {9, 0, 0},
        new List<int> {9, 2, 5},
        new List<int> {9, 3, 0},
        new List<int> {10, 0, 0},
        new List<int> {10, 0, 0},
        new List<int> {10, 2, 5},
        new List<int> {10, 3, 0},
        new List<int> {11, 0, 0},
        new List<int> {11, 0, 0},
        new List<int> {11, 0, 1},
        new List<int> {11, 1, 0},
        new List<int> {12, 0, 1},
        new List<int> {12, 1, 7},
        new List<int> {12, 2, 0},
        new List<int> {12, 3, 0}
    };

    public static List<Card> GetFullCardList() {
        List<Card> fullCard = new();

        foreach (List<int> cardInfo in fullCardInfo) {
            fullCard.Add(new Card(cardInfo.ElementAt(0), cardInfo.ElementAt(1), cardInfo.ElementAt(2)));
        }

        Random rand = new();
        int n = fullCard.Count;
        while(n > 1) {
            --n;
            int k = rand.Next(n + 1);
            (fullCard[n], fullCard[k]) = (fullCard[k], fullCard[n]);
        }

        return fullCard;
    }
}