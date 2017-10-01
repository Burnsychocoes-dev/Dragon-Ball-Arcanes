using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : WeaponScript {

    public bool firstWeapon = false;
    public float shootDuration = 0f;
    private float duration = 0f;
    public GameObject[] next = null;    
    
    

    void Start()
    {

        shootCooldown = 0f;
        duration = shootDuration;
        if (!firstWeapon)
        {
            gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        if( duration <= 0) // on a finit d'attaquer -> on relance la duration et on désactive cette arme
        {
            duration = shootDuration;
            shootCooldown = 0f;
            for(int i=0; i< next.Length; i++)
            {
                next[i].SetActive(true);
            }
           // GetComponentInParent<Animator>().SetBool(animatorParameter, false);
            gameObject.SetActive(false);
        }


    }

    //--------------------------------
    // 3 - Tirer depuis un autre script
    //--------------------------------

    /// <summary>
    /// Création d'un projectile si possible
    /// </summary>
    public override void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;
            //Ancienne version
            // Création d'un objet copie du prefab
            //var shotTransform = Instantiate(shotPrefab) as Transform;

            //nouvelle version
            Transform shotTransform = popBullet(bulletType);
            //Debug.Log("bullet poped");


            //Audio et animation
            AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
           

            // Propriétés du script            
            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();            
            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;

            }


            // On saisit la direction pour le mouvement
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null && !move.characterLock && !move.characterLockInit)
            {

                move.direction = this.transform.right; // ici la droite sera le devant de notre objet

            }
        }
    }

    /// <summary>
    /// L'arme est chargée ?
    /// </summary>s
    public override bool CanAttack
    {

        get
        {
            //Debug.Log("je shoot en boss");
            return (shootCooldown <= 0f && duration >0);
        }
    }
}
