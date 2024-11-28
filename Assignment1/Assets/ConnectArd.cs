using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using Unity.VisualScripting;
using System;
//using UnityEditor.Experimental.GraphView;

public class ConnectArd : MonoBehaviour
{
    public Transform BulletSpawn;
    public GameObject BulletSpawnPrefab;
    public float bulletSpeed = 10;

    public int ammo = 8;
    int ammoCount = 0;

    private Vector3 velocity;
    private bool isGrounded;
    SerialPort sp = new SerialPort("COM4", 9600);
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float jumpForce = 5f;

    public CharacterController controller;
    public Camera playerCamera;
    private float verticalRotation = 0f;

    public float normalHeight = 2f;
    public float crouchHeight = 1f;

    bool Apressed = false;
    bool Bpressed = false;
    bool Xpressed = false;
    bool Ypressed = false;
    bool Triggerpressed = false;

    public GameObject pistol;
    public GameObject rifle;


    bool moveUP;
    bool moveDOWN;
    bool moveLEFT;
    bool moveRIGHT;

    bool lookUP;
    bool lookDOWN;
    bool lookLEFT;
    bool lookRIGHT;


    // Start is called before the first frame updates
    void Start()
    {

        sp.Open();
        sp.ReadTimeout = 30;
        Debug.Log("Port Opened");
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        rifle.SetActive(true);
        pistol.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        moveDOWN = false;
        moveUP = false;
        moveLEFT = false;
        moveRIGHT = false;
        lookDOWN = false;
        lookUP= false;
        lookLEFT = false;
        lookRIGHT = false;
       




        if (sp.IsOpen)
        {
            try
            {
                // Check if there's data available to read
                if (sp.BytesToRead > 0)
                {
                    int buttonValue = sp.ReadByte(); // Read the byte


                    switch (buttonValue)
                    {
                        case 1:
                            Debug.Log("A is Pressed");
                            Apressed = true;
                            break;
                        case 2:
                            Debug.Log("B is Pressed");
                            Bpressed = true;


                            break;
                        case 3:
                            Debug.Log("X is Pressed");
                            Xpressed = true;
                            break;
                        case 4:
                            Debug.Log("Y is Pressed");
                            Ypressed = true;
                            break;
                        case 5:
                            Debug.Log("Trigger is Pressed");
                            Triggerpressed = true;
                            break;


                        




                      case 6:
                    Debug.Log("Move_UP");
                    moveUP = true;
                    break;


                case 7:
                    Debug.Log("Move_DOWN");
                    moveDOWN = true;
                    break;
                case 8:
                    Debug.Log("Move_RIGHT");
                    moveRIGHT = true;
                    break;
                case 9:
                    Debug.Log("Move_LEFT");
                    moveLEFT = true;
                    break;
                      

                       




                        case 10:
                            //Debug.Log("Look_RIGHT");
                            lookRIGHT = true;
                            Debug.Log("LOOK RIGHT");
                            break;
                        case 11:
                            //Debug.Log("Look_LEFT");
                            lookLEFT = true;
                            Debug.Log("LOOK LEFT");
                            break;
                        case 12:
                            //Debug.Log("Look_UP");
                            lookUP = true;
                            Debug.Log("LOOK  UP");
                            break;
                        case 13:
                            //Debug.Log("Look_DOWN");
                            lookDOWN = true;
                            Debug.Log("LOOK DOWN");
                            break;
                        default:

                            break;
                    }
                }
            }
            catch (TimeoutException)
            {
                Debug.LogWarning("Read timed out.");
            }
        }

        if (Ypressed == true)
        {
            if (rifle.activeSelf)
            {
                pistol.SetActive(true);
                rifle.SetActive(false);
                Ypressed = false;
            }

            else if (pistol.activeSelf)
            {
                rifle.SetActive(true);
                pistol.SetActive(false);
                Ypressed = false;
            }




        }


        else if (Xpressed == true)
        {

            ammoCount = 0;
            Xpressed = false;
            Triggerpressed = false;
        }


        else if (Triggerpressed == true)
        {


            if (ammoCount != 8)
            {
                var bullet = Instantiate(BulletSpawnPrefab, BulletSpawn.position, BulletSpawn.rotation);
                bullet.GetComponent<Rigidbody>().velocity = BulletSpawn.forward * bulletSpeed;
                ammoCount = ammoCount + 1;
                Triggerpressed = false;
            }

            Debug.Log("Out of Bullets");

        }

        else if (Bpressed == true)
        {
            if (controller.height == normalHeight)
            {
                controller.height = crouchHeight;
                Bpressed = false;
            }

            else if (controller.height == crouchHeight)
            {
                controller.height = 2.0f;
                Bpressed = false;
            }

        }


        Move();
        Look();


    }

    private void OnApplicationQuit()
    {
        sp.Close();
        Debug.Log("Port Closed");
    }

    void Move()
    {
        // Check if the player is on the ground
        isGrounded = controller.isGrounded;

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // A small value to keep the player grounded

        }

        // Create a movement direction based on input
        Vector3 direction = Vector3.zero;

        // Use moveUP, moveDOWN, moveLEFT, moveRIGHT flags
        if (moveUP == true)
        {
            direction += -transform.right; // Strafe right
            
            moveUP = false;
            //Debug.Log("up");
        }
        if (moveDOWN == true)
        {
            direction += transform.right; // Strafe left
            
            moveDOWN = false;
            //Debug.Log("down");
        }
        if (moveLEFT == true)
        {
            direction += -transform.forward; // Move backward
            moveLEFT = false;
            //Debug.Log("left");
        }
        if (moveRIGHT == true)
        {
            direction += transform.forward; // Move forward
            moveRIGHT = false;
            //Debug.Log("right");
        }

        // Normalize direction to maintain consistent speed
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }

        // Apply movement
        controller.Move(direction * moveSpeed * Time.deltaTime);

        // Move the player based on the velocity
        controller.Move(velocity * Time.deltaTime);

        // Jumping
        if (isGrounded && Apressed == true)
        {
            velocity.y = jumpForce; // Apply jump force
            Apressed = false;
        }

        // Apply gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;
    }

   
       

    void Look()
    {
        // Adjust camera rotation based on joystick input
        if (lookDOWN == true)
        {
            verticalRotation += lookSpeed; // Look up
            lookDOWN = false; // Reset after processing
        }
        if (lookUP == true)
        {
            verticalRotation -= lookSpeed; // Look down
            lookUP = false; // Reset after processing
        }

        // Adjust horizontal rotation based on joystick input
        if (lookLEFT == true)
        {
            transform.Rotate(Vector3.up, -lookSpeed); // Look left
            lookLEFT = false; // Reset after processing
        }
        if (lookRIGHT == true)
        {
            transform.Rotate(Vector3.up, lookSpeed); // Look right
            lookRIGHT = false; // Reset after processing
        }

        // Clamp vertical rotation to prevent flipping
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

}