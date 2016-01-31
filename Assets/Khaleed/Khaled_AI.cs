using UnityEngine;
using System.Collections;

public class Khaled_AI : MonoBehaviour {

    Rigidbody khaled_bod;
    int counter;

	public float xStart;// = 369.56f;
	public float zStart;// = 241.92f;

	// Use this for initialization
	void Start () 
    {
        khaled_bod = GetComponent<Rigidbody>();
		xStart = khaled_bod.position.x;
		zStart = khaled_bod.position.z;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        //khaled_bod.velocity = movement * 5;

        float moveHorizontal = 0.0f; //Input.GetAxis("Horizontal");
        float moveVertical = 0.0f; //Input.GetAxis("Vertical");

        randomMove(ref moveHorizontal, ref moveVertical);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        Vector3 positions = khaled_bod.position;

        float curX = positions.x;
        float curZ = positions.z;

        counter++;

        if (counter >= 45)
        {
            float zPlus = curZ + 1.5f;
            float xPlus = curX + 1.5f;
            float zMinus = curZ - 1.5f;
            float xMinus = curX - 1.5f;


            if (zPlus > zStart + 50)
            {
                movement.z = 0;
            }
            else if (xPlus > xStart + 50)
            {
                movement.x = 0;
            }
            else if (xMinus < xStart - 50)
            {
                movement.x = 0;
            }
            else if (zMinus < zStart - 50)
            {
                movement.z = 0;
            }

            khaled_bod.velocity = movement;

            counter = 0;
        }
	}

    void randomMove(ref float x, ref float z)
    {
        //random 0 or 1 for x or z value
        int direction = (int)Random.Range(0, 100);

        if (direction <= 33)//if 0, then x
        {
            x = Random.Range(0, 10);
            z = 0;
        }

        if (direction > 33 && direction <= 66)//if 0, then x
        {
            x = Random.Range(0, 10);
            z = Random.Range(0, 10); ;
        }

        else//else z
        {
            z = Random.Range(0, 10);
            x = 0;
        }

        //random 0 or 1 for - or + value
        int sign = (int)Random.Range(0, 100);
        if (sign <= 25)//if 0, then positive
        {
            z *= -1;
            x *= -1;
        }
        else if (sign > 25 && sign <= 50)
        {
            z *= -1;
            //x *= 1;
        }
        else if (sign > 50 && sign <= 75)
        {
            x *= -1;
            //x *= 1;
        }
        else//else negative
        {

        }
    }
}
