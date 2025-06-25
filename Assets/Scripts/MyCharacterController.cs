using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public class MyCharacterController : MonoBehaviour
{
    public static int CoinsCollected = 0;

    public TextMeshProUGUI coinsUi;

    public float speed = 1.0f;
    public float jumpHeight = 1.0f, gravityMultiply = 2.0f;
    public float coyoteTime = 0.5f;
    public float turnTime = 15.0f;
    public int jumpLimit = 2;

    public UnityEvent OnJumpEvent = new UnityEvent();

    public Transform dbgDest;
    double lastTimeOnGround;

    CharacterController characterController;

    Vector3 lastGroundedPosition;
    Vector3 movement;
    bool wantsToJump;
    Vector3 playerVelocity;
    CollisionFlags movementResult;
    int movementResultInt;

    int jumpsExecutedInAir;

    bool dbgGrounded;
    double timeInAir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        lastGroundedPosition = transform.position;

    }

    // Update is called once per frame
    private void Update()
    {
        movement.y = 0.0f; // Cancel out any unwanted "jumps"
        Vector3 playerMovement = movement.normalized * speed;

        if (characterController.isGrounded)
        {
            playerVelocity.y = 0.0f;
            lastTimeOnGround = Time.timeAsDouble;
            jumpsExecutedInAir = 0;
            lastGroundedPosition = transform.position;
        }

        timeInAir = Time.timeAsDouble - lastTimeOnGround;

        dbgGrounded = characterController.isGrounded;

        if (wantsToJump)
        {
            bool isAllowedToJump = timeInAir < coyoteTime && jumpsExecutedInAir <= jumpLimit;

            float jumpHeightMultiplier = 1.0f;
            switch (jumpsExecutedInAir)
            {
                case 0: 
                    // Erster Sprung
                    break;
                case 1: 
                    // Zweiter Sprung
                    jumpHeightMultiplier = 0.5f;
                    break;
            }

            if (isAllowedToJump)
            {
                // Führe Sprung aus
                playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpHeightMultiplier * -2.0f * Physics.gravity.y * gravityMultiply);
                jumpsExecutedInAir += 1;
                OnJumpEvent.Invoke();
            }

            wantsToJump = false;
        }

        
        // NOTE: Ist nicht richtig, da Gravitation nicht so funktioniert -> Objekt wird immer immer schneller. Außerdem: Luftwiederstandt wird nicht bedacht. 
        playerVelocity += Physics.gravity * Time.deltaTime * gravityMultiply;

        Vector3 movementSum = playerMovement + playerVelocity;


        movementResult = characterController.Move(movementSum * Time.deltaTime);

        if (playerMovement.sqrMagnitude > 0.5f)
        {
            
            dbgDest.localPosition = playerMovement;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(playerMovement), turnTime * Time.deltaTime);
            //transform.LookAt(transform.position + offset, Vector3.up);
        }

        // Respawn player if below 15 Meters.
        if (transform.position.y < -15.0f)
        {
            characterController.enabled = false;
            transform.position = lastGroundedPosition;
            characterController.enabled = true;
        }
        coinsUi.text = $"Coins: {CoinsCollected}";

        movementResultInt = (int)movementResult;
        
    }

    // Unsere schnittstelle für den Player Input
    public void SetMovement(Vector3 newMovement)
    {
        movement = newMovement;
    }

    public void SetJump()
    {
        wantsToJump = true;
    }
}
