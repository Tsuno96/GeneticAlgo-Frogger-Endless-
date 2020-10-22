using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public int sens;
    void Start()
    {

    }

    public void InitLane(Vehicule vehiculePrefab)
    {
        sens = Random.Range(0, 2);

        gameObject.AddComponent<Spawner>();
        GetComponent<Spawner>().vehiculePrefab = vehiculePrefab;
        GetComponent<Spawner>().SetPath();
    }
}
