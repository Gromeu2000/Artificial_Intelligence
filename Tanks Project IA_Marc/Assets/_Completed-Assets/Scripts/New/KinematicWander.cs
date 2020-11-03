using UnityEngine;
using System.Collections;

public class KinematicWander : MonoBehaviour {

	public float max_angle = 0.5f;

    public float secToChange = 0.5f;
    private float checkpoint;

    public float chanceEscalation = 5;
    private short way = 1;
    private float chanceToSway = 0;

    Move move;
    
	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// Update is called once per frame
	void Update () 
	{
        // TODO 9: Generate a velocity vector in a random rotation (use RandomBinominal) and some attenuation factor
        if (move.mov_velocity.magnitude == 0)   //If Idle, set velocity vector in a random direction
        {
            Vector2 vec;
            vec = Random.insideUnitCircle.normalized;
            move.mov_velocity.Set(vec.x * move.max_mov_velocity, 0.0f, vec.y * move.max_mov_velocity);
        }
        else if (Time.time - checkpoint > secToChange) // Else, deviate vector by random angle
        {
            Vector3 velVec = new Vector3(move.mov_velocity.x, 0.0f, move.mov_velocity.z);

            /*
            Angle variations will tend to be in the same way (way, pos/neg), but the longer the
            curve holds (chanceEscalation) the higher the chances to swap ways (chanceToSway)
            */
            float angleDev = Random.Range(0, max_angle);
            if (Random.Range(0, 100) < chanceToSway)
            {
                way *= -1;
                chanceToSway = 0;
            }
            else
            {
                chanceToSway += chanceEscalation;
            }

            angleDev *= way;
            velVec = Quaternion.AngleAxis(Mathf.Rad2Deg * angleDev, Vector3.up) * velVec;
            move.mov_velocity = velVec;

            checkpoint = Time.time;
        }

        
    }
}
