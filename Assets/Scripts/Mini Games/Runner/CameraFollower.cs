using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotateOffset;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotationSpeed;

    #endregion Serialized Fields

    #region Private Fields
    private float _sensitivity;

    #endregion Private Fields

    #region Public Methods
    public void Initialize(float sensitivity) 
    {
        _sensitivity = sensitivity;
    }

    #endregion Public Methods


    #region UnityLoop Events
    private void Start()
    {
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector3 rotatedDirection = Quaternion.Euler(rotateOffset) * directionToTarget;

            Quaternion targetRotation = Quaternion.LookRotation(rotatedDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    #endregion UnityLoop Events
}