using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public float spawnBurterCd = 2f;
    public GameObject burter;
    public float spawnBurterMovingCd = 5f;
    public GameObject burterMoving;
    public float spawnJayceCd = 7f;
    public GameObject jayce;

    public Transform[] spawnPoints;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnBurter", spawnBurterCd, spawnBurterCd);
        InvokeRepeating("SpawnBurterMoving", spawnBurterMovingCd, spawnBurterMovingCd);
        InvokeRepeating("SpawnJayce", spawnJayceCd, spawnJayceCd);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnBurter()
    {
        
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        //Vector3 spawnpoint = GetComponent<Transform>.
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(burter, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnBurterMoving()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int speedY = 0;
        while(speedY == 0)
        {
            speedY = Random.Range(-4, 4);
        }
        Instantiate(burterMoving, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation).GetComponent<MoveScript>().speed.y = speedY;
    }

    void SpawnJayce()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(jayce, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
