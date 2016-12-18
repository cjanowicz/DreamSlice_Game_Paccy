using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject textObject;
    Text scoreText;
    public GameObject dotContainer;
    public const int dotScore = 10;
    private int totalScore = 0;
    private int dotsRemaining = 0;

    public enum State { Game, Dead, Menu, Complete };
    public State myState = State.Game;
    public enum GhostState { Chase, Avoid, SpreadOut };
    public GhostState m_ghostState = GhostState.Chase;

    public static GameManager instance = null;

    public Transform mazeContainer;

    private float m_waveTimer = 0.0f;
    public float m_waveLengthTime = 30f;

    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        /*
        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();*/

        //scoreText = textObject.GetComponent<Text>();

        /*for(int i = 0; i < dotContainer.transform.childCount; i++) {
            if(dotContainer.transform.GetChild(i).gameObject.activeInHierarchy == true) {
                dotsRemaining++;
            }
        }

        foreach (Collider c in mazeContainer.gameObject.GetComponentsInChildren<Collider>()) {
            c.enabled = false;
        }
       */

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {

        m_waveTimer += Time.deltaTime;

        if (m_waveTimer >= m_waveLengthTime) {
            /// mess with ghost State
            if (m_ghostState == GhostState.Chase)
                m_ghostState = GhostState.SpreadOut;
            else if (m_ghostState == GhostState.SpreadOut)
                m_ghostState = GhostState.Chase;

            m_waveTimer = 0f;
        }
    }

    void EatDot() {
        totalScore += dotScore;
        scoreText.text = totalScore.ToString();
        dotsRemaining--;
        if (dotsRemaining <= 0) {
            Debug.Log("All Dots Eaten");
            ///level complete!
            myState = State.Complete;
        }
    }
    void PlayerDead() {
        myState = State.Dead;
    }

    /*
     public void SetAllCollidersStatus (bool active) {
     foreach(Collider c in GetComponents<Collider> ()) {
         c.enabled = active
     }
 }

    OR 

    public void DisableBearColliders()
 {
     foreach (Collider c in GetComponentInChildren<Collider>())
     {
         c.enabled = false;
     }
 }
 */
}
