class Play {
    static Deck deck = new(DeckType.Deck);
    static Deck fieldDeck = new(DeckType.Field);
    static Deck aiDeck = new(DeckType.Ai);
    static Deck playerDeck = new(DeckType.Player);
    static Deck turnDeck = new(DeckType.Player);
    static Deck opponentDeck = new(DeckType.Player);
    static Deck takeDeck = new(DeckType.Player);
    static PlayMG play = new PlayMG(deck, fieldDeck, playerDeck, aiDeck);
    static Random rand = new();
    static int playerChoice = -1;
    static int aiChoice = -1;

    public static void Main(){
        // 1. 패를 나눈다.
        PlayUtil.Deal(play);
        
        // 2. 선공 정한다.
        //  일단 콘솔로 진행
        WriteField();

        // 2.1 필드에서 패 한개 선택
        Console.Write("[선공을 정합니다.] 필드의 패 하나를 선택해주세요 (0 ~ " + (fieldDeck.getLength() - 1) + ") : ");
        playerChoice = int.Parse(Console.ReadLine());
        Console.WriteLine();
        int playerMonth = fieldDeck.getCardList().ElementAt(playerChoice).getCardMonth();
        fieldDeck.getCardList().ElementAt(playerChoice).setOpen(true);

        do {
            aiChoice = rand.Next(0, fieldDeck.getLength() - 1);
        } while(aiChoice == playerChoice);
        int aiMonth = fieldDeck.getCardList().ElementAt(aiChoice).getCardMonth();
        fieldDeck.getCardList().ElementAt(aiChoice).setOpen(true);

        // 월이 높은 쪽이 선 (보너스는 무조건 선)
        if (playerMonth == 0) playerDeck.setTurn(true);
        else if (aiMonth == 0) aiDeck.setTurn(true);
        else if (playerMonth >= aiMonth) playerDeck.setTurn(true);
        else aiDeck.setTurn(true);
        Console.WriteLine("상대 : " + aiMonth + "월 , 플레이어 : " + playerMonth + "월");
        Console.WriteLine();

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
            WriteField();
            Start();
        }

