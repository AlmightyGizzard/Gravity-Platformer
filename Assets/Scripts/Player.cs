using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ControllerRY))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    public bool gravityReversed = false;
    public float accelerationGrounded = 0.2f;
    public float accelerationAir = 0.1f;
    public float speed;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float gravity;
    public int maxGravCharges;
    public int gravCharges;
    private bool isFloorChecking;

    public float jumpVelocity;
    [SerializeField]
    Vector3 velocity;
    float velocityXsmoothing;

    SpriteRenderer face;

    AudioManager audio;


    ControllerRY controller;
    // Start is called before the first frame update
    void Start()
    {
        audio = FindObjectOfType<AudioManager>();
        controller = GetComponent<ControllerRY>();
        face = GetComponent<SpriteRenderer>();
        gravCharges = maxGravCharges;
        print("Gravity: " + gravity + "  Jumpy Velocity: " + jumpVelocity);
    }

    void recalculateGravity()
    {
        float result;
        if (gravityReversed)
        {
            result = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        }
        else
        {
            result = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        }
        if (velocity.y < -1)
        {
            result -= fallMultiplier;
        }
        else if (velocity.y > 0 && Input.GetKey(KeyCode.W))
        {
            result += lowJumpMultiplier;
        }
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        gravity = result;
    }



    // Update is called once per frame
    void Update()
    {
        //recalculateGravity();
        if (gravityReversed)
        {
            gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        }
        else
        {
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        }

        if (velocity.y < -1)
        {
            gravity -= fallMultiplier;
        }
        else if (velocity.y > 0 && Input.GetKey(KeyCode.W))
        {
            gravity -= lowJumpMultiplier;
        }


        if (controller.collisions.above || controller.collisions.below)
        {
            if ((velocity.y > 1 || velocity.y < -1))
            {
                audio.Play("Land");
                gravCharges = maxGravCharges;
                face.color = Color.white;
            }
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.W) && controller.collisions.below && gravity < 0)
        {
            //Debug.Log("Jumping");
            FindObjectOfType<AudioManager>().Play("Jump");
            velocity.y = jumpVelocity;
        }
        else if (Input.GetKeyDown(KeyCode.W) && controller.collisions.above && gravity > 0)
        {
            //Debug.Log("Jumping");
            audio.Play("Jump");
            velocity.y = -jumpVelocity;
        }

        if (Input.GetKeyDown(KeyCode.Space) && gravCharges > 0)
        {
            if (gravityReversed)
            {
                audio.Play("Grav");
                gravityReversed = false;
                face.color = (gravCharges == 2) ? Color.yellow : Color.magenta;
                gravCharges--;
            }
            else
            {
                audio.Play("Grav");
                gravityReversed = true;
                face.color = (gravCharges == 2) ? Color.yellow : Color.magenta;
                gravCharges--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            audio.Play("Quack");
        }

        

        float targetVelocityX = input.x * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXsmoothing, (controller.collisions.below) ? accelerationGrounded : accelerationAir);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator FloorCheck(float time)
    {
        if (isFloorChecking)
            yield break;
        isFloorChecking = true;
        yield return new WaitForSeconds(time);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            gravCharges = maxGravCharges;
            face.color = Color.blue;
        }
        isFloorChecking = false;
    }
}
