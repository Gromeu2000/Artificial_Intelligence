using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

namespace BBUnity.Actions
{
    [Action("OurBehaviorScripts/ShootingRed")]
    [Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
          "specified velocity.")]
    public class ShootingRed : GOAction
    {

        [InParam("shell")]
        [Help("Bullet shoot")]
        public Rigidbody m_Shell;
        
        [InParam("minLaunchForce")]
        public float m_MinLaunchForce;

        [InParam("maxLaunchForce")]
        public float m_MaxLaunchForce = 30f;

        [InParam("currentLaunchForce")]
        public float m_CurrentLaunchForce;

        [InParam("currentTank")]
        public GameObject CurrentTank;

        private Transform spawnShoot;

        public GameObject manager;

        bool fire = true;

        [InParam("delay")]
        [Help("Delay between bullets")]
        public float delay;
        private float currentTime;

        public override void OnStart()
        {

            if (spawnShoot == null)
            {
                Transform turret = CurrentTank.GetComponentInChildren<Transform>().Find("TankRenderers").Find("TankTurret").Find("FireTransform");

                spawnShoot = turret;
            }

            manager = GameObject.Find("GameManager");

            fire = true;
            currentTime = 0;
        }

        public override TaskStatus OnUpdate()
        {
            //If player has already shooted apply delay and shoot again
            if (!fire)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= delay)
                    fire = true;
            }

        
            
            if (fire && manager.GetComponent<Complete.GameManager>().p2_bullets > 0)
            {
                m_CurrentLaunchForce = (m_CurrentLaunchForce + m_MaxLaunchForce) / m_MinLaunchForce;
                Fire();
                fire = false;
            }
           

            return TaskStatus.RUNNING;

        }

        private void Fire()
        {
            Rigidbody shellInstance =
                GameObject.Instantiate(m_Shell, spawnShoot.position, spawnShoot.rotation) as Rigidbody;

            shellInstance.velocity = m_CurrentLaunchForce * (spawnShoot.forward);

            m_CurrentLaunchForce = 10;

            currentTime = 0;
            --manager.GetComponent<Complete.GameManager>().p2_bullets;

        }

    } // class Shooting

}