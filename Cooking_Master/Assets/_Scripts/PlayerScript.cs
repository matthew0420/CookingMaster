using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public bool isPlayer1;

    public float speed = 2f;

    Rigidbody2D rigidBody;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;
        if (isPlayer1)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
        } else
        {
            input.x = Input.GetAxisRaw("Horizontal2");
            input.y = Input.GetAxisRaw("Vertical2");
        }

        Vector3 direction = input.normalized;

        Vector3 movement = direction * speed * Time.fixedDeltaTime;

        rigidBody.MovePosition(transform.position + movement);
    }
}
