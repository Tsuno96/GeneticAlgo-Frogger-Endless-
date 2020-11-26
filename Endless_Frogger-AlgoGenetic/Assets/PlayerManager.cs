using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    Color c;
    public int generation;
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
        c = new Color(UnityEngine.Random.Range(0f, 1.0f), UnityEngine.Random.Range(0f, 1.0f), UnityEngine.Random.Range(0f, 1.0f), 0.4f);
        for (int i = 0; i < nPlayers; i++)
        {
            int z = Random.Range(0, -10); ;
            Player player = Instantiate(playerPrefab, new Vector3(0, 1, z), Quaternion.identity);
            player.SetRandomNN();
            player.currentCube = -z;
            player.currentLane = 0;
            player.name = "Player" + i;
            player.GetComponent<Renderer>().material.color = c;
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
        generation++;
    }

    void Repopulate()
    {
        int nNewAgent = nPlayers - nBestPlayersSelection;
        c = new Color(UnityEngine.Random.Range(0f, 1.0f), UnityEngine.Random.Range(0f, 1.0f), UnityEngine.Random.Range(0f, 1.0f), 0.4f);

        for (int i = 0; i< nNewAgent; i+=2)
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

            Player Child1;
            Player Child2;

            int z = Random.Range(0, -10); ;
            Child1 = Instantiate(playerPrefab, new Vector3(0, 1, z), Quaternion.identity);
            NeuralNetwork nnC1 = Child1.SetRandomNN();
            Child1.currentCube = -z;
            Child1.currentLane = 0;
            Child1.GetComponent<Renderer>().material.color = c;
            Child1.name = "Child1 " + Random.Range(0, 101) + "GEN" + generation;
            players.Add(Child1);


            z = Random.Range(0, -10); ;
            Child2 = Instantiate(playerPrefab, new Vector3(0, 1, z), Quaternion.identity);
            NeuralNetwork nnC2 = Child2.SetRandomNN();
            Child2.currentCube = -z;
            Child2.currentLane = 0;
            Child2.GetComponent<Renderer>().material.color = c;
            Child2.name = "Child2 " + Random.Range(0, 101) + "GEN" + generation;
            players.Add(Child2);

            NeuralNetwork nnPA = bestPlayers[iParentA].neuralNetwork;
            NeuralNetwork nnPB = bestPlayers[iParentB].neuralNetwork;

            for(int w = 0; w<nnC1.weights.Count;w++)
            {
                for (int x = 0; x < nnC1.weights[w].RowCount; x++)
                {

                    for (int y = 0; y < nnC1.weights[w].ColumnCount; y++)
                    {

                        if (Random.Range(0.0f, 1.0f) < mutateRate)
                        {
                            nnC1.weights[w][x,y] = nnPA.weights[w][x,y];
                        }

                        if (Random.Range(0.0f, 1.0f) < mutateRate)
                        {
                            nnC2.weights[w][x,y] = nnPB.weights[w][x,y];
                        }
                    }

                }

            }

            for (int w = 0; w < nnC1.biases.Count; w++)
            {
                if (Random.Range(0.0f, 1.0f) < mutateRate)
                {
                    nnC1.biases[w] = nnPB.biases[w];
                }

                if (Random.Range(0.0f, 1.0f) < mutateRate)
                {
                    nnC2.biases[w] = nnPA.biases[w];
                }
            }

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

    public void WatchBest()
    {
        foreach(Player bp in players)
        {
            if(!bestPlayers.Contains(bp))
            {
                bp.GetComponent<Renderer>().enabled = false;
            }

        }
    }
    public void WatchAll()
    {
        foreach (Player p in players)
        {
            p.GetComponent<Renderer>().enabled = true;
        }
    }

}

