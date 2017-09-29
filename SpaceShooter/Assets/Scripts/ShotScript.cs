using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour {

	// 1 - Designer variables

	/// <summary>
	/// Points de dégâts infligés
	/// </summary>
	public int damage = 1;

	/// <summary>
	/// Projectile ami ou ennemi ?
	/// </summary>
	public bool isEnemyShot = false;
	public bool isnotShot = false;

	void Start()
	{
		// 2 - Destruction programmée
		if(!isnotShot){
			Destroy(gameObject, 20); // 20sec
		}
	}
}
