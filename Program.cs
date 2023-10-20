class Play {
    static Deck deck = new(DeckType.Deck);
    static Deck fieldDeck = new(DeckType.Field);
    static Deck aiDeck = new(DeckType.Ai);
    static Deck playerDeck = new(DeckType.Player);
    static Deck turnDeck = new(DeckType.Player);
    static Deck opponentDeck = new(DeckType.Player);
    static PlayMG play = new PlayMG(deck, fieldDeck, playerDeck, aiDeck);
    static Random rand = new();
    static string? input;
    static int playerChoice = -1;
    static int aiChoice = -1;

    public static void Main(){
        // 1. 패를 나눈다.
        PlayUtil.Deal(play);
        
        // 2. 선공 정한다.
        //  일단 콘솔로 진행
        WriteField();

        // 2.1 필드에서 카드 한개 선택
        Console.Write("[선공을 정합니다.] 필드의 카드 하나를 선택해주세요 (0 ~ " + (fieldDeck.getLength() - 1) + ") : ");
        playerChoice = int.Parse(Console.ReadLine());
        int playerMonth = fieldDeck.getCardList().ElementAt(playerChoice).getCardMonth();
        fieldDeck.getCardList().ElementAt(playerChoice).setOpen(true);

        aiChoice = rand.Next(0, fieldDeck.getLength() - 1);
        do {
            aiChoice = rand.Next(0, fieldDeck.getLength() - 1);
        } while(aiChoice == playerChoice);
        int aiMonth = fieldDeck.getCardList().ElementAt(aiChoice).getCardMonth();
        fieldDeck.getCardList().ElementAt(aiChoice).setOpen(true);

        // 월이 높은 쪽이 선 (보너스는 무조건 선)
        if (playerMonth == 0) playerDeck.setTurn(true);
        else if (aiMonth == 0) aiDeck.setTurn(true);
        else if (playerMonth >= aiChoice) playerDeck.setTurn(true);
        else aiDeck.setTurn(true);
        Console.WriteLine("상대 : " + aiMonth + "월 , 플레이어 : " + playerMonth + "월");

        //3. 이제 게임을 시작하면 되는데
        // 필드 패 다 열고
        for (int i = 0; i < fieldDeck.getLength(); ++i) {
            if (fieldDeck.getCard(i).getCardMonth() == 0) {
                PlayUtil.GetTurnDeck(play).scoreList.addCard(fieldDeck.getCard(i));
                fieldDeck.removeCard(fieldDeck.getCard(i));
                --i;
            } else {
                fieldDeck.getCard(i).setOpen(true);
            }
        }

        // 시작
        while(!PlayUtil.IsEnd(play)) {
            DeckUtil.InitListByMonth(fieldDeck);
            WriteField();
            Start();
        }

        WriteField();
    }

    private static void Start() {
        HandOut();
        // 다 끝나고 필드에서 점수필드로 패 옮기기
        
    }

    private static void HandOut() {
        turnDeck = PlayUtil.GetTurnDeck(play);
        bool isPlayer = turnDeck.Equals(playerDeck) ? true : false;
        opponentDeck = isPlayer ? aiDeck : playerDeck;

        int choice = -1;
        if (isPlayer) {
            Console.Write("선택 : ");
            choice = int.Parse(Console.ReadLine());
        } else choice = rand.Next(0, fieldDeck.getLength() - 1);

        Card choiceCard = new Card(turnDeck.getCard(choice));
        turnDeck.removeCard(choiceCard);

        List<Card> fieldByMonth = fieldDeck.getListByMonth(choiceCard.getCardMonth());
        List<Card> turnByMonth = turnDeck.getListByMonth(choiceCard.getCardMonth());

        // 카드를 내면 항상 필드로 보냈다가 마지막에 가져오도록함
        // 필드로 보내면서 isTake 값 true로 바꿔놓고 
        // 턴 끝날때 일괄적으로 내 점수필드로 보냄

        if (choiceCard.getCardMonth() == 0) {
            // 보너스 카드인 경우
            choiceCard.setIsTake(true); // 이후에 점수필드로 이동하도록 설정.
            fieldDeck.addCard(choiceCard); // 필드에 넣고
            fieldDeck.steal(); // 피 뺏어오게끔 설정

            Draw(choiceCard);
        } else if (fieldByMonth.Count >= 1) {
            // 필드에 맞는 패가 하나라도 있는 경우
            if (fieldByMonth.Count == 3) {
                // 필드에 맞는 패가 3개인 경우 (카드 먹고 상대 패 뺏어옴)
            } else if (fieldByMonth.Count == 2) {
                // 필드에 맞는 패가 2개인 경우 (같은종류면 아무거나 치고 다르면 선택하도록)
            } else {
                // 필드에 맞는 패가 1개인 경우
                if (turnByMonth.Count == 3) {
                    // 핸드에 같은월이 3개인 경우 (폭탄)
                } else {
                    // 핸드에 같은 월이 3개가 아닌 경우 (일반적인 처리.)
                }
            }
        } else {
            // 필드에 맞는 패가 없는 경우
            if (turnByMonth.Count == 3) {
                // 핸드에 같은 월 3개인 경우 
                if (choiceCard.isShaked()) {
                    // 이미 흔든 패면 그냥 냄
                } else {
                    // 안흔들었으면 선택할 수 있게 함
                    if (isPlayer) {
                        Console.Write("[ 흔드시겠습니까? ] ( 0 : 흔들기 , 1 : 진행 ) >");
                        choice = int.Parse(Console.ReadLine());
                    } else choice = rand.Next(0, 1);

                    if (choice == 0) {
                        // 흔들기
                        
                        HandOut(); // 흔들었으면 다시 턴 진행
                    } else {
                        // 그냥 필드에 넣기
                    }
                }
            } else {
                // 그냥 필드에 넣기
            }
        }
    }

    // 덱 드로우
    private static void Draw(Card chooseCard) {
        Card drawCard = new Card(deck.getLastCard());
        deck.removeLastCard();

        // 보너스패, 쪽, 따닥, 뻑 체크해야함

        if (drawCard.getCardMonth() == 0) {
            // 보너스패 필드에 놓고 다시 드로우
            drawCard.setIsTake(true);
            fieldDeck.addCard(drawCard);
            turnDeck.steal();

            Draw(chooseCard);
        } else if (fieldDeck.getListByMonth(drawCard.getCardMonth()).Count > 0) {
            // 드로우한 패와 맞는 패가 있다면
            
        } else {
            // 드로우한 패와 맞는 패가 없다면
        }
    }

    private static void WriteField() {
        Console.WriteLine("============================");
        Console.WriteLine(aiDeck.ToString());
        Console.WriteLine();
        Console.WriteLine(deck.ToString());
        Console.WriteLine(fieldDeck.ToString());
        Console.WriteLine();
        Console.WriteLine(playerDeck.ToString());
    }
}
