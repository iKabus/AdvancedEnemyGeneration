using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Enemy : MonoBehaviour
{
    private Mover _mover;

    public event Action<Enemy> TriggerEntering;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out _))
        {
            TriggerEntering?.Invoke(this);
        }
    }

    public void Init(Vector3 position, Transform direction)
    {
        transform.position = position;
        _mover.SetTarget(direction);
    }
}
