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
    public bool isShot = false;
    public bool isCharacter = false;
    
	private Animator myAnimator;
	private EnemyScript enemyScript;



    void Start(){
		myAnimator = gameObject.GetComponent<Animator> ();
        maxhp = hp;
		if (isEnemy && !isShot && GetComponent<EnemyScript>())
        {
            enemyScript = GetComponent<EnemyScript>();
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

                if (isCharacter)
                {
                    myAnimator.SetTrigger("hurt");           
                }

                if (hp <= 0)
				{
                    
                    if (isEnemy && !isShot)
                    {
                        ScoreScript.score += score_value;
                    }

					if (myAnimator != null) {
                        myAnimator.SetTrigger("dead");
                        SoundEffectsHelper.Instance.MakeExplosionSound();
                        //Debug.Log("Je vais mourir");
                        //Debug.Log(gameObject);
                        // a la base, personne n'était considéré comme shot, donc où va la boule de goku ?

                        if (isEnemy && !isShot)
                        {
                            if(enemyScript)
                            {
                                enemyScript.setDead();
                            }							
                            Destroy(gameObject, myAnimator.GetCurrentAnimatorClipInfo(0).Length);
                        }
                        if (isShot)
                        {
                            //renvoyer le shot dans la pool;
                            StartCoroutine(GiveBulletBackAfterT(myAnimator.GetCurrentAnimatorClipInfo(0).Length, GetComponent<Transform>()));
                            
                        }
                        
                        if (isCharacter)
                        {
                            Destroy(gameObject, myAnimator.GetCurrentAnimatorClipInfo(0).Length*1.5f);
                        }
					}
                    else {
                        Destroy (gameObject);
					}
                    GetComponent<PolygonCollider2D>().enabled = false;
                    if (GetComponent<MoveScript>())
                    {
                        GetComponent<MoveScript>().enabled = false;
                    }
                }
			}
		}
	}

    private IEnumerator GiveBulletBackAfterT(float t, Transform bullet)
    {
        yield return new WaitForSecondsRealtime(t);
        GameObject.Find("Scripts").GetComponent<BulletFactory>().GiveBackBullet(bullet.GetComponent<ShotScript>().getBulletType(), bullet);
    }

    public int GetMaxHp()
    {
        return maxhp;
    }
}
