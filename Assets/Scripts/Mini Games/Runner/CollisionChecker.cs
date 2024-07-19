using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionChecker : MonoBehaviour
{
    #region Events
    public event Action OnCollide;

    #endregion Events

    #region UnityLoop Events
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            OnCollide?.Invoke();
        }
    }

    #endregion UnityLoop Events
}