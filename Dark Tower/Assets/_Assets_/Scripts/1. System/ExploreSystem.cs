using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ExploreSystem : MonoBehaviour
{
    private static ExploreSystem instance;
    public static ExploreSystem Instance
    {
        get { return instance; }
    }

    public ExploreMap exploreMap;

    #region �⺻
    [BoxGroup("�⺻")] public Camera exploreCamera, battleCamera;
    [BoxGroup("�⺻")] public GameObject explorelUI, battleUI;
    #endregion

    #region ���
    [BoxGroup("���")] public string theme;
    [BoxGroup("���")] public GameObject[] enemys;
    #endregion

    public Transform pfItemWorld;
    private Transform itemWorldTrnfParent;
    private GameSystem gameSystem;

    void Awake()
    {
        instance = this;
        enemys = Resources.LoadAll<GameObject>("Prefabs/Enemy/" + theme);
    }

    void Start()
    {
        gameSystem = GameSystem.Instance;
        gameSystem.State = GameState.EXPLORE;
    }

    public Transform GetItemWorldTrnf()
    {
        return exploreMap.itemSpawn;
    }

    public void AddMapItem(Transform mapItem)
    {
        exploreMap.object_G.Add(mapItem);
    }

    public void RemoveMapItem(Transform mapItem)
    {
        exploreMap.object_G.Remove(mapItem);
    }

    public void RemoveMapObject(Transform mapObject)
    {
        exploreMap.object_E.Remove(mapObject);
    }

    public void RemoveMapEnemy(Transform mapEnemy)
    {
        exploreMap.object_Q.Remove(mapEnemy);
    }

    // Ž�迡�� ���� ����
    public void ExploreToBattle()
    {
        battleCamera.enabled = true;
        exploreCamera.enabled = false;

        battleUI.SetActive(true);
        explorelUI.SetActive(false);

        gameSystem.State = GameState.BATTLE;
    }

    // �������� �ٽ� Ž��
    public void BattleToExplore()
    {
        exploreCamera.enabled = true;
        battleCamera.enabled = false;

        explorelUI.SetActive(true);
        battleUI.SetActive(false);

        gameSystem.State = GameState.EXPLORE;
    }
}
