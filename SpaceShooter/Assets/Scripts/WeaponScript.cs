using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------

	/// <summary>
	/// Prefab du projectile
	/// </summary>
	//public Transform shotPrefab;    
    [SerializeField]
    protected BulletFactory.BulletType bulletType;

	/// <summary>
	/// Temps de rechargement entre deux tirs
	/// </summary>
	public float shootingRate = 0.25f;

	//--------------------------------
	// 2 - Rechargement
	//--------------------------------

	protected float shootCooldown;
	public bool isEnemy = true;
    public int mana_cost = 0;
    public string animatorParameter = ""; //animation à faire
    public AudioClip soundToPlay = null; //son à faire

    // Multiplicateur de dommage permettant de gérer des modes super sayen ou autre
    private int damageMultiplicator = 1;


    void Start()
	{
		shootCooldown = 0f;
	}

	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
		
	}

	//--------------------------------
	// 3 - Tirer depuis un autre script
	//--------------------------------

	/// <summary>
	/// Création d'un projectile si possible
	/// </summary>
	public virtual void Attack(bool isEnemy)
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
            
			

            // Propriétés du script
            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            SoundEffectsHelper.Instance.MakePlayerShotSound();
            if (shot != null)
			{
				shot.isEnemyShot = isEnemy;               
			}

            // On multiplie les dégats du shoot par le damage multiplicateur
            shot.damage *= damageMultiplicator;

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
	/// </summary>
	public virtual bool CanAttack
	{
        
        get{
			return shootCooldown <= 0f;
		}
	}

    protected Transform popBullet(BulletFactory.BulletType bulletType)
    {
        Transform shotTransform=null;
        shotTransform = GameObject.Find("Scripts").GetComponent<BulletFactory>().GetBullet(bulletType);
        // Position
        shotTransform.position = transform.position;
        //Debug.Log(shotTransform.position);
        shotTransform.rotation = transform.rotation;
        //components du shot
        shotTransform.gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        shotTransform.gameObject.GetComponent<Renderer>().enabled = true;
        shotTransform.gameObject.GetComponent<MoveScript>().enabled = true;
        shotTransform.gameObject.GetComponent<ShotScript>().enabled = true;
        shotTransform.gameObject.GetComponent<HealthScript>().enabled = true;
        shotTransform.gameObject.GetComponent<Animator>().SetBool("pool", false);
        //Si on doit viser, on calcule les coordonées et on attend l'animation si on est un projectile
        shotTransform.gameObject.GetComponent<MoveScript>().CalculDirectionForHeadHunter();


        return shotTransform;
    }

    public void SetDamageMultiplicator(int multiplicator)
    {
        damageMultiplicator = multiplicator;
    }

    public int GetDamageMultiplicator()
    {
        return damageMultiplicator;
    }
}
