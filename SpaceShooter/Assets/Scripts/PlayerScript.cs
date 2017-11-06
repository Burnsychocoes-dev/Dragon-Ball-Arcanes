using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contrôleur du joueur
/// </summary>
public class PlayerScript : MonoBehaviour {
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
    // Stockage du mouvement
    private Vector2 movement;

    // Mode super sayen
    private bool isSuperSayen = false;

    // Mode tired
    private bool isTired = false;


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

        if (!isTired)
        {
            // Récupérer les informations du clavier/manette
            float inputX = Input.GetAxis("Horizontal") + Input.GetAxis("HorizontalCroix");
            float inputY = Input.GetAxis("Vertical") + Input.GetAxis("VerticalCroix");
            HandleMovement(inputX, inputY);

            // Tir
            bool shoot = Input.GetButton("Fire1") || Input.GetButton("buttonX");
            HandleShoot(shoot);

            // Teleportation
            bool teleportup = Input.GetButtonDown("TeleportationUp") || Input.GetButtonDown("buttonY");
            bool teleportdown = Input.GetButtonDown("TeleportationDown") || Input.GetButtonDown("buttonA");
            HandleTeleportation(teleportdown, teleportup);

            bool sayenModeButton = Input.GetButtonDown("SuperSayenMode") || Input.GetButtonDown("buttonB");
            HandleSuperSayenTransformation(sayenModeButton);

            if (inputX >= 0)
            {
                animator.SetBool("right", true);
            }
            else
            {
                animator.SetBool("right", false);
            }
            if (Input.GetButton("Fire1") || Input.GetButton("buttonX"))
            {
                animator.SetBool("attack", true);

            }
            else
            {
                animator.SetBool("attack", false);
            }
            if (Input.GetButtonDown("TeleportationUp") || Input.GetButtonDown("buttonY") || Input.GetButtonDown("TeleportationDown") || Input.GetButtonDown("buttonA"))
            {
                animator.SetTrigger("teleport");
            }
        }
        else
        {
            ManaRegenTiredState();
        }

    }

	void FixedUpdate()
	{
        // Déplacement
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
                case Item.ItemName.boule1Etoile:
                    ScoreScript.score += 50;
                    break;
            }
            //Destroy(collider.gameObject);
            GameObject.Find("Scripts").GetComponent<ItemFactory>().GiveBackItem(item.GetItemName(), collider.transform);
        }
    }

    public void HandleMovement(float horizontal, float vertical)
    {
        // Calcul du mouvement
        movement = new Vector2(
          speed.x * horizontal,
          speed.y * vertical);


        // Déplacement limité au cadre de la caméra
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

    public void ManaRegen()
    {
        float add_mana = mana_regen * Time.deltaTime * mana_regen_multiplicateur;
        mana += add_mana;
        if (mana > mana_max)
        {
            mana = mana_max;
        }
        if (mana <= 0)
        {
            mana = 0;
        }
    }

    public void ManaRegenTiredState()
    {
        movement = new Vector2(0f, 0f);
        float add_mana = mana_regen * Time.deltaTime * 5;
        mana += add_mana;
        if (mana > mana_max/4)
        {
            mana = mana_max/4;
            isTired = false;
            animator.SetBool("tired", false);
        }
    }

    public void HandleShoot(bool shoot)
    {
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
            ManaRegen();
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

    public void HandleSuperSayenTransformation(bool sayenModeButton)
    {
        if (sayenModeButton)
        {
            if(CanTransformToSuperSayen && !isSuperSayen)
            {
                ActiveSuperSayenMode();
                isSuperSayen = true;
                animator.SetBool("superSayen", true);
                SoundEffectsHelper.Instance.MakeEnterSuperSayanSound();
                GetComponent<AudioSource>().enabled = true;
                
            }
            else if(isSuperSayen)
            {
                CancelSuperSayenMode();
                isSuperSayen = false;
                animator.SetBool("superSayen", false);
                GetComponent<AudioSource>().enabled = false;
                
            }
        }
        else
        {
            if(mana <= 0)
            {
                mana = 0;
                CancelSuperSayenMode();
                isTired = true;
                animator.SetBool("superSayen", false);
                animator.SetBool("tired", true);
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

    public bool CanTransformToSuperSayen
    {
        get
        {
            return (mana > mana_max/2);
        }
    }

    public void ActiveSuperSayenMode()
    {
        mana_regen_multiplicateur = -1;
        health.resistance = 2;
        weapon.SetDamageMultiplicator(2);
        speed.x *= 1.5f;
        speed.y *= 1.5f;
    }

    public void CancelSuperSayenMode()
    {
        mana_regen_multiplicateur = 1;
        health.resistance = 1;
        weapon.SetDamageMultiplicator(1);
        speed.x /= 1.5f;
        speed.y /= 1.5f;
    }

    void OnDestroy()
    {
        // Game Over
        Debug.Log("Goku est detruit");
        GameObject.Find("Menu_death").GetComponent<Menu_death>().PopDeathMenu();
    }
}
