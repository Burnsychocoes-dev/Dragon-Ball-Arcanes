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
    private HealthScript health;
    private WeaponScript weapon;
	private Animator animator;
    public Vector2 speed = new Vector2(50, 50);
    public float mana = 100;
    private float mana_max;
    public float mana_regen = 1f;
    private int mana_regen_multiplicateur = 1; // multiplicateur de regen (pour le super sayan)
    private float tp_correction = 1.5f;
    public float tp_cd = 1f; //cooldown de la teleportation
    private float tp_cd_count; // compteur du cooldown de la tp
    public float tp_mana_cost = 10;
    // 2 - Stockage du mouvement
    private Vector2 movement;




    void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
        health = GetComponent<HealthScript>();
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
        HandleMovement(inputX, inputY);

        // 5 - Tir && Teleportations
        bool shoot = Input.GetButton("Fire1");
        HandleShoot(shoot);


        bool teleportup = Input.GetButtonDown("TeleportationUp");
        bool teleportdown = Input.GetButtonDown("TeleportationDown");
        HandleTeleportation(teleportdown, teleportup);

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
        if (Input.GetButtonDown("TeleportationUp") || Input.GetButtonDown("TeleportationDown")){
            animator.SetTrigger("teleport");
        }

		// 5 - Déplacement
		rigidbody.velocity = movement;

        // Update mana bar
        UpdateBar();

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Item item = collider.gameObject.GetComponent<Item>();
        if (item != null)
        { 
            switch (item.GetItemName())
            {
                case Item.ItemName.senzu:
                    health.hp = health.GetMaxHp();
                    break;
                case Item.ItemName.capsuleEnergy:
                    mana = mana_max;
                    break;
            }
            Destroy(collider.gameObject);
        }
    }

        public void HandleMovement(float horizontal, float vertical)
    {
        // 4 - Calcul du mouvement
        movement = new Vector2(
          speed.x * horizontal,
          speed.y * vertical);


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

        GetComponent<Rigidbody2D>().velocity = movement;
    }


    public void HandleShoot(bool shoot)
    {
        WeaponScript weapon = GetComponentInChildren<WeaponScript>();
        //GameObject manamask = GameObject.Find("manamask");
        if (shoot)
        {
            if (weapon != null)
            {
                // false : le joueur n'est pas un ennemi
                int mana_cost = weapon.mana_cost;
                if ((mana - mana_cost > 0) && weapon.CanAttack)
                {
                    mana -= mana_cost;
                    weapon.Attack(false);
                }
            }
        }
        else
        {
            float add_mana = mana_regen * Time.deltaTime * mana_regen_multiplicateur;
            mana += add_mana;
            if (mana > mana_max)
            {
                mana = mana_max;
            }
        }
    }


    public void HandleTeleportation(bool tpDown, bool tpUp)
    {
        if (CanTp && (tpDown || tpUp))
        {    
            tp_cd_count = tp_cd;
            float height = GetComponent<Renderer>().bounds.size.y;
            mana -= tp_mana_cost;
            float percent = tp_mana_cost / mana_max;
            GetComponent<BarScript>().MoveManaBar(percent);

            //Teleportation up
            if (tpUp)
            {
                float inputYblock = 1;

                transform.position = new Vector3(transform.position.x, transform.position.y + height * inputYblock * tp_correction);
                SoundEffectsHelper.Instance.MakeTeleportationSound();
            }
            // teleportation down
            else if (tpDown)
            {
                float inputYblock = -1;
                transform.position = new Vector3(transform.position.x, transform.position.y + height * inputYblock * tp_correction);
                SoundEffectsHelper.Instance.MakeTeleportationSound();
            }
        }
    }

    public void UpdateBar()
    {
        float manaPercent = (float)mana / mana_max;
        float hpPercent = (float)GetComponent<HealthScript>().hp / GetComponent<HealthScript>().GetMaxHp();
        GetComponent<BarScript>().MoveManaBar(manaPercent);
        GetComponent<BarScript>().MoveHealthBar(hpPercent);
    }


    public bool CanTp
    {
        get
        {
            return ((tp_cd_count <= 0f) && mana >= tp_mana_cost);
        }
    }

    void OnDestroy()
    {
        // Game Over
        Debug.Log("Goku est detruit");
        GameObject.Find("Menu_death").GetComponent<Menu_death>().PopDeathMenu();
    }
}
