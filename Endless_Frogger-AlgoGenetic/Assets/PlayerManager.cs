using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        int z = Random.Range(0, -10); ;
        Player player = Instantiate(playerPrefab, new Vector3(0,1,z), Quaternion.identity);
        player.currentCube = -z;
        player.currentLane = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
