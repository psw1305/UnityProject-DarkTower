using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class UnitPlayer : UnitSystem
{
    [TitleGroup("클래스")]
    [BoxGroup("클래스/기본")] public SkillData[] skillData;
    [BoxGroup("클래스/기본")] public UI_ItemEquipment[] equipmentData;

    protected override void Init()
    {
        var entity = BG_Class_Stat.GetEntity(className);
        HP = entity.bg_HLT;
        SPD = entity.bg_ACT;

        STRMax.BaseValue = entity.bg_STR;
        STRMin.BaseValue = Mathf.CeilToInt(STRMax.BaseValue * 0.5f);
        DEF.BaseValue = entity.bg_DEF;
        HIT.BaseValue = entity.bg_ACR;
        AGI.BaseValue = entity.bg_AGI;
        CRIT.BaseValue = entity.bg_CRT;
    }

    public override void MyTurnStart()
    {
        unitUI.turnLight.DOFade(1, 0.4f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            StartCoroutine(nameof(PlayerTurnStart));
        });
    }

    IEnumerator PlayerTurnStart()
    {
        isTurn = true;

        yield return new WaitForSeconds(0.4f);

        switch (unitState)
        {
            case UnitState.NORMAL:
                TurnSystem.Instance.turnState = TurnState.PLAYER_TURN;
                break;
            case UnitState.STUN:
                MyTurnEnd();
                break;
        }
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Status ---------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void AllRemoveBuff()
    {
        foreach (BuffPrefab buff in buffList)
        {
            buff.turnCount = 0;
            buff.Deactive();
            Destroy(buff.gameObject);
        }

        buffList.Clear();
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Equip ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public Sprite GetEquipmentSprite(int num)
    {
        if (equipmentData[num] == null) return null;

        return equipmentData[num].itemImage.sprite;
    }

    public void Equip(UI_Item uiItem, int num)
    {
        equipmentData[num] = (UI_ItemEquipment)uiItem;
        equipmentData[num].AddStat(this);
    }

    public void Unequip(UI_Item uiItem)
    {
        for (int i = 0; i < equipmentData.Length; i++)
        {
            if (equipmentData[i] == (UI_ItemEquipment)uiItem)
            {
                equipmentData[i].RemoveStat(this);
                equipmentData[i] = null;
            }
        }
    }

    public void NoticeSkillDisable()
    {
        unitUI.ShowNotice(NoticeType.NOTICE, "스킬 사용불가");
    }
}
