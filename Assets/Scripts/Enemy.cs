using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;

    public event Action<Enemy> OnTriggerEntered;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out _))
        {
            OnTriggerEntered?.Invoke(this);
        }
    }

    public void Init(Vector3 position, Transform direction)
    {
        transform.position = position;
        _mover.GetDirection(direction);
    }
}
