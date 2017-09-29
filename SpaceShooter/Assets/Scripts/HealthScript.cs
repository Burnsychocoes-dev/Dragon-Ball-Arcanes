using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

	/// <summary>
	/// Points de vies
	/// </summary>
	public int hp = 1;
    public int score_value = 1;
    private int maxhp ;

	/// <summary>
	/// Ennemi ou joueur ?
	/// </summary>
	public bool isEnemy = true;
	public bool isBorder = false;
    public bool isFire = false;
    public bool isCharacter = false;
    
	private Animator Death_animator;
	private EnemyScript enemyScript;



    void Start(){
		Death_animator = gameObject.GetComponent<Animator> ();
        maxhp = hp;
		if (isEnemy) {
            if (!isFire)
            {
                enemyScript = gameObject.GetComponent<EnemyScript>();
            }
            else
            {
                //Death_animator.SetInteger("enemy",1);
            }
        }
        else
        {
            if (isFire)
            {
               // Death_animator.SetBool("goku", true);
            }
        }
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		//anim = gameObject.GetComponent<Animation> ();
		// Est-ce un tir ?
		ShotScript shot = collider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Tir allié
			if (shot.isEnemyShot != isEnemy )
			{
				hp -= shot.damage;

                // Destruction du projectile
                // On détruit toujours le gameObject associé
                // sinon c'est le script qui serait détruit avec ""this""
                /*if (!shot.isBorder) {
					Destroy (shot.gameObject);
				}*/
                if (isCharacter)
                {
                    Death_animator.SetTrigger("hurt");
                    //GameObject hpmask = GameObject.Find("hpmask");
                    //float barsize = hpmask.GetComponent<BoxCollider2D>().size.x;
                    float percent = (shot.damage)/(float)maxhp;
                    //Vector3 bar_move = new Vector3(-barsize * percent, 0, 0);
                    //hpmask.transform.Translate(bar_move);
                    BarScript.MoveHealthBar(-percent);

                }

                SoundEffectsHelper.Instance.MakeExplosionSound();
                if (hp <= 0 && !isBorder)
				{
                    gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                    if (isEnemy && !isFire)
                    {
                        ScoreScript.score += score_value;
                    }

					if (Death_animator != null) {
                        Death_animator.SetTrigger ("dead");
						if (isEnemy && !isFire) {
							enemyScript.setDead();
						}
						Destroy (gameObject, Death_animator.GetCurrentAnimatorClipInfo(0).Length);

                        if (isCharacter)
                        {
                            Debug.Log("wait");
                            StartCoroutine(wait((float)(Death_animator.GetCurrentAnimatorClipInfo(0).Length*0.95)));                            
                            /*Menu_death menu = FindObjectOfType<Menu_death>();
                            menu.PopDeathMenu();*/
                        }
					} else {
						// Destruction !
						
						Destroy (gameObject);
					}
				}
			}
		}
	}

    IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
        //Debug.Log("let's go");
        Menu_death menu = FindObjectOfType<Menu_death>();
        menu.PopDeathMenu();
       // Menu_death.Instance.PopDeathMenu();
    }
}
