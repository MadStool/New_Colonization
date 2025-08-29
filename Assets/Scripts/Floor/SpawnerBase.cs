using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _basesParent;
    [SerializeField] private Scanner _scanner;

    public Base SpawnBase(Vector3 position, Bot builderBot = null)
    {
        Base createdBase = Instantiate(_basePrefab, position, Quaternion.identity, _basesParent);
        createdBase.Initialize(_scanner, this, builderBot);

        return createdBase;
    }
}