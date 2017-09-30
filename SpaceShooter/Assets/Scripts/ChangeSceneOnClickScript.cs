using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClickScript : MonoBehaviour
{
    public Renderer rend;
    public string _nextScene = "";
    public Color color = Color.red;
    public Sprite sprite;
    private Sprite spritesave;
    private SpriteRenderer spriterender;
    void Start()
    {
        rend = GetComponent<Renderer>();
        spriterender = GetComponent<SpriteRenderer>();
        if (_nextScene.Equals(""))
        {
            _nextScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
    }
    void OnMouseEnter()
    {

        if (sprite != null)
        {
            spritesave = spriterender.sprite;
            spriterender.sprite = sprite;
        }
        else
        {
            rend.material.color = color;
        }
        SoundEffectsHelper.Instance.MakeButtonSelectSound();
    }
    void OnMouseOver()
    {
        //rend.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
    }
    void OnMouseExit()
    {
        rend.material.color = Color.white;
        if (sprite != null)
        {
            spriterender.sprite = spritesave;
        }

    }
    private IEnumerator OnMouseDown()
    {
        SoundEffectsHelper.Instance.MakeButtonSelectedSound();
        float fadeTime = GameObject.Find("FadeScript").GetComponent<FadingScene>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(_nextScene);
    }

    
}
