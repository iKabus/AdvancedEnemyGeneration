using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;

    public event Action<Enemy> OnTriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out _))
        {
            OnTriggerEntered?.Invoke(this);
        }
    }

     public void Init(Vector3 position)
    {
        transform.position = position;
    }

    public void GetTarget(Transform direction)
    {
        _mover = GetComponent<Mover>();
        _mover.GetDirection(direction);
    }
}
