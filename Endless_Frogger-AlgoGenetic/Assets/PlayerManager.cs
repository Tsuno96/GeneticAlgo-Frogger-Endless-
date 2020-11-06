using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Player playerPrefab;

    public static PlayerManager instance = null;

    public float mutateRate;

    public int nPlayers;

    public int maxSteps;

    public int nDeadPlayers;

    public int nBestPlayersSelection;

    public List<Player> players;
    List<Player> bestPlayers;
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
            player.SetRandomNN();
            player.currentCube = -z;
            player.currentLane = 0;
            player.name = "Player" + i;
            players.Add(player);
        }
    }


    void NextGeneration()
    {
        nDeadPlayers = 0;
        players = new List<Player>();
        foreach(Player p in bestPlayers)
        {
            p.ResetPlayer();
            players.Add(p);
        }
        Repopulate();
    }

    void Repopulate()
    {
        int nNewAgent = nPlayers - nBestPlayersSelection;

        for(int i = 0; i< nNewAgent; i+=2)
        {
            Crossover();
        }
    }

    void Crossover()
    {
        if (bestPlayers.Count > 1)
        {
            int iParentA = Random.Range(0, bestPlayers.Count);
            int iParentB;
            do
            {
                iParentB = Random.Range(0, bestPlayers.Count);
            }
            while (iParentA == iParentB);

            NeuralNetwork nnC1 = new NeuralNetwork();
            NeuralNetwork nnC2 = new NeuralNetwork();
            nnC1.Initialise(2, 4);
            nnC2.Initialise(2, 4);

            NeuralNetwork nnPA = bestPlayers[iParentA].neuralNetwork;
            NeuralNetwork nnPB = bestPlayers[iParentB].neuralNetwork;

            if (Random.Range(0.0f, 1.0f) < mutateRate)
            {
                nnC1.weights = nnPA.weights;
            }
            if (Random.Range(0.0f, 1.0f) < mutateRate)
            {
                nnC1.biases = nnPB.biases;
            }
            if (Random.Range(0.0f, 1.0f) < mutateRate)
            {
                nnC2.weights = nnPB.weights;
            }
            if (Random.Range(0.0f, 1.0f) < mutateRate)
            {
                nnC2.biases = nnPA.biases;
            }

            int z = Random.Range(0, -10); ;
            Player Child1 = Instantiate(playerPrefab, new Vector3(0, 1, z), Quaternion.identity);
            Child1.SetNN(nnC1);
            Child1.currentCube = -z;
            Child1.currentLane = 0;
            Child1.name = "Child1 " + Random.Range(0, 101);
            players.Add(Child1);

            z = Random.Range(0, -10); ;
            Player Child2 = Instantiate(playerPrefab, new Vector3(0, 1, z), Quaternion.identity);
            Child2.SetNN(nnC2);
            Child2.currentCube = -z;
            Child2.currentLane = 0;
            Child2.name = "Child2 " +Random.Range(0,101);
            players.Add(Child2);
        }
        else
        {
            Debug.LogError("PAS ASSEZ DE PARENTS");
        }
    }

    void PickBestGen()
    {
        players.Sort();
        bestPlayers = players.Take(nBestPlayersSelection).ToList();
        foreach(Player p in players.Skip(nBestPlayersSelection).ToList())
        {
            Destroy(p.gameObject);
        }

        NextGeneration();

    }


}
