using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MyCharacterController : MonoBehaviour
{
    public static int CoinsCollected = 0;
    public TextMeshProUGUI coinsUi;

    public float speed = 1.0f;


    CharacterController characterController;

    Vector2 movement;
    bool wantsToJump;
    Vector3 playerVelocity;
    CollisionFlags movementResult;
    int movementResultInt;
    public float jumpHeight = 1.0f;

    bool dbgGrounded;

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
        }

        dbgGrounded = characterController.isGrounded;

        if (wantsToJump)
        {
            if (characterController.isGrounded)
            {
                // Führe Sprung aus
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y);
            }
            wantsToJump = false;
        }

        
        playerVelocity += Physics.gravity * Time.deltaTime;

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
