using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExploreEnemy: ExploreObject
{
    [TabGroup("전투", "준비")] public List<GameObject> enemyPrefabs;

    public override void Interact()
    {
        exploreSystem.ExploreToBattle();

        TurnEnemySpawn();
        gameSystem.exploreEnemy = this;
        gameSystem.BattleStart();
    }

    public void TurnEnemySpawn()
    {
        float posX = -3f;

        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            GameObject enemyGO = Instantiate(enemyPrefab, gameSystem.enemyGround);
            enemyGO.transform.localPosition = new Vector3(posX, 0, 0);
            posX += 2f;

            UnitEnemy enemyUnit = enemyGO.GetComponent<UnitEnemy>();
            enemyUnit.Setting();

            gameSystem.enemyUnitCount += 1;
            gameSystem.unitArray.Add(enemyUnit);
            gameSystem.enemyUnits.Add(enemyUnit);
        }
    }

    public void Defeat()
    {
        ImageTween(false);
        exploreSystem.RemoveMapEnemy(transform);
        Destroy(gameObject);
    }
}
