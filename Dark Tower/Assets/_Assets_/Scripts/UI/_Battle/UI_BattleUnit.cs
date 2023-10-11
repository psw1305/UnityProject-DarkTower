using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UI_BattleUnit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized] public UnitSystem unit;
    private Canvas mainCanvas;
    private Canvas damageCanvas;

    [TitleGroup("Hp Bar")]
    [TabGroup("Hp Bar/����", "Slider")] public Image hpSlider, hpBackSlider, hpHealSlider;
    [TabGroup("Hp Bar/����", "Text")] public UI_TextDamage uiTextDamage;
    [TabGroup("Hp Bar/����", "Text")] public UI_TextNotice uiTextNotice;
    [TabGroup("Hp Bar/����", "Text")] public Transform damagePanel;
    [TabGroup("Hp Bar/����", "Text")] public Transform noticePanel;

    [TitleGroup("Buff")]
    [TabGroup("Buff/����", "�⺻")] public Transform buffPanel;

    [TitleGroup("Turn")]
    [TabGroup("Turn/����", "��")] public Image turnLight;

    public void Init(UnitSystem unitSystem)
    {
        mainCanvas = GetComponent<Canvas>();
        mainCanvas.worldCamera = BattleCamera.Instance.gameCamera;

        damageCanvas = damagePanel.GetComponent<Canvas>();
        damageCanvas.overrideSorting = true;
        damageCanvas.sortingOrder = 150;

        unit = unitSystem;
        hpSlider.fillAmount = 1.0f;
    }

    public void ShowDamage(DamageType attribute, string text)
    {
        uiTextDamage.TextShow(attribute, text, damagePanel);
    }

    public void ShowNotice(NoticeType notice, string text)
    {
        uiTextNotice.TextShow(notice, text, noticePanel);
    }


    // ���콺 ������ unitInfo on/off
    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < unit.turnUnits.Count; i++)
        {
            unit.turnUnits[i].ImageFadeOn();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (int i = 0; i < unit.turnUnits.Count; i++)
        {
            unit.turnUnits[i].ImageFadeOff();
        }
    }
}
