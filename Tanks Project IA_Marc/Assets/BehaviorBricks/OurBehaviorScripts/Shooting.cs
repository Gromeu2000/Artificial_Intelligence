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

        [OutParam("BulletNumber")]
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

            numberBullets = 3;
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

            if (fire && numberBullets>0)
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
            --numberBullets;

        }

    } // class Shooting

}