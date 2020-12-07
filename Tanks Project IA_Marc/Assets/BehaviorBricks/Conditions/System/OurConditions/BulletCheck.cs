using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore.Framework;
using Pada1.BBCore;

using UnityEngine.AI;



[Condition("OurConditions/BulletChecker")]

public class BulletCheck : ConditionBase
{
   
    // Start is called before the first frame update

    public GameObject manager;

    public override bool Check()
    {
        manager = GameObject.Find("GameManager");

        if (manager != null)
        {
            if (manager.GetComponent<Complete.GameManager>().p1_bullets <= 1)
            {

                return true;
            }
        }

        return false;
    }
}
