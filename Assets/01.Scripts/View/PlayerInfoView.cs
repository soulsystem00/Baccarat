using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoView : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerNickname;
    [SerializeField] private TextMeshProUGUI playerBalance;

    public void SetPlayerImage(Sprite sprite)
    {
        playerImage.sprite = sprite;
    }

    public void SetPlayerNickname(string nickname)
    {
        playerNickname.text = nickname;
    }

    public void SetPlayerBalance(long balance)
    {
        playerBalance.text = balance.ToString("N0");
    }
}
