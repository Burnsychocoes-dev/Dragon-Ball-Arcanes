using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour {

    //public static BulletFactory Instance;
    public Transform playerBulletPrefab;
    public Transform burterBulletPrefab;
    public Transform friezaDefaultBulletPrefab;
    public Transform friezaMegaBulletPrefab;
    public Transform friezaUltiBulletPrefab;
    public Transform friezaRayonBulletPrefab;
    
    private Transform[] playerBulletPool;
    private Transform[] burterBulletPool;
    private Transform[] friezaDefaultBulletPool;
    private Transform[] friezaMegaBulletPool;
    private Transform[] friezaUltiBulletPool;
    private Transform[] friezaRayonBulletPool;

    public int playerBulletPoolLength = 50;
    public int burterBulletPoolLength = 50;
    public int friezaDefaultBulletPoolLength = 0; //10
    public int friezaMegaBulletPoolLength = 0;  //3
    public int friezaUltiBulletPoolLength = 0; //1
    public int friezaRayonBulletPoolLength = 0; //50 ?

    public enum BulletType
    {
        NONE,
        PLAYERBULLET,
        BURTERBULLET,
        FRIEZADEFAULTBULLET,
        FRIEZAMEGABULLET,
        FRIEZAULTIBULLET,
        FRIEZARAYONBULLET
    }
    
	// Use this for initialization
	void Start () {

        //initialisation de la pool
        playerBulletPool = new Transform[playerBulletPoolLength];
        burterBulletPool = new Transform[burterBulletPoolLength];
        friezaDefaultBulletPool = new Transform[friezaDefaultBulletPoolLength];
        friezaMegaBulletPool = new Transform[friezaMegaBulletPoolLength];
        friezaUltiBulletPool = new Transform[friezaUltiBulletPoolLength];
        friezaRayonBulletPool = new Transform[friezaRayonBulletPoolLength];

        initBulletList(playerBulletPool, playerBulletPoolLength, BulletType.PLAYERBULLET);
        initBulletList(burterBulletPool, burterBulletPoolLength, BulletType.BURTERBULLET);
        initBulletList(friezaDefaultBulletPool, friezaDefaultBulletPoolLength, BulletType.FRIEZADEFAULTBULLET);
        initBulletList(friezaMegaBulletPool, friezaMegaBulletPoolLength, BulletType.FRIEZAMEGABULLET);
        initBulletList(friezaUltiBulletPool, friezaUltiBulletPoolLength, BulletType.FRIEZAULTIBULLET);
        initBulletList(friezaRayonBulletPool, friezaRayonBulletPoolLength, BulletType.FRIEZARAYONBULLET);
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void initBulletList(Transform[] pool, int poolLength, BulletType bulletType)
    {
        for(int i=0; i < poolLength; i++)
        {
            pool[i] = SpawnBullet(bulletType);
        }
    }

    //spawn un bullet dans la pool
    public Transform SpawnBullet(BulletType bulletType)
    {
        Transform bullet = null;
        switch (bulletType)
        {
            case BulletType.PLAYERBULLET:
                bullet = Instantiate(playerBulletPrefab) as Transform;
                break;
            case BulletType.BURTERBULLET:
                bullet = Instantiate(burterBulletPrefab) as Transform;
                break;
            case BulletType.FRIEZADEFAULTBULLET:
                bullet = Instantiate(friezaDefaultBulletPrefab) as Transform;
                break;
            case BulletType.FRIEZAMEGABULLET:
                bullet = Instantiate(friezaMegaBulletPrefab) as Transform;
                break;
            case BulletType.FRIEZAULTIBULLET:
                bullet = Instantiate(friezaUltiBulletPrefab) as Transform;
                break;
            case BulletType.FRIEZARAYONBULLET:
                bullet = Instantiate(friezaRayonBulletPrefab) as Transform;
                break;
                
        }
        bullet.position = new Vector3(-100, -100, -10);
        bullet.gameObject.GetComponent<Renderer>().enabled = false;
        bullet.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        bullet.gameObject.GetComponent<MoveScript>().enabled = false;
        bullet.gameObject.GetComponent<ShotScript>().enabled = false;
        bullet.gameObject.GetComponent<HealthScript>().enabled = false;

        return bullet;
    } 

    //permet d'obtenir un bullet de la pool
    public Transform GetBullet(BulletType bulletType)
    {
        Transform bullet = null;
        switch (bulletType)
        {
            case BulletType.PLAYERBULLET:
                bullet = GetABulletFromAPool(playerBulletPool, playerBulletPoolLength);
                break;

            case BulletType.BURTERBULLET:
                bullet = GetABulletFromAPool(burterBulletPool, burterBulletPoolLength);
                break;
            case BulletType.FRIEZADEFAULTBULLET:
                bullet = GetABulletFromAPool(friezaDefaultBulletPool, friezaDefaultBulletPoolLength);
                break;
            case BulletType.FRIEZAMEGABULLET:
                bullet = GetABulletFromAPool(friezaMegaBulletPool, friezaMegaBulletPoolLength);
                break;
            case BulletType.FRIEZAULTIBULLET:
                bullet = GetABulletFromAPool(friezaUltiBulletPool, friezaUltiBulletPoolLength);
                break;
            case BulletType.FRIEZARAYONBULLET:
                bullet = GetABulletFromAPool(friezaRayonBulletPool, friezaRayonBulletPoolLength);
                break;
        }
        if (bullet == null)
        {
            bullet = SpawnBullet(bulletType);
        }
        return bullet;
    }

    //permet de rendre un bullet dans la pool
    public void GiveBackBullet(BulletType bulletType, Transform bullet)
    {
        bullet.position = new Vector3(-100, -100, -10);
        bullet.gameObject.GetComponent<Renderer>().enabled = false;
        bullet.gameObject.GetComponent<PolygonCollider2D>().enabled = false;        
        bullet.gameObject.GetComponent<MoveScript>().enabled = false;
        bullet.gameObject.GetComponent<ShotScript>().enabled = false;
        bullet.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        HealthScript hpScript = bullet.gameObject.GetComponent<HealthScript>();
        hpScript.hp = hpScript.GetMaxHp();
        bullet.gameObject.GetComponent<Animator>().SetBool("pool",true);
        switch (bulletType)
        {
            case BulletType.PLAYERBULLET:
                PutBulletBackInAPool(bullet, playerBulletPool, playerBulletPoolLength);
                break;
            case BulletType.BURTERBULLET:
                PutBulletBackInAPool(bullet, burterBulletPool, burterBulletPoolLength);
                break;
            case BulletType.FRIEZADEFAULTBULLET:
                PutBulletBackInAPool(bullet, friezaDefaultBulletPool, friezaDefaultBulletPoolLength);
                break;
            case BulletType.FRIEZAMEGABULLET:
                PutBulletBackInAPool(bullet, friezaMegaBulletPool, friezaMegaBulletPoolLength);
                break;
            case BulletType.FRIEZAULTIBULLET:
                PutBulletBackInAPool(bullet, friezaUltiBulletPool, friezaUltiBulletPoolLength);
                break;
            case BulletType.FRIEZARAYONBULLET:
                PutBulletBackInAPool(bullet, friezaRayonBulletPool, friezaRayonBulletPoolLength);
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
    private void PutBulletBackInAPool(Transform bullet, Transform[] bulletPool, int poolLength )
    {
        for(int i=0; i < poolLength; i++)
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
