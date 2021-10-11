using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class GraphicsGradientParent : MonoBehaviour
  {
    [SerializeField] private GraphicsGradientColourScriptableObject gradientColours = null;

    private List<GraphicsGradient> boldGraphics = new List<GraphicsGradient>();
    private List<GraphicsGradient> lightGraphics = new List<GraphicsGradient>();
    private List<GraphicsGradient> materialGraphics = new List<GraphicsGradient>();

    private bool isInitialised = false;

    private void Awake()
    {
      if (!isInitialised) Initialise();
    }

    private void Initialise()
    {
      // Get every single graphics gradient
      GraphicsGradient[] allGraphicsGradient = GetComponentsInChildren<GraphicsGradient>(true);

      foreach (GraphicsGradient graphicsGradient in allGraphicsGradient)
      {
        switch (graphicsGradient.GraphicsType)
        {
          case GraphicsType.Bold:
            boldGraphics.Add(graphicsGradient);
            break;

          case GraphicsType.Light:
            lightGraphics.Add(graphicsGradient);
            break;

          case GraphicsType.Material:
            materialGraphics.Add(graphicsGradient);
            break;
        }
      }

      // Get every single graphics gradient parent inside this parent and exclude those graphics
      GraphicsGradientParent[] graphicsGradientParents = GetComponentsInChildren<GraphicsGradientParent>(true);

      foreach (GraphicsGradientParent graphicsGradientParent in graphicsGradientParents)
      {
        if (graphicsGradientParent == this) continue;

        foreach (GraphicsGradient graphicGradient in graphicsGradientParent.GetComponentsInChildren<GraphicsGradient>(true))
        {
          switch (graphicGradient.GraphicsType)
          {
            case GraphicsType.Bold:
              boldGraphics.Remove(graphicGradient);
              break;

            case GraphicsType.Light:
              lightGraphics.Remove(graphicGradient);
              break;

            case GraphicsType.Material:
              materialGraphics.Remove(graphicGradient);
              break;
          }
        }
      }
    }

    /// <summary>
    /// Sets every graphics gradient object to the specified value. 0 is negative, 1 is neutral, 2 is positive
    /// </summary>
    public void SetGraphicsToValue( float value )
    {
      if (!isInitialised) Initialise();

      Color boldColour = value <= 1f ? Color.Lerp(gradientColours.RedBoldColour, gradientColours.YellowBoldColour, value)
        : Color.Lerp(gradientColours.YellowBoldColour, gradientColours.GreenBoldColour, value - 1f);
      Color lightColour = value <= 1f ? Color.Lerp(gradientColours.RedLightColour, gradientColours.YellowLightColour, value)
        : Color.Lerp(gradientColours.YellowLightColour, gradientColours.GreenLightColour, value - 1f);

      foreach (GraphicsGradient graphicsGradient in boldGraphics)
      {
        graphicsGradient.AdjustGraphicsGradientColour(boldColour);
      }

      foreach (GraphicsGradient graphicsGradient in lightGraphics)
      {
        graphicsGradient.AdjustGraphicsGradientColour(lightColour);
      }

      Material startMaterial = value <= 1f ? gradientColours.RedBlur : gradientColours.YellowBlur;
      Material endMaterial = value <= 1f ? gradientColours.YellowBlur : gradientColours.GreenBlur;
      float t = value <= 1f ? value : value - 1f;

      foreach (GraphicsGradient graphicsGradient in materialGraphics)
      {
        graphicsGradient.AdjustGraphicsGradientMaterial(startMaterial, endMaterial, value);
      }
    }

    public void SetGraphicsPositive()
    {
      SetGraphicsToValue(2f);
    }

    public void SetGraphicsNeutral()
    {
      SetGraphicsToValue(1f);
    }

    public void SetGraphicsNegative()
    {
      SetGraphicsToValue(0f);
    }
  }
}
