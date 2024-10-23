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

    public int CounterTargets { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        _coroutine = StartCoroutine(TargetsCirculation());
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
            ChangeCheckPoint(this);
        }
    }

    private IEnumerator TargetsCirculation()
    {
        while (enabled)
        {
            Circulate();

            yield return new WaitUntil(() => _isCome == true);
        }
    }

    private void Circulate()
    {
        GetCheckPoint(CounterTargets);
        Init(transform.position);
        GetTarget(_checkPoint.transform);
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

    private void Init(Vector3 position )
    {
        transform.position = position;
    }

    private void GetTarget(Transform direction)
    {
        _mover.SetTarget(direction);
    }
}
