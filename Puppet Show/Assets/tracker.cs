using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class tracker : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {

        


        if(Input.GetKeyDown(KeyCode.F))
        {
           

            Debug.LogWarning(gameObject.name + ": " + gameObject.transform.localEulerAngles.z);


        }



    }
}
