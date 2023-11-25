using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public List<GameObject> Checkpoints = new List<GameObject>();
    [HideInInspector] public GameObject lastCheckpoint;
    [HideInInspector] public Scene gameScene; 
    [HideInInspector] public bool playerMoving = true;

    public GameObject Player;
    public GameObject vfxPoof;
    public GameObject startPosition;
    public bool playerFlach = false;

    [SerializeField] float restartDelay = 2f;

    Vector3 playerCheckPointPosition; 

    bool gameIstZuEnde;
    bool levelIstGewonnen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        gameScene = SceneManager.GetActiveScene();

        Player.transform.position = startPosition.transform.position; 

        //Checkpoints for Respawn 
        Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
        playerCheckPointPosition = Player.transform.position; 
        lastCheckpoint = Player;
        lastCheckpoint.transform.position = playerCheckPointPosition;
        
        //rotatingObjectList = GameObject.FindGameObjectsWithTag("Drehplatte").ToList();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void GameOver()
    {
        // ich habe das game verloren
        if (gameIstZuEnde == false)
        {
            gameIstZuEnde = true;
            Debug.Log("GAME OVER");
            Invoke("Restart", restartDelay); // delay von 2sec für restart
        }
    }

    // wenn ich  winn position erreiche, habe ich das game gewonnen
    // maybe colision detection mit colider
    public void LevelGewonnen()
    {
        if (levelIstGewonnen == false)
        {
            levelIstGewonnen = true;
            Debug.Log("Ich habe das level gemeistert");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // loading scene LEVEL II
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // aktuelle Scene wird Neu geladen 
        //Jedes Scene könnte könnte eine individuelle spawnZone haben, auf die der Player mit dieser Methode gesetzt wird. 
    }
}