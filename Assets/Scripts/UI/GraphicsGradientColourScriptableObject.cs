using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  [CreateAssetMenu(fileName = "GraphicsGradientColour", menuName = "Scriptable Objects/Graphics Gradient Colour", order = 0)]
  public class GraphicsGradientColourScriptableObject : ScriptableObject
  {
    [Header("Positive/Default")]
    public Color GreenBoldColour;
    public Color GreenLightColour;
    public Material GreenBlur;

    [Header("Neutral")]
    public Color YellowBoldColour;
    public Color YellowLightColour;
    public Material YellowBlur;

    [Header("Negative")]
    public Color RedBoldColour;
    public Color RedLightColour;
    public Material RedBlur;
  }
}
