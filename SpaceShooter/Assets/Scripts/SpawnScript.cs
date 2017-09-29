using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public float spawn_cd = 2f;
    public GameObject mob_level_1;
    public Transform[] spawnPoints;

    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", spawn_cd, spawn_cd);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Spawn()
    {
        
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        //Vector3 spawnpoint = GetComponent<Transform>.
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(mob_level_1, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
