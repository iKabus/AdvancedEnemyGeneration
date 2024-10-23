using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float _speed;

    private Transform _targetPosition;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition.position, _speed * Time.deltaTime);
    }

    public void SetTarget(Transform direction)
    {
        _targetPosition = direction;
    }
}
