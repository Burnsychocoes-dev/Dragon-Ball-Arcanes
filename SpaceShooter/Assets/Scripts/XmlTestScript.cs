using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ennemyType
{
    burter,
    burter_move,
    jayce,
    freezer
}

public class XmlTestScript : MonoBehaviour{

    // Partie test xml steve
    [SerializeField]
    LevelDescription levelDescriptionTest;

    // Use this for initialization
    void Start () {
        // Partie test ecriture d'un fichier XML à partir d'un objet levelDescription
        Debug.Log("On lance la partie xml");
        levelDescriptionTest = GetComponent<LevelDescription>();
        XmlHelpers.SerializeToXML<LevelDescription>("LevelsXmlFiles.xml", levelDescriptionTest);
        Debug.Log("la partie xml est fini");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
