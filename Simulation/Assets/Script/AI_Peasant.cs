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

    void CalculateAngle();
    
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
        
    }
}
