using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pokeball : MonoBehaviour
{
    public Transform Pokeball_Rest_Position;
    public Transform Slingshot_left;
    public Transform Slingshot_right;
    public Rigidbody2D Pokeball;
    public float Slingshot_Power = 0f;
    public float Drag_Distance = 0f;
    public LineRenderer Slingshot_Line_Left;
    public LineRenderer Slingshot_Line_Right;
    public Text ScoreText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
