using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public float spawnBurterCd = 2f;
    public MobFactory.MobType burter;
    public float spawnBurterMovingCd = 5f;
    public MobFactory.MobType burterMoving;
    public float spawnJayceCd = 7f;
    public MobFactory.MobType jayce;
    public float spawnFriezaCd = 60f;
    public MobFactory.MobType frieza;
    public float senzuCd = 31f;
    public Item.ItemName senzu;
    public float boule1EtoileCd = 13f;
    public Item.ItemName boule1Etoile;
    /* public float boule2EtoileCd = 15f;
     public Item.ItemName boule2Etoile;
     public float boule3EtoileCd = 15f;
     public Item.ItemName boule3Etoile;
     public float boule4EtoileCd = 15f;
     public Item.ItemName boule4Etoile;
     public float boule5EtoileCd = 15f;
     public Item.ItemName boule5Etoile;
     public float boule6EtoileCd = 15f;
     public Item.ItemName boule6Etoile;
     public float boule7EtoileCd = 15f;
     public Item.ItemName boule7Etoile;*/
    public float capsuleCd = 43f;
    public Item.ItemName capsule;

    public Transform[] spawnPoints;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnBurter", spawnBurterCd, spawnBurterCd);
        InvokeRepeating("SpawnBurterMoving", spawnBurterMovingCd, spawnBurterMovingCd);
        InvokeRepeating("SpawnJayce", spawnJayceCd, spawnJayceCd);
        InvokeRepeating("SpawnFrieza", spawnFriezaCd, spawnFriezaCd);
        InvokeRepeating("SpawnSenzu", senzuCd, senzuCd);
        InvokeRepeating("SpawnBoule1Etoile", boule1EtoileCd, boule1EtoileCd);
        InvokeRepeating("SpawnCapsule", capsuleCd, capsuleCd);
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
        spawnMob(burter, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnBurterMoving()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int speedY = 0;
        while(speedY == 0)
        {
            speedY = Random.Range(-4, 4);
        }
        spawnMob(burterMoving, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation).GetComponent<MoveScript>().speed.y = speedY;
    }

    void SpawnJayce()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        spawnMob(jayce, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnFrieza()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        spawnMob(frieza, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnSenzu()
    {
        SpawnItem(senzu);
    }

    void SpawnBoule1Etoile()
    {
        SpawnItem(boule1Etoile);
    }

    /*
     void SpawnBoule2Etoile()
    {
        SpawnItem(boule2Etoile);
    }
    void SpawnBoule3Etoile()
    {
        SpawnItem(boule3Etoile);
    }
    void SpawnBoule4Etoile()
    {
        SpawnItem(boule4Etoile);
    }
    void SpawnBoule5Etoile()
    {
        SpawnItem(boule5Etoile);
    }
    void SpawnBoule6Etoile()
    {
        SpawnItem(boule6Etoile);
    }
    void SpawnBoule7Etoile()
    {
        SpawnItem(boule7Etoile);
    }
    */

    void SpawnCapsule()
    {
        SpawnItem(capsule);
    }

    private Transform spawnMob(MobFactory.MobType mobType, Vector3 position, Quaternion rotation)
    {
        Transform shotTransform = null;
        shotTransform = GameObject.Find("Scripts").GetComponent<MobFactory>().GetMob(mobType);
        // Position
        shotTransform.position = position;
        //Debug.Log(shotTransform.position);
        shotTransform.rotation = rotation;    
        shotTransform.gameObject.GetComponent<Animator>().SetBool("pool", false);

        return shotTransform;
    }

    private void SpawnItem(Item.ItemName itemType)
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Transform item = GameObject.Find("Scripts").GetComponent<ItemFactory>().GetItem(itemType);
        // Position
        item.position = spawnPoints[spawnPointIndex].position;
        //Debug.Log(shotTransform.position);
        item.rotation = spawnPoints[spawnPointIndex].rotation;
    }

}
