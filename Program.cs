class Makgo {
    private static Game game = new();
    private static Random rand = new Random();

    public static void Main() {
        string input = "";
        int choice = 0;
        int aiChoice = 0;
        do {
            // 게임 초기화
            Deck deck = new Deck((int) DeckType.Deck);
            Deck field = new Deck((int) DeckType.Field);
            Deck player = new Deck((int) DeckType.Player);
            Deck ai = new Deck((int) DeckType.Ai);
            Deck dummy = new Deck((int) DeckType.Dummy);
            game = new Game(deck, field, player, ai, dummy);

            Console.WriteLine("[ 게임을 시작합니다. ]");
            GameUtil.Deal(game);
            Console.WriteLine(game.toString());

            // 선공 정하기
            Console.WriteLine("[ 선공을 정합니다. 필드의 카드를 선택해주세요. (1부터 순서대로) ] (0 : 게임 종료)");
            input = GetInput();

            if (input.Equals("0")) break;
            choice = int.Parse(input);

            do {
                aiChoice = rand.Next(1, GameUtil.GetFieldCount(game));
            } while(choice == aiChoice);

            GameUtil.FieldOpen(game);

            int playerMonth = (int) game.getField().getCard(choice - 1).getMonth();
            int aiMonth = (int) game.getField().getCard(aiChoice - 1).getMonth();
            if (playerMonth == 0) game.getPlayer().setTurn(true);
            else if (aiMonth == 0) game.getAi().setTurn(true);
            else if (playerMonth >= aiMonth) game.getPlayer().setTurn(true);
            else game.getAi().setTurn(true);

            GameUtil.SetTurnDeck(game);

            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Player : " + game.getField().getCard(choice - 1).toString());
            Console.WriteLine("AI : " + game.getField().getCard(aiChoice - 1).toString());
            Console.WriteLine(game.getTurn().getType().ToString() + " 선");
            Console.WriteLine("-----------------------------------------------------");

            DeckUtil.CardListSortByMonth(game.getField().getCardList()); // 필드 월별로 정리 하고

            // 필드에 보너스 패 있다면 선공한테 넘기기 (정렬 되어있으니 보너스패 이후로 break)
            if (game.getField().getCountByMonth(0) > 0) {
                for (int i = 0; i < game.getField().getCount(); ++i) {
                    if (game.getField().getCard(i).getMonth() != CardMonth.Bonus) break;
                    game.getTurn().addScore(new Card(game.getField().getCard(i)));
                    game.getField().removeCard(i--);
                }
            }

             // 게임 진행
            while(!GameUtil.IsEnd(game)) { // 총통, 3뻑, 남은 카드 장수만 검증함 (스탑은 안에서 처리)
                // 화면 콘솔로 출력
                Console.WriteLine(game.toString());
                
                // 시작하면 더미덱 초기화
                dummy = new Deck((int) DeckType.Dummy);
                game.setDummyDeck(dummy);

                Card hand = Turn(); // 턴 시작

                Draw(hand); // 드로우 시작

                // 더미에 있는 패 턴덱의 점수 필드로 옮기고
                int dummyCount = game.getDummy().getCount();
                for (int i = 0; i < dummyCount; ++i) {
                    if (game.getDummy().getCardList().Last().getMonth() == CardMonth.Bonus) game.getTurn().steal();
                    game.getTurn().addScore(DeckUtil.GetLastCardForMove(game.getDummy()));
                }

                // 피 뺏어올거 있는 경우 뺏어오고
                int stealCount = game.getTurn().getStealCount();
                for (; stealCount > 0; --stealCount) {
                    int bloodCount = game.getTurn().Equals(game.getPlayer()) ? game.getAi().getScore().getScoreList()[(int) CardType.Blood].Count : game.getPlayer().getScore().getScoreList()[(int) CardType.Blood].Count;
                    if (bloodCount <= 0) break;
                    if (game.getTurn().Equals(game.getPlayer())) game.getTurn().addScore(ScoreUtil.GetBloodForSteal(game.getAi().getScore()));
                    else game.getTurn().addScore(ScoreUtil.GetBloodForSteal(game.getPlayer().getScore()));
                }

                // 뺏어왔으면 그만 뺏어와야겠지?
                game.getTurn().setStealCount();

                // 정렬
                DeckUtil.CardListSortByMonth(field.getCardList());
                foreach (List<Card> list in game.getPlayer().getScore().getScoreList()) {
                    DeckUtil.CardListSortByMonth(list);
                }
                foreach (List<Card> list in game.getAi().getScore().getScoreList()) {
                    DeckUtil.CardListSortByMonth(list);
                }

                // 점수 계산
                ScoreUtil.CalcScore(game.getTurn().getScore());

                // 고 스톱 가능 여부 및 선택
                if (game.getTurn().getScore().getTotalScore() >= 7 && game.getTurn().getScore().getTotalScore() > game.getTurn().getScore().getGoCount()) {
                    // 고 스탑이 가능한데 핸드가 없는경우 그냥 스탑
                    if (game.getTurn().getCount() == 0) {
                        GameUtil.stop(game);
                        return;
                    }
                    
                    if (game.getTurn().Equals(player)) {
                        Console.WriteLine("[ 고 / 스탑 (1 : 고, 2 : 스탑) ]");
                        input = GetInput();
                    } else input = rand.Next(1, 2) == 1 ? "1" : "2";

                    if (input.Equals("1")) {
                        // 고
                        game.getTurn().go();
                    } else {
                        // 스탑
                        GameUtil.stop(game);
                        return;
                    }
                }

                // 턴 다 끝나면 턴 넘기기
                if (game.getTurn().Equals(game.getPlayer())) {
                    game.getAi().setTurn(true);
                    game.setTurnDeck(game.getAi());
                } else {
                    game.getPlayer().setTurn(true);
                    game.setTurnDeck(game.getPlayer());
                }
                game.getTurn().setTurn(false);

            }
        } while (input.Equals("0"));
    }

    private static Card Turn() {
        bool isPlayer = game.getTurn().Equals(game.getPlayer());
        int choice = -1;
        if (isPlayer) {
            Console.WriteLine("[ 패를 선택합니다. (1 ~ " + game.getTurn().getCount() + ") ]");
            choice = int.Parse(GetInput()) - 1;
        } else choice = rand.Next(0, game.getTurn().getCount() - 1);
        Card choiceCard = new Card(game.getTurn().getCard(choice));
        choiceCard.setOpen(true);

        Console.WriteLine("---------------------------------");
        Console.WriteLine("선택한 패 : " + choiceCard.toString());
        Console.WriteLine("---------------------------------");

        // 선택한 패에 따른 분기 처리
        if (choiceCard.getMonth() == CardMonth.Bonus) {
            // 보너스 카드인 경우
            game.getTurn().steal(); // 카드 뺏어오고
            game.getTurn().addScore(DeckUtil.GetCardForMove(game.getTurn(), choice)); // 바로 점수필드로 옮김
            return choiceCard; // 바로 턴 종료 (드로우 시작)
        } else if (game.getTurn().getCountByMonth(choiceCard.getMonth()) == 3 && !game.getTurn().isShaked()) {
            if (game.getField().getCountByMonth(choiceCard.getMonth()) == 0) {
                // 핸드에 선택한 카드와 같은 월이 3장 있고 흔든적이 없다면 (필드에도 없어야 흔듬)
                string shake = "";
                if (isPlayer) {
                    Console.WriteLine("[ 흔드시겠습니까? (Y / N) ]");
                    shake = GetInput();
                } else shake = rand.Next(0, 1) == 0 ? "Y" : "N";
                if (shake.Equals("Y")) {
                    // 흔들기
                    game.getTurn().getListByMonth(choiceCard.getMonth()).ForEach(card => card.setOpen(true));
                    game.getTurn().shake();
                    Console.WriteLine(game.toString());
                    Turn(); // 흔들었다면 다시 턴 시작
                    return choiceCard; // 턴 끝나면 드로우로 넘어가기위해 return
                }
            }
        }

        // 선택한 패와 필드 패 비교
        if (game.getField().getCountByMonth(choiceCard.getMonth()) > 0) {
            if (game.getField().getCountByMonth(choiceCard.getMonth()) == 3) {
                // 필드에 맞는 패가 3개인 경우
                game.getTurn().steal();
                // 선택한 패 더미덱에 넣고
                game.getDummy().addCard(DeckUtil.GetCardForMove(game.getTurn(), choice));
                foreach (int i in game.getField().getCardIndexByMonth(choiceCard.getMonth())) {
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), i));
                } // 더미로 옮김
            } else if (game.getField().getCountByMonth(choiceCard.getMonth()) == 2) {
                // 필드에 맞는 패가 2개인 경우
                List<Card> cardByMonth = game.getField().getListByMonth(choiceCard.getMonth());
                if ((cardByMonth.ElementAt(0).getType() != cardByMonth.ElementAt(1).getType()) ||
                    (cardByMonth.ElementAt(0).getOption() != cardByMonth.ElementAt(1).getOption())) {
                    // 패의 종류 혹은 특수옵션이 다른 경우 선택해서 고름
                    int fieldChoice = -1;
                    if (isPlayer) {
                        Console.WriteLine("1 : " + cardByMonth.ElementAt(0).toString() + ", 2 : " + cardByMonth.ElementAt(1).toString());
                        fieldChoice = int.Parse(GetInput());
                    } else fieldChoice = rand.Next(1, 2);
                    // 고른 패 더미로 옮김
                    if (fieldChoice == 1) game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardList().IndexOf(cardByMonth.ElementAt(0))));
                    else game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardList().IndexOf(cardByMonth.ElementAt(1))));
                } else {
                    // 패의 종류와 옵션이 같으면 암거나 줌
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardList().IndexOf(cardByMonth.ElementAt(0))));
                }
                // 선택한 패 더미덱에 넣고
                game.getDummy().addCard(DeckUtil.GetCardForMove(game.getTurn(), choice));
            } else {
                // 필드에 맞는 패가 1개인 경우
                if (game.getTurn().getCountByMonth(choiceCard.getMonth()) == 3) {
                    // 폭탄
                    game.getTurn().shake();
                    game.getTurn().steal();
                    // 폭탄이면 덱에있는 패 모두 더미로 옮김
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardIndexByMonth(choiceCard.getMonth())[0]));
                    foreach (int index in game.getTurn().getCardIndexByMonth(choiceCard.getMonth())) {
                        game.getDummy().addCard(DeckUtil.GetCardForMove(game.getTurn(), index));
                    }
                } else {
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getTurn(), choice));
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardIndexByMonth(choiceCard.getMonth())[0]));
                }
            }
        } else {
            // 필드에 맞는게 없다면 그냥 필드로 보냄
            game.getField().addCard(DeckUtil.GetCardForMove(game.getTurn(), choice));
            game.getField().getCardList().Last().setOpen(true);
        }

        return choiceCard;
    }

    private static void Draw(Card hand) {
        // 드로우
        Card drawCard = new Card(game.getDeck().getCardList().Last());
        drawCard.setOpen(true);

        Console.WriteLine("---------------------------------");
        Console.WriteLine("드로우 : " + drawCard.toString());
        Console.WriteLine("---------------------------------");

        if (drawCard.getMonth() == CardMonth.Bonus) {
            // 보너스 패인경우 다음 드로우 시 뻑날수 있으니 일단 더미에 넣어놓는다.
            game.getTurn().steal();
            game.getDummy().addCard(DeckUtil.GetLastCardForMove(game.getDeck()));
            Draw(hand);
            return;
        }

        int dummyCount = game.getDummy().getCountByMonth(drawCard.getMonth());
        int fieldCount = game.getField().getCountByMonth(drawCard.getMonth());

        // 더미 먼저 비교 하고
        if (dummyCount == 2) {
            Console.WriteLine("[ 뻑 ]");

            // 더미에 같은 월 2장 있다면 뻑이니 더미에 있는 모든 패를 다시 필드로 돌려보내고 return
            int dummyCountForBack = game.getDummy().getCount();
            for (int i = 0; i < dummyCountForBack; ++i) {
                game.getField().addCard(DeckUtil.GetLastCardForMove(game.getDummy()));
                game.getField().getCardList().Last().setOpen(true);
            }
            // 드로우한 패도 필드로
            game.getField().addCard(DeckUtil.GetLastCardForMove(game.getDeck()));
            game.getField().getCardList().Last().setOpen(true);
            game.getTurn().fuck();
            return;
        }

        // 더미에 패 없는 경우 (필드만 비교)
        if (fieldCount != 0) { // 필드에 같은 월이 있는 경우
            if (fieldCount == 3) {
                // 3장 있는경우 뻑난거 먹은거고
                game.getTurn().steal();
                foreach (int index in game.getField().getCardIndexByMonth(drawCard.getMonth())) {
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), index));
                    game.getDummy().getCardList().Last().setOpen(true);
                }
            } else if (fieldCount == 2) {
                // 2장인 경우 타입이 다르면 선택해서 먹도록함
                List<Card> cardByMonth = game.getField().getListByMonth(drawCard.getMonth());
                if ((cardByMonth.ElementAt(0).getType() != cardByMonth.ElementAt(1).getType()) ||
                    (cardByMonth.ElementAt(0).getOption() != cardByMonth.ElementAt(1).getOption())) {
                    // 패의 종류 혹은 특수옵션이 다른 경우 선택해서 고름
                    bool isPlayer = game.getTurn().Equals(game.getPlayer());
                    int choice = -1;
                    if (isPlayer) {
                        Console.WriteLine("1 : " + cardByMonth.ElementAt(0).toString() + ", 2 : " + cardByMonth.ElementAt(1).toString());
                        choice = int.Parse(GetInput());
                    } else choice = rand.Next(1, 2);
                    // 고른 패 더미로 옮김
                    if (choice == 1) game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardList().IndexOf(cardByMonth.ElementAt(0))));
                    else game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardList().IndexOf(cardByMonth.ElementAt(1))));
                } else {
                    // 패의 종류와 옵션이 같으면 암거나 줌
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardList().IndexOf(cardByMonth.ElementAt(0))));
                }
            } else {
                // 1장인 경우
                if (game.getField().getListByMonth(drawCard.getMonth()).Equals(hand)) {
                    Console.WriteLine("[ 쪽 ]");
                    // 쪽
                    game.getTurn().steal();
                    game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardIndexByMonth(drawCard.getMonth())[0]));
                    game.getDummy().getCardList().Last().setOpen(true);
                }
                game.getDummy().addCard(DeckUtil.GetCardForMove(game.getField(), game.getField().getCardIndexByMonth(drawCard.getMonth())[0]));
                game.getDummy().getCardList().Last().setOpen(true);
            }
            game.getDummy().addCard(DeckUtil.GetLastCardForMove(game.getDeck()));
            game.getDummy().getCardList().Last().setOpen(true);
        } else { // 필드에 같은 월이 없는 경우 (그냥 필드로 옮기고 끝)
            game.getField().addCard(DeckUtil.GetLastCardForMove(game.getDeck()));
            game.getField().getCardList().Last().setOpen(true);
        }
    }

    private static string GetInput() {
        string ?input = "";
        Console.Write("선택 : ");
        input = Console.ReadLine();

        if (input != null) return input;
        else return "";   
    }
}