using UnityEngine;

public class FirstBaseInitializer : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private SpawnerBase _baseSpawner;

    private void Awake()
    {
        Base baseComponent = GetComponent<Base>();

        if (baseComponent != null && _scanner != null && _baseSpawner != null)
        {
            baseComponent.Initialize(_scanner, _baseSpawner);
        }
    }

    private void Start()
    {
        UI uiComponent = GetComponent<UI>();

        if (uiComponent != null)
        {
            Invoke(nameof(ForceUIUpdate), 0.1f);
        }
    }

    private void ForceUIUpdate()
    {
        UI uiComponent = GetComponent<UI>();

        if (uiComponent != null)
        {
            uiComponent.UpdateUI();
        }
    }
}