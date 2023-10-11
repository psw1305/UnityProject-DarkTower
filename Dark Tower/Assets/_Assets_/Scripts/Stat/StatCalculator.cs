using UnityEngine;

namespace DT.StatCalc
{
    public class StatCalculator
    {
        // 적중 확률 계산 (자신의 명중률 - 상대의 회피율)
        public static bool EvadeCheck(float hit, float dodge)
        {
            float percent = (hit - dodge) * 10;
            int ran = Random.Range(0, 1000);
            bool isCheck = (ran <= percent);
            //Debug.Log("회피 확률: " + percent + " 확률값 : " + ran + " 최종: " + isCheck);
            return isCheck;
        }

        // 치명 확률 계산
        public static bool CritChance(float crt)
        {
            float percent = crt * 10;
            int ran = Random.Range(0, 1000);
            bool isCheck = (ran <= percent);
            //Debug.Log("치명 확률: " + percent + " 확률값 : " + ran + " 최종: " + isCheck);
            return isCheck;
        }
    }
}
