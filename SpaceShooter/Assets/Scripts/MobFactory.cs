using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobFactory : MonoBehaviour {

    
    public Transform burterPrefab;
    public Transform burterMovingPrefab;
    public Transform jaycePrefab;
    public Transform friezaPrefab;


    private Transform[] burterPool;
    private Transform[] burterMovingPool;
    private Transform[] jaycePool;
    private Transform[] friezaPool;
    

    public int burterPoolLength = 5;
    public int burterMovingPoolLength = 3;
    public int jaycePoolLength = 1;
    public int friezaPoolLength = 1;


    public enum MobType
    {
        NONE,
        BURTER,
        BURTERMOVING,
        JAYCE,
        FRIEZA
    }

    // Use this for initialization
    void Start()
    {

        //initialisation de la pool
        
        burterPool = new Transform[burterPoolLength];
        burterMovingPool = new Transform[burterMovingPoolLength];
        jaycePool = new Transform[jaycePoolLength];
        friezaPool = new Transform[friezaPoolLength];
        

        
        initList(burterPool, burterPoolLength, MobType.BURTER);
        initList(burterMovingPool, burterMovingPoolLength, MobType.BURTERMOVING);
        initList(jaycePool, jaycePoolLength, MobType.JAYCE);
        initList(friezaPool, friezaPoolLength, MobType.FRIEZA);
        


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void initList(Transform[] pool, int poolLength, MobType mobType)
    {
        for (int i = 0; i < poolLength; i++)
        {
            pool[i] = SpawnMob(mobType);
        }
    }

    //spawn un bullet dans la pool
    public Transform SpawnMob(MobType mobType)
    {
        Transform bullet = null;
        switch (mobType)
        {
            case MobType.BURTER:
                bullet = Instantiate(burterPrefab) as Transform;
                break;
            case MobType.BURTERMOVING:
                bullet = Instantiate(burterMovingPrefab) as Transform;
                break;
            case MobType.JAYCE:
                bullet = Instantiate(jaycePrefab) as Transform;
                break;
            case MobType.FRIEZA:
                bullet = Instantiate(friezaPrefab) as Transform;
                break;

        }
        bullet.position = new Vector3(-100, -100, -10);
        /*bullet.gameObject.GetComponent<Renderer>().enabled = false;
        bullet.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        bullet.gameObject.GetComponent<MoveScript>().enabled = false;
        bullet.gameObject.GetComponent<ShotScript>().enabled = false;
        bullet.gameObject.GetComponent<HealthScript>().enabled = false;*/

        return bullet;
    }

    //permet d'obtenir un bullet de la pool
    public Transform GetMob(MobType mobType)
    {
        Transform bullet = null;
        switch (mobType)
        {

            case MobType.BURTER:
                bullet = GetABulletFromAPool(burterPool, burterPoolLength);
                break;
            case MobType.BURTERMOVING:
                bullet = GetABulletFromAPool(burterMovingPool, burterMovingPoolLength);
                break;
            case MobType.JAYCE:
                bullet = GetABulletFromAPool(jaycePool, jaycePoolLength);
                break;
            case MobType.FRIEZA:
                bullet = GetABulletFromAPool(friezaPool, friezaPoolLength);
                break;

        }
        if (bullet == null)
        {
            bullet = SpawnMob(mobType);
        }
        return bullet;
    }

    //permet de rendre un bullet dans la pool
    public void GiveBackMob(MobType mobType, Transform bullet)
    {
        bullet.position = new Vector3(-100, -100, -10);
        /*bullet.gameObject.GetComponent<Renderer>().enabled = false;
        bullet.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        bullet.gameObject.GetComponent<MoveScript>().enabled = false;
        bullet.gameObject.GetComponent<ShotScript>().enabled = false;*/
        bullet.gameObject.GetComponent<EnemyScript>().unSpawn();
        bullet.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        HealthScript hpScript = bullet.gameObject.GetComponent<HealthScript>();
        hpScript.hp = hpScript.GetMaxHp();
        bullet.gameObject.GetComponent<Animator>().SetBool("pool", true);
        switch (mobType)
        {
            
            case MobType.BURTER:
                PutBulletBackInAPool(bullet, burterPool, burterPoolLength);
                break;
            case MobType.BURTERMOVING:
                PutBulletBackInAPool(bullet, burterMovingPool, burterMovingPoolLength);
                break;
            case MobType.JAYCE:
                PutBulletBackInAPool(bullet, jaycePool, jaycePoolLength);
                break;
            case MobType.FRIEZA:
                PutBulletBackInAPool(bullet, friezaPool, friezaPoolLength);
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
