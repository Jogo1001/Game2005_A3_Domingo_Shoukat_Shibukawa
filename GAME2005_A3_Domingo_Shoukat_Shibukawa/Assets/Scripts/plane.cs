using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public Transform groundStart;  
    public Transform groundEnd;    
    public float groundFriction = 0.5f;  

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundStart.position, groundEnd.position);
    }

    public bool CheckGroundCollision(ref Vector2 position, ref Vector2 velocity)
    {
        
        Vector2 groundDirection = groundEnd.position - groundStart.position;
        Vector2 groundNormal = new Vector2(-groundDirection.y, groundDirection.x).normalized;  

    
        Vector2 pointToGroundStart = position - (Vector2)groundStart.position;
        float distanceAlongGround = Vector2.Dot(pointToGroundStart, groundDirection.normalized);

       
        if (distanceAlongGround >= 0 && distanceAlongGround <= groundDirection.magnitude)
        {
           
            float displacement = Vector2.Dot(position - (Vector2)groundStart.position, groundNormal);

            if (displacement < 0)
            {
                
                velocity *= (1 - groundFriction);

               
                position -= groundNormal * displacement;

                
                velocity = Vector2.Reflect(velocity, groundNormal);

                return true;  
            }
        }

        return false;  
    }
}
