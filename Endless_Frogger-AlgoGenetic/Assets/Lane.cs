using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public int sens;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<Cube>().id = i; ;
    }

    public void InitLane(Vehicule vehiculePrefab)
    {
        sens = Random.Range(0, 2);

        gameObject.AddComponent<Spawner>();
        GetComponent<Spawner>().vehiculePrefab = vehiculePrefab;
        GetComponent<Spawner>().SetPath();
    }
}
