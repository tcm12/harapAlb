using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidBody2D;

    [SerializeField] float movingSpeed = 1f;
    Vector2 flip = new Vector2(1f, 1f);

    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myRigidBody2D.velocity = new Vector2(movingSpeed, 0f);
    }

    private void flipOnTouch()
    {
        myRigidBody2D.velocity = new Vector2((-1) * myRigidBody2D.velocity.x, 0f);
        flip = new Vector2(flip.x * (-1), 1f);
        myRigidBody2D.transform.localScale = flip;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        flipOnTouch();
    }
}
