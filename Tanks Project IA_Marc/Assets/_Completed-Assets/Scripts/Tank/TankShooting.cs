using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_MinLaunchForce;        // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
        public float m_CurrentLaunchForce;

       
               // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        //private bool m_Fired;                       // Whether or not the shell has been launched with this button press.



        //STUFF

        public float ShootCD;
        private float TimerCurr;

        public GameObject CurrentTank;
        public GameObject EnemyTank;

        private void OnEnable()
        {
            m_CurrentLaunchForce = m_MinLaunchForce;
          
        }

        private void Start ()
        {
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }

        private void Update ()
        {
            m_AimSlider.value = m_MinLaunchForce;
            float DistanceVal = Vector3.Distance(CurrentTank.transform.position, EnemyTank.transform.position);

                TimerCurr += Time.deltaTime;

                if (TimerCurr >= ShootCD)
                {
                    m_CurrentLaunchForce = 30;

                    Fire();

                    TimerCurr = 0;
                }
        }

        private void Fire ()
        {
            //m_Fired = true;

            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; 

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
    }
}