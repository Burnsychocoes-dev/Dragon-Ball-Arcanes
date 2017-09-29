using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add by Steve
public class BarScript : MonoBehaviour {

    // negatif = on vire de la barre, positif = on ajoute
    public static void MoveHealthBar(float percent)
    {
        GameObject hpmask = GameObject.Find("hpmask");
        float barsize = hpmask.GetComponent<BoxCollider2D>().size.x;
        Vector3 bar_move = new Vector3(barsize * percent*0.5f, 0, 0);
        hpmask.transform.Translate(bar_move);
    }

    public static void MoveManaBar(float percent)
    {
        GameObject manamask = GameObject.Find("manamask");
        float barsize = manamask.GetComponent<BoxCollider2D>().size.x;
        Vector3 bar_move = new Vector3(barsize * percent*0.5f, 0, 0);
        manamask.transform.Translate(bar_move);
    }
}
