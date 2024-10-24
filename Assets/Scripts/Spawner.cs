using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private Target[] _targets;

    [SerializeField] private float _repeatRate = 2f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private Target _target;

    private Coroutine _coroutine;
    private ObjectPool<Enemy> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            createFunc: () => Instantiate(_prefabEnemy),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => Destroy(enemy.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        _coroutine = StartCoroutine(SpawnCooldown());
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator SpawnCooldown()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            Spawn();

            yield return wait;
        }
    }

    private void Spawn()
    {
        Enemy enemy = _pool.Get();
        ChooseTarget();
        enemy.Init(transform.position, _target.transform);
        enemy.TriggerEntering += Release;
    }

    private void ChooseTarget()
    {
        int minIndexTarget = 0;
        int maxIndexTarget = _targets.Length;
        int indexTarget = Random.Range(minIndexTarget, maxIndexTarget);

        _target = _targets[indexTarget];                
    }

    private void Release(Enemy enemy)
    {
        enemy.TriggerEntering -= Release;
        _pool.Release(enemy);
    }
}
