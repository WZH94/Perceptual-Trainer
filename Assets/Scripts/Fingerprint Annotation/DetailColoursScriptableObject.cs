using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  [CreateAssetMenu(fileName = "Detail Colours", menuName = "Scriptable Objects/Detail Colours", order = 0)]
  public class DetailColoursScriptableObject: ScriptableObject
  {
    public Color CoreColour;
    public Color DeltaColour;
    public Color MinutiaeColour;

    private Dictionary<DetailType, Color> detailColoursDictionary;

    public void Initialise()
    {
      detailColoursDictionary = new Dictionary<DetailType, Color>();

      detailColoursDictionary.Add(DetailType.Core, CoreColour);
      detailColoursDictionary.Add(DetailType.Delta, DeltaColour);
      detailColoursDictionary.Add(DetailType.Minutiae, MinutiaeColour);
    }

    public Color GetColourOfDetail( DetailType detailType )
    {
      return detailColoursDictionary[detailType];
    }
  }
}
