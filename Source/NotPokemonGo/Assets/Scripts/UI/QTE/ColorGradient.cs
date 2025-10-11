using UnityEngine;

namespace UI.QTE
{
  public class ColorGradient : MonoBehaviour
  {
    private Renderer _renderer;
    private Color _defaultColor;

    private void Awake()
    {
      _renderer = GetComponentInChildren<Renderer>();
      _defaultColor = _renderer.material.color;
    }

    public void MarkProcess()
    {
      _renderer.material.SetColor("_Color", Color.red);
    }
    
    public void MarkInterract()
    {
      _renderer.material.SetColor("_Color", Color.yellow);
    }

    public void FinalizeProcess()
    {
      _renderer.material.color = _defaultColor;
    }
  }
}