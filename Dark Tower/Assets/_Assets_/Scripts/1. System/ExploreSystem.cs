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

    #region 기본
    [BoxGroup("기본")] public Camera exploreCamera, battleCamera;
    [BoxGroup("기본")] public GameObject explorelUI, battleUI;
    #endregion

    #region 배경
    [BoxGroup("배경")] public string theme;
    [BoxGroup("배경")] public GameObject[] enemys;
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

    // 탐험에서 전투 개시
    public void ExploreToBattle()
    {
        battleCamera.enabled = true;
        exploreCamera.enabled = false;

        battleUI.SetActive(true);
        explorelUI.SetActive(false);

        gameSystem.State = GameState.BATTLE;
    }

    // 전투에서 다시 탐색
    public void BattleToExplore()
    {
        exploreCamera.enabled = true;
        battleCamera.enabled = false;

        explorelUI.SetActive(true);
        battleUI.SetActive(false);

        gameSystem.State = GameState.EXPLORE;
    }
}
