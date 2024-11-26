using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    public float BLock_Mass = 1f; 
    public float Block_Gravity = -9.81f; 
    public float Block_Friction = 0.5f; 
    public float Block_Bounciness = 0.5f; 
    public float Block_Rotation = 0.95f; 
    public float Block_Drag = 0.1f; 

    private Vector2 Block_Velocity; 
    private Vector2 Block_Acceleration;
    private float angularVelocity; 
    private float angularAcceleration; 

    private Rigidbody2D rb; 
    public plane groundPlane; 

    void Start()
    {
       
        Block_Velocity = Vector2.zero;
        Block_Acceleration = new Vector2(0, Block_Gravity);
        angularVelocity = 0f;
        angularAcceleration = 0f;

        rb = GetComponent<Rigidbody2D>(); 
        rb.gravityScale = 1; 
    }

    void Update()
    {
       
        ApplyGravity();
        ApplyFriction();
        ApplyRotationFriction();

       
        transform.position += (Vector3)Block_Velocity * Time.deltaTime;

       
        transform.Rotate(0, 0, angularVelocity * Time.deltaTime);

       
        ApplyDrag();

      
        if (groundPlane != null)
        {
            Vector2 blockPosition = transform.position;
            Vector2 blockVelocity = Block_Velocity;

         
            if (groundPlane.CheckGroundCollision(ref blockPosition, ref blockVelocity))
            {
              
                transform.position = (Vector3)blockPosition;
                Block_Velocity = blockVelocity;
            }
        }
    }

    private void ApplyGravity()
    {
       
        Block_Acceleration = new Vector2(0, Block_Gravity);
        Block_Velocity += Block_Acceleration * Time.deltaTime;
    }

    private void ApplyFriction()
    {
       
        if (Block_Velocity.x != 0)
        {
            float frictionForce = Block_Friction * Mathf.Sign(Block_Velocity.x);
            Block_Velocity.x -= frictionForce * Time.deltaTime;
        }

      
        if (Block_Velocity.y != 0)
        {
            float frictionForce = Block_Friction * Mathf.Sign(Block_Velocity.y);
            Block_Velocity.y -= frictionForce * Time.deltaTime;
        }
    }

    private void ApplyRotationFriction()
    {
       
        angularVelocity *= Block_Rotation;
    }

    private void ApplyDrag()
    {
       
        Block_Velocity *= (1 - Block_Drag);
    }

   
    public void ApplyForce(Vector2 force)
    {
       
        Block_Velocity += force / BLock_Mass;
    }

   
    public void ApplyTorque(float torque)
    {
        angularVelocity += torque;
    }

   
    public void ApplyBounce()
    {
        Block_Velocity.y = -Block_Velocity.y * Block_Bounciness; 
        Block_Velocity.x *= 0.8f;

       
        angularVelocity *= 0.5f;
    }
}