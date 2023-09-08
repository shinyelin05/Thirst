using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    float currTime;
    public GameObject monsterPrefab;

    void Update()
    {
        currTime += Time.deltaTime;

        if (currTime > 5)
        {
            float newX = Random.Range(10f, 15f), newZ = Random.Range(60, 63);

            GameObject monster = Instantiate(monsterPrefab);

            monster.transform.position = new Vector3(newX, 0, newZ);

            currTime = 0;
        }
    }
}
