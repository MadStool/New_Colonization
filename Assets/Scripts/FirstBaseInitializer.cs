using UnityEngine;

public class FirstBaseInitializer : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private BaseSpawner _baseSpawner;

    private void Awake()
    {
        Base baseComponent = GetComponent<Base>();

        if (baseComponent != null && _scanner != null && _baseSpawner != null)
        {
            baseComponent.Initialize(_scanner, _baseSpawner);
        }
    }
}