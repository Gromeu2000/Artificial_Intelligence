using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class Turret_Aim : MonoBehaviour
{
    // Start is called before the first frame update
    //Shooting and Aiming

 
    public GameObject CurrentTank;          //Aiming Tank
    public GameObject Turret;               //Turrent Tank Aiming            

    void Update()
    {
        if (CurrentTank.tag == "Red")
        {
            if (GameObject.FindGameObjectWithTag("Blue") != null)
            {
                Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Blue").transform);
            }
        }
        else if (CurrentTank.tag == "Blue")
        {
            if (GameObject.FindGameObjectWithTag("Red") != null)
            {
                Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Red").transform);
            }
        }
        
    }

}
