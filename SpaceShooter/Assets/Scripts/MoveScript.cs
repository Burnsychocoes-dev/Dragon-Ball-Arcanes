using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

	private Rigidbody2D rigidbody2D;
    // 1 - Designer variables

    /// <summary>
    /// Vitesse de déplacement
    /// </summary>
    public Vector2 speed = new Vector2(10, 10);

    /// <summary>
    /// Direction
    /// </summary>
    public Vector2 direction = new Vector2(-1, 0);

    /// <summary>
    /// permet de limiter le déplacement de l'unité par rapport au haut et au bas de la caméra (utile pour les déplacements de haut en bas, pour qu'il puisse rebondir)
    /// </summary>
    public double limitDeplacement = 1f; // en %de caméra doit être 1 0.75 0.5 0.25 en raison de problème d'arrondi
    /// <summary>
    /// Pour calculer le mouvement
    /// </summary>
    private Vector2 movement;

    public bool isShot = false;
    /// <summary>
    /// Si le mouvement doit suivre le joueur ou bas (ex tête chercheuse)
    /// </summary>
    
    public bool characterLock = false;
    /// <summary>
    /// S'il s'agit d'un missile visé (pas forcemment tête chercheuse, mais on vise le joueur)
    /// </summary>
    public bool characterLockInit = false;


    void Start () {
		rigidbody2D = GetComponent<Rigidbody2D> ();
        
        
        
	}
    

	void Update()
	{

        // Déplacement limité au cadre de la caméra
        if (!isShot)
        {
            LimitationDeplacement();
        }

        //Si on est une tête chercheuse, on change la direction en fonction du personnage
        if (characterLock)
        {
            CalculDirectionForHeadHunter();
        }
        
        //Calcul du mouvement
        movement = new Vector2(
            speed.x * direction.x,
            speed.y * direction.y);
    }

    void FixedUpdate()
	{
		// Application du mouvement
		// new pos = position + speed * maxspeed*time deltatime Time.deltaTime si on utilise pas velocity
		rigidbody2D.velocity = movement;
	}

    public void setDead()
    {
        speed = new Vector2(0, 0);
    }

    private void LimitationDeplacement()
    {
        var dist = (transform.position - Camera.main.transform.position).z;

        var topBorder = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 0, dist)
        ).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(
            new Vector3(0, 1, dist)
        ).y;

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, topBorder * (float)limitDeplacement, bottomBorder * (float)limitDeplacement),
            transform.position.z
        );

        //Si on est sur un bord -> on change de direction y
        if (transform.position.y == topBorder * (float)limitDeplacement || transform.position.y == bottomBorder * (float)limitDeplacement)
        {
            direction.y = -direction.y;
        }
    }


    public void CalculDirectionForHeadHunter()
    {
        if (characterLockInit || characterLock)
        {
            PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
            Vector3 position = player.GetComponent<Transform>().position;
            //Debug.Log(position);
            direction = position - transform.position;
            //Debug.Log(direction);
        }
    }

    private IEnumerator WaitForInitAnimation()
    {
        Vector2 speedSave = speed;
        Animator animator;
        speed = new Vector2(0, 0);
        if (animator=GetComponent<Animator>())
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        speed = speedSave;
        CalculDirectionForHeadHunter();
    }

    //Pour donner la direction à un headhunter avec son animation
    public void AnimateHeadHunter()
    {
        if (characterLockInit)
        {
            StartCoroutine(WaitForInitAnimation());

        }
    }
}
