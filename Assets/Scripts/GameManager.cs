using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] float SpawnXMax;
    [SerializeField] float SpawnY;
    [SerializeField] GameObject Bouncer;
    [SerializeField] GameObject Diver;
    [SerializeField] GameObject Mine;
    [SerializeField] float StartTimeBetweenWaves;
    [SerializeField] float TimeBetweenWavesMultiplier;
    [SerializeField] int BaseSpawnsPerRound;
    [SerializeField] int SpawnsIncreasePerRound;
    [SerializeField] float RoundBreakTime;
    [SerializeField] NewRoundFlash Flasher;

    float TimeBetweenWaves;
    float WaveCooldown = 0;
    int RoundRemainingSpawns;
    int RoundCount;
    float RoundBreakTimer = 0;
    bool RoundBreak = true;
    public List<GameObject> ActiveEnemies = new List<GameObject>();

    List<Wave> PotentialWaves;//A list of all possible wave configurations.

    private void Awake()
    {
        if (Instance != null) //Ensures there is always only 1 GameManager, accessible through the static GameManager.Instance.
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        TimeBetweenWaves = StartTimeBetweenWaves;

        PotentialWaves = new List<Wave> //Fills up the PotentialWaves list
        {
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = -7 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 0 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 5 }
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = -4 },
                new EnemySpawn {Enemy = Bouncer, Xposition = -8 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 8 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 4 }
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = -5 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 0 },
                new EnemySpawn {Enemy = Diver, Xposition = 2 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 4 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Diver, Xposition = -5 },
                new EnemySpawn {Enemy = Diver, Xposition = 3 },

            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = -5 },
                new EnemySpawn {Enemy = Diver, Xposition = 0 },
                new EnemySpawn {Enemy = Diver, Xposition = 4 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = -7 },
                new EnemySpawn {Enemy = Mine, Xposition = 1 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 4 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Mine, Xposition = -2 },
                new EnemySpawn {Enemy = Diver, Xposition = 3 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Mine, Xposition = 0 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Mine, Xposition = -7 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 4 },
                new EnemySpawn {Enemy = Bouncer, Xposition = -2 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Mine, Xposition = -6 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = 9 },
                new EnemySpawn {Enemy = Bouncer, Xposition = -9 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Bouncer, Xposition = 4 },
                new EnemySpawn {Enemy = Bouncer, Xposition = -1 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 9 },
            }},
        new Wave{Spawns = new List<EnemySpawn>
            {
                new EnemySpawn {Enemy = Diver, Xposition = -4 },
                new EnemySpawn {Enemy = Diver, Xposition = 0 },
                new EnemySpawn {Enemy = Bouncer, Xposition = 7 },
            }},
        };
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Goes to the main menu when the player hits esc
        {
            SceneManager.LoadScene("Main Menu");
        }

        if(RoundBreak) //If we are in the break between rounds
        {
            RoundBreakTimer += Time.deltaTime;
            if(RoundBreakTimer > RoundBreakTime) //If break time's over
            {
                StartRound();
            }
        }
        else
        {
            if (RoundRemainingSpawns == 0 && ActiveEnemies.Count == 0) //If the round has no more spawns and all enemies are dead
            {
                EndRound();
            }
            else if (RoundRemainingSpawns > 0) //If the round has more spawns.
            {
                WaveCooldown -= Time.deltaTime;
                if (WaveCooldown <= 0)
                {
                    WaveCooldown += TimeBetweenWaves;
                    SpawnRandomWave();
                    RoundRemainingSpawns--;
                }
            }
        }
    }
    /// <summary>
    /// Ends the game.
    /// </summary>
    public void GameOver()
    {
        SceneManager.LoadScene("Death Screen");
    }
    /// <summary>
    /// Spawns a wave from the list.
    /// </summary>
    void SpawnRandomWave()
    {
        SpawnWave(PotentialWaves[Random.Range(0, PotentialWaves.Count)]);
    }

    /// <summary>
    /// Places down a wave
    /// </summary>
    /// <param name="Wave"></param>
    void SpawnWave(Wave Wave)
    {
        int Mirror = 1; //If this is -1, the spawn will be flipped.
        if(Random.Range(0,2) == 0) { Mirror = -1; } //50% chance to flip.
        
        foreach (EnemySpawn Spawn in Wave.Spawns)
        {
            ActiveEnemies.Add(Instantiate(Spawn.Enemy, new Vector2(Spawn.Xposition * Mirror, SpawnY), Quaternion.identity));
        }
    }

    /// <summary>
    /// Ends the current round
    /// </summary>
    void EndRound()
    {
        RoundBreakTimer = 0;
        RoundBreak = true;
    }

    /// <summary>
    /// Starts a new round
    /// </summary>
    void StartRound()
    {
        RoundCount++;
        RoundBreak = false;
        Flasher.Flash();
        ClearGore();
        RoundRemainingSpawns = BaseSpawnsPerRound + (SpawnsIncreasePerRound*RoundCount);
        TimeBetweenWaves *= TimeBetweenWavesMultiplier;
    }

    /// <summary>
    /// Clears all gores.
    /// </summary>
    void ClearGore()
    {
        DeathEffect[] Gores = FindObjectsByType<DeathEffect>(FindObjectsSortMode.None);
        foreach (DeathEffect Gore in Gores) 
        {
            Gore.Clear();
        }
    }
}

/// <summary>
/// Contains a list of spawns
/// </summary>
class Wave
{
    public List<EnemySpawn> Spawns;
}

/// <summary>
/// Contains an enemy to spawn and a place to spawn it
/// </summary>
class EnemySpawn 
{
    public GameObject Enemy;
    public float Xposition = 0;
}
