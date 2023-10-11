using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Com.LuisPedroFonseca.ProCamera2D;

public class BattleCamera : MonoBehaviour
{
    private static BattleCamera instance;
    public static BattleCamera Instance
    {
        get { return instance; }
    }

    [BoxGroup("전투 연출")] public Renderer blurWindow;
    [BoxGroup("전투 연출")] public Camera gameCamera;
    private ProCamera2DCinematics proCamera;

    [BoxGroup("액션 유닛")] public UnitSystem actionUnit;
    [BoxGroup("액션 유닛")] public List<UnitSystem> targetUnits = new List<UnitSystem>();
    private GameSystem battleSystem;

    void Awake()
    {
        instance = this;
        blurWindow.sharedMaterial.SetFloat("_Blur", 0);
    }

    void Start()
    {
        proCamera = gameCamera.GetComponent<ProCamera2DCinematics>();
        battleSystem = GameSystem.Instance;
    }

    public Vector2 ScreenToWorldPoint()
    {
        return gameCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void AddUnit(UnitSystem unit)
    {
        targetUnits.Add(unit);
    }

    public void Action()
    {
        StartCoroutine(nameof(ActionTime));
    }

    IEnumerator ActionTime()
    {
        UnitActionOn();
        yield return new WaitForSeconds(1.2f);
        UnitActionOff();

        yield return new WaitForSeconds(0.6f);

        actionUnit = null;
        targetUnits.Clear();
        battleSystem.selectedUnit.MyTurnEnd();
    }

    public void UnitActionOn()
    {
        proCamera.Play();

        blurWindow.sharedMaterial.DOFloat(15, "_Blur", 0.2f).OnStart(() => 
        {
            actionUnit.portrait.SetSortingOrder(145);

            foreach (UnitSystem targetUnit in targetUnits)
            {
                targetUnit.portrait.SetSortingOrder(140);
            }
        });
    }

    public void UnitActionOff()
    {
        proCamera.Stop();

        blurWindow.sharedMaterial.DOFloat(0, "_Blur", 0.2f).OnComplete(() =>
        {
            actionUnit.MotionByState("Idle");
            actionUnit.portrait.SetSortingOrder(-10);

            foreach (UnitSystem targetUnit in targetUnits)
            {
                targetUnit.MotionByState("Idle");
                targetUnit.portrait.SetSortingOrder(-10);
                targetUnit.ActiveBuff();
            }
        });
    }
}
