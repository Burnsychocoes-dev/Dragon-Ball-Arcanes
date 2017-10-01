using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Création d'effets sonores en toute simplicité
/// </summary>
public class SoundEffectsHelper : MonoBehaviour
{

    /// <summary>
    /// Singleton
    /// </summary>
    public static SoundEffectsHelper Instance;

    public AudioClip explosionSound;
    public AudioClip playerShotSound;
    public AudioClip enemyShotSound;
    public AudioClip teleportationSound;
    public AudioClip jayceSound;
    public AudioClip enterSuperSayanSound;
    public AudioClip buttonSelectSound;
    public AudioClip buttonSelectedSound;
    

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundEffectsHelper!");
        }
        Instance = this;
    }

    public void MakeExplosionSound()
    {
        MakeSound(explosionSound);
    }

    public void MakePlayerShotSound()
    {
        MakeSound(playerShotSound);
    }

    public void MakeEnemyShotSound()
    {
        MakeSound(enemyShotSound);
    }

    public void MakeButtonSelectSound()
    {
        MakeSound(buttonSelectSound);
    }
    public void MakeButtonSelectedSound()
    {
        MakeSound(buttonSelectedSound);
    }

    public void MakeTeleportationSound()
    {
        MakeSound(teleportationSound);
    }

    public void MakeJayceSound()
    {
        MakeSound(jayceSound);
    }

    public void MakeEnterSuperSayanSound()
    {
        MakeSound(enterSuperSayanSound);
    }

    /// <summary>
    /// Lance la lecture d'un son
    /// </summary>
    /// <param name="originalClip"></param>
    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }
}
