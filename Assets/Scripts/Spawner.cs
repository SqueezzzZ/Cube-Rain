using System.Collections;
using UnityEngine.Pool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Transform _spawnCenterPoint;
    [SerializeField] private float _maxSpawnDistance = 15;
    [SerializeField] private float _repeatRate = 1;
    [SerializeField] private float _minCubeLifeTime = 2;
    [SerializeField] private float _maxCubeLifeTime = 5;

    private ObjectPool<Cube> _cubePool;
    private int _poolCapacity = 10;
    private int _poolMaxSize = 30;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () => InstantiateCube(),
            actionOnGet: (cubeObject) => ActionOnGet(cubeObject),
            actionOnRelease: (cubeObject) => cubeObject.SetActive(false),
            actionOnDestroy: (cubeObject) => DestroyCube(cubeObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _cubePool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        cube.SetPosition(GetRandomPointInQuad(_spawnCenterPoint, _maxSpawnDistance));
        cube.SetActive(true);
    }

    private Cube InstantiateCube()
    {
        Cube cube = Instantiate(_cubePrefab);

        cube.BarrierTouched += ActionOnBarrierTouched;

        return cube;
    }

    private void DestroyCube(Cube cube)
    {
        cube.BarrierTouched -= ActionOnBarrierTouched;
        Destroy(cube);
    }

    private void ActionOnBarrierTouched(Cube cube)
    {
        cube.SetColor(Random.ColorHSV());
        StartCoroutine(DestroyCubeDelayed(cube));
    }

    private IEnumerator DestroyCubeDelayed(Cube cube)
    {
        yield return new WaitForSeconds(Random.Range(_minCubeLifeTime, _maxCubeLifeTime));
        _cubePool.Release(cube);
    }

    private Vector3 GetRandomPointInQuad(Transform centerPosition, float maxDistance)
    {
        return new Vector3(
            Random.Range(centerPosition.position.x - maxDistance, centerPosition.position.x + maxDistance),
            centerPosition.position.y,
            Random.Range(-centerPosition.position.z - maxDistance, centerPosition.position.z + maxDistance));
    }
}