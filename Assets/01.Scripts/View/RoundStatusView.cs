using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundStatusView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    public void SetMessage(string message)
    {
        statusText.text = message;
    }
}
