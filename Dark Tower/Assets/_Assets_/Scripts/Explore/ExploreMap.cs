using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExploreMap : MonoBehaviour
{
    #region ������Ʈ
    [BoxGroup("������Ʈ")] public ExplorePlayer map_player;
    [BoxGroup("������Ʈ")] public Transform itemSpawn;
    [BoxGroup("������Ʈ")] public List<Transform> object_Q;
    [BoxGroup("������Ʈ")] public List<Transform> object_E;
    [BoxGroup("������Ʈ")] public List<Transform> object_G;
    [BoxGroup("������Ʈ")] public Transform closet_Q = null;
    [BoxGroup("������Ʈ")] public Transform closet_E = null;
    [BoxGroup("������Ʈ")] public Transform closet_G = null;
    #endregion

    public bool drawTarget = false;
    public LayerMask layerMask;

    void Update()
    {
        // ��ȣ�ۿ� => ��
        if (object_Q.Count != 0)
        {
            float closetDist = Mathf.Infinity;

            for (int i = 0; i < object_Q.Count; i++)
            {
                float currentDist = Vector3.Distance(map_player.transform.position, object_Q[i].position);

                if (currentDist < closetDist)
                {
                    closetDist = currentDist;
                    closet_Q = object_Q[i];
                    map_player.SetClosestEnemy(closet_Q);
                }
            }
        }

        // ��ȣ�ۿ� => ����
        if (object_E.Count != 0)
        {
            float closetDist = Mathf.Infinity;

            for (int i = 0; i < object_E.Count; i++)
            {
                float currentDist = Vector3.Distance(map_player.transform.position, object_E[i].position);

                if (currentDist < closetDist)
                {
                    closetDist = currentDist;
                    closet_E = object_E[i];
                    map_player.SetClosestObject(closet_E);
                }
            }
        }

        // ������ �ݱ�
        if (object_G.Count != 0)
        {
            float closetDist = Mathf.Infinity;

            for (int i = 0; i < object_G.Count; i++)
            {
                float currentDist = Vector3.Distance(map_player.transform.position, object_G[i].position);

                if (currentDist <= 5.0f)
                {
                    if (currentDist < closetDist)
                    {
                        closetDist = currentDist;
                        closet_G = object_G[i];
                        map_player.SetClosestItem(closet_G);
                    }
                }
                else
                {
                    closet_G = null;
                    map_player.SetClosestItemNULL();
                }
            }
        }
    }
}
