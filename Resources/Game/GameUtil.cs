using System.Runtime.InteropServices;

class GameUtil {
    private GameUtil() {}
    public static int GetDeckCount(Game game) { return game.getDeck().getCount(); }
    public static int GetPlayerCount(Game game) { return game.getPlayer().getCount(); }
    public static int GetAiCount(Game game) { return game.getAi().getCount(); }
    public static int GetFieldCount(Game game) { return game.getField().getCount(); }
    public static int GetDummyCount(Game game) { return game.getDummy().getCount(); }

    public static void Deal(Game game) {
        for(int i = 0; i < 2; ++i) {
            // Player 
            for (int p = 0; p < 5; ++p) {
                game.getPlayer().getCardList().Add(DeckUtil.GetLastCardForMove(game.getDeck()));
                game.getPlayer().getCardList().Last().setOpen(true);
            }

            // Ai
            for (int a = 0; a < 5; ++a) {
                game.getAi().getCardList().Add(DeckUtil.GetLastCardForMove(game.getDeck()));
            }

            // Field
            for (int f = 0; f < 4; ++f) {
                game.getField().getCardList().Add(DeckUtil.GetLastCardForMove(game.getDeck()));
            }
        }

        DeckUtil.CardListSortByMonth(game.getPlayer().getCardList());
    }

    public static bool IsEnd(Game game) {
        bool end = false;

        for (int i = 0; i < 13; ++i) {
            if (game.getPlayer().getCountByMonth(i) == 4) {
                Console.WriteLine("[ 플레이어가 총통으로 승리했습니다. ]");
                game.getPlayer().setWin();
                end = true;
                return end;
            } else if (game.getAi().getCountByMonth(i) == 4) {
                Console.WriteLine("[ AI가 총통으로 승리했습니다. ]");
                game.getAi().setWin();
                end = true;
                return end;
            }
        }

        if (game.getPlayer().getPooCount() >= 3) {
            Console.WriteLine("[ 플레이어가 3뻑으로 승리했습니다. ]");
            game.getPlayer().setWin();
            end = true;
            return end;
        } else if (game.getAi().getPooCount() >= 3) {
            Console.WriteLine("[ AI가 3뻑으로 승리했습니다. ]");
            game.getAi().setWin();
            end = true;
            return end;
        }

        if (game.getAi().getCount() + game.getPlayer().getCount() == 0) {
            Console.WriteLine("[ 남은 카드가 없어 무승부 처리되었습니다. ]");
            end = true;
            return end;
        }

        return end;
    }

    public static void SetTurnDeck(Game game) { 
        if (game.getPlayer().isTurn()) game.setTurnDeck(game.getPlayer());
        else game.setTurnDeck(game.getAi());
    }

    public static void FieldOpen(Game game) { 
        game.getField().getCardList().ForEach(card => {
            card.setOpen(true);
        });
    }

    public static void stop(Game game) {
        game.getTurn().setWin();
        if (game.getTurn().Equals(game.getPlayer())) {
            ScoreUtil.CalcScore(game.getPlayer().getScore(), true, game.getAi().getScore());
            game.getPlayer().getScore().toString();
            Console.WriteLine("[ 플레이어 승리 ]");
            ScoreUtil.CalcScore(game.getTurn().getScore(), true, game.getAi().getScore());
        } else {
            ScoreUtil.CalcScore(game.getPlayer().getScore(), true, game.getAi().getScore());
            game.getAi().getScore().toString();
            Console.WriteLine("[ AI 승리 ]");
            ScoreUtil.CalcScore(game.getTurn().getScore(), true, game.getPlayer().getScore());
        }
    }
}