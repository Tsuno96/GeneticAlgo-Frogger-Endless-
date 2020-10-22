using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<Cube> lstPath;
    public Vehicule vehiculePrefab;
    private void Update()
    {
        
    }

    IEnumerator SpawnVehicule()
    {

        yield return new WaitForSeconds(1);
    }



}
