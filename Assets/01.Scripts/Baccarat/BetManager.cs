using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetManager
{
    public BetType CurrentBetType { get; private set; } = BetType.None;
    public BetAmount CurrentBetAmount { get; private set; } = BetAmount.None;
    public long Balance { get; private set; }

    private readonly Dictionary<BetType, long> bets = new Dictionary<BetType, long>();
    private Dictionary<BetType, long> prevBets = new Dictionary<BetType, long>();

    public BetManager(long initBalance)
    {
        Balance = initBalance;
    }

    public void SetBetType(BetType betType)
    {
        CurrentBetType = betType;
    }

    public void SetBetAmount(BetAmount betAmount)
    {
        CurrentBetAmount = betAmount;
    }

    public bool TryBet()
    {
        bool result = false;

        if (CurrentBetType == BetType.None || CurrentBetAmount == BetAmount.None)
        {
            return result;
        }

        int bet = BetAmountUtility.GetBetAmount(CurrentBetAmount);

        if (Balance >= bet)
        {
            if (bets.ContainsKey(CurrentBetType) == false)
            {
                bets[CurrentBetType] = 0;
            }

            bets[CurrentBetType] += bet;
            Balance -= bet;
            return true;
        }

        return result;
    }

    public List<BetResolve> ApplyBetResults(Winner winner, bool isPlayerPair, bool isBankerPair)
    {
        List<BetResolve> result = new List<BetResolve>();

        foreach (var bet in bets)
        {
            var betType = bet.Key;
            var betAmount = bet.Value;
            BetResolveType resolveType = GetResolveType(betType, winner, isPlayerPair, isBankerPair);
            long returnAmount = CalculateReturnAmount(betType, resolveType, betAmount);
            long profitAmount = CalculateProfit(resolveType, returnAmount, betAmount);

            BetResolve betResolve = new BetResolve(betType, resolveType, betAmount, returnAmount, profitAmount);

            result.Add(betResolve);
        }

        long temp = 0;
        foreach (var item in result)
        {
            Balance += item.ReturnAmount;
            temp += item.ProfitAmount;
        }

        Debug.Log($"Earn : {temp}");

        prevBets = new Dictionary<BetType, long>(bets);
        bets.Clear();

        return result;
    }

    private long CalculateProfit(BetResolveType resolveType, long returnAmount, long betAmount)
    {
        if (resolveType == BetResolveType.Win)
        {
            return returnAmount - betAmount;
        }
        else
        {
            return 0;
        }
    }

    private long CalculateReturnAmount(BetType betType, BetResolveType resolveType, long betAmount)
    {
        if (resolveType == BetResolveType.Lose)
        {
            return 0;
        }
        else if (resolveType == BetResolveType.Push)
        {
            return betAmount;
        }
        else
        {
            if (betType == BetType.Player)
            {
                return betAmount * 2;
            }
            else if (betType == BetType.Banker)
            {
                return betAmount + (betAmount * 95 / 100);
            }
            else if (betType == BetType.Tie)
            {
                return betAmount * 9;
            }
            else if (betType == BetType.PlayerPair)
            {
                return betAmount * 12;
            }
            else if (betType == BetType.BankerPair)
            {
                return betAmount * 12;
            }
            else
            {
                return 0;
            }
        }
    }

    private BetResolveType GetResolveType(BetType betType, Winner winner, bool isPlayerPair, bool isBankerPair)
    {
        if (betType == BetType.Player)
        {
            if (winner == Winner.Player)
            {
                return BetResolveType.Win;
            }
            else if (winner == Winner.Banker)
            {
                return BetResolveType.Lose;
            }
            else
            {
                return BetResolveType.Push;
            }
        }
        else if (betType == BetType.Banker)
        {
            if (winner == Winner.Player)
            {
                return BetResolveType.Lose;
            }
            else if (winner == Winner.Banker)
            {
                return BetResolveType.Win;
            }
            else
            {
                return BetResolveType.Push;
            }
        }
        else if (betType == BetType.Tie)
        {
            if (winner == Winner.Player)
            {
                return BetResolveType.Lose;
            }
            else if (winner == Winner.Banker)
            {
                return BetResolveType.Lose;
            }
            else
            {
                return BetResolveType.Win;
            }
        }
        else if (betType == BetType.PlayerPair)
        {
            if (isPlayerPair == true)
            {
                return BetResolveType.Win;
            }
            else
            {
                return BetResolveType.Lose;
            }
        }
        else if (betType == BetType.BankerPair)
        {
            if (isBankerPair == true)
            {
                return BetResolveType.Win;
            }
            else
            {
                return BetResolveType.Lose;
            }
        }
        else
        {
            return BetResolveType.Lose;
        }
    }

    public bool TryDoubleBet(out Dictionary<BetType, long> result)
    {
        long totalBet = 0;
        result = new Dictionary<BetType, long>();

        foreach (var item in prevBets.Values)
        {
            totalBet += item * 2;
        }

        if (Balance >= totalBet)
        {
            foreach (var item in prevBets)
            {
                if (bets.ContainsKey(item.Key) == false)
                {
                    bets[item.Key] = 0;
                }

                long doubleBetValue = item.Value * 2;

                bets[item.Key] += doubleBetValue;
                Balance -= doubleBetValue;
                result[item.Key] = doubleBetValue;
            }

            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }

    public void Reset()
    {
        foreach (var item in bets.Values)
        {
            Balance += item;
        }

        bets.Clear();
    }

}
