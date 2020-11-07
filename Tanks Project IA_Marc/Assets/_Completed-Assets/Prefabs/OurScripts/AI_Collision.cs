using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Collision : MonoBehaviour
{
    public Collision CollisionItem1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision ColliderItem)
    {

        Debug.Log("Collision Detected");
        Debug.Log(ColliderItem.gameObject.name);
    }
}
