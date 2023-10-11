using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExploreMap : MonoBehaviour
{
    #region 오브젝트
    [BoxGroup("오브젝트")] public ExplorePlayer map_player;
    [BoxGroup("오브젝트")] public Transform itemSpawn;
    [BoxGroup("오브젝트")] public List<Transform> object_Q;
    [BoxGroup("오브젝트")] public List<Transform> object_E;
    [BoxGroup("오브젝트")] public List<Transform> object_G;
    [BoxGroup("오브젝트")] public Transform closet_Q = null;
    [BoxGroup("오브젝트")] public Transform closet_E = null;
    [BoxGroup("오브젝트")] public Transform closet_G = null;
    #endregion

    public bool drawTarget = false;
    public LayerMask layerMask;

    void Update()
    {
        // 상호작용 => 적
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

        // 상호작용 => 상자
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

        // 아이템 줍기
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
