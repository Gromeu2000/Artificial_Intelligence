using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

namespace BBUnity.Actions
{
    [Action("OurBehaviorScripts/Shooting")]
    [Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
          "specified velocity.")]
    public class Shooting : GOAction
    {

        [InParam("shell")]
        [Help("Bullet shoot")]
        public Rigidbody m_Shell;
        public int numberBullets;

        [InParam("minLaunchForce")]
        public float m_MinLaunchForce;

        [InParam("maxLaunchForce")]
        public float m_MaxLaunchForce = 30f;

        [InParam("currentLaunchForce")]
        public float m_CurrentLaunchForce;

        [InParam("currentTank")]
        public GameObject CurrentTank;


        private Transform spawnShoot;

        private void OnEnable()
        {
            m_CurrentLaunchForce = m_MinLaunchForce;
        }

        public override void OnStart()
        {
           if(spawnShoot == null)
            {
                Transform turret = CurrentTank.GetComponentInChildren<Transform>().Find("TankRenderers").Find("TankTurret").Find("FireTransform");

                spawnShoot = turret;
            }
        }

        public override TaskStatus OnUpdate()
        {

            m_CurrentLaunchForce = 30;

            Fire();

            return TaskStatus.RUNNING;

        }

        private void Fire()
        {
            Rigidbody shellInstance =
                GameObject.Instantiate(m_Shell, spawnShoot.position, spawnShoot.rotation) as Rigidbody;

            shellInstance.velocity = m_CurrentLaunchForce * (spawnShoot.forward);

            m_CurrentLaunchForce = m_MinLaunchForce;
        }

    } // class Shooting

}