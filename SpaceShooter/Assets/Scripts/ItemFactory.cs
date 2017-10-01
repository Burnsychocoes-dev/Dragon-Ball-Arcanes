using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour {

    public Transform senzu;
    public Transform boule1Etoile;
    public Transform boule2Etoile;
    public Transform boule3Etoile;
    public Transform boule4Etoile;
    public Transform boule5Etoile;
    public Transform boule6Etoile;
    public Transform boule7Etoile;
    public Transform capsuleEnergy;

    private Transform[] senzuPool;
    private Transform[] boule1EtoilePool;
    private Transform[] boule2EtoilePool;
    private Transform[] boule3EtoilePool;
    private Transform[] boule4EtoilePool;
    private Transform[] boule5EtoilePool;
    private Transform[] boule6EtoilePool;
    private Transform[] boule7EtoilePool;
    private Transform[] capsuleEnergyPool;

    public int senzuPoolLength;
    public int bouleEtoilePoolLength;
    public int capsuleEnergyPoolLength;
    // Use this for initialization
    void Start()
    {

        //initialisation de la pool

        senzuPool = new Transform[senzuPoolLength];
        boule1EtoilePool = new Transform[bouleEtoilePoolLength];
        /*boule2EtoilePool = new Transform[bouleEtoilePoolLength];
        boule3EtoilePool = new Transform[bouleEtoilePoolLength];
        boule4EtoilePool = new Transform[bouleEtoilePoolLength];
        boule5EtoilePool = new Transform[bouleEtoilePoolLength];
        boule6EtoilePool = new Transform[bouleEtoilePoolLength];
        boule7EtoilePool = new Transform[bouleEtoilePoolLength];*/
        capsuleEnergyPool = new Transform[capsuleEnergyPoolLength];



        initList(senzuPool,senzuPoolLength,Item.ItemName.senzu);
        initList(boule1EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule1Etoile);
        /*initList(boule2EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule2Etoile);
        initList(boule3EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule3Etoile);
        initList(boule4EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule4Etoile);
        initList(boule5EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule5Etoile);
        initList(boule6EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule6Etoile);
        initList(boule7EtoilePool, bouleEtoilePoolLength, Item.ItemName.boule7Etoile);*/
        initList(capsuleEnergyPool, capsuleEnergyPoolLength, Item.ItemName.capsuleEnergy);




    }

    // Update is called once per frame
    void Update()
    {

    }

    private void initList(Transform[] pool, int poolLength, Item.ItemName itemType)
    {
        for (int i = 0; i < poolLength; i++)
        {
            pool[i] = SpawnItem(itemType);
        }
    }

    //spawn un bullet dans la pool
    public Transform SpawnItem(Item.ItemName itemType)
    {
        Transform bullet = null;
        switch (itemType)
        {
            case Item.ItemName.senzu:
                bullet = Instantiate(senzu) as Transform;
                break;
            case Item.ItemName.boule1Etoile:
                bullet = Instantiate(boule1Etoile) as Transform;
                break;
            case Item.ItemName.boule2Etoile:
                bullet = Instantiate(boule2Etoile) as Transform;
                break;
            case Item.ItemName.boule3Etoile:
                bullet = Instantiate(boule3Etoile) as Transform;
                break;
            case Item.ItemName.boule4Etoile:
                bullet = Instantiate(boule4Etoile) as Transform;
                break;
            case Item.ItemName.boule5Etoile:
                bullet = Instantiate(boule5Etoile) as Transform;
                break;
            case Item.ItemName.boule6Etoile:
                bullet = Instantiate(boule6Etoile) as Transform;
                break;
            case Item.ItemName.boule7Etoile:
                bullet = Instantiate(boule7Etoile) as Transform;
                break;
            case Item.ItemName.capsuleEnergy:
                bullet = Instantiate(capsuleEnergy) as Transform;
                break;

        }
        bullet.position = new Vector3(-100, -100, -10);

        return bullet;
    }

    //permet d'obtenir un bullet de la pool
    public Transform GetItem(Item.ItemName itemType)
    {
        Transform bullet = null;
        switch (itemType)
        {

            
            case Item.ItemName.senzu:
                bullet = GetABulletFromAPool(senzuPool,senzuPoolLength);
                break;
            case Item.ItemName.boule1Etoile:
                bullet = GetABulletFromAPool(boule1EtoilePool,bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule2Etoile:
                bullet = GetABulletFromAPool(boule2EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule3Etoile:
                bullet = GetABulletFromAPool(boule3EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule4Etoile:
                bullet = GetABulletFromAPool(boule4EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule5Etoile:
                bullet = GetABulletFromAPool(boule5EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule6Etoile:
                bullet = GetABulletFromAPool(boule6EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule7Etoile:
                bullet = GetABulletFromAPool(boule7EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.capsuleEnergy:
                bullet = GetABulletFromAPool(capsuleEnergyPool, capsuleEnergyPoolLength);
                break;

        }
        if (bullet == null)
        {
            bullet = SpawnItem(itemType);
        }
        return bullet;
    }

    //permet de rendre un bullet dans la pool
    public void GiveBackItem(Item.ItemName mobType, Transform bullet)
    {
        bullet.position = new Vector3(-100, -100, -10);
        
        switch (mobType)
        {

            case Item.ItemName.senzu:
                PutBulletBackInAPool(bullet,senzuPool, senzuPoolLength);
                break;
            case Item.ItemName.boule1Etoile:
                PutBulletBackInAPool(bullet,boule1EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule2Etoile:
                PutBulletBackInAPool(bullet,boule2EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule3Etoile:
                PutBulletBackInAPool(bullet, boule3EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule4Etoile:
                PutBulletBackInAPool(bullet, boule4EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule5Etoile:
                PutBulletBackInAPool(bullet, boule5EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule6Etoile:
                PutBulletBackInAPool(bullet, boule6EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.boule7Etoile:
                PutBulletBackInAPool(bullet, boule7EtoilePool, bouleEtoilePoolLength);
                break;
            case Item.ItemName.capsuleEnergy:
                PutBulletBackInAPool(bullet, capsuleEnergyPool, capsuleEnergyPoolLength);
                break;
        }

    }

    //sort un bullet d'une pool
    private Transform GetABulletFromAPool(Transform[] bulletPool, int poolLength)
    {
        Transform bullet = null;
        for (int i = 0; i < poolLength; i++)
        {
            if (bulletPool[i] != null)
            {
                bullet = bulletPool[i];
                bulletPool[i] = null;
                break;
            }
        }
        return bullet;
    }

    //met un bullet dans une pool
    private void PutBulletBackInAPool(Transform bullet, Transform[] bulletPool, int poolLength)
    {
        for (int i = 0; i < poolLength; i++)
        {

            if (bulletPool[i] == null)
            {
                //Debug.Log(bulletPool[i]);
                bulletPool[i] = bullet;
                return;
            }
        }
        //Si on est ici, il n'y a plus de place
        Destroy(bullet.gameObject);
    }


}
