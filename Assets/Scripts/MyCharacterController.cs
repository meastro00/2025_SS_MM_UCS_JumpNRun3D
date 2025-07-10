using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public class MyCharacterController : MonoBehaviour
{
    public static int CoinsCollected = 0;

    public TextMeshProUGUI coinsUi;

    public float speed = 1.0f, movementSmoothing = 0.5f;
    public float jumpHeight = 1.0f, gravityMultiply = 2.0f;
    public float coyoteTime = 0.5f;
    public float turnTime = 15.0f;
    public int jumpLimit = 2;


    public Animator animator;
    public UnityEvent OnJumpEvent = new UnityEvent();

    public Transform gunBarrelOrigin;

    double lastTimeOnGround;

    CharacterController characterController;

    Vector3 lastGroundedPosition;
    Vector3 movement;

    float rotation;

    Vector3 currentPlayerMovement, playerMovementVelocity;
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
        Vector3 targetPlayerMovement = movement.normalized * speed;

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
        transform.Rotate(0f, rotation * Time.deltaTime * turnTime, 0f);
        currentPlayerMovement = Vector3.SmoothDamp(currentPlayerMovement, targetPlayerMovement, ref playerMovementVelocity, movementSmoothing);
        Vector3 movementSum = currentPlayerMovement + playerVelocity;

        animator.SetFloat("Movement", targetPlayerMovement.magnitude);
        movementResult = characterController.Move(movementSum * Time.deltaTime);


        if (targetPlayerMovement.sqrMagnitude > 0.5f)
        {
           // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetPlayerMovement), turnTime * Time.deltaTime);
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

        RaycastHit hit;
        if(Physics.Raycast(gunBarrelOrigin.transform.position, gunBarrelOrigin.transform.forward, out hit))
        {
            if (hit.transform.TryGetComponent<IAttackable>(out IAttackable attackable))
            {
                attackable.Damage(3);
            }
            //print(hit.transform.gameObject.name);
        }
    }

    // Unsere schnittstelle für den Player Input
    public void SetMovement(Vector3 newMovement)
    {
        movement = newMovement;
    }

    public void SetRotation(float rotation)
    {
        this.rotation = rotation;
    }

    public void SetJump()
    {
        wantsToJump = true;
    }
}
