using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BaccaratRules
{
    public static int CardToScore(int cardId)
    {
        int score = (cardId % 13) + 1;

        if (score >= 10)
        {
            score = 0;
        }

        return score;
    }

    public static int AddScore(int currentScore, int score)
    {
        return (currentScore + score) % 10;
    }

    public static bool IsPair(int firstCardId, int secondCardId)
    {
        return (firstCardId % 13) == (secondCardId % 13);
    }

    public static bool IsNatural(int score)
    {
        return score >= 8;
    }

    public static bool IsNatural(int playerScore, int bankerScore)
    {
        return playerScore >= 8 || bankerScore >= 8;
    }

    public static bool ShouldPlayerDraw(int playerScore, int bankerScore)
    {
        if (IsNatural(playerScore, bankerScore))
        {
            return false;
        }

        return playerScore <= 5;
    }

    public static bool ShouldBankerDraw(int playerScore, int bankerScore, int playerThirdCardScore, bool didPlayerDrawThirdCard)
    {
        if (didPlayerDrawThirdCard == false)
        {
            return bankerScore <= 5;
        }

        if (bankerScore <= 2)
        {
            return true;
        }

        if (bankerScore == 3)
        {
            return playerThirdCardScore != 8;
        }

        if (bankerScore == 4)
        {
            return 2 <= playerThirdCardScore && playerThirdCardScore <= 7;
        }

        if (bankerScore == 5)
        {
            return 4 <= playerThirdCardScore && playerThirdCardScore <= 7;
        }

        if (bankerScore == 6)
        {
            return 6 <= playerThirdCardScore && playerThirdCardScore <= 7;
        }

        return false;
    }

    public static Winner GetWinner(int playerScore, int bankerScore)
    {
        if (playerScore > bankerScore)
        {
            return Winner.Player;
        }
        else if (playerScore < bankerScore)
        {
            return Winner.Banker;
        }
        else
        {
            return Winner.Tie;
        }
    }
}
