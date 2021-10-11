using System.Collections.Generic;

using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  [System.Serializable]
  public class FingerprintDetails
  {
    public int RidgeCount;
    public PatternType PatternType;

    public string FingerprintSpriteName;
    public List<SerializedVector2> CoreCoordinates;
    public List<SerializedVector2> DeltaCoordinates;
    public List<SerializedVector2> MinutiaeCoordinates;

    public bool CanUseForCore = true;
    public bool CanUseForDelta = true;

    public float DetailsScale = 1f;
    public int NumberOfMinutiae => MinutiaeCoordinates.Count;

    // Loaded in runtime and not saved
    public Sprite FingerprintImage = null;

    public FingerprintDetails()
    {
      CoreCoordinates = new List<SerializedVector2>();
      DeltaCoordinates = new List<SerializedVector2>();
      MinutiaeCoordinates = new List<SerializedVector2>();
    }

    public bool IsUpToDate()
    {
      return PatternType != 0 && NumberOfMinutiae > 0 && !string.IsNullOrEmpty(FingerprintSpriteName);
    }

    public IList<SerializedVector2> GetDetailsOfType( DetailType detailType )
    {
      switch (detailType)
      {
        case DetailType.Core: return CoreCoordinates;
        case DetailType.Delta: return DeltaCoordinates;
        case DetailType.Minutiae: return MinutiaeCoordinates;
        default: return new List<SerializedVector2>();
      }
    }
  }
}
