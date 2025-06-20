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
    public int jumpLimit = 2;

    public UnityEvent OnJumpEvent = new UnityEvent();

    double lastTimeOnGround;

    CharacterController characterController;

    Vector3 lastGroundedPosition;
    Vector2 movement;
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
        Vector3 playerMovement = new Vector3(movement.x, 0.0f, movement.y) * speed;

        if(characterController.isGrounded)
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
                // F¸hre Sprung aus
                playerVelocity.y = Mathf.Sqrt(jumpHeight * jumpHeightMultiplier * -2.0f * Physics.gravity.y * gravityMultiply);
                jumpsExecutedInAir += 1;
                OnJumpEvent.Invoke();
            }

            wantsToJump = false;
        }

        
        // NOTE: Ist nicht richtig, da Gravitation nicht so funktioniert -> Objekt wird immer immer schneller. Auﬂerdem: Luftwiederstandt wird nicht bedacht. 
        playerVelocity += Physics.gravity * Time.deltaTime * gravityMultiply;

        Vector3 movementSum = playerMovement + playerVelocity;

        movementResult = characterController.Move(movementSum * Time.deltaTime);

        if(transform.position.y < -15.0f)
        {
            characterController.enabled = false;
            transform.position = lastGroundedPosition;
            characterController.enabled = true;
        }
        coinsUi.text = $"Coins: {CoinsCollected}";

        movementResultInt = (int)movementResult;
        
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    void OnJump()
    {
        wantsToJump = true;
    }
}
