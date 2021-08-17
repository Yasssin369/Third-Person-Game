using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour
{
   public CharacterController controller;

    public float health = 1000;
    public float currentHealth;
    public HealthBar healthBar;
    public  float speed = 6f;
    public float jump = 3f;
    public float gravity = -9.81f;

    public Animator animator;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public float rotationSpeed = 8f;

    public Transform Cam;

    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
     bool isGrounded;
    public WeaponManager weaponManager;
    public GameManager gameManager;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cam = Camera.main.transform;
        weaponManager = GetComponentInChildren<WeaponManager>();
        currentHealth = health;
        healthBar.SetMaxHealth(currentHealth);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
            controller.slopeLimit = 45.0f;
            animator.SetBool("isJumping", false);
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //////////////////
        ////////jump and gravity
        /////////////////
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("isJumping", true);
            velocity.y = Mathf.Sqrt(jump * -2 * gravity);
            controller.slopeLimit = 100.0f;


            Debug.Log("jump jump jump");
            if ((controller.collisionFlags & CollisionFlags.Above) != 0)

            {

                velocity.y = -2f;

            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        ///new lines so player can rotate with cameramovement for aiming 
        ///disable them if you want free look around 
        ///the player
        Quaternion targertRotation = Quaternion.Euler(0, Cam.transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targertRotation, rotationSpeed * Time.deltaTime);

        if (direction.magnitude >=0.1)
        {
            animator.SetBool("isRunning", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg+Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,turnSmoothTime);
            /*
             * makes you move in the direction you 
             * looking at with the camera
             */
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.E))weaponManager.SwitchWeapon((weaponManager.weaponindex < 1)? weaponManager.weaponindex+1:0);

    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth<= 0)
        {
            //Destroy(gameObject);
           // SceneManager.LoadScene(0);
            gameManager.EndGame();
        }
    }
}
