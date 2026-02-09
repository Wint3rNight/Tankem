using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D rb;
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnRate = 360f;
    
    private Vector2 _prevMoveInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.MoveEvent += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputReader.MoveEvent -= HandleMove;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        float zRotation = _prevMoveInput.x * -turnRate * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f,zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        rb.linearVelocity =(Vector2)bodyTransform.up*_prevMoveInput.y*moveSpeed;
    }

    private void HandleMove(Vector2 moveInput)
    {
        _prevMoveInput = moveInput;
    }
}
