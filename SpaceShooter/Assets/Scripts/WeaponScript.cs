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
	public Transform shotPrefab;

	/// <summary>
	/// Temps de rechargement entre deux tirs
	/// </summary>
	public float shootingRate = 0.25f;

	//--------------------------------
	// 2 - Rechargement
	//--------------------------------

	private float shootCooldown;
	public bool isEnemy = true;
    public int mana_cost = 0;

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
	public void Attack(bool isEnemy)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;

			// Création d'un objet copie du prefab
			var shotTransform = Instantiate(shotPrefab) as Transform;

			// Position
			shotTransform.position = transform.position;
			shotTransform.rotation = transform.rotation;
			// Propriétés du script
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            SoundEffectsHelper.Instance.MakePlayerShotSound();
            if (shot != null)
			{
				shot.isEnemyShot = isEnemy;
                
			}
            

			// On saisit la direction pour le mouvement
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				
				move.direction = this.transform.right; // ici la droite sera le devant de notre objet
				
			}
		}
	}

	/// <summary>
	/// L'arme est chargée ?
	/// </summary>
	public bool CanAttack
	{
        
        get{
			return shootCooldown <= 0f;
		}
	}
}
