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

    public int Counter { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        _coroutine = StartCoroutine(Circulating());
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
            ChangeCheckPoint();
        }
    }

    private IEnumerator Circulating()
    {
        while (enabled)
        {
            Circulate();

            yield return new WaitUntil(() => _isCome == true);
        }
    }

    private void Circulate()
    {
        GetCheckPoint(Counter);
        Init(transform.position);
        Set(_checkPoint.transform);
    }

    private void ChangeCheckPoint()
    {
        int lastTarget = _checkPoints.Length - 1;

        _isCome = true;

        if (Counter == lastTarget)
        {
            StartCounter();
            GetCheckPoint(Counter);
            Set(_checkPoint.transform);
        }
        else
        {
            UpperCounter();
            GetCheckPoint(Counter);
            Set(_checkPoint.transform);
        }

        _isCome = false;
    }

    private CheckPoint GetCheckPoint(int count)
    {
        _checkPoint = _checkPoints[count];

        return _checkPoint;
    }

    private int StartCounter()
    {
        return Counter = 0;
    }

    private int UpperCounter()
    {
        return Counter++;
    }

    private void Init(Vector3 position )
    {
        transform.position = position;
    }

    private void Set(Transform target)
    {
        _mover.SetTarget(target);
    }
}
