using UnityEngine;
using UnityEngine.InputSystem;

public class MyCharacterController : MonoBehaviour
{
    CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        characterController.SimpleMove(new Vector3(movement.x, 0.0f, movement.y) * 10.5f);

    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    void OnJump()
    {
        characterController.Move(new Vector3(0, 4.0f, 0.0f));
    }
}
