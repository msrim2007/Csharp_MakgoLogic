class ScoreUtil {
    private ScoreUtil() {}

    public static Card GetBloodForSteal(Score score) {
        List<Card> bloodList = score.getScoreList()[(int) CardType.Blood];
        Card blood = new Card(bloodList.Last());

        int bloodIndex = bloodList.Count - 1;
        for (; bloodIndex >= 0; --bloodIndex) {
            if (bloodList.ElementAt(bloodIndex).getOption() == CardOption.Normal) {
                blood = new Card(bloodList.ElementAt(bloodIndex));
                bloodList.RemoveAt(bloodIndex);
                return blood;
            }
        }

        // 결국 리턴하지 못했다면 그냥 마지막꺼 준다. (남은게 모두 쌍피나 3피인 경우임.)
        blood = new Card(bloodList.Last());
        bloodList.RemoveAt(bloodList.Count - 1);
        return blood;
    }

    public static void CalcScore(Score win, bool isFinal, Score ?lose = null) {
        // 점수 계산
        int totScore = 0;

        int bloodCount = 0;
        int paintCount = 0;
        int birdCount = 0;
        int lineCount = 0;
        int redCount = 0;
        int blueCount = 0;
        int grassCount = 0;
        int kwangCount = 0;
        bool rain = false;

        foreach (List<Card> list in win.getScoreList()) {
            list.ForEach(card => {
                if (card.getType() == CardType.Blood) {
                    if (card.getOption() == CardOption.Double) bloodCount += 2;
                    else if (card.getOption() == CardOption.Triple) bloodCount += 3;
                    else ++bloodCount;
                } else if (card.getType() == CardType.Kwang) {
                    if (card.getOption() == CardOption.RainKwang) rain = true;
                    ++kwangCount;
                } else if (card.getType() == CardType.Paint) {
                    if (card.getOption() == CardOption.Bird) ++birdCount;
                    ++paintCount;
                } else if (card.getType() == CardType.Kwang) {
                    if (card.getOption() == CardOption.RedLine) ++redCount;
                    else if (card.getOption() == CardOption.BlueLine) ++blueCount;
                    else if (card.getOption() == CardOption.GrassLine) ++grassCount;
                    ++lineCount;
                }
            });
        }

        if (bloodCount >= 10) totScore += (bloodCount - 9);
        if (kwangCount >= 3) {
            if (kwangCount == 3 && rain) totScore += 2;
            else if (kwangCount == 5) totScore += 15;
            else totScore += kwangCount;
        }
        if (paintCount >= 5) totScore += (paintCount - 4);
        if (birdCount == 3) totScore += 5;
        if (lineCount >= 5) totScore += (lineCount - 4);
        if (redCount == 3) totScore += 3;
        if (blueCount == 3) totScore += 3;
        if (grassCount == 3) totScore += 3;

        if (isFinal && lose != null) {
            // 최종점수인 경우 추가 배수 체크

            // 고
            if (win.getGoCount() >= 3) {
                // 3번 이상 고
                totScore += win.getGoCount();
                totScore *= (int) Math.Pow(2, win.getGoCount() - 2);
            } else if (win.getGoCount() >= 1) {
                // 1 ~ 2번 고
                totScore += win.getGoCount();
            }

            // 역고 (역역고까지는 체크 안한다. 어케 체크할지 모르겟음. 번뜩이면 추가)
            if (lose.getGoCount() != 0) totScore *= 2; // 상대가 고 한번이라도 했으면 역고

            // 흔들기 or 폭탄
            if (win.isShaked()) totScore *= 2;

            // 상대 점수
            int loseBloodCount = 0;
            int losePaintCount = 0;
            int loseLineCount = 0;
            int loseKwangCount = 0;

            foreach (List<Card> list in lose.getScoreList()) {
                list.ForEach(card => {
                    if (card.getType() == CardType.Blood) {
                        if (card.getOption() == CardOption.Double) loseBloodCount += 2;
                        else if (card.getOption() == CardOption.Triple) loseBloodCount += 3;
                        else ++loseBloodCount;
                    } else if (card.getType() == CardType.Kwang) {
                        ++loseKwangCount;
                    } else if (card.getType() == CardType.Paint) {
                        ++losePaintCount;
                    } else if (card.getType() == CardType.Kwang) {
                        ++loseLineCount;
                    }
                });
            }

            // 광박, 피박 (멍박은 하지 말자)
            if (kwangCount >= 3 && loseKwangCount == 0) totScore *= 2;
            if (bloodCount >= 10 && loseBloodCount != 0 && loseBloodCount <= 5) totScore *= 2;
        }

        win.setTotalScore(totScore);
    }

    public static void CalcScore(Score score) { CalcScore(score, false, null); }
}