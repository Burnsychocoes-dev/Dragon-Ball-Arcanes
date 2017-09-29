using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Add by Steve
public class BarScript : MonoBehaviour {

    // image contenu des bars de vie et mana
    [SerializeField]
    private Image manaBarContent;

    [SerializeField]
    private Image healthBarContent;

    public void MoveHealthBar(float percent)
    {
        healthBarContent.fillAmount = percent;

    }

    public void MoveManaBar(float percent)
    {
        manaBarContent.fillAmount = percent;
    }
}
