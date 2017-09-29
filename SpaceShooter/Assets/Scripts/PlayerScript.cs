using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrôleur du joueur
/// </summary>
public class PlayerScript : MonoBehaviour {
	/// <summary>
	/// 1- La vitesse de déplacement
	/// </summary>
	// Use this for initialization
	private Rigidbody2D rigidbody;
    private WeaponScript weapon;
	private Animator animator;
    public Vector2 speed = new Vector2(50, 50);
    public float mana = 100;
    private float mana_max;
    public float mana_regen = 10f;
    private int mana_regen_correction = 2; // multiplicateur de regen (pour le super sayan)
    private float tp_correction = 1.5f;
    public float tp_cd = 0; //cooldown de la teleportation
    private float tp_cd_count; // compteur du cooldown de la tp
    public float tp_mana_cost = 10;
    // 2 - Stockage du mouvement
    private Vector2 movement;


    void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<WeaponScript>();
        mana_max = mana;
        tp_cd_count = 0;
	}	
	

	void Update()
	{
        //Delai de tp/supersayan
        if (tp_cd_count > 0)
        {
            tp_cd_count -= Time.deltaTime;
        }

        // 3 - Récupérer les informations du clavier/manette
        float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// 4 - Calcul du mouvement
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);

        // 5 - Tir && Teleportations
        bool shoot = Input.GetButton("Fire1");
        //shoot |= Input.GetButton("Fire2");
        // Astuce pour ceux sous Mac car Ctrl + flèches est utilisé par le système
        WeaponScript weapon = GetComponentInChildren<WeaponScript>();
        //GameObject manamask = GameObject.Find("manamask");
        if (shoot)
        {
            if (weapon != null)
            {
                // false : le joueur n'est pas un ennemi
                
                int mana_cost = weapon.mana_cost;
                if (mana - mana_cost > 0 && weapon.CanAttack)
                {
                    mana -= mana_cost;
                    float percent = mana_cost / mana_max;
                    /*float barsize = manamask.GetComponent<BoxCollider2D>().size.x;                    
                    Vector3 bar_move = new Vector3(-barsize * percent, 0, 0);
                    manamask.transform.Translate(bar_move);*/
                    BarScript.MoveManaBar(-percent); // appel au script pour animer la mana
                    weapon.Attack(false);
                }
                

            }
        }
        else
        {
            float add_mana = mana_regen * Time.deltaTime * mana_regen_correction;
            mana += add_mana;
            float residu = 0;
            float percent = 0;
            ///float barsize = manamask.GetComponent<BoxCollider2D>().size.x;
            
            if (mana > mana_max)
            {
                residu = mana - mana_max;
                mana = mana_max;
                percent = (add_mana - residu) / mana_max;
            }
            else
            {
                percent = add_mana / mana_max;                
            }
            /*Vector3 bar_move = new Vector3(barsize * percent, 0, 0);
            manamask.transform.Translate(bar_move);*/
            BarScript.MoveManaBar(percent);



        }

        bool teleportup = Input.GetButtonDown("Teleportationup");
        bool teleportdown = Input.GetButtonDown("Teleportationdown");
        
        if (CanTp && (teleportup || teleportdown))
        {
            Debug.Log("salut");
            tp_cd_count = tp_cd;
            float height = GetComponent<Renderer>().bounds.size.y;
            mana -= tp_mana_cost;
            float percent = tp_mana_cost / mana_max;
            BarScript.MoveManaBar(-percent);

            //Teleportation up
            if (teleportup) { 
                
                float inputYblock = 1;
                
                transform.position = new Vector3(transform.position.x, transform.position.y + height * inputYblock * tp_correction);
                SoundEffectsHelper.Instance.MakeTeleportationSound();
            }
            else if (teleportdown) // teleportation down
            {
                
                float inputYblock = -1;                
                transform.position = new Vector3(transform.position.x, transform.position.y + height * inputYblock * tp_correction);
                SoundEffectsHelper.Instance.MakeTeleportationSound();
            }
        }


        // 6 - Déplacement limité au cadre de la caméra
        var dist = (transform.position - Camera.main.transform.position).z;

		var leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
		).x;

		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
		).x;

		var topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
		).y;

		var bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
		).y;

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
			transform.position.z
		);

		// Fin d'Update

	}

	void FixedUpdate()
	{
		float inputX = Input.GetAxis("Horizontal");
		if (inputX >= 0) {
			animator.SetBool ("right", true);
		} else if (inputX < 0) {
			animator.SetBool ("right", false);
		}
		if (Input.GetButton ("Fire1")) {
			animator.SetBool ("attack", true);         
            
		} else {
			animator.SetBool ("attack", false);
		}
        if (Input.GetButtonDown("Teleportationup") || Input.GetButtonDown("Teleportationdown")){
            animator.SetTrigger("teleport");
        }
		// 5 - Déplacement
		rigidbody.velocity = movement;
	}

    public bool CanTp
    {
        get
        {
            return ((tp_cd_count <= 0f) && mana>=tp_mana_cost);
        }
    }


}
