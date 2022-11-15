using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody PlayerRB;
    public Camera PlayerCamera;
    public float mouseSensitivity = 90f;
    public float playerSpeed = 10f;
    public float jumpForce = 10f;
    public GameObject PortalA;
    public GameObject PortalB;
    public GameObject Player;
    public GameObject SpawnPointA;
    public GameObject SpawnPointB;
    public GameObject PortalGun;
    public GameObject Shootgun;
    public float AttackPoints = 5f;
    public GameObject AmmoShotgun;
    public int AmmoCount = 10;
    public Text AmmoCounterText;


    private Vector3 targetPosition;
    private float rotationOnX;
    private bool isJumping;
    private Vector3 prevoiusMousePosition;
    private Vector3 prevoiusPlayerPosition;
    private float z = 0;
    private bool isWalking = false;
    private bool usePortalGun = false;
    private Animator shotGunAnimator;
    private int animatorExitTime = 38;
    private bool animatorState = false;



    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        shotGunAnimator = GetComponentInChildren<Animator>();
    }


    void Update()
    {

        if (Input.mousePosition != prevoiusMousePosition)
        {
            rotateCamera();
        }

        changeWeaponType();
        playerMovement();
        MouseButton();

        if (PlayerRB.transform.position == prevoiusPlayerPosition)
        {
            isWalking = false;
        }
        prevoiusPlayerPosition = PlayerRB.transform.position;

        ArenaSpawnHandler.PlayerPosition = transform.position;

        if (animatorState)
        {
            animatorExitTimeCounter();
        }

        AmmoCounterText.text = "AMMO: " + AmmoCount.ToString();
  
    }
    private void playerMovement()
    {
        if (Input.GetKey(KeyCode.E))
        {
            openDoor();
        }
        if (Input.GetKey(KeyCode.W))
        {
            PlayerMoveForward();
            isWalking = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerMoveBackward();
            isWalking = true;

        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerMoveRight();
            isWalking = true;

        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerMoveLeft();
            isWalking = true;

        }
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            PlayerJump();
            isJumping = true;
        }

    }

    private void MouseButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (usePortalGun)
            {
                setPortalA();
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (usePortalGun)
            {
                setPortalB();
            }
            else
            {

                shootEnemy();
            }

        }
    }

    private void changeWeaponType()
    {
        if (Input.GetKey(KeyCode.Alpha2))
        {
            usePortalGun = true;
            PortalGun.SetActive(true);
            Shootgun.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            usePortalGun = false;
            PortalGun.SetActive(false);
            Shootgun.SetActive(true);
        }
    }

    private void rotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;

        rotationOnX -= mouseY;
        rotationOnX = Mathf.Clamp(rotationOnX, -90f, 90f);
        PlayerCamera.transform.localEulerAngles = new Vector3(rotationOnX, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    #region Player Movement
    private void PlayerMoveForward()
    {
        transform.position += transform.forward * Time.deltaTime * playerSpeed;
    }
    private void PlayerMoveBackward()
    {
        transform.position -= transform.forward * Time.deltaTime * playerSpeed;
    }
    private void PlayerMoveRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
    }
    private void PlayerMoveLeft()
    {
        transform.Translate(-Vector3.right * Time.deltaTime * playerSpeed);
    }
    private void PlayerJump()
    {
        PlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    #endregion
    #region Portal
    private void setPortalA()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            if (hit.collider.gameObject.tag == "Ground")
            {
                Vector3 lookDirection = targetPosition - transform.position;
                var rotation = Quaternion.LookRotation(lookDirection).eulerAngles;
                PortalA.transform.rotation = Quaternion.Euler(0,rotation.y+90f,0);
                Vector3 positionNormalized = new Vector3(targetPosition.x, targetPosition.y + 2f, targetPosition.z);
                PortalA.transform.position = positionNormalized;
            }

        }
    }
    private void setPortalB()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            if (hit.collider.gameObject.tag == "Ground")
            {
                Vector3 lookDirection = targetPosition - transform.position;
                var rotation = Quaternion.LookRotation(lookDirection).eulerAngles;
                PortalB.transform.rotation = Quaternion.Euler(0, rotation.y + 90f, 0);
                Vector3 positionNormalized = new Vector3(targetPosition.x, targetPosition.y + 2f, targetPosition.z);
                PortalB.transform.position = positionNormalized;
            }


        }
    }
    #endregion

    private void openDoor()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag == "ArenaSpawner")
            {
                ArenaSpawnHandler.SpawnNewArena = true;
                Animator animator = hit.collider.gameObject.GetComponentInParent<Animator>();
                animator.SetBool("isOpen", true);

            }


        }
    }

    private void shootEnemy()
    {
        if (AmmoCount > 0)
        {
            var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;



            if (Physics.Raycast(Ray, out hit))
            {
                targetPosition = hit.point;
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    EnemyController enemy = hit.collider.gameObject.GetComponentInParent<EnemyController>();
                    enemy.HealthPoints -= AttackPoints;
                    Debug.Log(enemy.HealthPoints);
                    if (enemy.HealthPoints <= 0)
                    {
                        Vector3 newTransfortm = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                        Instantiate(AmmoShotgun, newTransfortm, transform.rotation);
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
            shotGunAnimator.SetBool("isShooting", true);
            animatorState = true;
        }
        AmmoCount--;
        if (AmmoCount < 0)
        {
            AmmoCount = 0;
        }


    }

    private void animatorExitTimeCounter()
    {
        animatorExitTime--;
        if (animatorExitTime <= 0)
        {
            shotGunAnimator.SetBool("isShooting", false);
            animatorState = false;
            animatorExitTime = 38;
        }
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            ArenaSpawnHandler.LastArenaCenter = collision.gameObject.transform.position;
        }
        if (collision.gameObject.tag == "PortalA")
        {
            transform.position = SpawnPointB.transform.position;
            transform.rotation = SpawnPointB.transform.rotation;

        }
        if (collision.gameObject.tag == "PortalB")
        {
            transform.position = SpawnPointA.transform.position;
            transform.rotation = SpawnPointA.transform.rotation;
        }
        if (collision.gameObject.tag == "Doorway")
        {
            isJumping = false;
        }
        if (collision.gameObject.tag == "ammo")
        {
            AmmoCount += 5;
            Destroy(collision.gameObject);
        }
    }

}
