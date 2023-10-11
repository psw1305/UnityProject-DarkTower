using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnyPortrait;
using DT.StatUnit;
using DT.StatCalc;
using JetBrains.Annotations;

public enum UnitType
{
    Player,
    Enemy,
}

[SerializeField]
public enum UnitState
{
    NORMAL,
    STUN,
    CONFUSE,
    DEAD,
}

public abstract class UnitSystem : MonoBehaviour
{
    #region 능력치

    [TitleGroup("능력치")]
    [TabGroup("능력치/구분", "정보")] public UnitType unitType;
    [TabGroup("능력치/구분", "정보")] public string className, nickName, level;
    [TabGroup("능력치/구분", "정보")] public apPortrait portrait;

    [TabGroup("능력치/구분", "스탯")] public float HP;      // 체력
    [TabGroup("능력치/구분", "스탯")] public float SP;      // 쉴드
    [TabGroup("능력치/구분", "스탯")] public float SPD;     // 속도
    [NonSerialized] public float BaseHP, BaseSPD;

    [TabGroup("능력치/구분", "스탯")] public StatUnit STRMin;  // 최소 공격력
    [TabGroup("능력치/구분", "스탯")] public StatUnit STRMax;  // 최대 공격력
    [TabGroup("능력치/구분", "스탯")] public StatUnit HIT;     // 명중률
    [TabGroup("능력치/구분", "스탯")] public StatUnit CRIT;    // 치명
    [TabGroup("능력치/구분", "스탯")] public StatUnit DEF;     // 방어력
    [TabGroup("능력치/구분", "스탯")] public StatUnit AGI;   // 회피
    #endregion

    #region 상태

    [TitleGroup("상태")]
    [TabGroup("상태/구분", "턴")] public int turnCnt;
    [TabGroup("상태/구분", "턴")] public List<TurnUnit> turnUnits = new List<TurnUnit>();

    [TabGroup("상태/구분", "상태이상")] public List<BuffPrefab> buffList = new List<BuffPrefab>();
    [TabGroup("상태/구분", "상태이상")] public string stateIdle = "Normal";
    private List<BuffPrefab> buffCache = new List<BuffPrefab>();

    [TabGroup("상태/구분", "체크")] public UnitState unitState;
    [TabGroup("상태/구분", "체크"), LabelWidth(80)] public bool isTurn, isChange, isStance, isRestrict;

    #endregion

    [TitleGroup("UI")]
    [BoxGroup("UI/기본")] public GameObject unitUIPrefab;
    [BoxGroup("UI/기본")] public GameObject turnFlagPrefab;

    protected UI_BattleUnit unitUI = null;
    private GameSystem battleSystem;

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------- Init ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    void Start()
    {
        battleSystem = GameSystem.Instance;
    }

    protected abstract void Init();

    public void Setting()
    {
        isChange = false;
        isStance = false;
        unitState = UnitState.NORMAL;

        Init();
        BaseHP = HP;
        BaseSPD = SPD;
    }

    public void UnitUISetting()
    {
        if (unitUI != null) return;

        unitUI = Instantiate(unitUIPrefab, transform).GetComponent<UI_BattleUnit>();
        unitUI.Init(this);
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------- Turn ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public virtual void MyTurnStart() { }

    public void MyTurnEnd()
    {
        unitUI.turnLight.DOFade(0, 0.4f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            StartCoroutine(nameof(TurnEnd));
        });
    }

