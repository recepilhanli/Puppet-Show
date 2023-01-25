using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Picture : MonoBehaviour
{

    

    public Image ActorPic;
    public Image Background;
    public Manager LevelManager;
    public int Pose;
    private float showTime = 5;

    public Sprite[] Sources;

    public Image Model;

    public AudioClip readyClip;

    // Start is called before the first frame update
    void Start()
    {

     ShowPicture();

    }

    // Update is called once per frame
    void Update()
    {
    

        if(Background.color.a != 1f) {

            float r = Background.color.r;
            float g = Background.color.g;
            float b = Background.color.b;
            float a = Background.color.a;

            Background.color = new Color(r, g, b, a+Time.deltaTime/2);

        }

        if (ActorPic.rectTransform.sizeDelta.y < 900)
        {
            float x = ActorPic.rectTransform.sizeDelta.x;
            float y = ActorPic.rectTransform.sizeDelta.y;
            ActorPic.rectTransform.sizeDelta = new Vector2(x + Time.deltaTime*150, y + Time.deltaTime*150);
        }


        showTime -= Time.deltaTime;

        if(showTime < 0 && gameObject.activeInHierarchy == true)
        {
            gameObject.SetActive(false);
            LevelManager.time = 25f;
            LevelManager.update = true;
        }
    }



    public void ShowPicture()
    {

        int oldPose = Pose;
        while(oldPose == Pose)
        { 
        Pose = Random.Range(0, Sources.Length);
        }

       LevelManager.poseNumber = Pose;


        ActorPic.sprite = Sources[Pose];
        Model.sprite = Sources[Pose];


        LevelManager.update = false;

        float r = Background.color.r;
        float g = Background.color.g;
        float b = Background.color.b;
        float a = Background.color.a;

        Background.color = new Color(r, g, b, 0.1f);


        ActorPic.rectTransform.sizeDelta = new Vector2(700, 700);

        showTime = 7f;

        gameObject.SetActive(true);


        GameObject sourobj = new GameObject("Ready");

        AudioSource source = sourobj.AddComponent<AudioSource>();
        source.volume = 0.6f;
        source.PlayOneShot(readyClip);

        Destroy(source, readyClip.length);



    }




}
