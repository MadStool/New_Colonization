using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _basesParent;
    [SerializeField] private Scanner _scanner;

    public Base SpawnBase(Vector3 position, Bot builderBot = null)
    {
        Base createdBase = Instantiate(_basePrefab, position, Quaternion.identity, _basesParent);

        createdBase.Initialize(_scanner, builderBot);
        createdBase.BaseCreationRequested += HandleBaseCreationRequest;

        return createdBase;
    }

    public void HandleBaseCreationRequest(Base requestingBase, Vector3 position, Bot builderBot)
    {
        Base newBase = SpawnBase(position, null);

        if (builderBot != null)
        {
            builderBot.ChangeBase(newBase);
            builderBot.ChangeParent(newBase.transform);
            newBase.AddBot(builderBot);
        }
    }
}