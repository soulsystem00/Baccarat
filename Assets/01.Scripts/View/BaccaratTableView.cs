using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaccaratTableView : MonoBehaviour
{
    [SerializeField] private Image cardPosition;
    [SerializeField] private RectTransform bankerPosition;
    [SerializeField] private TextMeshProUGUI remainCardCountText;

    [Header("Player Cards")]
    [SerializeField] private Image playerFirstCard;
    [SerializeField] private Image playerSecondCard;
    [SerializeField] private Image playerThirdCard;

    [Header("Banker Cards")]
    [SerializeField] private Image bankerFirstCard;
    [SerializeField] private Image bankerSecondCard;
    [SerializeField] private Image bankerThirdCard;

    [Header("Card Backs")]
    [SerializeField] private Image playerFirstCardBack;
    [SerializeField] private Image playerSecondCardBack;
    [SerializeField] private Image playerThirdCardBack;
    [SerializeField] private Image bankerFirstCardBack;
    [SerializeField] private Image bankerSecondCardBack;
    [SerializeField] private Image bankerThirdCardBack;

    [Header("Scores")]
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI bankerScoreText;
    [SerializeField] private TextMeshProUGUI playerNaturalText;
    [SerializeField] private TextMeshProUGUI bankerNaturalText;

    [Header("Card Sprites")]
    [SerializeField] private Sprite[] cardSprites;

    private Vector3 playerFirstPosition;
    private Vector3 playerSecondPosition;
    private Vector3 playerThirdPosition;
    private Vector3 bankerFirstPosition;
    private Vector3 bankerSecondPosition;
    private Vector3 bankerThirdPosition;

    private void Awake()
    {
        playerFirstPosition = playerFirstCard.rectTransform.anchoredPosition;
        playerSecondPosition = playerSecondCard.rectTransform.anchoredPosition;
        playerThirdPosition = playerThirdCard.rectTransform.anchoredPosition;
        bankerFirstPosition = bankerFirstCard.rectTransform.anchoredPosition;
        bankerSecondPosition = bankerSecondCard.rectTransform.anchoredPosition;
        bankerThirdPosition = bankerThirdCard.rectTransform.anchoredPosition;

        playerFirstCard.gameObject.SetActive(false);
        playerSecondCard.gameObject.SetActive(false);
        playerThirdCard.gameObject.SetActive(false);

        bankerFirstCard.gameObject.SetActive(false);
        bankerSecondCard.gameObject.SetActive(false);
        bankerThirdCard.gameObject.SetActive(false);

        playerScoreText.gameObject.SetActive(false);
        bankerScoreText.gameObject.SetActive(false);
        playerNaturalText.gameObject.SetActive(false);
        bankerNaturalText.gameObject.SetActive(false);
    }

    public void ResetTable()
    {
        ArrangeCard(CardSlot.PlayerFirst);
        ArrangeCard(CardSlot.PlayerSecond);
        ArrangeCard(CardSlot.PlayerThird);
        ArrangeCard(CardSlot.BankerFirst);
        ArrangeCard(CardSlot.BankerSecond);
        ArrangeCard(CardSlot.BankerThird);

        playerScoreText.gameObject.SetActive(false);
        bankerScoreText.gameObject.SetActive(false);
        playerNaturalText.gameObject.SetActive(false);
        bankerNaturalText.gameObject.SetActive(false);
    }

    public void SetDeckCount(int remain, int maxCount)
    {
        remainCardCountText.text = $"{remain} / {maxCount}";
    }

    public void SetPlayerScore(int score)
    {
        playerScoreText.text = score.ToString();
        playerScoreText.gameObject.SetActive(true);
    }

    public void SetBankerScore(int score)
    {
        bankerScoreText.text = score.ToString();
        bankerScoreText.gameObject.SetActive(true);
    }

    public void SetActivePlayerNatural(bool active)
    {
        playerNaturalText.gameObject.SetActive(active);
    }

    public void SetActiveBankerNatural(bool active)
    {
        bankerNaturalText.gameObject.SetActive(active);
    }

    public void SetCardSprite(CardSlot slot, int cardID)
    {
        if (slot == CardSlot.PlayerFirst)
        {
            playerFirstCard.sprite = cardSprites[cardID];
            playerFirstCard.gameObject.SetActive(true);
        }
        else if (slot == CardSlot.PlayerSecond)
        {
            playerSecondCard.sprite = cardSprites[cardID];
            playerSecondCard.gameObject.SetActive(true);
        }
        else if (slot == CardSlot.PlayerThird)
        {
            playerThirdCard.sprite = cardSprites[cardID];
            playerThirdCard.gameObject.SetActive(true);
        }
        else if (slot == CardSlot.BankerFirst)
        {
            bankerFirstCard.sprite = cardSprites[cardID];
            bankerFirstCard.gameObject.SetActive(true);
        }
        else if (slot == CardSlot.BankerSecond)
        {
            bankerSecondCard.sprite = cardSprites[cardID];
            bankerSecondCard.gameObject.SetActive(true);
        }
        else if (slot == CardSlot.BankerThird)
        {
            bankerThirdCard.sprite = cardSprites[cardID];
            bankerThirdCard.gameObject.SetActive(true);
        }
    }

    public IEnumerator MoveCard(CardSlot slot)
    {
        Image card = GetCard(slot);
        Image cardBack = GetCardBack(slot);

        yield return MoveCard(card, cardBack);
    }

    public IEnumerator OpenCard(CardSlot slot, bool useDelay = false)
    {
        Image cardBack = GetCardBack(slot);
        yield return OpenCard(cardBack, useDelay);
    }

    private IEnumerator MoveCard(Image card, Image cardBack)
    {
        yield return cardBack.rectTransform.DOAnchorPos(card.rectTransform.anchoredPosition, 0.5f).WaitForCompletion();
    }

    private IEnumerator OpenCard(Image cardBack, bool useDelay)
    {
        yield return cardBack.rectTransform.DOAnchorPosX(cardBack.rectTransform.anchoredPosition.x + 25, 0.5f).WaitForCompletion();

        if (useDelay == true)
        {
            yield return new WaitForSeconds(0.5f);
        }

        Sequence seq = DOTween.Sequence();
        seq.Join(cardBack.rectTransform.DOAnchorPosX(cardBack.rectTransform.anchoredPosition.x + 120, 0.5f));
        seq.Join(cardBack.DOFade(0f, 0.5f));
        yield return seq.WaitForCompletion();

        cardBack.rectTransform.position = cardPosition.rectTransform.position;
        cardBack.color = Color.white;
    }

    private void ArrangeCard(CardSlot slot)
    {
        Image card = GetCard(slot);
        Vector3 pos = GetCardPosition(slot);

        card.rectTransform.DOMove(bankerPosition.position, 0.5f).OnComplete(() =>
        {
            card.rectTransform.anchoredPosition = pos;
            card.gameObject.SetActive(false);
        });
    }

    private Image GetCard(CardSlot slot)
    {
        if (slot == CardSlot.PlayerFirst)
        {
            return playerFirstCard;
        }
        else if (slot == CardSlot.PlayerSecond)
        {
            return playerSecondCard;
        }
        else if (slot == CardSlot.PlayerThird)
        {
            return playerThirdCard;
        }
        else if (slot == CardSlot.BankerFirst)
        {
            return bankerFirstCard;
        }
        else if (slot == CardSlot.BankerSecond)
        {
            return bankerSecondCard;
        }
        else if (slot == CardSlot.BankerThird)
        {
            return bankerThirdCard;
        }

        return null;
    }

    private Image GetCardBack(CardSlot slot)
    {
        if (slot == CardSlot.PlayerFirst)
        {
            return playerFirstCardBack;
        }
        else if (slot == CardSlot.PlayerSecond)
        {
            return playerSecondCardBack;
        }
        else if (slot == CardSlot.PlayerThird)
        {
            return playerThirdCardBack;
        }
        else if (slot == CardSlot.BankerFirst)
        {
            return bankerFirstCardBack;
        }
        else if (slot == CardSlot.BankerSecond)
        {
            return bankerSecondCardBack;
        }
        else if (slot == CardSlot.BankerThird)
        {
            return bankerThirdCardBack;
        }

        return null;
    }

    private Vector3 GetCardPosition(CardSlot slot)
    {
        if (slot == CardSlot.PlayerFirst)
        {
            return playerFirstPosition;
        }
        else if (slot == CardSlot.PlayerSecond)
        {
            return playerSecondPosition;
        }
        else if (slot == CardSlot.PlayerThird)
        {
            return playerThirdPosition;
        }
        else if (slot == CardSlot.BankerFirst)
        {
            return bankerFirstPosition;
        }
        else if (slot == CardSlot.BankerSecond)
        {
            return bankerSecondPosition;
        }
        else if (slot == CardSlot.BankerThird)
        {
            return bankerThirdPosition;
        }

        return Vector3.zero;
    }
}
