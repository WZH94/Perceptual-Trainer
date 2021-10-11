using UnityEngine;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using FingerprintAnnotation;

  public class AnnotationAnswerPanel : MonoBehaviour
  {
    [SerializeField] private TMP_Text detailText = null;
    [SerializeField] private TMP_Text distanceText = null;
    [SerializeField] private TMP_Text precisionText = null;
    [SerializeField] private TMP_Text pointsText = null;

    public void SetInformation( PopupAnnotationInformation info )
    {
      gameObject.SetActive(true);

      detailText.text = info.DetailType.ToString();

      if (info.DetailType == DetailType.Minutiae)
      {
        distanceText.gameObject.SetActive(false);
        precisionText.gameObject.SetActive(false);
      }

      else
      {
        distanceText.gameObject.SetActive(true);
        precisionText.gameObject.SetActive(true);
      }

      if (info.IsLinked)
      {
        distanceText.text = $"Distance From Answer: {info.DistanceBetween.ToString("F2")}";
        precisionText.text = $"Precision %: {(info.PrecisionPerc).ToString("F2")}";
        pointsText.text = $"Points Scored: {info.PointsScored.ToString("F2")}";
      }

      else
      {
        distanceText.text = $"Distance From Answer: -";
        precisionText.text = $"Precision %: -";
        pointsText.text = $"Points Scored: 0";
      }
    }

    public void Hide()
    {
      gameObject.SetActive(false);
    }
  }
}
