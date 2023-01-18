using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 movement;
    Transform playerTransform;
    [SerializeField] float moveSpeed;
    Animator animator;
    string currentState;
    // Start is called before the first frame update
    void Awake()
    {
        animator= GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
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
