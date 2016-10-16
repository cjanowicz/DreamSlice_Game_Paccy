using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject textObject;
    Text scoreText;
    public GameObject dotContainer;
    public int dotScore = 10;
    private int totalScore = 0;
    


    public static GameManager instance = null;

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


        scoreText = textObject.GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void EatDot() {
        totalScore += dotScore;
        scoreText.text = totalScore.ToString();
        if(dotContainer.transform.childCount == 0) {
            Debug.Log("All Dots Eaten");
        }
    }
}
