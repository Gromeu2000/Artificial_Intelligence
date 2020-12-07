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
    [InParam("BulletNumber")]
    public int numberBullets;

    public override bool Check()
    {
        
        if (numberBullets < 1) {

            return true;
        }





        return false;
    }
}
