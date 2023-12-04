using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeBehaviour : MonoBehaviour
{
    [Tooltip("how fast the snake moves ")]
    public float moveSpeed = 5f;

    [Tooltip("How fast the snake turns left or right")]
    public float steerSpeed = 180f;

    [Tooltip("How big we want the space between body parts")]
    public int gap = 10;

    [Tooltip("The speed at which body parts move")]
    public float bodySpeed = 5f;

    [Tooltip("THe body prefab game object")]
    public GameObject bodyPrefab;

    [Tooltip("a reference to the game manager object")]
    public GameManager gameManager;

    [Tooltip("Reference to the score text at top of screen")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Reference to the score text at bottom of screen")]
    public TextMeshProUGUI highscoreText;

    [Tooltip("Reference to the score text in the game over panel")]
    public TextMeshProUGUI finalScore;

    [Tooltip("Reference to the highscore text in game over panel")]
    public TextMeshProUGUI finalHighScore;

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionHistory = new List<Vector3>();

    private float score = 0;

    public float Score{
        get
        {
            return score;
        }
        set
        {
            score = value;

            //check if scoreText has been assigned
            if(scoreText == null && finalScore == null)
            {
                Debug.LogError("Score Text & final score is not set. Please go to Inspector and assing it");
                return;
            }
            //Update text to display score number
            scoreText.text = string.Format("{0:0}", score);
            finalScore.text = "Score: " + string.Format("{0:0}", score);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GrowSnake();
        //set the highscore value
        highscoreText.text = "Highscore: " + string.Format("{0:0}", PlayerPrefs.GetFloat("Highscore", 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        //Get the steer direction via user input
        float steerDirection = Input.GetAxis("Horizontal"); //used this before... 1 for left, -1 for right

        //Use the steer direction and perform a rotation of the snake
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);

        //store history of body parts
        positionHistory.Insert(0, transform.position);

        //Move the body parts
        int index = 1;
        foreach(var body in bodyParts)
        {
            Vector3 point = positionHistory[Mathf.Min(index * gap, positionHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point); //look same direction as rest of body
            body.SetActive(true);
            index++;
        }

        //If snake falls off map it is game over
        if(transform.position.y < -5)
        {
            ResetGame();
        }
                    
    }

    /// <summary>
    /// Adds a body part to the snake
    /// </summary>
    private void GrowSnake()
    {
        GameObject body = Instantiate(bodyPrefab);
        bodyParts.Add(body);
    }

    private void ResetGame()
    {
        var gameOverMenu = GetGameOverMenu();
        SetHighscore();
        gameOverMenu.SetActive(true);
    }

    private GameObject GetGameOverMenu()
    {
        var canvas = GameObject.Find("Canvas").transform;
        finalHighScore.text = "Highscore: " + string.Format("{0:0}", PlayerPrefs.GetFloat("Highscore", 0));
        return canvas.Find("GameOver Menu").gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //what to do when we hit an apple
        if(collision.gameObject.GetComponent<AppleBehaviour>())
        {
            Debug.Log("Snake hit apple");
            //Destroy apple
            Destroy(collision.gameObject);
            //Grow the snake by one body part
            GrowSnake();
            //Update our score value
            Score += 1;
            //remove an apple from the appleList
            gameManager.appleList.RemoveAt(gameManager.appleList.Count - 1);
        }

        //Debug.Log(collision.gameObject);
        //what to do if we hit our own body
        if(collision.gameObject.name == "Cube")
        {   
            //check if the front of our snake head
            //hit the Cube
            Debug.Log("we hit our own body");
            Destroy(this.gameObject);
            //also destroy all the body parts
            foreach(var body in bodyParts)
            {
                Destroy(body);
            }
            //Display Game Over screen
            //Find canvas
            ResetGame();
            //then restart the game...
            
            
        }
    }

    /// <summary>
    /// Checks our highscore and changes it if new score is higher
    /// than our current highscore
    /// </summary>
    public void SetHighscore()
    {
        float highscore = PlayerPrefs.GetFloat("Highscore", 0);
        if(Score > highscore)
        {
            PlayerPrefs.SetFloat("Highscore", Score);
        }
    }


}
