using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionChecker : MonoBehaviour
{
    public event Action OnCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            OnCollide?.Invoke();
        }
    }
}