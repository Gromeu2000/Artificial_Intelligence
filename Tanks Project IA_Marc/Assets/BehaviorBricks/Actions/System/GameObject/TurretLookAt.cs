using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to rotate the Gameobject so that it points to the target GameObject position.
    /// </summary>
    [Action("GameObject/TurretLookAt")]
    [Help("Rotates the turret transform so the forward vector of the game object points at target's current position")]
    public class TurretLookAt : GOAction
    {
        ///<value>Input Target game object Parameter.</value>
        [InParam("Turret")]
        [Help("Target game object")]
        public GameObject Turret;               //Turrent Tank Aiming            

        public override TaskStatus OnUpdate()
        {
            if (gameObject.tag == "Red")
            {
                if (GameObject.FindGameObjectWithTag("Blue") != null)
                {
                    Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Blue").transform);
                    return TaskStatus.COMPLETED;
                }
                else
                {
                    return TaskStatus.FAILED;
                }

            }
            else if (gameObject.tag == "Blue")
            {
                if (GameObject.FindGameObjectWithTag("Red") != null)
                {
                    Turret.transform.LookAt(GameObject.FindGameObjectWithTag("Red").transform);
                    return TaskStatus.COMPLETED;
                }
                else
                {
                    return TaskStatus.FAILED;
                }
            }
            else
            {
                return TaskStatus.FAILED;
            }

        }
  
    }
}
