using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using UnityEngine.AI;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to obtain a random position of an area.
    /// </summary>
    [Action("Vector3/WanderMove")]
    [Help("Gets a random position from a given area")]
    public class WanderMove : GOAction
    {
        /// <summary>The Name property represents the GameObject Input Parameter that must have a BoxCollider or SphereColider.</summary>
        /// <value>The Name property gets/sets the value of the GameObject field, area.</value>
        [InParam("area")]
        [Help("GameObject that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted")]
        public GameObject area { get; set; }

        [InParam("wandertimer")]
        [Help("Time to calculate another position")]
        public float wanderTimer;

        private float timer;

        /// <summary>Initialization Method of GetRandomInArea</summary>
        /// <remarks>Verify if there is an area, showing an error if it does not exist.Check that the area is a box or sphere to differentiate the
        /// calculations when obtaining the random position of those areas. Once differentiated, you get the limits of the area to calculate a
        /// random position.</remarks>

        Vector3 mindist;
        Vector3 maxdist;
        public override void OnStart()
        {
            timer = wanderTimer;

            if (area == null)
            {
                Debug.LogError("The area of moving is null", gameObject);
                return;
            }
            BoxCollider boxCollider = area.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                float minDistanceX = area.transform.position.x - area.transform.localScale.x * boxCollider.size.x * 0.5f;
                float maxDistanceX = area.transform.position.x + area.transform.localScale.x * boxCollider.size.x * 0.5f;
                float distanceY = area.transform.position.y;
                float minDistanceZ = area.transform.position.z - area.transform.localScale.z * boxCollider.size.z * 0.5f;
                float maxDistaceZ = area.transform.position.z + area.transform.localScale.z * boxCollider.size.z * 0.5f;

                mindist = new Vector3(minDistanceX, distanceY, minDistanceZ);
                maxdist = new Vector3(maxDistanceX, distanceY, maxDistaceZ);
            }
            else
            {
                Debug.LogError("The " + area + " GameObject must have a Box Collider or a Sphere Collider component", gameObject); 
            }
        }

        /// <summary>Abort method of GetRandomInArea.</summary>
        /// <remarks>Complete the task.</remarks>
        public override TaskStatus OnUpdate()
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNav(gameObject.transform.position, mindist, maxdist, -1);

                gameObject.GetComponent<NavMeshAgent>().SetDestination(newPos);

                Debug.Log(newPos);

                timer = 0;
                return TaskStatus.COMPLETED;
            }
            else
            {
                return TaskStatus.FAILED;
            }
            
        }
        public static Vector3 RandomNav(Vector3 origin, Vector3 mindist, Vector3 maxdist, int layermask)
        {
            Vector3 randDirection = new Vector3(UnityEngine.Random.Range(mindist.x, maxdist.x), 
                                                UnityEngine.Random.Range(mindist.y, maxdist.y), 
                                                UnityEngine.Random.Range(mindist.z, maxdist.z));

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, 90, layermask);

            return navHit.position;
        }

    }
}
