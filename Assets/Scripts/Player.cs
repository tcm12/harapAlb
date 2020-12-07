using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;
    
    float startingGravity;

    int lives = 3;
    int resetLives = 3;
    [SerializeField] Text livesText;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(-25f, 25f);

    bool isAlive = true;
    Vector2 myPosition;

    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        lives = data.lives;
        resetLives = data.resetLives;
        livesText.text = resetLives.ToString();
        Debug.Log("Lives" + lives);
        Debug.Log("ResetLives" + resetLives);

        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        startingGravity = myRigidBody.gravityScale;
    }

    void Update()
    {
        if (resetLives == 0)
        {
            resetLives = 3;
            SaveSystem.SavePlayer(this);
            SceneManager.LoadScene(0);
        }

        if (!isAlive)
        {
            if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Rigidbody2D.Destroy(myRigidBody, 0f);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            return;
        }

        MovePlayer();
        Jump();
        Climb();
        Die();
    }


    public int GetLives()
    {
        return lives;
    }

    public int currentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            resetLives--;
            GetComponent<Rigidbody2D>().velocity = deathKick;

            myAnimator.SetBool("Dead", true);
            SaveSystem.SavePlayer(this);
        }
    }
     
    public void StartTheGame()
    {
        lives = 3;
        resetLives = 3;

        SaveSystem.SavePlayer(this);
        SceneManager.LoadScene(1);
    }

    public int GetResetLives()
    {
        return resetLives;
    }

    private void MovePlayer()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        float horizontalSpeed = myRigidBody.velocity.x;

        myRigidBody.velocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        Vector2 leftFlip = new Vector2(-1f, 1f);
        Vector2 rightFlip = new Vector2(1f, 1f);


        if (horizontalSpeed > 0)
        {
            myAnimator.SetBool("Running", true);
            myRigidBody.transform.localScale = rightFlip;
        }
        else if (horizontalSpeed < 0)
             {
             myAnimator.SetBool("Running", true);
             myRigidBody.transform.localScale = leftFlip;
             }
             else 
             { 
             myAnimator.SetBool("Running", false);
             }
    }

    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
            return;
        else
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity += jumpVelocity;
            }
    }

    private void Climb()
    {
        if (!myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder"))) 
        {
            myAnimator.SetBool("Climbing Ladder", false);
            myRigidBody.gravityScale = startingGravity;
            return;
        }
        else
        {
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            myRigidBody.gravityScale = 0;

            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

            if (playerHasVerticalSpeed) 
            { 
                myAnimator.SetBool("Climbing Ladder", true);
                myAnimator.SetBool("Running", false);
            }
        }
    }

    public void SavePlayer()
    {
        lives = resetLives;
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        resetLives = data.lives;

        SaveSystem.SavePlayer(this);
        SceneManager.LoadScene(data.level);
    }
}
