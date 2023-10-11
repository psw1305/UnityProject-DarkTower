using UnityEngine;

namespace DT.StatCalc
{
    public class StatCalculator
    {
        // ���� Ȯ�� ��� (�ڽ��� ���߷� - ����� ȸ����)
        public static bool EvadeCheck(float hit, float dodge)
        {
            float percent = (hit - dodge) * 10;
            int ran = Random.Range(0, 1000);
            bool isCheck = (ran <= percent);
            //Debug.Log("ȸ�� Ȯ��: " + percent + " Ȯ���� : " + ran + " ����: " + isCheck);
            return isCheck;
        }

        // ġ�� Ȯ�� ���
        public static bool CritChance(float crt)
        {
            float percent = crt * 10;
            int ran = Random.Range(0, 1000);
            bool isCheck = (ran <= percent);
            //Debug.Log("ġ�� Ȯ��: " + percent + " Ȯ���� : " + ran + " ����: " + isCheck);
            return isCheck;
        }
    }
}
