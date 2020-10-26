using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerPrefab;

    public static PlayerManager instance = null;

    public int nPlayers;

    public int maxSteps;

    public int nDeadPlayers;

    public int nBestPlayersSelection;

    public List<Player> players;
    public List<Player> bestPlayers;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            if(instance != this)
                Destroy(gameObject);
    }
    void Start()
    {
        CreateGeneration();
    }

    // Update is called once per frame
    void Update()
    {
        if (nDeadPlayers >= nPlayers)
        {
            Debug.Log("End");
            PickBestGen();
        }


    }

    void CreateGeneration()
    {
        nDeadPlayers = 0;
        players = new List<Player>();

        for (int i = 0; i < nPlayers; i++)
        {
            int z = Random.Range(0, -10); ;
            Player player = Instantiate(playerPrefab, new Vector3(0, 1, z), Quaternion.identity);
            player.currentCube = -z;
            player.currentLane = 0;
            players.Add(player);
        }
    }

    void PickBestGen()
    {
        bestPlayers = players;
        players.Sort();
        

    }


}
