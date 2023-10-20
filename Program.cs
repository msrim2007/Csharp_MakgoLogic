class Play {
    static Deck deck = new(DeckType.Deck);
    static Deck fieldDeck = new(DeckType.Field);
    static Deck aiDeck = new(DeckType.Ai);
    static Deck playerDeck = new(DeckType.Player);
    static Random rand = new();

    public static void Main() {
        // 게임을 시작한다 치면 위에서 덱 만들고
        
        // 1. 패를 나눈다.
        PlayUtil.Deal(deck, fieldDeck, aiDeck, playerDeck);
        
        // 2. 선공 정한다.
        //  일단 콘솔로 진행
        Console.WriteLine(aiDeck.ToString());
        Console.WriteLine();
        Console.WriteLine(deck.ToString());
        Console.WriteLine(fieldDeck.ToString());
        Console.WriteLine();
        Console.WriteLine(playerDeck.ToString());

        // 2.1 필드에서 카드 한개 선택
        string? input = Console.ReadLine();
        int playerChoice = int.Parse(input);
        int playerMonth = fieldDeck.getCardList().ElementAt(playerChoice).getCardMonth();
        fieldDeck.getCardList().ElementAt(playerChoice).setOpen(true);


        int aiChoice = rand.Next(0, fieldDeck.getLength() - 1);
        do {
            aiChoice = rand.Next(0, fieldDeck.getLength() - 1);
        } while(aiChoice == playerChoice);
        int aiMonth = fieldDeck.getCardList().ElementAt(aiChoice).getCardMonth();
        fieldDeck.getCardList().ElementAt(aiChoice).setOpen(true);

        // if (playerMonth >= aiChoice) 
        // else 

    }
}
