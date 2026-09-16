using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingView : MonoBehaviour
{
    private readonly Color activeColor = new Color(1f, 0f, 0f, 1f);
    private readonly Color inactiveColor = new Color(1f, 1f, 1f, 1f);

    [Header("Betting Buttons")]
    [SerializeField] private Button buttonPlayer;
    [SerializeField] private Button buttonBanker;
    [SerializeField] private Button buttonTie;
    [SerializeField] private Button buttonPlayerPair;
    [SerializeField] private Button buttonBankerPair;

    [Header("Amount Buttons")]
    [SerializeField] private Button button10;
    [SerializeField] private Button button50;
    [SerializeField] private Button button100;
    [SerializeField] private Button button500;
    [SerializeField] private Button button1k;
    [SerializeField] private Button button5k;
    [SerializeField] private Button button10k;
    [SerializeField] private Button button50k;

    [Header("Action Buttons")]
    [SerializeField] private Button buttonDouble;
    [SerializeField] private Button buttonReset;
    [SerializeField] private Button buttonComplete;

    private Action<BetType> onBetTypeClicked;
    public event Action<BetType> OnBetTypeClicked
    {
        add
        {
            onBetTypeClicked -= value;
            onBetTypeClicked += value;
        }
        remove
        {
            onBetTypeClicked -= value;
        }
    }

    private Action<BetAmount> onBetAmountClicked;
    public event Action<BetAmount> OnBetAmountClicked
    {
        add
        {
            onBetAmountClicked -= value;
            onBetAmountClicked += value;
        }
        remove
        {
            onBetAmountClicked -= value;
        }
    }

    private Action onDoubleClicked;
    public event Action OnDoubleClicked
    {
        add
        {
            onDoubleClicked -= value;
            onDoubleClicked += value;
        }
        remove
        {
            onDoubleClicked -= value;
        }
    }

    private Action onResetClicked;
    public event Action OnResetClicked
    {
        add
        {
            onResetClicked -= value;
            onResetClicked += value;
        }
        remove
        {
            onResetClicked -= value;
        }
    }

    private Action onCompleteClicked;
    public event Action OnCompleteClicked
    {
        add
        {
            onCompleteClicked -= value;
            onCompleteClicked += value;
        }
        remove
        {
            onCompleteClicked -= value;
        }
    }

    private void Awake()
    {
        buttonPlayer.onClick.AddListener(() => onBetTypeClicked?.Invoke(BetType.Player));
        buttonBanker.onClick.AddListener(() => onBetTypeClicked?.Invoke(BetType.Banker));
        buttonTie.onClick.AddListener(() => onBetTypeClicked?.Invoke(BetType.Tie));
        buttonPlayerPair.onClick.AddListener(() => onBetTypeClicked?.Invoke(BetType.PlayerPair));
        buttonBankerPair.onClick.AddListener(() => onBetTypeClicked?.Invoke(BetType.BankerPair));

        button10.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.Ten));
        button50.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.Fifty));
        button100.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.Hundred));
        button500.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.FiveHundred));
        button1k.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.OneThousand));
        button5k.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.FiveThousand));
        button10k.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.TenThousand));
        button50k.onClick.AddListener(() => onBetAmountClicked?.Invoke(BetAmount.FiftyThousand));

        buttonDouble.onClick.AddListener(() => onDoubleClicked?.Invoke());
        buttonReset.onClick.AddListener(() => onResetClicked?.Invoke());
        buttonComplete.onClick.AddListener(() => onCompleteClicked?.Invoke());
    }

    public void SetAmountButtonColor(BetAmount betAmount)
    {
        button10.image.color = betAmount == BetAmount.Ten ? activeColor : inactiveColor;
        button50.image.color = betAmount == BetAmount.Fifty ? activeColor : inactiveColor;
        button100.image.color = betAmount == BetAmount.Hundred ? activeColor : inactiveColor;
        button500.image.color = betAmount == BetAmount.FiveHundred ? activeColor : inactiveColor;
        button1k.image.color = betAmount == BetAmount.OneThousand ? activeColor : inactiveColor;
        button5k.image.color = betAmount == BetAmount.FiveThousand ? activeColor : inactiveColor;
        button10k.image.color = betAmount == BetAmount.TenThousand ? activeColor : inactiveColor;
        button50k.image.color = betAmount == BetAmount.FiftyThousand ? activeColor : inactiveColor;
    }

    public void SetActiveButtons(bool active)
    {
        buttonPlayer.interactable = active;
        buttonBanker.interactable = active;
        buttonTie.interactable = active;
        buttonPlayerPair.interactable = active;
        buttonBankerPair.interactable = active;

        button10.interactable = active;
        button50.interactable = active;
        button100.interactable = active;
        button500.interactable = active;
        button1k.interactable = active;
        button5k.interactable = active;
        button10k.interactable = active;
        button50k.interactable = active;

        buttonDouble.interactable = active;
        buttonReset.interactable = active;
        buttonComplete.interactable = active;
    }
}