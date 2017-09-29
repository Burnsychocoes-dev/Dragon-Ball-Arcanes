using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	private Rigidbody2D rigidbody2D;
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D> ();

	}
	
	// 1 - Designer variables

	/// <summary>
	/// Vitesse de déplacement
	/// </summary>
	public Vector2 speed = new Vector2(10, 10);

	/// <summary>
	/// Direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
    public double limit_deplacement = 1f; // en %de caméra doit être 1 0.75 0.5 0.25 en raison de problème d'arrondi
	private Vector2 movement;

	void Update()
	{
        
        

        // 6 - Déplacement limité au cadre de la caméra
        var dist = (transform.position - Camera.main.transform.position).z;

        var topBorder = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, dist)
        ).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 1, dist)
        ).y;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, topBorder*(float)limit_deplacement, bottomBorder*(float)limit_deplacement),
            transform.position.z
        );

        //Si on est sur un bord -> on change de direction y
        if(transform.position.y == topBorder*(float)limit_deplacement || transform.position.y == bottomBorder*(float)limit_deplacement)
        {
            direction.y = -direction.y;
        }

        // 2 - Calcul du mouvement
        movement = new Vector2(
            speed.x * direction.x,
            speed.y * direction.y);
    }

    void FixedUpdate()
	{
		// Application du mouvement
		//new pos = position + speed * maxspeed*time deltatime Time.deltaTime
		rigidbody2D.velocity = movement;
	}

    public void setDead()
    {
        speed = new Vector2(0, 0);
    }
}
