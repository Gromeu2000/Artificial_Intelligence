using Pada1.BBCore.Framework;
using Pada1.BBCore;
using UnityEngine;
using System;

/// <summary>
/// It is a basic condition to check if Booleans have the same value.
/// </summary>
[Condition("OurConditions/CanShoot")]
[Help("Checks if the current tank can shoot")]
public class CanShoot : ConditionBase
{
    [OutParam("CanFire")]
    public bool fire = false;

    [InParam("currentTank")]
    public GameObject currentTank;

    [InParam("targetTank")]
    public GameObject targetTank;

    
    public override bool Check()
    {
        if (targetTank != null)
        {

            Vector3 currentPos = currentTank.transform.position;
            Vector3 targetPos = GameObject.Find("CompleteTank(Clone)").transform.position;
            if(currentPos== targetPos)
            {
                targetPos = GameObject.Find("CompleteTank2(Clone)").transform.position;
            }

            Vector3 distance;
            //La position del target tank no s'actualitza
            distance.x = Mathf.Abs(targetPos.x - currentPos.x);
            distance.y = Mathf.Abs(targetPos.y - currentPos.y);
            distance.z = Mathf.Abs(targetPos.z - currentPos.z);

            if (InRange(15, distance))
            {
                fire = false;
                return fire;
            }
            else
            {
                fire = true;
                return fire;
            }
        }

        else return true;

    }
    private bool InRange(int maxDistance, Vector3 currentDistance)
    {
        if (currentDistance.x < maxDistance && currentDistance.z < maxDistance)
        {
            return true;
        }
       

         return false;
    }
}
