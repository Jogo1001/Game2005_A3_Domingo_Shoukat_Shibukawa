using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class pokeball : MonoBehaviour
{
    public Transform Pokeball_Rest_Position;
    public Transform Slingshot_left;
    public Transform Slingshot_right;
    public Rigidbody2D Pokeball;
    public float Slingshot_Power = 1f;
    public float Max_Drag_Distance = 1f;
    public LineRenderer Slingshot_Line_Left;
    public LineRenderer Slingshot_Line_Right;
    public Text ScoreText;


    private bool IsDragging = false;
    private Vector2 Drag_Start_Position;
    private Vector2 Drag_Release_Position;
    private int score = 0;
    void Start()
    {
        ResetPokeball();
        UpdateScore();


        Slingshot_Line_Left.positionCount = 2;
        Slingshot_Line_Right.positionCount = 2;
        ResetSlingShotLines();




    }

    // Update is called once per frame
    void Update()
    {
        if (IsDragging)
        {
            Vector2 MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 Displacement = MouseWorldPosition - (Vector2)Pokeball_Rest_Position.position;

            if (Displacement.x > 0) Displacement.x = 0;
            if(Displacement.magnitude > Max_Drag_Distance)
            {
                Displacement = Displacement.normalized * Max_Drag_Distance;
            }
            Pokeball.transform.position = (Vector2)Pokeball_Rest_Position.position + Displacement;

            UpdateSlingShotPosition();

        }

    }


    private void OnMouseDown()
    {
        if (!IsDragging)
        {
            IsDragging = true;
            Pokeball.isKinematic = true;
        }
    }

    private void OnMouseUp()
    {
        if (IsDragging)
        {
            IsDragging = false;
            Pokeball.isKinematic = false;

            Drag_Release_Position = Pokeball.transform.position;
            Vector2 Displacement = (Vector2)Pokeball_Rest_Position.position - Drag_Release_Position;
            Vector2 LaunchVelocity = Displacement * Slingshot_Power / Pokeball.mass;

            Pokeball.velocity = LaunchVelocity;

            ResetSlingShotLines();

            StartCoroutine(ResetPokeballTime(5f));
        }

    }
    private void UpdateSlingShotPosition()
    {
        Slingshot_Line_Left.SetPosition(0, Slingshot_left.position);
        Slingshot_Line_Left.SetPosition(1, Pokeball.transform.position);

        Slingshot_Line_Right.SetPosition(0, Slingshot_right.position);
        Slingshot_Line_Right.SetPosition(1, Pokeball.transform.position);
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
        Pokeball.isKinematic = true;
        Pokeball.velocity = Vector2.zero;
        Pokeball.angularVelocity = 0f;
        Pokeball.transform.position = Pokeball_Rest_Position.position;
        ResetSlingShotLines();
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
