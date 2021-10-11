using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public struct PopupAnnotationInformation
  {
    public DetailType DetailType;
    public bool IsLinked;
    public float DistanceBetween;
    public float PrecisionPerc;
    public float PointsScored;

    public PopupAnnotationInformation( DetailType detailType, bool isLinked, float distance, float precision, float points )
    {
      DetailType = detailType;
      IsLinked = isLinked;
      DistanceBetween = distance;
      PrecisionPerc = precision;
      PointsScored = points;
    }
  }
}
