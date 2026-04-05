using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputActionMap movementMap;

    private InputAction moveAction;
    private InputAction lookAction;

    [Header("Player Settings")]
    public Transform player;
    public float moveSpeed = 5f;
    public bool isMoving = false;
    private bool canMove = true;

    [Header("Mouse Settings")]
    public float sensitivity = 0.5f;
    public float clampAngle = 80f;
    private float rotX = 0f;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        movementMap = playerInput.actions.FindActionMap("Movement");

        moveAction = movementMap.FindAction("Move");
        lookAction = movementMap.FindAction("Look");

        movementMap.Enable();
    }

    public void setCanMove(bool canPlayerMove)
    {
        canMove = canPlayerMove;
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        Move();
        Look();
    }

    void Move()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        isMoving = direction.sqrMagnitude > 0.01f;
        Vector3 move = player.right * direction.x + player.forward * direction.y;
        player.position += move * moveSpeed * Time.deltaTime;
    }

    void Look()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }
}


