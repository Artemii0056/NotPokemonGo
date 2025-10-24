using System;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
  [SerializeField] private Button _restartButton;
  [SerializeField] private Button _backMainMenuButton;
    
  public event Action RestartButtonPressed;
  public event Action BackMainMenuButtonPressed;

  private void OnEnable()
  {
    _restartButton.onClick.AddListener(() => RestartButtonPressed?.Invoke());
    _backMainMenuButton.onClick.AddListener(() => BackMainMenuButtonPressed?.Invoke());
  }

  private void OnDisable()
  {
    _restartButton.onClick.RemoveAllListeners();
    _backMainMenuButton.onClick.RemoveAllListeners();
  }
}