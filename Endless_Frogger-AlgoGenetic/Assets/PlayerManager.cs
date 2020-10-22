using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.currentCube = 9;
        player.currentLane = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
