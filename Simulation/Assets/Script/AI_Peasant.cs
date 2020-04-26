using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Peasant : MonoBehaviour
{
    public float speed = 5.0f; //can manually change speed in Unity to quickly adjust
    public Transform goal;//position of an object the AI will move to
    public int mag = 5; // magnitude distance
    public bool drawRay = true;
    public GameObject ore;
    
    void Start()
    {
        
    }

    void CalculateAngle() //Will calculate the angle
    {
        Vector3 curF = this.transform.up; //character direction
        Vector3 curD = ore.transform.position - this.transform.position; //Ore direction

        float dotProduct = ((curF.x * curD.x) + (curF.y * curD.y));
        float angle = Mathf.Acos(dotProduct /(curF.magnitude*curD.magnitude)); //This is the Angle  θ=cos^-1((v*w)/(||v||*||w||))

        Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
        Debug.Log("Angle: " + Vector3.Angle(curF,curD));

        Debug.DrawRay(this.transform.position, curF * 5, Color.blue, 2); //Debug to find the direction of the tank
        Debug.DrawRay(this.transform.position, curD, Color.yellow, 2); //Debug to find the direction of the ore

        
        //int clockwise = 1;
        //   if( Cross(curF,curD).z < 0)//This will test to see where the angle is and adjust the objects direction
        //   {
        //       clockwise = -1;
        //   }
        //this.transform.Rotate(0,0,angle * Mathf.Rad2Deg*clockwise);

        float unityAngle = Vector3.SignedAngle(curF, curD, this.transform.forward); // unity angle
        this.transform.Rotate(0,0,unityAngle); //this is the short and simple version


    }

    Vector3 Cross(Vector3 v, Vector3 w) //CrossProduct
    {
        float xMult = v.y * w.z - v.z * w.y; //Check the pattern, x doesn't cross the x coordinates
        float yMult = v.z * w.x - v.x * w.z; //Check the pattern, y doesn't cross any y coordinates
        float zMult = v.x * w.y - v.y * w.x; //Check the pattern, z doesn't cross the z coordinates
        Vector3 crossProd = new Vector3(xMult, yMult, zMult);
        return crossProd;
    }
    
    void CalculateDistance()  //Will calculate distance from object
    {
        Vector3 currentPosition = this.transform.position;
        Vector3 orePosition = ore.transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow((currentPosition.x - orePosition.x), 2)
            + Mathf.Pow((currentPosition.y - orePosition.y),2));
        float unityDistance = Vector3.Distance(currentPosition,orePosition);// will calculate distance using the <Distance> method

        Debug.Log("Distance: " + distance);
        Debug.Log("Unity Distance" + unityDistance);
    }

    void Update()
    {
        // this.transform.LookAt(goal.position); ##Only usable in 3D## Ai will face target

        Vector2 direction = goal.position - this.transform.position; //calculate the vector to move to position
        if (drawRay == true)
        {
            Debug.DrawRay(this.transform.position, direction, Color.red); //Use to debug vectors
        }
        
        if(direction.magnitude > mag) //magnitude will halt the AI position if too close
        {
            this.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateDistance();
            CalculateAngle();
        }
        CalculateDistance();
        CalculateAngle();
    }
}
