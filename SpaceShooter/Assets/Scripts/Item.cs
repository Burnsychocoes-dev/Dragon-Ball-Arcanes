using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : MonoBehaviour {

    public enum ItemName
    {
        senzu,
        boule1Etoile,
        boule2Etoile,
        boule3Etoile,
        boule4Etoile,
        boule5Etoile,
        boule6Etoile,
        boule7Etoile,
        capsuleEnergy
    }

    [SerializeField]
    private ItemName itemName;

    public ItemName GetItemName()
    {
        return (itemName);
    }
}
