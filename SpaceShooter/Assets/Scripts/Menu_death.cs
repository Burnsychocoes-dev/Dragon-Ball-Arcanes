using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_death : MonoBehaviour {

    // Use this for initialization
    //public static Menu_death Instance;
    

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PopDeathMenu()
    {
        GameObject.Find("HealthBar").GetComponent<Image>().enabled = false;
        GameObject.Find("healthContent").GetComponent<Image>().enabled = false;
        GameObject.Find("ManaBar").GetComponent<Image>().enabled = false;
        GameObject.Find("manaContent").GetComponent<Image>().enabled = false;


        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        ChangeSceneOnClickScript[] scripts = GetComponentsInChildren<ChangeSceneOnClickScript>();

        foreach(SpriteRenderer sprite in sprites)

        {
            sprite.enabled = true;
        }

        foreach(BoxCollider2D collider in colliders)
        {
            collider.enabled = true;
        }

        foreach(ChangeSceneOnClickScript script in scripts)
        {
            script.enabled = true;
        }

        MoveScript[] movescripts = FindObjectsOfType<MoveScript>();
        ScrollingScript[] scrollingscripts = FindObjectsOfType<ScrollingScript>();
        WeaponScript[] weaponscripts = FindObjectsOfType<WeaponScript>();
        SpawnScript spawnscript = FindObjectOfType<SpawnScript>();

        foreach (MoveScript script in movescripts)
        {
            script.enabled = false;
        }

        foreach (ScrollingScript script in scrollingscripts)
        {
            script.enabled = false;
        }

        foreach (WeaponScript script in weaponscripts)
        {
            script.enabled = false;
        }

        spawnscript.enabled = false;
    }

    
}
