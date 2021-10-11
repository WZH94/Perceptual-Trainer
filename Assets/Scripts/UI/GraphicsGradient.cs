using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class GraphicsGradient : MonoBehaviour
  {
    [SerializeField] private GraphicsType graphicsType;
    public GraphicsType GraphicsType => graphicsType;

    private Graphic graphic;

    private bool isSetup = false;

    private void Awake()
    {
      if (!isSetup) Setup();
    }

    private void Setup()
    {
      graphic = GetComponent<Graphic>();
      isSetup = true;

      if (graphicsType == GraphicsType.Material) graphic.material = new Material(graphic.material);
    }

    public void AdjustGraphicsGradientColour( Color colour )
    {
      if (!isSetup) Setup();
      if (graphicsType == GraphicsType.Material) return;

      graphic.color = colour;
    }

    public void AdjustGraphicsGradientMaterial( Material startMaterial, Material endMaterial, float t )
    {
      if (!isSetup) Setup();
      if (graphicsType != GraphicsType.Material) return;

      graphic.materialForRendering.Lerp(startMaterial, endMaterial, t);
    }
  }
}
