using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	private bool hasSpawn;
	private MoveScript moveScript;
	private WeaponScript[] weapons;
	private bool alive = true;

	void Awake()
	{
		// Récupération de toutes les armes de l'ennemi
		weapons = GetComponentsInChildren<WeaponScript>();

		// Récupération du script de mouvement lié
		moveScript = GetComponent<MoveScript>();
	}

	// 1 - Disable everything
	void Start()
	{
		hasSpawn = false;

		// On désactive tout
		// -- collider
		GetComponent<Collider2D>().enabled = false;
		// -- Mouvement
		moveScript.enabled = false;
		// -- Tir
		foreach (WeaponScript weapon in weapons)
		{
			weapon.enabled = false;
		}
	}

	void Update()
	{
		// 2 - On vérifie si l'ennemi est apparu à l'écran
		if (hasSpawn == false)
		{
			if (GetComponent<Renderer>().IsVisibleFrom(Camera.main))
			{
				Spawn();
                
			}
		}
		else
		{
            // On fait tirer toutes les armes automatiquement si il est vivant
            HandleShootWithWeapons();

			// Si L'ennemi n'a pas été détruit, il faut faire le ménage
			if (GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false)
			{
				Destroy(gameObject);
			}
		}
	}

	// 3 - Activation
	private void Spawn()
	{
		hasSpawn = true;

		// On active tout
		// -- Collider
		GetComponent<Collider2D>().enabled = true;
		// -- Mouvement
		moveScript.enabled = true;
        // -- Tir
        //Si c'est une tête chercheuse, on l'anime, sinon on met les armes en place
        if (moveScript.characterLockInit)
        {
            GetComponent<Animator>().SetTrigger("attack");
            moveScript.AnimateHeadHunter();

        }
        else
        {
            foreach (WeaponScript weapon in weapons)
            {
                weapon.enabled = true;
            }
        }
	}

	public void setDead(){
		alive = false;
	}

    private void HandleShootWithWeapons()
    {
        if (alive)
        {
            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true);
                    SoundEffectsHelper.Instance.MakeEnemyShotSound();
                }
            }
        }
    }

    
}
