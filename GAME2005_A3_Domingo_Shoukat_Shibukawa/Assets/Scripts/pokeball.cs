using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pokeball : MonoBehaviour
{
    public Transform Pokeball_Rest_Position;
    public Transform Slingshot_left;
    public Transform Slingshot_right;
    public float Slingshot_Power = 1f;
    public float Max_Drag_Distance = 1f;
    public LineRenderer Slingshot_Line_Left;
    public LineRenderer Slingshot_Line_Right;
    public Text ScoreText;

    private bool IsDragging = false;
    private Vector2 Drag_Start_Position;
    private Vector2 Drag_Release_Position;
    private int score = 0;

  
    private Vector2 Pokeball_Velocity;
    private Vector2 Pokeball_Acceleration;
    private float Pokeball_Gravity = -9.81f;

    void Start()
    {
        ResetPokeball();
        UpdateScore();

        Slingshot_Line_Left.positionCount = 2;
        Slingshot_Line_Right.positionCount = 2;
        ResetSlingShotLines();
    }

    void Update()
    {
        if (IsDragging)
        {
            
            Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 Displacement = MouseWorldPosition - (Vector2)Pokeball_Rest_Position.position;

            if (Displacement.x > 0) Displacement.x = 0;
            if (Displacement.magnitude > Max_Drag_Distance)
            {
                Displacement = Displacement.normalized * Max_Drag_Distance;
            }
            transform.position = (Vector2)Pokeball_Rest_Position.position + Displacement;

            UpdateSlingShotPosition();
        }
        else
        {
           
            if (transform.position != (Vector3)Pokeball_Rest_Position.position) 
            {
                Pokeball_Acceleration = new Vector2(0, Pokeball_Gravity);
                Pokeball_Velocity += Pokeball_Acceleration * Time.deltaTime;
                transform.position += (Vector3)(Pokeball_Velocity * Time.deltaTime);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!IsDragging)
        {
            IsDragging = true;
            Pokeball_Velocity = Vector2.zero;  
        }
    }

    private void OnMouseUp()
    {
        if (IsDragging)
        {
            IsDragging = false;

            Drag_Release_Position = transform.position;
            Vector2 Displacement = (Vector2)Pokeball_Rest_Position.position - Drag_Release_Position;
            Pokeball_Velocity = Displacement * Slingshot_Power;

            
            ResetSlingShotLines();

            StartCoroutine(ResetPokeballTime(5f));
        }
    }

    private void UpdateSlingShotPosition()
    {
        
        Slingshot_Line_Left.SetPosition(0, Slingshot_left.position);
        Slingshot_Line_Left.SetPosition(1, transform.position);

        Slingshot_Line_Right.SetPosition(0, Slingshot_right.position);
        Slingshot_Line_Right.SetPosition(1, transform.position);
    }



    private void ResetSlingShotLines()
    {
       
        Slingshot_Line_Left.SetPosition(0, Slingshot_left.position);
        Slingshot_Line_Left.SetPosition(1, Pokeball_Rest_Position.position);

        Slingshot_Line_Right.SetPosition(0, Slingshot_right.position);
        Slingshot_Line_Right.SetPosition(1, Pokeball_Rest_Position.position);
    }

    private IEnumerator ResetPokeballTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetPokeball();
    }

    private void ResetPokeball()
    {
        Pokeball_Velocity = Vector2.zero;
        transform.position = Pokeball_Rest_Position.position;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Voltrob"))
        {
            Destroy(collision.gameObject);
            score++;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        ScoreText.text = "SCORE: " + score;
    }
}
