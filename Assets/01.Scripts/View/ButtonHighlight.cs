using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonHighlight : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    [SerializeField] private float highlightAlpha = 0.3f;
    Color colorCache;


    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        colorCache = image.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.DOKill();
        image.color = colorCache;
        image.DOFade(highlightAlpha, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
}