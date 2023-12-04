using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("A reference to the ground game object")]
    public Transform ground;

    [Tooltip("A reference to the apple prefab")]
    public GameObject applePrefab;

    public List<GameObject> appleList = new List<GameObject>();

    private int numOfApples = 5;
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        while(i < numOfApples)
        {
            SpawnApple();
            i++;
        }
        Debug.Log($"Starting Apple count {appleList.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        if(appleList.Count < numOfApples)
        {
            Debug.Log("apple count = " + appleList.Count);
            SpawnApple();
        }
    }

    /// <summary>
    /// Selects a random local position on the game object
    /// Eventually make sure we don't spawn an apple in the same
    /// spot or relatively near any current apple location
    /// </summary>
    /// <param name="ground">The ground game object</param>
    /// <returns>a vector3 that is a random position </returns>
    public Vector3 randomLocalSpawn(Transform ground)
    {
        //Get the max scale something can spawn at
        float maxXScale = ground.localScale.x;
        float maxZScale = ground.localScale.z;

        //need to return a position based on the scale
        //y should always be 0... just need to pick
        //x and z bw (-15, 15)
        float xScaleSplit = maxXScale / 2;
        float zScaleSplit = maxZScale / 2;

        float negativeXSplit = -xScaleSplit;
        float negativeZSplit = -zScaleSplit;

        //pick a random number between sScaleSplit
        //and negativeXSplit
        float randomX = Random.Range(negativeXSplit,xScaleSplit);
        float randomZ = Random.Range(negativeZSplit, zScaleSplit);

        Vector3 spawnLocation = new Vector3(randomX, 0, randomZ);
        return spawnLocation;
    }

    /// <summary>
    /// Spawns an apple at a random location on map
    /// </summary>
    public void SpawnApple()
    {
        
        GameObject apple = Instantiate(applePrefab);
        //set the position of the apple
        Vector3 appleLocation = randomLocalSpawn(ground);
        apple.transform.position = appleLocation;
        appleList.Add(apple);
    }
}
