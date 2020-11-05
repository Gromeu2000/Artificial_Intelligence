using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Aim : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CurrentTank;
    public GameObject Turret;

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
