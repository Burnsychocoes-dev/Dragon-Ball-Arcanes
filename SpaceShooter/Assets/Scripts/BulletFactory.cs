using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour {

    //public static BulletFactory Instance;
    public Transform playerBulletPrefab;
    public Transform burterBulletPrefab;
    
    private Transform[] playerBulletPool;
    private Transform[] burterBulletPool;

    public int playerBulletPoolLength = 150;
    public int burterBulletPoolLength = 150;

    public enum BulletType
    {
        NONE,
        PLAYERBULLET,
        BURTERBULLET
    }
    
	// Use this for initialization
	void Start () {

        //initialisation de la pool
        playerBulletPool = new Transform[playerBulletPoolLength];
        burterBulletPool = new Transform[burterBulletPoolLength];

        for(int i=0; i<playerBulletPoolLength; i++)
        {
            playerBulletPool[i] = SpawnBullet(BulletType.PLAYERBULLET);
        }
        for(int i=0; i < burterBulletPoolLength; i++)
        {
            burterBulletPool[i] = SpawnBullet(BulletType.BURTERBULLET);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
                
        }
        bullet.position = new Vector3(-100, -100, -10);
        bullet.gameObject.GetComponent<Renderer>().enabled = false;
        bullet.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        bullet.gameObject.GetComponent<MoveScript>().enabled = false;
        bullet.gameObject.GetComponent<ShotScript>().enabled = false;
        bullet.gameObject.GetComponent<HealthScript>().enabled = false;

        return bullet;
    } 

    public Transform GetBullet(BulletType bulletType)
    {
        Transform bullet = null;
        switch (bulletType)
        {
            case BulletType.PLAYERBULLET:
                bullet = GetABulletFromAPool(playerBulletPool, playerBulletPoolLength);
                if (bullet == null)
                {
                    bullet = SpawnBullet(bulletType);
                }

                break;

            case BulletType.BURTERBULLET:
                bullet = GetABulletFromAPool(burterBulletPool, burterBulletPoolLength);
                if (bullet == null)
                {
                    bullet = SpawnBullet(bulletType);
                }
                break;
        }
        return bullet;
    }

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

        }

    }

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
    private void PutBulletBackInAPool(Transform bullet, Transform[] bulletPool, int poolLength )
    {
        for(int i=0; i < poolLength; i++)
        {

            if (bulletPool[i] == null)
            {
                Debug.Log(bulletPool[i]);
                bulletPool[i] = bullet;
                return;
            }
        }
        //Si on est ici, il n'y a plus de place
        Destroy(bullet.gameObject);
    }
    
}
