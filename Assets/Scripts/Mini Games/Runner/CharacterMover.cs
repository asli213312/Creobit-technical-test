using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed;

    private float _moveSpeed;

    public void Initialize(float moveSpeed) 
    {
        _moveSpeed = moveSpeed;
    }

    private void Update() 
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        if (move != Vector3.zero)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 desiredMoveDirection = forward * moveZ + right * moveX;
            desiredMoveDirection.Normalize();

            target.Translate(desiredMoveDirection * _moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}