using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pokeball : MonoBehaviour
{
    public Transform Pokeball_Rest_Position;
    public Transform Slingshot_left;
    public Transform Slingshot_right;
    public float Slingshot_Power = 8f;
    public float Max_Drag_Distance = 2f;
    public LineRenderer Slingshot_Line_Left;
    public LineRenderer Slingshot_Line_Right;
    public Text ScoreText;
    public plane Plane_Horizontal; // reference plane script
    public plane Plane_Inclined; // reference plane script

    public float Mass = 1f; // Mass 
    public float Gravity = -10f; // Gravity 

    public Sprite PokeballSprite;
    public Sprite RockSprite;
    public Button ChangeButton;

    private SpriteRenderer spriteRenderer;
    private bool IsDragging = false;
    private Vector2 Drag_Start_Position;
    private Vector2 Drag_Release_Position;
    private int score = 0;

    private Vector2 Pokeball_Velocity;
    private Vector2 Pokeball_Acceleration;

    void Start()
    {
        ResetPokeball();
        UpdateScore();

        Slingshot_Line_Left.positionCount = 2;
        Slingshot_Line_Right.positionCount = 2;
        ResetSlingShotLines();

        spriteRenderer = GetComponent<SpriteRenderer>();


        ChangeButton.onClick.AddListener(TogglePokeballType);

        SetPokeball();
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

                Pokeball_Acceleration = new Vector2(0, Gravity / Mass);
                Pokeball_Velocity += Pokeball_Acceleration * Time.deltaTime;

                Vector2 newPosition = (Vector2)transform.position + Pokeball_Velocity * Time.deltaTime;


                if (Plane_Horizontal.CheckGroundCollision(ref newPosition, ref Pokeball_Velocity))
                {
                    transform.position = newPosition;
                }

                if (Plane_Inclined.CheckGroundCollision(ref newPosition, ref Pokeball_Velocity))
                {
                    transform.position = newPosition;
                }
                else
                {
                    transform.position = newPosition;
                }


                VoltrobCollision(newPosition);
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

            Pokeball_Velocity = Displacement * Slingshot_Power * Mass;

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

    void VoltrobCollision(Vector2 pokeballPosition)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(pokeballPosition, 0.1f);

        foreach (Collider2D hit in hitEnemies)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                Destroy(hit.gameObject);
                score++;
                UpdateScore();
            }
        }
    }

    private void UpdateScore()
    {
        ScoreText.text = "SCORE: " + score;
    }

    public void TogglePokeballType()
    {

        if (spriteRenderer.sprite == PokeballSprite)
        {
            spriteRenderer.sprite = RockSprite;
            Mass = 5f;  // mass for the stone
            Gravity = -30f;
            Slingshot_Power = 1;

        }
        else
        {
            spriteRenderer.sprite = PokeballSprite;
            Mass = 1f; //mass for the Pokeball
            Gravity = -10f;
            Slingshot_Power = 8;
        }
    }

    private void SetPokeball()
    {
        spriteRenderer.sprite = PokeballSprite;
        Mass = 1f;
        Slingshot_Power = 8;

    }
}
