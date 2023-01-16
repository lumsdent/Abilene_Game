using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 movement;
    Transform playerTransform;
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
        StartCoroutine("CircleCheck");
    }

    IEnumerator CircleCheck()
    {
        if (DecayCircle.IsOutsideCircle(playerTransform.localPosition))
        {
            Debug.Log("Outside");
        }
            // execute block of code here
            yield return new WaitForSeconds(1f);
        
    }

    void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
    }
}
