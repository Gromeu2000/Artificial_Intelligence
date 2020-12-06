using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Tasks;     // TaskStatus
using Pada1.BBCore.Framework; // BasePrimitiveAction

[Action("OurBehaviorScripts/Shooting")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
      "specified velocity.")]
public class Shooting : BasePrimitiveAction
{
   
    [InParam("shell")]
    [Help("Bullet shoot")]
    public Rigidbody m_Shell;

    [InParam("spawnTransform")]
    [Help("Starting position of the shell")]
    public Transform m_FireTransform;

    [InParam("minLaunchForce")]
    public float m_MinLaunchForce;

    [InParam("maxLaunchForce")]
    public float m_MaxLaunchForce = 30f;

    [InParam("currentLaunchForce")]
    public float m_CurrentLaunchForce;

    [InParam("currentTank")]
    public GameObject CurrentTank;

    [InParam("enemyTank")]
    public GameObject EnemyTank;

    [InParam("numBullet")]
    bool bullet { get; set; }

    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    public override void OnStart()
    {
        bullet = true;
    }

    public override TaskStatus OnUpdate()
    {
      
    
        m_CurrentLaunchForce = 30;

        Fire();

        bullet = false;

        return TaskStatus.RUNNING;
    
    }

    private void Fire()
    {
        Rigidbody shellInstance =
            GameObject.Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = m_CurrentLaunchForce * (m_FireTransform.forward);

        m_CurrentLaunchForce = m_MinLaunchForce;
    }

} // class Shooting
