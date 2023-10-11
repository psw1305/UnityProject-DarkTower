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
    [TabGroup("Hp Bar/구분", "Slider")] public Image hpSlider, hpBackSlider, hpHealSlider;
    [TabGroup("Hp Bar/구분", "Text")] public UI_TextDamage uiTextDamage;
    [TabGroup("Hp Bar/구분", "Text")] public UI_TextNotice uiTextNotice;
    [TabGroup("Hp Bar/구분", "Text")] public Transform damagePanel;
    [TabGroup("Hp Bar/구분", "Text")] public Transform noticePanel;

    [TitleGroup("Buff")]
    [TabGroup("Buff/구분", "기본")] public Transform buffPanel;

    [TitleGroup("Turn")]
    [TabGroup("Turn/구분", "턴")] public Image turnLight;

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


    // 마우스 오버시 unitInfo on/off
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
