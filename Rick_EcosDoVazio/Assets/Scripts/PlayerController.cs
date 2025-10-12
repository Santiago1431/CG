using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rgb;

    public float moveSpeed;
    public float jumpForce;

    public Transform groundPoint;
    public bool isOnGround;
    public LayerMask whatIsGround;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rgb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rgb.linearVelocity.y);
    
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);

        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            rgb.linearVelocity = new Vector2(rgb.linearVelocity.x, jumpForce);
        }
    }
}
