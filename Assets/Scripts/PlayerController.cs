using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpSpeed;
    [SerializeField] private LayerMask ground;
 
    private PlayerControls playerControls;
    private Rigidbody2D myRB;
    private Collider2D col;

    private void Awake(){
        playerControls = new PlayerControls();
        myRB = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

        private void OnDisable(){
        playerControls.Disable();
    }

    void Start()
    {
        playerControls.Ground.Jump.performed += _ => Jump();
    }

    private void Jump(){
        if(IsGrounded()){
            myRB.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded(){
        Vector2 topLeftPoint = transform.position;
        topLeftPoint.x -= col.bounds.extents.x;
        topLeftPoint.y += col.bounds.extents.y;

        Vector2 bottomRight = transform.position;
        bottomRight.x += col.bounds.extents.x;
        bottomRight.y -= col.bounds.extents.y;

        return Physics2D.OverlapArea(topLeftPoint, bottomRight, ground);
    }

    void Update()
    {
        // Read movement value
        float movementInput = playerControls.Ground.Move.ReadValue<float>();

        //Move Player
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;
    }
}
