using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _basesParent;
    [SerializeField] private Scanner _scanner;

    public Base SpawnBase(Vector3 position, Bot builderBot = null)
    {
        Base newBase = Instantiate(_basePrefab, position, Quaternion.identity, _basesParent);
        newBase.Initialize(_scanner, this, builderBot);

        return newBase;
    }
}