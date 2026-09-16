using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BetAmountUtility
{
    public static int GetBetAmount(BetAmount betAmount)
    {
        if (betAmount == BetAmount.Ten)
        {
            return 10;
        }
        else if (betAmount == BetAmount.Fifty)
        {
            return 50;
        }
        else if (betAmount == BetAmount.Hundred)
        {
            return 100;
        }
        else if (betAmount == BetAmount.FiveHundred)
        {
            return 500;
        }
        else if (betAmount == BetAmount.OneThousand)
        {
            return 1000;
        }
        else if (betAmount == BetAmount.FiveThousand)
        {
            return 5000;
        }
        else if (betAmount == BetAmount.TenThousand)
        {
            return 10000;
        }
        else if (betAmount == BetAmount.FiftyThousand)
        {
            return 50000;
        }
        else
        {
            return 0;
        }
    }
}
