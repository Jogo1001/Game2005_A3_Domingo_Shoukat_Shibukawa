using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public GameObject pokeball; 
    public Rigidbody2D[] voltrobs;
    public float Bounce_Physics = 0.2f;
    public float Friction_Physics = 0.5f;

    private Vector2 pokeballVelocity;  

    private void FixedUpdate()
    {
        Vector2 Plane_Normal = GetPlaneNormal();

      
        if (pokeball != null)
        {
            PlaneCollisionForGameObject(pokeball, ref pokeballVelocity, Plane_Normal);
        }

      
        foreach (Rigidbody2D voltrob in voltrobs)
        {
            if (voltrob != null)
            {
                PlaneCollision(voltrob, Plane_Normal);
            }
        }
    }

    private Vector2 GetPlaneNormal()
    {
        return transform.up;
    }

    
    private void PlaneCollisionForGameObject(GameObject obj, ref Vector2 velocity, Vector2 Plane_Normal)
    {
        CircleCollider2D collider = obj.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            return;
        }

        
        float radius = collider.radius * obj.transform.lossyScale.x;
        Vector2 Object_Position = obj.transform.position;
        Vector2 Plane_Point = transform.position;

      
        float Distance_from_Plane = Vector2.Dot(Object_Position - Plane_Point, Plane_Normal) - radius;

       
        if (Distance_from_Plane < 0)
        {
            
            obj.transform.position += (Vector3)(-Distance_from_Plane * Plane_Normal);

           
            velocity -= (1 + Bounce_Physics) * Vector2.Dot(velocity, Plane_Normal) * Plane_Normal;
            Vector2 tangent = velocity - Vector2.Dot(velocity, Plane_Normal) * Plane_Normal;
            velocity -= tangent * Friction_Physics;

           
            pokeballVelocity = velocity;

           
            obj.transform.position += (Vector3)(velocity * Time.fixedDeltaTime);
        }
    }

    
    private void PlaneCollision(Rigidbody2D rb, Vector2 Plane_Normal)
    {
        CircleCollider2D collider = rb.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            return;
        }

        float radius = collider.radius * rb.transform.lossyScale.x;

        Vector2 Object_Position = rb.position;
        Vector2 Plane_Point = transform.position;

        float Distance_from_Plane = Vector2.Dot(Object_Position - Plane_Point, Plane_Normal) - radius;

        if (Distance_from_Plane < 0)
        {
            rb.position += -Distance_from_Plane * Plane_Normal;
            Vector2 velocity = rb.velocity;
            velocity -= (1 + Bounce_Physics) * Vector2.Dot(velocity, Plane_Normal) * Plane_Normal;
            Vector2 tangent = velocity - Vector2.Dot(velocity, Plane_Normal) * Plane_Normal;
            velocity -= tangent * Friction_Physics;
            rb.velocity = velocity;
        }
    }
}
