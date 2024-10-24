using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;
    [SerializeField] float SpawnXMax;
    [SerializeField] float SpawnY;
    [SerializeField] GameObject Bouncer;
    [SerializeField] GameObject Diver;
    [SerializeField] GameObject Mine;
    [SerializeField] float StartTimeBetweenWaves;
    [SerializeField] float TimeBetweenWavesMultiplier;
    [SerializeField] int SpawnsPerRound;
    [SerializeField] float RoundBreakTime;
    [SerializeField] NewRoundFlash Flasher;
    float TimeBetweenWaves;
    float WaveCooldown = 0;
    int RoundRemainingSpawns;
    int RoundCount;
    float RoundBreakTimer = 0;
    bool RoundBreak = true;
    public List<GameObject> ActiveEnemies = new List<GameObject>();


    List<Wave> PotentialWaves;

    bool LoadingScene = false;

    //Anv�nds ej just nu, men ni kan anv�nda de senare
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        TimeBetweenWaves = StartTimeBetweenWaves;

        PotentialWaves = new List<Wave>
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
        };
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();
    }

    private void Update()
    {
        /*
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!LoadingScene)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }

        if(RoundBreak)
        {
            RoundBreakTimer += Time.deltaTime;
            if(RoundBreakTimer > RoundBreakTime)
            {
                StartRound();
            }
        }
        else
        {
            if (RoundRemainingSpawns == 0 && ActiveEnemies.Count == 0)
            {
                EndRound();
            }
            else if (RoundRemainingSpawns > 0)
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
    /*
    private void NewGame()
    {

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        //invaders.ResetInvaders();
        //invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }
    */
    private void GameOver()
    {
        //invaders.gameObject.SetActive(false);
        if(!LoadingScene)
        {
            SceneManager.LoadScene("Death Screen");
        }
    }
    
    private void SetScore(int score)
    {
        
    }

    private void SetLives(int lives)
    {
       
    }

    public void OnPlayerKilled(Player player)
    {

        GameOver();
        //player.gameObject.SetActive(false);

    }
    /*
    public void OnInvaderKilled(Invader invader)
    {
        invader.gameObject.SetActive(false);

       

        if (invaders.GetInvaderCount() == 0)
        {
            NewRound();
        }
    }
    
    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        mysteryShip.gameObject.SetActive(false);
    }
    */
    public void OnBoundaryReached()
    {
        /*
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled(player);
        }
        */
        GameOver();
    }

    void SpawnRandomWave()
    {
        SpawnWave(PotentialWaves[Random.Range(0, PotentialWaves.Count)]);
    }

    void SpawnWave(Wave Wave)
    {
        int Mirror = 1; //If this is -1, the spawn will be flipped.
        if(Random.Range(0,2) == 0) { Mirror = -1; } //50% chance to flip.
        
        foreach (EnemySpawn Spawn in Wave.Spawns)
        {
            ActiveEnemies.Add(Instantiate(Spawn.Enemy, new Vector2(Spawn.Xposition * Mirror, SpawnY), Quaternion.identity));
        }
    }

    void EndRound()
    {
        RoundBreakTimer = 0;
        RoundBreak = true;
    }

    void StartRound()
    {
        RoundCount++;
        RoundBreak = false;
        Flasher.Flash();
        ClearGore();
        RoundRemainingSpawns = SpawnsPerRound;
        TimeBetweenWaves *= TimeBetweenWavesMultiplier;
    }

    void ClearGore()
    {
        DeathEffect[] Gores = FindObjectsByType<DeathEffect>(FindObjectsSortMode.None);
        foreach (DeathEffect Gore in Gores) 
        {
            Gore.Clear();
        }
    }
}

class Wave
{
    public List<EnemySpawn> Spawns;
}

class EnemySpawn
{
    public GameObject Enemy;
    public float Xposition = 0;
}
