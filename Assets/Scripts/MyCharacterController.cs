using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MyCharacterController : MonoBehaviour
{
    public static int CoinsCollected = 0;

    public TextMeshProUGUI coinsUi;

    public float speed = 1.0f;
    public float jumpHeight = 1.0f, gravityMultiply = 2.0f;
    public float coyoteTime = 0.5f;

    double lastTimeOnGround;

    CharacterController characterController;

    Vector2 movement;
    bool wantsToJump;
    Vector3 playerVelocity;
    CollisionFlags movementResult;
    int movementResultInt;

    bool dbgGrounded;
    double timeInAir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 playerMovement = new Vector3(movement.x, 0.0f, movement.y) * speed;

        if(characterController.isGrounded)
        {
            playerVelocity.y = 0.0f;
            lastTimeOnGround = Time.timeAsDouble;
        }

        timeInAir = Time.timeAsDouble - lastTimeOnGround;

        dbgGrounded = characterController.isGrounded;

        if (wantsToJump)
        {
            if (characterController.isGrounded || timeInAir < coyoteTime)
            {
                // F¸hre Sprung aus
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y * gravityMultiply);
            }
            wantsToJump = false;
        }

        
        // NOTE: Ist nicht richtig, da Gravitation nicht so funktioniert -> Objekt wird immer immer schneller. Auﬂerdem: Luftwiederstandt wird nicht bedacht. 
        playerVelocity += Physics.gravity * Time.deltaTime * gravityMultiply;

        Vector3 movementSum = playerMovement + playerVelocity;

        movementResult = characterController.Move(movementSum * Time.deltaTime);
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
