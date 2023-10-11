using System.Collections;
using UnityEngine;

public class ExploreObjectChest : ExploreObject
{
    public ItemData[] list;

    public int itemCount;

    public override void Interact()
    {
        StartCoroutine(ItemRandomLoot());
    }

    IEnumerator ItemRandomLoot()
    {
        exploreSystem.RemoveMapObject(transform);

        for (int i = 0; i < itemCount; i++)
        {
            ItemWorld.DropItem(transform.position, list[i]);

            //int p = Random.Range(0, 1000);

            //if (p <= 333)
            //{
            //    ItemWorld.DropItem(transform.position, list[0]);
            //}
            //else if (p <= 666)
            //{
            //    ItemWorld.DropItem(transform.position, list[1]);
            //}
            //else
            //{
            //    ItemWorld.DropItem(transform.position, list[2]);
            //}

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
