using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Failed : MonoBehaviour
{
    // Start is called before the first frame update

    public Image Background;
    public Picture pic;
    public Manager LevelManager;
    public void ShowFailed()
    {

        Background.color = new Color(1, 0, 0, 0.1f);
        gameObject.SetActive(true);

        StartCoroutine(FinishScreen());


    }

    IEnumerator FinishScreen()
    {
        yield return new WaitForSeconds(3);
        pic.ShowPicture();
        gameObject.SetActive(false);
        LevelManager.sectionFinished = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(Background.color.a < 1)
        {
            Background.color = new Color(1, 0, 0, Background.color.a + Time.deltaTime*2);
        }
    }
}
