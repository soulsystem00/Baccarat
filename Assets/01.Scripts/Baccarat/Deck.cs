using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private const int CardCount = 52;

    private readonly int packCount;
    private readonly int[] cards;
    private int index;

    public int MaxCardCount => cards.Length;
    public int RemainCardCount => cards.Length - index;

    public Deck(int packCount = 8)
    {
        this.packCount = packCount;
        cards = new int[CardCount * packCount];

        Init();
        Shuffle();
    }

    private void Init()
    {
        index = 0;

        for (int i = 0; i < packCount; i++)
        {
            for (int j = 0; j < CardCount; j++)
            {
                cards[index++] = j;
            }
        }

        index = 0;
    }

    private void Shuffle()
    {
        for (int i = cards.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);

            int temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }

        index = 0;
    }

    public int Draw()
    {
        if (index >= cards.Length)
        {
            Shuffle();
        }

        return cards[index++];
    }
}
