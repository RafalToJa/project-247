using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Decoy : MonoBehaviour
{
    [SerializeField] private float _range = 4f;
    [SerializeField] private float _lureTime = 2f;
    [SerializeField] private float _lureCooldown = 0.1f;
    [SerializeField] private bool _startLureOnAwake = false;
    [SerializeField] private bool _destroyWhenFinished = false;
    [SerializeField] private UnityEvent _onLureStart = null;
    [SerializeField] private UnityEvent _onLureEnd = null;

    private bool _isCoroutineRunning = false;
    private WaitForSeconds _coroutineWaitTime = null;

    private void Awake()
    {
        if(_startLureOnAwake)
        {
            ActivateDecoy();
        }
    }

    public void ActivateDecoy()
    {
        if (!_isCoroutineRunning)
        {
            StartCoroutine(LureCoroutine());
        }
    }

    private IEnumerator LureCoroutine()
    {
        _isCoroutineRunning = true;
        float time = _lureTime;
        _coroutineWaitTime = new WaitForSeconds(_lureCooldown * 0.2f);

        while (_isCoroutineRunning)
        {
            _onLureStart.Invoke();

            LureNearbyEnemies();

            yield return _coroutineWaitTime;

            time -= _lureCooldown;

            if (time <= 0)
            {
                _isCoroutineRunning = false;
            }

            _onLureEnd.Invoke();
            yield return _coroutineWaitTime;
        }


        if(_destroyWhenFinished)
        {
            Destroy(gameObject);
        }
    }

    private void LureNearbyEnemies()
    {
        for(int i = 0; i < NPCSpawnSystem.Instance.spawnedEnemies.Count; i++)
        {
            if(NPCSpawnSystem.Instance.spawnedEnemies[i] == null)
            {
                continue;
            }

            if((NPCSpawnSystem.Instance.spawnedEnemies[i].transform.position - transform.position).magnitude <= _range)
            {
                NPCSpawnSystem.Instance.spawnedEnemies[i].GoToLocation(transform.position);
            }
        }
    }
}
