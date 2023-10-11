using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using DT.StatUnit;
using Unity.Android.Types;

[Serializable]
struct TurnStruct
{
    public TurnBuffType turnBuff;
};

public class BuffPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized] public UnitSystem unit;

    [TitleGroup("����")]
    [TabGroup("����/����", "�̺�Ʈ")] public string buffName;
    [TabGroup("����/����", "�̺�Ʈ")] public string buffDesc;
    [TabGroup("����/����", "�̺�Ʈ")] public UnityEvent buffActive;
    [TabGroup("����/����", "�̺�Ʈ")] public UnityEvent buffCheck;
    [TabGroup("����/����", "�̺�Ʈ")] public UnityEvent buffDeactive;

    [TabGroup("����/����", "üũ")] public bool noIcon = false;
    [TabGroup("����/����", "üũ")] public bool noCheck = false;
    [TabGroup("����/����", "üũ")] public bool passive = false;
    [TabGroup("����/����", "üũ")] public bool stack = true;

    [TabGroup("����/����", "üũ")] public int turnCount;
    [TabGroup("����/����", "üũ")] public int turnPlus;
    [TabGroup("����/����", "üũ")] public NoticeType noticeType;
    [SerializeField][TabGroup("����/����", "üũ")] StatStruct[] stat;
    [SerializeField][TabGroup("����/����", "üũ")] TurnStruct[] turn;

    private CanvasGroup canvasGroup;
    private Image plus;
    private TextMeshProUGUI countText;

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Setting --------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        plus = transform.Find("plus").GetComponent<Image>();
        countText = transform.Find("count").GetComponent<TextMeshProUGUI>();

        canvasGroup.alpha = 0;
        TurnCountText();
        ActiveSequence();
    }

    void TurnCountText()
    {
        if (countText == null) return;
        if (!passive) countText.text = turnCount.ToString();
    }

    // ���� Ȱ��ȭ
    public void Active(UnitSystem unit) 
    {
        Init();
        this.unit = unit;

        buffActive.Invoke();
    }

    public void TurnCheck()
    {
        turnCount -= 1;
        TurnCountText();
    }

    // �� ���� ���� üũ
    public void Check()
    {
        buffCheck.Invoke();
        CheckSequence();
    }

    public void Deactive()
    {
        if (!passive)
        {
            buffDeactive.Invoke();
        }
    }

    // ���� ��ø
    public void Stack()
    {
        turnCount += turnPlus;
        TurnCountText();
        StackSequence();
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Active ---------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void Active_Stat()
    {
        for (int i = 0; i < stat.Length; i++)
        {
            UI_BattleStatTable.Instance.StatAdd(unit, stat[i].statName, stat[i].statValue, this);
        }
    }

    public void Active_Turn()
    {
        for (int i = 0; i < turn.Length; i++)
        {
            TurnSystem.Instance.TurnState(turn[i].turnBuff, unit);
        }
    }

    public void Active_State()
    {
        unit.unitState = UnitState.STUN;
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Check ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void Check_TurnDamage(int damage)
    {
        unit.InstantDamage(noticeType, damage);
    }

    public void Check_TurnHeal(int heal)
    {
        unit.InstantHeal(heal);
    }

    ///-----------------------------------------------------------------------------------------------///
    ///----------------------------------------- Deactive --------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void Deactive_Stat()
    {
        for (int i = 0; i < stat.Length; i++)
        {
            UI_BattleStatTable.Instance.StatRemove(unit, stat[i].statName, this);
        }
    }

    public void Deactive_State()
    {
        unit.unitState = UnitState.NORMAL;
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Tween ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    Sequence ActiveSequence()
    {
        return
            DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() =>
            {
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                canvasGroup.alpha = 0;
            })
            .Append(transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce))
            .Join(canvasGroup.DOFade(1, 0.4f))
            .SetDelay(0.2f);
    }

    Sequence CheckSequence()
    {
        return
            DOTween.Sequence()
            .SetAutoKill(false)
            .Append(transform.DOPunchScale(new Vector3(1f, 1f, 1f), 0.3f, 4))
            .SetDelay(0.2f);
    }

    Sequence StackSequence()
    {
        return
            DOTween.Sequence()
            .SetAutoKill(false)
            .OnStart(() =>
            {
                plus.color = new Color(1, 1, 1, 0);
                plus.transform.localPosition = new Vector3(0, 25, 0);
            })
            .Append(plus.DOFade(1, 0.4f))
            .Join(plus.transform.DOLocalMoveY(50, 0.6f))
            .SetDelay(0.2f)
            .Append(plus.DOFade(0, 0.4f));
    }

    ///-----------------------------------------------------------------------------------------------///
    ///----------------------------------------- Tooltip ---------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_TooltipBuff.Show(transform.position, buffName, buffDesc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_TooltipBuff.Hide();
    }
}
