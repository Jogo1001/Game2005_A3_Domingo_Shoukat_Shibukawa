using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public Rigidbody2D pokeball;
    public Rigidbody2D[] voltrobs;
    public float Bounce_Physics = 0.2f;
    public float Friction_Physics = 0.5f;


    private void FixedUpdate()
    {

        Vector2 Plane_Normal = GetPlaneNormal();

        if(pokeball != null)
        {
            PlaneCollision(pokeball, Plane_Normal);
        }


        foreach (Rigidbody2D voltrob in voltrobs )
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

    private void PlaneCollision(Rigidbody2D rb, Vector2 Plane_Normal)
    {
        CircleCollider2D collider = rb.GetComponent<CircleCollider2D>();
        if(collider == null)
        {
            return;
        }

        float radius = collider.radius * rb.transform.lossyScale.x;

        Vector2 Object_Position = rb.position;
        Vector2 Plane_Point = transform.position;

        float Distance_from_Plane = Vector2.Dot(Object_Position - Plane_Point, Plane_Normal)- radius;

        if(Distance_from_Plane < 0)
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
