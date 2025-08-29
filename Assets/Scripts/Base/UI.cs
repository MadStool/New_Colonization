using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _uiText;

    private Base _base;
    private Wallet _baseWallet;
    private BulderBase _baseBuilder;
    private Camera _mainCamera;

    private void Awake()
    {
        _base = GetComponent<Base>();
        _baseWallet = GetComponent<Wallet>();
        _baseBuilder = GetComponent<BulderBase>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        if (_baseWallet != null)
            _baseWallet.PointsChanged += OnPointsChanged;

        if (_base != null)
            _base.BotsCountChanged += OnBotsCountChanged;

        if (_baseBuilder != null)
        {
            _baseBuilder.BuildStarted += OnBuildStateChanged;
            _baseBuilder.BuildCompleted += OnBuildStateChanged;
        }

        UpdateUI();
    }

    private void OnDisable()
    {
        if (_baseWallet != null)
            _baseWallet.PointsChanged -= OnPointsChanged;

        if (_base != null)
            _base.BotsCountChanged -= OnBotsCountChanged;

        if (_baseBuilder != null)
        {
            _baseBuilder.BuildStarted -= OnBuildStateChanged;
            _baseBuilder.BuildCompleted -= OnBuildStateChanged;
        }
    }

    public void UpdateUI()
    {
        if (_uiText == null || _base == null || _baseWallet == null)
            return;

        int botsCount = _base.BotsCount;
        int resourcesCount = _baseWallet.CurrentPoints;
        bool canBuild = botsCount > 1;

        string buildStatus = canBuild ? "You can build" : "You can't build yet";

        _uiText.text = $"Resources: {resourcesCount}\n" +
                      $"Bots: {botsCount}\n" +
                      $"{buildStatus}";
    }

    private void OnPointsChanged(int points) => UpdateUI();

    private void OnBotsCountChanged(int count) => UpdateUI();

    private void OnBuildStateChanged() => UpdateUI();

    private void LateUpdate() => LookAtCamera();

    private void LookAtCamera()
    {
        if (_uiText != null && _mainCamera != null)
            _uiText.transform.rotation = _mainCamera.transform.rotation;
    }
}