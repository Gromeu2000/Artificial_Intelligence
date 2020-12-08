using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

namespace BBUnity.Actions
{
    [Action("OurBehaviorScripts/GoReloadBlue")]

    public class GoReloadBlue : GOAction
    {

        [InParam("ReloadPos")]
        public GameObject ReloadPoint;

        [InParam("ReloadAudioSource")]
        public AudioSource reloadAudio;

        public GameObject manager;

        private UnityEngine.AI.NavMeshAgent navAgent;

        private Transform targetTransform;

        public override void OnStart()
        {
            manager = GameObject.Find("GameManager");

            if (ReloadPoint == null)
            {
                return;
            }

            targetTransform = ReloadPoint.transform;

            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {

                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            navAgent.SetDestination(targetTransform.position);

#if UNITY_5_6_OR_NEWER
                       navAgent.isStopped = false;
#else
            navAgent.Resume();
#endif
        }

        public override TaskStatus OnUpdate()
        {

            if (ReloadPoint == null)
            {
                return TaskStatus.FAILED;
            }
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                manager.GetComponent<Complete.GameManager>().p1_bullets = 3;
                reloadAudio.Play();
                return TaskStatus.COMPLETED;
            }
            else if (navAgent.destination != targetTransform.position)
            {
                navAgent.SetDestination(targetTransform.position);
                return TaskStatus.RUNNING;
            }
            else
            {
                return TaskStatus.COMPLETED;
            }




        }



    } // class Shooting

}