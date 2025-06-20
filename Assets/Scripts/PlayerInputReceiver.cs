using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    public MyCharacterController player;

    Vector2 rawInput;
    private void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();

    }

    private void Update()
    {
        Vector3 worldSpaceMovement = new Vector3(rawInput.x, 0.0f, rawInput.y);
        Vector3 movementRelativeToCamera = transform.TransformDirection(worldSpaceMovement);
        player.SetMovement(movementRelativeToCamera);
    }

    void OnJump()
    {
        player.SetJump();
    }
}
