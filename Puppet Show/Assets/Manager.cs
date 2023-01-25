using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public LeanJoystick Joystick;
    [System.NonSerialized]
    public GameObject selectedBone;
    [System.NonSerialized]
    public GameObject selectedSprite;

    public TextMeshProUGUI RemainingTime;
    public TextMeshProUGUI GainedScore;
    public TextMeshProUGUI GameOver_GainedScore;
    public TextMeshProUGUI RemainingHealth;

    public CompleteEvent GoodWorkScreen;
    public Failed FailedScreen;
    public GameObject GameOverScreen;

    public GameObject RoleModel;

    [System.NonSerialized]
    public float time = 10f;

    [System.NonSerialized]
    public int poseNumber = 0;

    //find angle
    private Vector2 Center;

    public bool update;
    [System.NonSerialized]
    public bool sectionFinished = false;
    [System.NonSerialized]
    public int Health = 3;
    [System.NonSerialized]
    public int Score = 0;
    // Start is called before the first frame update

    void Start()
    {
        update = false;
        Center = Joystick.Handle.transform.position;
#if UNITY_ANDROID
        Screen.SetResolution(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2, true);
#endif
        Application.targetFrameRate = 80;
        Health = 3;
        Score = 0;


    }



    public void ToggleModel()
    {
        if (RoleModel.activeInHierarchy == false) RoleModel.SetActive(true);
        else RoleModel.SetActive(false);
    }


    private bool inRange(int referance,int val)
    {
        if (val - referance > 30) return false;
        if (val - referance < -30) return false;


        return true;
    }



    private void FinishSection()
    {
        sectionFinished = true;
        GameObject[] t = GameObject.FindGameObjectsWithTag("Bone");


        int correctposes = 0;

        foreach (GameObject Bone in t)
        {

            int currrentRot = (int)Bone.transform.localEulerAngles.z;

            int neededRot = GetRotofBone(Bone.name + ": ");

            if (inRange(neededRot, currrentRot) == true) correctposes++;

        }


        if(correctposes > 10)
        {
            GoodWorkScreen.CompletePose();

            Score++;

        }
        else
        {
            Health--;
            if(Health > 0) FailedScreen.ShowFailed();
            else
            {
                GameOverScreen.SetActive(true);
            }

        }


       
    }






    // Update is called once per frame
    void Update()
    {
        if(update == false) return;


   

        time -= Time.deltaTime;

        if (time > 1) RemainingTime.text = "Time: " + time.ToString("0.0") + " seconds";
        else if (time > 0) RemainingTime.text = "Time: " + time.ToString("0.00") + " second";
        else if (time <= 0) RemainingTime.text = "Time: Ran Out!";



        GainedScore.text = "Score: " + Score;
        GameOver_GainedScore.text = "Score: " + Score;



        RemainingHealth.text = "Health: " + Health;



        if (time < 0 && sectionFinished == false)
        {
            FinishSection();
        }
        

        if (selectedBone != null)
        {
           
            Vector2 t = Joystick.ScaledValue;
            if(t != new Vector2(0,0))
            {           
            float rot = Vector2.SignedAngle(Center,  t);
            //selectedBone.transform.Rotate(0,0,rot / (Time.deltaTime * 50000));
            selectedBone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rot));
                return;
            }
        }



        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
#if UNITY_ANDROID
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
#elif PLATFORM_STANDALONE_WIN
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);

            if (Physics.Raycast(ray, out hit))
            {
                

               string t = hit.transform.gameObject.name;
               string name = t.Replace("_1", "").Replace("  ", " ");
                Debug.Log(name);
               GameObject a = GameObject.Find(name);

               selectedBone = hit.transform.gameObject;

                if (a != null)
                {

                    if(selectedSprite != null )
                    {
                        selectedSprite.GetComponent<SpriteRenderer>().color = Color.white;
                    }

                    
                    selectedSprite = a;
                    selectedSprite.GetComponent<SpriteRenderer>().color = Color.green;

                   
                }

            }
      
        }
    

}


    public void ResetRot()
    {
        if (selectedBone == null) return;
        Joystick.ScaledValue.Set(0f,0f);
        selectedBone.transform.rotation = Quaternion.Euler(new Vector3());
    }




    private int GetRotofBone(string bone)
    {
        string path = "Pose" + poseNumber;

        string[] Array;
        Array = Resources.Load<TextAsset>(path).text.Split('\n');

        string str = null;
        foreach(string essentiaBone in Array)
        {
            if(essentiaBone.Contains(bone))
            {
                str = essentiaBone;
                break;
            }
        }
        if (str == null) return 0;

        string valstr = str.Replace(bone, "");

        int val = 0;
        Int32.TryParse(valstr, out val);



        return val;
    }




    public void RestartLevel()
    {
        Screen.SetResolution(Screen.currentResolution.width * 2, Screen.currentResolution.height * 2, true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



}