    IEnumerator TurnEnd()
    {
        isTurn = false;
        RemoveBuff();
        TurnSystem.Instance.currentTurn.Deactive();

        yield return new WaitForSeconds(0.3f);

        TurnSystem.Instance.TurnDestroy();
        TurnSystem.Instance.TurnStart();
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------- Dead ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void UnitDead(bool instantDead = false)
    {
        StartCoroutine(DeadCheck(instantDead));
    }

    public IEnumerator DeadCheck(bool instant)
    {
        unitState = UnitState.DEAD;
        Motion("Dead");

        yield return new WaitForSeconds(1.2f);

        if (unitType == UnitType.Player)
        {
            battleSystem.playerUnitCount -= 1;
            battleSystem.PlayerDeadCheck(GetComponent<UnitPlayer>());
        }
        else if (unitType == UnitType.Enemy)
        {
            battleSystem.enemyUnitCount -= 1;
            battleSystem.EnemyDeadCheck(GetComponent<UnitEnemy>());
        }

        // 유닛 사망시 BattleSystem 에서 유닛 수 체크
        battleSystem.BattleResult();
        if (instant) TurnSystem.Instance.TurnStart();
        gameObject.SetActive(false);
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------ Damage ---------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void UnitDamage(DamageType attribute, float sSTRMin, float sSTRMAX, float sCrit)
    {
        int damage = (int)UnityEngine.Random.Range(sSTRMin, sSTRMAX + 1);
        int resultDamage = (int)(damage * ((DEF.Value + 100) / 100));

        if (resultDamage < 0) resultDamage = 0;

        string damageText;

        if (StatCalculator.CritChance(sCrit))
        {
            resultDamage = Mathf.RoundToInt(resultDamage * 1.5f);
            damageText = "치명타! " + resultDamage.ToString();
        }
        else
        {
            damageText = resultDamage.ToString();
        }

        DamageText(attribute, damageText);

        float preHP = HP;
        unitUI.hpBackSlider.fillAmount = preHP / BaseHP;

        HP -= resultDamage;

        if (HP <= 0)
        {
            HP = 0;
            unitUI.hpSlider.fillAmount = 0.0f;
            unitUI.hpHealSlider.fillAmount = 0.0f;
            UnitDead();
        }
        else
        {
            unitUI.hpSlider.fillAmount = HP / BaseHP;
            unitUI.hpHealSlider.fillAmount = HP / BaseHP;
            unitUI.hpBackSlider.fillAmount = preHP / BaseHP;
            unitUI.hpBackSlider.DOFillAmount(HP / BaseHP, 0.2f).SetEase(Ease.OutSine).SetDelay(1.8f);
            Motion("Damaged");
        }
    }

    public void InstantDamage(NoticeType noticeType, int damage)
    {
        string damageText = damage.ToString();
        NoticeText(noticeType, damageText);

        float preHP = HP;
        unitUI.hpBackSlider.fillAmount = preHP / BaseHP;

        HP -= damage;

        if (HP <= 0)
        {
            HP = 0;
            unitUI.hpSlider.fillAmount = 0.0f;
            unitUI.hpHealSlider.fillAmount = 0.0f;
            UnitDead(true);
        }
        else
        {
            unitUI.hpSlider.fillAmount = HP / BaseHP;
            unitUI.hpHealSlider.fillAmount = HP / BaseHP;
            unitUI.hpBackSlider.fillAmount = preHP / BaseHP;
            unitUI.hpBackSlider.DOFillAmount(HP / BaseHP, 0.2f).SetEase(Ease.OutSine).SetDelay(1f);
        }
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------- Heal ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void UnitHeal(float sSTRMin, float sSTRMax, float sCrit)
    {
        int resultHeal = (int)UnityEngine.Random.Range(sSTRMin, sSTRMax + 1);
        string healText;

        if (StatCalculator.CritChance(sCrit))
        {
            resultHeal = Mathf.RoundToInt(resultHeal * 1.5f); ;
            healText = "효율" + resultHeal.ToString();
        }
        else
        {
            healText = resultHeal.ToString();
        }

        DamageText(DamageType.HEAL, healText);

        float preHP = HP;
        HP += resultHeal;

        if (HP >= BaseHP)
        {
            HP = BaseHP;
            unitUI.hpHealSlider.fillAmount = 1.0f;
        }
        else
        {
            unitUI.hpHealSlider.fillAmount = HP / BaseHP;
        }

        unitUI.hpSlider.fillAmount = preHP / BaseHP;
        unitUI.hpSlider.DOFillAmount(HP / BaseHP, 0.2f).SetEase(Ease.OutSine).SetDelay(1.8f);
    }

    public void InstantHeal(int heal)
    {
        string healText = heal.ToString();
        DamageText(DamageType.HEAL, healText);

        float preHP = HP;
        HP += heal;

        if (HP >= BaseHP)
        {
            HP = BaseHP;
            unitUI.hpHealSlider.fillAmount = 1.0f;
        }
        else
        {
            unitUI.hpHealSlider.fillAmount = HP / BaseHP;
        }

        unitUI.hpSlider.fillAmount = preHP / BaseHP;
        unitUI.hpSlider.DOFillAmount(HP / BaseHP, 0.2f).SetEase(Ease.OutSine).SetDelay(1f);
    }

    public void SliderValueReset()
    {
        unitUI.hpHealSlider.fillAmount = 0.0f;
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------- Buff ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    void BuffInstance(GameObject buff, string buffLabel)
    {
        BuffPrefab buffClone = Instantiate(buff, unitUI.buffPanel).GetComponent<BuffPrefab>();
        buffList.Add(buffClone);
        buffClone.Active(this);
        NoticeText(NoticeType.BUFF, buffLabel);
    }

    public void AddBuff(GameObject buff) 
    {
        buffCache.Add(buff.GetComponent<BuffPrefab>());
    }

    public void ActiveBuff()
    {
        if (buffCache.Count <= 0) return;

        bool isExist = false;

        for (int i = 0; i < buffCache.Count; i++)
        {
            for (int k = 0; k < buffList.Count; k++)
            {
                if (buffCache[i].buffName == buffList[k].buffName)
                {
                    if (buffList[k].stack)
                    {
                        isExist = true;
                        buffList[k].Stack();
                        NoticeText(NoticeType.BUFF, buffList[k].buffName + " 중첩");
                    }
                }
            }

            if (!isExist)
            {
                BuffInstance(buffCache[i].gameObject, buffCache[i].buffName);
            }
        }

        buffCache.Clear();
    }

    public void CheckBuff()
    {
        StartCoroutine(nameof(DelayBuffCheck));
    }

    IEnumerator DelayBuffCheck()
    {
        foreach (BuffPrefab buff in buffList)
        {
            buff.TurnCheck();

            if (!buff.noCheck)
            {
                if (unitState != UnitState.DEAD)
                {
                    buff.Check();
                    yield return new WaitForSeconds(1.5f);
                }
                else if (unitState == UnitState.DEAD)
                {
                    yield break;
                }
            }
            else
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.2f);
        MyTurnStart();
    }

    void RemoveBuff()
    {
        var toRemove = new HashSet<BuffPrefab>();

        foreach (BuffPrefab buff in buffList)
        {
            // 턴 소진 시 버프 제거
            if (buff.turnCount == 0)
            {
                buff.Deactive();
                toRemove.Add(buff);
                Destroy(buff.gameObject);
            }
        }

        buffList.RemoveAll(toRemove.Contains);
    }

    ///-----------------------------------------------------------------------------------------------///
    ///---------------------------------------- Animation --------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public virtual void Motion(string clipName) { portrait.Play(clipName); }
    public virtual void MotionByState(string idleName)
    {
        string clipName = idleName + "_" + stateIdle;
        portrait.Play(clipName);
    }

    ///-----------------------------------------------------------------------------------------------///
    ///------------------------------------------- Text ----------------------------------------------///
    ///-----------------------------------------------------------------------------------------------///

    public void DamageText(DamageType attribute, string text)
    {
        unitUI.ShowDamage(attribute, text);
    }

    public void NoticeText(NoticeType textType, string text)
    {
        unitUI.ShowNotice(textType, text);
    }
}
