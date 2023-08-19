using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHPBar : MonoBehaviour
{
    [Header("HPBar")]
    [SerializeField] private Entity entity;
    [SerializeField] private Transform canvas;
    [SerializeField] private Slider hpSlider;
    private CanvasGroup canvasGroup;

    [Header("위치/크기")]
    [SerializeField] private Vector3 hpBarOffset;
    [SerializeField] private Vector2 hpBarSize;

    public void Start()
    {
        SetUp();
    }

    public void OnValidate()
    {
        transform.position = entity.transform.position;
        canvas.transform.position = transform.position + hpBarOffset;
        hpSlider.GetComponent<RectTransform>().sizeDelta = hpBarSize * 100;
    }

    public void SetUp()
    {
        hpSlider.maxValue = entity.MaxHp;
        hpSlider.value = entity.MaxHp;
        canvasGroup = canvas.GetComponent<CanvasGroup>();
        //canvasGroup.alpha = 0.2f;

        entity.OnHpChange += UpdateHpBar;
        //entity.OnFocusIn += () => { DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 0.1f); };
        //entity.OnFocusOut += () => { DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0.2f, 0.5f); };
    }

    public void UpdateHpBar()
    {
        hpSlider.value = entity.Hp;
    }
}
