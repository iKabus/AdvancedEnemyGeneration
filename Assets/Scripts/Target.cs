using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Target : MonoBehaviour
{
    [SerializeField] private CheckPoint[] _checkPoints;

    private Mover _mover;
    private CheckPoint _checkPoint;
    private Coroutine _coroutine;

    private bool _isCome = false;

    public event Action<Target> OnTriggerEntered;

    public int CounterTargets { get; private set; }

    private void Start()
    {
        _coroutine = StartCoroutine(TargetsCirculation());
    }

    private IEnumerator TargetsCirculation()
    {
        while (enabled)
        {
            Circulation();

            yield return new WaitUntil(() => _isCome == true);
        }
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CheckPoint>(out _))
        {
            OnTriggerEntered?.Invoke(this);
        }
    }

    private void Circulation()
    {
        GetCheckPoint(CounterTargets);
        Init(transform.position);
        GetTarget(_checkPoint.transform);
        OnTriggerEntered += ChangeCheckPoint;
    }

    private void ChangeCheckPoint(Target target)
    {
        int lastTarget = _checkPoints.Length - 1;

        _isCome = true;

        if (CounterTargets == lastTarget)
        {
            StartCounterTarget();
            GetCheckPoint(CounterTargets);
            GetTarget(_checkPoint.transform);
        }
        else
        {
            UpperCounterTarget();
            GetCheckPoint(CounterTargets);
            GetTarget(_checkPoint.transform);
        }

        _isCome = false;
    }

    private CheckPoint GetCheckPoint(int count)
    {
        _checkPoint = _checkPoints[count];

        return _checkPoint;
    }

    private int StartCounterTarget()
    {
        return CounterTargets = 0;
    }

    private int UpperCounterTarget()
    {
        return CounterTargets++;
    }

    private void Init(Vector3 position)
    {
        transform.position = position;
    }

    private void GetTarget(Transform direction)
    {
        _mover = GetComponent<Mover>();
        _mover.GetDirection(direction);
    }
}
