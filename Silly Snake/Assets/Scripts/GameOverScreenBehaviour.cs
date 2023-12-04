using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreenBehaviour : MonoBehaviour
{

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        //Stop game play audio
        SceneManager.LoadScene("MainMenu");
    }

}
