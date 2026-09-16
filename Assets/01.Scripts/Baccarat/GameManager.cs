using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private RoundStatusView roundStatusView;
    [SerializeField] private PlayerInfoView playerInfoView;

    [SerializeField] private BaccaratTableView baccaratTableView;
    [SerializeField] private BettingView bettingView;
    [SerializeField] private BetChipView betChipView;

    [Header("User Info")]
    [SerializeField] private string nickName = "Player";
    [SerializeField] private long balance = 1000000000;

    private BetManager betManager;
    private Deck deck;

    public int PlayerSum { get; private set; }
    public int BankerSum { get; private set; }
    public bool IsPlayerPair { get; private set; }
    public bool IsBankerPair { get; private set; }

    private int scoreCache;

    private int playerFirstCard;
    private int playerSecondCard;
    private int playerThirdCard;
    private int bankerFirstCard;
    private int bankerSecondCard;
    private int bankerThirdCard;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (stateMachine == null)
        {
            stateMachine = GetComponent<StateMachine>();

            if (stateMachine == null)
            {
                stateMachine = gameObject.AddComponent<StateMachine>();
            }
        }

        betManager = new BetManager(balance);
        deck = new Deck();

        InitPlayerInfoView();

        InitBettingView();
    }

    private void Start()
    {
        StateMachine.Instance.ChangeState(StateType.Start);
    }

    private void InitPlayerInfoView()
    {
        playerInfoView.SetPlayerNickname(nickName);
        playerInfoView.SetPlayerBalance(balance);
    }

    private void InitBettingView()
    {
        bettingView.OnBetAmountClicked += OnBetAmountButtonClicked;
        bettingView.OnBetTypeClicked += OnBetTypeClicked;
        bettingView.OnDoubleClicked += OnDoubleClicked;
        bettingView.OnResetClicked += OnResetClicked;
        bettingView.OnCompleteClicked += OnCompleteClicked;
    }

    private void OnBetAmountButtonClicked(BetAmount betAmount)
    {
        if (betManager.CurrentBetAmount == betAmount)
        {
            betManager.SetBetAmount(BetAmount.None);
            bettingView.SetAmountButtonColor(betManager.CurrentBetAmount);
        }
        else
        {
            betManager.SetBetAmount(betAmount);
            bettingView.SetAmountButtonColor(betManager.CurrentBetAmount);
        }
    }

    private void OnBetTypeClicked(BetType betType)
    {
        betManager.SetBetType(betType);
        if (betManager.TryBet())
        {
            balance = betManager.Balance;
            playerInfoView.SetPlayerBalance(betManager.Balance);
            betChipView.AddChip(betManager.CurrentBetType, betManager.CurrentBetAmount);
        }
    }

    private void OnDoubleClicked()
    {
        if (betManager.TryDoubleBet(out var betResult))
        {
            foreach (var item in betResult)
            {
                betChipView.AddChips(item.Key, item.Value);
            }

            balance = betManager.Balance;
            playerInfoView.SetPlayerBalance(betManager.Balance);
        }
    }

    private void OnResetClicked()
    {
        betManager.Reset();
        betChipView.ClearChips();

        balance = betManager.Balance;
        playerInfoView.SetPlayerBalance(betManager.Balance);
    }

    private void OnCompleteClicked()
    {
        if (stateMachine.GetCurrentState() == StateType.Betting)
        {
            stateMachine.ChangeState(StateType.Show);
        }
    }

    public void GameStart()
    {
        roundStatusView.SetMessage("Game Start");
        bettingView.SetActiveButtons(false);
        baccaratTableView.ResetTable();

        PlayerSum = 0;
        BankerSum = 0;

        playerFirstCard = 0;
        playerSecondCard = 0;
        playerThirdCard = 0;
        bankerFirstCard = 0;
        bankerSecondCard = 0;
        bankerThirdCard = 0;

        IsPlayerPair = false;
        IsBankerPair = false;
    }

    public void BettingStart()
    {
        bettingView.SetActiveButtons(true);
    }

    public void BettingEnd()
    {
        bettingView.SetActiveButtons(false);
    }

    public void UpdateBettingTimer(float timer)
    {
        roundStatusView.SetMessage(timer.ToString("F0"));
    }

    public void ShowCard()
    {
        roundStatusView.SetMessage("Show Down");
        ClearBetSelection();

        baccaratTableView.SetPlayerScore(PlayerSum);
        baccaratTableView.SetBankerScore(BankerSum);

        StartCoroutine(CoShowCard());
    }

    private void ClearBetSelection()
    {
        betManager.SetBetType(BetType.None);
    }

    private IEnumerator CoShowCard()
    {
        playerFirstCard = deck.Draw();
        baccaratTableView.SetDeckCount(deck.RemainCardCount, deck.MaxCardCount);
        yield return baccaratTableView.MoveCard(CardSlot.PlayerFirst);
        baccaratTableView.SetCardSprite(CardSlot.PlayerFirst, playerFirstCard);

        bankerFirstCard = deck.Draw();
        baccaratTableView.SetDeckCount(deck.RemainCardCount, deck.MaxCardCount);
        yield return baccaratTableView.MoveCard(CardSlot.BankerFirst);
        baccaratTableView.SetCardSprite(CardSlot.BankerFirst, bankerFirstCard);

        playerSecondCard = deck.Draw();
        baccaratTableView.SetDeckCount(deck.RemainCardCount, deck.MaxCardCount);
        yield return baccaratTableView.MoveCard(CardSlot.PlayerSecond);
        IsPlayerPair = BaccaratRules.IsPair(playerFirstCard, playerSecondCard);
        baccaratTableView.SetCardSprite(CardSlot.PlayerSecond, playerSecondCard);

        bankerSecondCard = deck.Draw();
        baccaratTableView.SetDeckCount(deck.RemainCardCount, deck.MaxCardCount);
        yield return baccaratTableView.MoveCard(CardSlot.BankerSecond);
        IsBankerPair = BaccaratRules.IsPair(bankerFirstCard, bankerSecondCard);
        baccaratTableView.SetCardSprite(CardSlot.BankerSecond, bankerSecondCard);

        yield return baccaratTableView.OpenCard(CardSlot.PlayerFirst);
        scoreCache = BaccaratRules.CardToScore(playerFirstCard);
        PlayerSum = BaccaratRules.AddScore(PlayerSum, scoreCache);
        baccaratTableView.SetPlayerScore(PlayerSum);

        yield return baccaratTableView.OpenCard(CardSlot.BankerFirst);
        scoreCache = BaccaratRules.CardToScore(bankerFirstCard);
        BankerSum = BaccaratRules.AddScore(BankerSum, scoreCache);
        baccaratTableView.SetBankerScore(BankerSum);

        yield return baccaratTableView.OpenCard(CardSlot.PlayerSecond);
        scoreCache = BaccaratRules.CardToScore(playerSecondCard);
        PlayerSum = BaccaratRules.AddScore(PlayerSum, scoreCache);
        baccaratTableView.SetPlayerScore(PlayerSum);

        yield return baccaratTableView.OpenCard(CardSlot.BankerSecond, true);
        scoreCache = BaccaratRules.CardToScore(bankerSecondCard);
        BankerSum = BaccaratRules.AddScore(BankerSum, scoreCache);
        baccaratTableView.SetBankerScore(BankerSum);

        if (BaccaratRules.IsNatural(PlayerSum, BankerSum) == true)
        {
            baccaratTableView.SetActivePlayerNatural(BaccaratRules.IsNatural(PlayerSum));
            baccaratTableView.SetActiveBankerNatural(BaccaratRules.IsNatural(BankerSum));

            yield return new WaitForSeconds(1f);

            stateMachine.ChangeState(StateType.Result);

            yield break;
        }

        bool isPlayerDrawThirdCard = false;
        int playerThirdCardScore = 0;
        if (BaccaratRules.ShouldPlayerDraw(PlayerSum, BankerSum) == true)
        {
            yield return new WaitForSeconds(0.5f);
            isPlayerDrawThirdCard = true;

            playerThirdCard = deck.Draw();
            baccaratTableView.SetDeckCount(deck.RemainCardCount, deck.MaxCardCount);
            yield return baccaratTableView.MoveCard(CardSlot.PlayerThird);
            baccaratTableView.SetCardSprite(CardSlot.PlayerThird, playerThirdCard);

            yield return baccaratTableView.OpenCard(CardSlot.PlayerThird, true);
            playerThirdCardScore = BaccaratRules.CardToScore(playerThirdCard);
            PlayerSum = BaccaratRules.AddScore(PlayerSum, playerThirdCardScore);
            baccaratTableView.SetPlayerScore(PlayerSum);
        }

        if (BaccaratRules.ShouldBankerDraw(PlayerSum, BankerSum, playerThirdCardScore, isPlayerDrawThirdCard))
        {
            yield return new WaitForSeconds(0.5f);

            bankerThirdCard = deck.Draw();
            baccaratTableView.SetDeckCount(deck.RemainCardCount, deck.MaxCardCount);
            yield return baccaratTableView.MoveCard(CardSlot.BankerThird);
            baccaratTableView.SetCardSprite(CardSlot.BankerThird, bankerThirdCard);

            yield return baccaratTableView.OpenCard(CardSlot.BankerThird, true);
            scoreCache = BaccaratRules.CardToScore(bankerThirdCard);
            BankerSum = BaccaratRules.AddScore(BankerSum, scoreCache);
            baccaratTableView.SetBankerScore(BankerSum);
        }

        yield return new WaitForSeconds(1f);

        stateMachine.ChangeState(StateType.Result);
    }

    public void ResultStart()
    {
        StartCoroutine(CoResult());
    }

    private IEnumerator CoResult()
    {
        yield return null;
        Winner winner = BaccaratRules.GetWinner(PlayerSum, BankerSum);
        var betResolveList = betManager.ApplyBetResults(winner, IsPlayerPair, IsBankerPair);
        yield return betChipView.ResolveChips(betResolveList);
        balance = betManager.Balance;
        playerInfoView.SetPlayerBalance(betManager.Balance);

        stateMachine.ChangeState(StateType.Start);
    }
}