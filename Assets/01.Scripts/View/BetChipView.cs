using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetChipView : MonoBehaviour
{
    private const float CHIP_PADDING = 45;
    private const float STACK_PADDING = 13;
    private const int MAX_CHIP_COUNT = 10;

    private class ChipStack
    {
        public BetAmount ChipType { get; set; }
        public RectTransform BaseObject { get; set; }
        public int Count { get; set; }
    }

    [Header("Positions")]
    [SerializeField] private RectTransform playerPosition;
    [SerializeField] private RectTransform bankerPosition;

    [Header("Betting Area")]
    [SerializeField] private RectTransform playerArea;
    [SerializeField] private RectTransform bankerArea;
    [SerializeField] private RectTransform tieArea;
    [SerializeField] private RectTransform playerPairArea;
    [SerializeField] private RectTransform bankerPairArea;

    [Header("Chip Prefabs")]
    [SerializeField] private GameObject chip10;
    [SerializeField] private GameObject chip50;
    [SerializeField] private GameObject chip100;
    [SerializeField] private GameObject chip500;
    [SerializeField] private GameObject chip1k;
    [SerializeField] private GameObject chip5k;
    [SerializeField] private GameObject chip10k;
    [SerializeField] private GameObject chip50k;

    private readonly Dictionary<BetType, Dictionary<BetAmount, Stack<ChipStack>>> stacks = new Dictionary<BetType, Dictionary<BetAmount, Stack<ChipStack>>>();

    public void AddChip(BetType betType, BetAmount chipType)
    {
        AddChip(betType, chipType, playerPosition.position);
    }

    public void AddChips(BetType betType, long betAmount)
    {
        while (betAmount >= 10)
        {
            for (BetAmount i = BetAmount.FiftyThousand; i > BetAmount.None; i--)
            {
                var curAmount = BetAmountUtility.GetBetAmount(i);

                if (betAmount >= curAmount)
                {
                    AddChip(betType, i, playerPosition.position);
                    betAmount -= curAmount;
                    break;
                }
            }
        }
    }

    private void AddChip(BetType betType, BetAmount chipType, Vector3 position)
    {
        if (betType == BetType.None || chipType == BetAmount.None)
        {
            return;
        }

        var curStack = GetOrCreateStack(betType, chipType);

        if (curStack == null)
        {
            curStack = new Stack<ChipStack>();
        }

        if (curStack.Count <= 0)
        {
            var tmp = new ChipStack();
            tmp.ChipType = chipType;
            tmp.BaseObject = InstantiateChip(betType, chipType, position).GetComponent<RectTransform>();
            var pos = GetRandomWorldPositionInRect(GetRectTransform(betType));
            tmp.BaseObject.transform.DOMove(pos, 0.5f);
            tmp.Count++;
            curStack.Push(tmp);
            return;
        }

        ChipStack chipStack = curStack.Peek();

        if (chipStack.Count >= MAX_CHIP_COUNT)
        {
            var tmp = new ChipStack();
            tmp.ChipType = chipType;
            tmp.BaseObject = InstantiateChip(betType, chipType, position).GetComponent<RectTransform>();
            var pos = GetRandomWorldPositionInRect(GetRectTransform(betType));
            tmp.BaseObject.transform.DOMove(pos, 0.5f);
            tmp.Count++;
            curStack.Push(tmp);
        }
        else
        {
            var go = InstantiateChip(betType, chipType, position, chipStack.BaseObject).GetComponent<RectTransform>();
            go.DOAnchorPos(new Vector2(0, chipStack.Count * STACK_PADDING), 0.5f);
            chipStack.Count++;
        }
    }

    public void ClearChips()
    {
        Sequence sequence = DOTween.Sequence();
        for (BetType i = 0; i <= BetType.BankerPair; i++)
        {
            var tmp = ClearChips(i, playerPosition.position);

            if (tmp != null)
            {
                sequence.Join(tmp);
            }

        }
        sequence.Play();
        stacks.Clear();
    }

    private Sequence ClearChips(BetType betType, Vector3 position)
    {
        if (stacks.ContainsKey(betType))
        {
            var curStacks = stacks[betType];

            if (curStacks == null)
            {
                return null;
            }

            Sequence seq = DOTween.Sequence();

            foreach (var stack in curStacks.Values)
            {
                while (stack.Count > 0)
                {
                    var cur = stack.Pop();
                    seq.Join(cur.BaseObject.DOMove(position, 0.2f).OnComplete(() =>
                    {
                        Destroy(cur.BaseObject.gameObject);
                    }));
                }
            }

            stacks.Remove(betType);

            return seq;
        }

        return null;
    }

    public IEnumerator ResolveChips(List<BetResolve> betResolves)
    {
        Sequence seq = DOTween.Sequence();
        foreach (var item in betResolves)
        {
            if (item.ResolveType == BetResolveType.Lose)
            {
                seq.Join(ClearChips(item.BetType, bankerPosition.position));
            }
        }
        yield return seq.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);

        foreach (var item in betResolves)
        {
            if (item.ResolveType == BetResolveType.Win)
            {
                AddProfitChips(item.BetType, item.ProfitAmount);
            }
        }

        yield return new WaitForSeconds(1f);
        ClearChips();
        yield return new WaitForSeconds(0.5f);
    }

    private void AddProfitChips(BetType betType, long betAmount)
    {
        while (betAmount >= 10)
        {
            for (BetAmount i = BetAmount.FiftyThousand; i > BetAmount.None; i--)
            {
                var curAmount = BetAmountUtility.GetBetAmount(i);

                if (betAmount >= curAmount)
                {
                    AddChip(betType, i, bankerPosition.position);
                    betAmount -= curAmount;
                    break;
                }
            }
        }
    }

    private Stack<ChipStack> GetOrCreateStack(BetType betType, BetAmount chipType)
    {
        if (stacks.ContainsKey(betType) == false)
        {
            stacks[betType] = new Dictionary<BetAmount, Stack<ChipStack>>();
        }

        if (stacks[betType].ContainsKey(chipType) == false)
        {
            stacks[betType][chipType] = new Stack<ChipStack>();
        }

        return stacks[betType][chipType];
    }


    private Vector3 GetRandomWorldPositionInRect(RectTransform rectTransform)
    {
        Rect rect = rectTransform.rect;

        Vector2 localPosition = new Vector2(
            Random.Range(rect.xMin + CHIP_PADDING, rect.xMax - CHIP_PADDING),
            Random.Range(rect.yMin + CHIP_PADDING, rect.yMax - CHIP_PADDING)
        );

        return rectTransform.TransformPoint(localPosition);
    }


    private GameObject InstantiateChip(BetType betType, BetAmount chipType, Vector3 position, RectTransform p = null)
    {
        GameObject prefab = GetChipPrefab(chipType);

        RectTransform parent = p == null ? GetRectTransform(betType) : p;

        var result = Instantiate(prefab, parent);
        RectTransform rect = result.GetComponent<RectTransform>();

        rect.localScale = Vector3.one;
        rect.position = position;

        return result;
    }

    private GameObject GetChipPrefab(BetAmount chipType)
    {
        GameObject go = null;

        if (chipType == BetAmount.Ten)
        {
            go = chip10;
        }
        else if (chipType == BetAmount.Fifty)
        {
            go = chip50;
        }
        else if (chipType == BetAmount.Hundred)
        {
            go = chip100;
        }
        else if (chipType == BetAmount.FiveHundred)
        {
            go = chip500;
        }
        else if (chipType == BetAmount.OneThousand)
        {
            go = chip1k;
        }
        else if (chipType == BetAmount.FiveThousand)
        {
            go = chip5k;
        }
        else if (chipType == BetAmount.TenThousand)
        {
            go = chip10k;
        }
        else if (chipType == BetAmount.FiftyThousand)
        {
            go = chip50k;
        }

        return go;
    }

    private RectTransform GetRectTransform(BetType betType)
    {
        RectTransform rect = null;

        if (betType == BetType.Player)
        {
            rect = playerArea;
        }
        else if (betType == BetType.Banker)
        {
            rect = bankerArea;
        }
        else if (betType == BetType.Tie)
        {
            rect = tieArea;
        }
        else if (betType == BetType.PlayerPair)
        {
            rect = playerPairArea;
        }
        else if (betType == BetType.BankerPair)
        {
            rect = bankerPairArea;
        }

        return rect;
    }
}


