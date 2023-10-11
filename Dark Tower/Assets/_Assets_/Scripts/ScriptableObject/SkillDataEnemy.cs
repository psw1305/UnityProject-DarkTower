using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Skill/Enemy/Instant")]
public class SkillDataEnemy : SkillData
{
	public void EnemyUseSkill()
	{
		SkillMotion();
        SkillTarget();
	}

	void SkillTarget()
	{
        int count = GameSystem.Instance.playerUnits.Count;
        int rand;

        switch (targetType)
        {
            case TargetType.Enemy:
                rand = Random.Range(0, count);
                UseSkillForSingle(GameSystem.Instance.enemyUnits[rand]);
                break;

            case TargetType.EnemyAll:
                UseSkillForEnemys(GameSystem.Instance.enemyUnits);
                break;

            case TargetType.Player:
                rand = Random.Range(0, count);
                UseSkillForSingle(GameSystem.Instance.playerUnits[rand]);
                break;

            case TargetType.PlayerAll:
                UseSkillForPlayers(GameSystem.Instance.playerUnits);
                break;
        }
    }
}
