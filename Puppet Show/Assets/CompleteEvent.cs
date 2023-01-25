using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteEvent : MonoBehaviour
{
    // Start is called before the first frame update

    public Manager LevelManager;

  
    public AudioClip FlashSound;

    public Picture pic;

    public Image background;

    public void CompletePose()
    {
        background.color = new Color(1, 1, 1, 0.1f);
        gameObject.SetActive(true);

        GameObject sourobj = new GameObject("Flash");

        AudioSource source = sourobj.AddComponent<AudioSource>();

        source.PlayOneShot(FlashSound);

        Destroy(source, FlashSound.length);

        StartCoroutine(FinishScreen());
       
    }



    IEnumerator FinishScreen()
    {
        yield return new WaitForSeconds(3);

        gameObject.SetActive(false);
        if (LevelManager.Health < 3) LevelManager.Health++;
        Time.timeScale = 1.0f;
        pic.ShowPicture();
        LevelManager.sectionFinished = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(background.color.a < 1)
        {
            background.color = new Color(1, 1, 1, background.color.a + Time.deltaTime * 5);
        }
    }
}
