
public enum BetType
{
    None,
    Player,
    Banker,
    Tie,
    PlayerPair,
    BankerPair
}

public enum BetAmount
{
    None,
    Ten,
    Fifty,
    Hundred,
    FiveHundred,
    OneThousand,
    FiveThousand,
    TenThousand,
    FiftyThousand
}

public enum CardSlot
{
    PlayerFirst,
    PlayerSecond,
    PlayerThird,
    BankerFirst,
    BankerSecond,
    BankerThird,
}

public enum Winner
{
    Player,
    Banker,
    Tie,
}

public enum BetResolveType
{
    Lose,
    Win,
    Push,
}
