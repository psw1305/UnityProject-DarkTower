using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour {
    public ItemData item;

    private void Start() {
        Vector3 spawnPosition = new Vector3(transform.position.x, 0.055f, transform.position.z);
        ItemWorld.SpawnItemWorld(spawnPosition, item);
        Destroy(gameObject);
    }
}
