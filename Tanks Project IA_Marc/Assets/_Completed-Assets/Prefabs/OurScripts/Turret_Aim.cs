using System.Collections;
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
    public float Missile_Speed = 15.0f;
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

       // ShootMissile();
    }

    float CalculateShotAngle(float Bullet_Speed, Vector3 target) //Calculations are correct
    {
        float distance = Vector3.Distance(CurrentTank.transform.position, target);

        float parenthesis = Physics.gravity.y * distance * distance; //g * x^2

        double numerator = Math.Sqrt(Math.Pow(Bullet_Speed, 4) - (Physics.gravity.y * parenthesis)); //v^4 - g * (g*x^2)

        double ATangle = ((Math.Pow(Bullet_Speed, 2)) - numerator) / (Physics.gravity.y * distance); //

        double angle = Math.Atan(ATangle);

        float result = (float)angle * Mathf.Rad2Deg;


        print("Angle in Degrees");
        print(result);
        return result;
    }

    void ShootMissile()
    {
        float X_Angle = CalculateShotAngle(Missile_Speed, Target.transform.position);
        Debug.Log(X_Angle);
        if (X_Angle > 100)
        {
            if (float.IsNaN(Math.Abs(X_Angle)))
            {
                print("Target out of range");
                return;
            }

            Turret.transform.Rotate(X_Angle, 0.0f, 0.0f);


            print("Turret ROT" + Turret.transform.eulerAngles);

            Rigidbody missile_inst = Instantiate(Bullet, CurrentTank.transform.position, Turret.transform.rotation) as Rigidbody;
            missile_inst.velocity = Missile_Speed * CurrentTank.transform.forward;
        }

        Shoot_Timer = UnityEngine.Random.Range(2f, 4f);
    }
}
