using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Player : MonoBehaviour

{

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private bool isGrounded;
    private Vector3 _moveDir;
    private Rigidbody rb;


    // Start is called before the first frame update

    void Start()

    {

        InputManager.Init(this);

        InputManager.GameMode();
        rb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame

    void Update()

    {

        transform.position += speed * Time.deltaTime * _moveDir;
        CheckGround();

    }



    public void SetMovementDirection(Vector3 newDirection)

    {

        _moveDir = newDirection;



    }

    public void Jump()
    {
        Debug.Log("Jump called");
        if (isGrounded)
        {
            Debug.Log("I jumped!");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.size.y);
        Debug.DrawRay(transform.position, Vector3.down * GetComponent<Collider>().bounds.size.y, Color.green, 0, false);
    }



}

