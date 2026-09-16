using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BetResolve
{
    public BetType BetType { get; }
    public BetResolveType ResolveType { get; }
    public long BetAmount { get; }
    public long ReturnAmount { get; }
    public long ProfitAmount { get; }

    public BetResolve(BetType betType, BetResolveType resolveType, long betAmount, long returnAmount, long profitAmount)
    {
        BetType = betType;
        ResolveType = resolveType;
        BetAmount = betAmount;
        ReturnAmount = returnAmount;
        ProfitAmount = profitAmount;
    }
}