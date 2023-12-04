using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioBehavior : MonoBehaviour
{
    public AudioSource mainMenuAudio;
    public AudioSource gameplayAudio;

    //ensure there is only one instance of the audiobehaviour
    private static AudioBehavior instance;

    private void Awake()
    {
        //If an instance already exists, destroy this one
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        //oterwise, set this instance as the singleton
        instance = this;

        DontDestroyOnLoad(gameObject);

        //Register the method to be called when a scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        //Unregister the method when the AudioBehaviour is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene name: " + scene.name);
        //check the name of the loaded scene
        if(scene.name == "MainMenu")
        {
            //Stop gameplay audio when returning to MainMenu
            if(gameplayAudio.isPlaying)
            {
                gameplayAudio.Stop();
            }
            //Start or resume MainMenu audio
            if(!mainMenuAudio.isPlaying)
            {
                mainMenuAudio.Play();
            }
        }
        else if (scene.name == "Gameplay")
        {
            //Stop MainMenu audio when entering Gameplay
            if(mainMenuAudio.isPlaying)
            {
                mainMenuAudio.Stop();
            }
            //Start or resume Gameplay audio
            if(!gameplayAudio.isPlaying)
            {
                Debug.Log("playing gameplay audio");
                gameplayAudio.Play();
            }
        }
        
    }
}
