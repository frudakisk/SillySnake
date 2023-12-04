using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayAudioBehaviour : MonoBehaviour
{
    //ensure there is only one instance of the audio
    private static GameplayAudioBehaviour instance = null;

    public static GameplayAudioBehaviour Instance
    {
        get { return instance;}
    }

    void Awake()
    {
        //also dont play when going to main menu
    if(instance != null && instance != this)
    {
        Destroy(this.gameObject);
        return;
    }
    else
    {
        instance = this;
    }
    
    DontDestroyOnLoad(transform.gameObject);
        
    }
}