        WriteField();
    }

    private static void Start() {
        HandOut(false);
        TurnEnd();
    }

    private static void HandOut(bool isShake) {
        turnDeck = PlayUtil.GetTurnDeck(play);
        bool isPlayer = turnDeck.Equals(playerDeck) ? true : false;
        opponentDeck = isPlayer ? aiDeck : playerDeck;
        if (!isShake) takeDeck = new Deck(DeckType.Player); // 가져갈 패 리스트

        // 필드 업데이트
        DeckUtil.InitListByMonth(fieldDeck);
        // 핸드 업데이트
        DeckUtil.InitListByMonth(turnDeck);

        int choice = -1;
        if (isPlayer) {
            Console.Write("선택 : ");
            choice = int.Parse(Console.ReadLine());
            Console.WriteLine();
        } else choice = rand.Next(0, fieldDeck.getLength() - 1);

        Card choiceCard = turnDeck.getCard(choice);
        // 카드 필드로 옮기고
        turnDeck.removeCard(choiceCard);
        fieldDeck.addCard(choiceCard);
        fieldDeck.getLastCard().setOpen(true);
        Console.WriteLine("선택한 패 : " + fieldDeck.getLastCard().ToString());
        Console.WriteLine();

        List<Card> fieldByMonth = fieldDeck.getListByMonth(choiceCard.getCardMonth());
        List<Card> turnByMonth = turnDeck.getListByMonth(choiceCard.getCardMonth());

        if (choiceCard.getCardMonth() == 0) {
            // 보너스 패인 경우
            takeDeck.addCard(choiceCard); // 가져올 패 리스트에 넣고
            fieldDeck.getLastCard().setIsTake(true);
            turnDeck.scoreList.steal(); // 피 뺏어오게끔 설정
            Draw(choiceCard);
        } else if (fieldByMonth.Count >= 1) {
            // 필드에 맞는 패가 하나라도 있는 경우
            if (fieldByMonth.Count == 3) {
                // 필드에 맞는 패가 3개인 경우 (패 먹고 상대 패 뺏어옴)
                fieldDeck.getCardList().ForEach(card => { // 4개 반복될거임
                    if (card.getCardMonth() == choiceCard.getCardMonth()) {
                        takeDeck.addCard(card);
                        card.setIsTake(true);
                    }
                });
                turnDeck.scoreList.steal();
            } else if (fieldByMonth.Count == 2) {
                // 필드에 맞는 패가 2개인 경우 (같은종류면 아무거나 치고 다르면 선택하도록)
                List<Card> tmp = fieldDeck.getListByMonth(choiceCard.getCardMonth());
                if (tmp.ElementAt(0).getCardType() == tmp.ElementAt(1).getCardType()) {
                    // 같은 종류
                    if (tmp.ElementAt(0).getCardType() == (int) CardType.Blood
                        && (tmp.ElementAt(0).getCardOption() != tmp.ElementAt(1).getCardOption())) {
                        // 피인경우 쌍피(11월)가 있을수 있으니 선택하게끔 함
                        if (isPlayer) {
                            Console.Write("0 : " + tmp.ElementAt(0).ToString() + "1 : " + tmp.ElementAt(1).ToString() + " > ");
                            choice = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                        } else choice = rand.Next(0, 1);

                        // 내가 낸 패 처리 (내가 낸 패는 필드의 마지막에 있음)
                        fieldDeck.getLastCard().setIsTake(true);
                        takeDeck.addCard(fieldDeck.getLastCard());
                        tmp.ElementAt(choice == 0 ? 0 : 1).setIsTake(true);
                        takeDeck.addCard(tmp.ElementAt(choice == 0 ? 0 : 1));
                    } else {
                        // 내가 낸 패 처리
                        fieldDeck.getLastCard().setIsTake(true);
                        takeDeck.addCard(fieldDeck.getLastCard());

                        // 딴 패 처리
                        tmp.ElementAt(0).setIsTake(true);
                        takeDeck.addCard(tmp.ElementAt(0));
                    }
                } else {
                    // 카드 타입이 다른 경우                    
                    if (isPlayer) {
                        Console.Write("0 : " + tmp.ElementAt(0).ToString() + "1 : " + tmp.ElementAt(1).ToString());
                        choice = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                    } else choice = rand.Next(0, 1);

                    // 내가 낸 패, 딴 패 처리
                    fieldDeck.getLastCard().setIsTake(true);
                    takeDeck.addCard(fieldDeck.getLastCard());
                    tmp.ElementAt(choice == 0 ? 0 : 1).setIsTake(true);
                    takeDeck.addCard(tmp.ElementAt(choice == 0 ? 0 : 1));
                }
            } else {
                // 필드에 맞는 패가 1개인 경우
                if (turnByMonth.Count == 3) {
                    // 핸드에 같은월이 3개인 경우 (폭탄)
                    for (int i = 0; i < turnDeck.getLength(); ++i) {
                        if (turnDeck.getCard(i).getCardMonth() == choiceCard.getCardMonth()) {
                            fieldDeck.addCard(turnDeck.getCard(i));
                            fieldDeck.getLastCard().setIsTake(true);
                            fieldDeck.getLastCard().setOpen(true);
                            takeDeck.addCard(fieldDeck.getLastCard());
                            turnDeck.removeCard(turnDeck.getCard(i));
                        }
                    }
                    // 폭탄인 경우 폭탄 패를 2개 가져오도록 함. (구현 필요)
                    turnDeck.scoreList.steal();
                } else {
                    // 핸드에 같은 월이 3개가 아닌 경우 (일반적인 처리.)
                    fieldDeck.getLastCard().setIsTake(true);
                    takeDeck.addCard(fieldDeck.getLastCard());
                    
                    fieldDeck.getListByMonth(choiceCard.getCardMonth()).ElementAt(0).setIsTake(true);
                    takeDeck.addCard(fieldDeck.getListByMonth(choiceCard.getCardMonth()).ElementAt(0));
                }
            }

            Draw(choiceCard);
        } else {
            // 필드에 맞는 패가 없는 경우
            if (turnByMonth.Count == 3 && !choiceCard.isShaked()) {
                // 핸드에 같은 월 3개인 경우 
                // 안흔들었으면 선택할 수 있게 함
                if (isPlayer) {
                    Console.Write("[ 흔드시겠습니까? ] ( 0 : 흔들기 , 1 : 진행 ) > ");
                    choice = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                } else choice = rand.Next(0, 1);

                if (choice == 0) {
                    // 흔들기
                    Console.WriteLine("흔들었습니다.");
                    Console.WriteLine();
                    turnDeck.scoreList.shake();   // 흔든 횟수 증가만 시켜줌 (나중에 계산할때 사용)
                    if (!isPlayer) {
                        turnDeck.getCardList().ForEach(card => {
                            if (card.getCardMonth() == choiceCard.getCardMonth()) {
                                card.setOpen(true);
                                card.setShake(true);
                            }
                        });
                    }
                    HandOut(true); // 다시 턴 진행
                } else {
                    // 그냥 필드에 넣기 (필드에 맞는 패(월)가 없는 경우임)
                    Draw(choiceCard);
                }
            } else {
                // 그냥 필드에 넣기 (필드에 맞는 패(월)가 없는 경우임)
                Draw(choiceCard);
            }
        }
    }

    // 덱 드로우
    private static void Draw(Card chooseCard) {
        DeckUtil.InitListByMonth(fieldDeck);
        DeckUtil.InitListByMonth(turnDeck);
        Card drawCard = new Card(deck.getLastCard());
        deck.removeLastCard();
        fieldDeck.addCard(drawCard);
        fieldDeck.getLastCard().setOpen(true);
        Console.WriteLine("드로우 : " + fieldDeck.getLastCard().ToString());
        Console.WriteLine();
        bool isPlayer = turnDeck.Equals(playerDeck) ? true : false;
        int choice = -1;

        // 보너스패인지 여부
        if (drawCard.getCardMonth() == 0) {
            // 보너스패 필드에 놓고 다시 드로우
            drawCard.setIsTake(true);
            fieldDeck.addCard(drawCard);
            takeDeck.addCard(drawCard);
            turnDeck.scoreList.steal();

            Draw(chooseCard);
        } else if (fieldDeck.getListByMonth(drawCard.getCardMonth()).Count > 0) {
            // 드로우한 패와 맞는 패가 있다면
            if (fieldDeck.getListByMonth(drawCard.getCardMonth()).Count == 3) {
                // 필드에 드로우와 맞는 패가 3개인 경우 (따닥이랑 뻑난거 먹으면 얻는건 동일하니 동일하게 진행)
                fieldDeck.getLastCard().setIsTake(true);
                takeDeck.addCard(fieldDeck.getLastCard());
                fieldDeck.getListByMonth(drawCard.getCardMonth()).ForEach(card => {
                    if (!card.getIsTake()) {
                        card.setIsTake(true);
                        takeDeck.addCard(card);
                    }
                });
                turnDeck.scoreList.steal();
            } else if (fieldDeck.getListByMonth(drawCard.getCardMonth()).Count == 2) {
                // 필드에 드로우와 맞는 패가 2개인 경우
                if (drawCard.getCardMonth() == chooseCard.getCardMonth()) {
                    // 뻑
                    fieldDeck.getListByMonth(drawCard.getCardMonth()).ForEach(card => {
                        card.setIsTake(false);
                        takeDeck.removeCard(card);
                    });
                } else {
                    // 2개의 패가 타입, 옵션이 같으면 아무거나 먹고 아니면 선택
                    List<Card> tmp = fieldDeck.getListByMonth(drawCard.getCardMonth());
                    if (tmp.ElementAt(0).getCardType() == tmp.ElementAt(1).getCardType()) {
                        if (tmp.ElementAt(0).getCardType() == (int) CardType.Blood
                            && (tmp.ElementAt(0).getCardOption() == tmp.ElementAt(1).getCardOption())) {
                            // 쌍피, 피 인 경우도 선택하게끔 (어차피 쌍피먹겠찌만)
                            if (isPlayer) {
                                Console.Write("0 : " + tmp.ElementAt(0).ToString() + "1 : " + tmp.ElementAt(1).ToString() + " > ");
                                choice = int.Parse(Console.ReadLine());
                                Console.WriteLine();
                            } else choice = rand.Next(0, 1);

                            fieldDeck.getLastCard().setIsTake(true);
                            takeDeck.addCard(fieldDeck.getLastCard());
                            tmp.ElementAt(choice == 0 ? 0 : 1).setIsTake(true);
                            takeDeck.addCard(tmp.ElementAt(choice == 0 ? 0 : 1));
                        }
                    } else {
                        // 타입이 다르면 선택
                        if (isPlayer) {
                            Console.Write("0 : " + tmp.ElementAt(0).ToString() + "1 : " + tmp.ElementAt(1).ToString());
                            choice = int.Parse(Console.ReadLine());
                            Console.WriteLine();
                        } else choice = rand.Next(0, 1);

                        fieldDeck.getLastCard().setIsTake(true);
                        takeDeck.addCard(fieldDeck.getLastCard());
                        tmp.ElementAt(choice == 0 ? 0 : 1).setIsTake(true);
                        takeDeck.addCard(tmp.ElementAt(choice == 0 ? 0 : 1));
                    }
                }
            } else {
                // 필드에 드로우한 패와 맞는게 1개인 경우
                fieldDeck.getLastCard().setIsTake(true);
                takeDeck.addCard(fieldDeck.getLastCard());
                fieldDeck.getListByMonth(drawCard.getCardMonth()).ElementAt(0).setIsTake(true);
                takeDeck.addCard(fieldDeck.getListByMonth(drawCard.getCardMonth()).ElementAt(0));
                if (drawCard.getCardMonth() == chooseCard.getCardMonth()) {
                    turnDeck.scoreList.steal();
                }
            }
        }
    }

    private static void TurnEnd() {
        // 카드 처리 (takeDeck 반복하면서 필드에서 해당 카드 제거 및 턴플레이어의 점수필드에 추가)
        for (int i = 0; i < fieldDeck.getLength(); ++i) {
            if (fieldDeck.getCard(i).getIsTake()) fieldDeck.removeCard(i);
        }
        takeDeck.getCardList().ForEach(card => {
            turnDeck.scoreList.addCard(card);
        });
        takeDeck = new(DeckType.Player);

        // 점수 확인해서 났는지 여부 확인 (고 스톱 처리)
        WriteScore();

        // 턴 넘기기
        if (turnDeck.Equals(playerDeck)) {
            playerDeck.setTurn(false);
            aiDeck.setTurn(true);
        } else {
            playerDeck.setTurn(true);
            aiDeck.setTurn(false);
        }
    }

    private static void WriteField() {
        Console.WriteLine("============= FIELD ===============");
        Console.WriteLine(aiDeck.ToString());
        Console.WriteLine();
        Console.WriteLine(deck.ToString());
        Console.WriteLine(fieldDeck.ToString());
        Console.WriteLine();
        Console.WriteLine(playerDeck.ToString());
        Console.WriteLine("===================================");
        Console.WriteLine();
    }

    private static void WriteScore() {
        Console.WriteLine("============= SCORE ===============");
        Console.WriteLine("[AI]");
        Console.WriteLine(aiDeck.scoreList.ToString());
        Console.WriteLine("[PLAYER]");
        Console.WriteLine(playerDeck.scoreList.ToString());
        Console.WriteLine("===================================");
        Console.WriteLine();
    }
}
