﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class Turret_Aim : MonoBehaviour
{
    // Start is called before the first frame update
    //Shooting and Aiming
    public GameObject CurrentTank;
    public GameObject Turret;
   
    public Rigidbody Bullet;
  
    private float Shoot_Timer;
    public GameObject Target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTank.tag == "Red")
        {
            Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Blue").transform);
        }
        else if (CurrentTank.tag == "Blue")
        {
            Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Red").transform);
        }

       
    }


}
