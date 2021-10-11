using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public class FingerprintImageSet : MonoBehaviour, IPointerClickHandler
  {
    [SerializeField] private TMP_Text imageNameText = null;
    [SerializeField] private Image ridgeCountImage = null;
    [SerializeField] private Image hasCoreImage = null;
    [SerializeField] private Image hasDeltaImage = null;
    [SerializeField] private Image hasMinutiaeImage = null;

    private Sprite originalImage;
    private FingerprintDetails fingerprintDetails;

    public Action<FingerprintImageSet> OnImageSetSelected;

    public (Sprite, FingerprintDetails) ImageSet => (originalImage, fingerprintDetails);

    public void OnPointerClick( PointerEventData eventData )
    {
      OnImageSetSelected?.Invoke(this);
    }

    public void SetImageSet( (Sprite, FingerprintDetails) imageSet )
    {
      originalImage = imageSet.Item1;
      fingerprintDetails = imageSet.Item2;

      imageNameText.text = originalImage.name;

      if (fingerprintDetails != null) 
      {
        imageNameText.text += ": " + fingerprintDetails.PatternType.ToString();

        if (fingerprintDetails.PatternType == PatternType.LeftLoop || fingerprintDetails.PatternType == PatternType.RightLoop)
        {
          ridgeCountImage.gameObject.SetActive(true);

          if (fingerprintDetails.RidgeCount > 0) ridgeCountImage.color = Color.green;
          else ridgeCountImage.color = Color.red;
        }

        if (fingerprintDetails.CanUseForCore && fingerprintDetails.CoreCoordinates.Count > 0) hasCoreImage.gameObject.SetActive(true);
        if (fingerprintDetails.CanUseForDelta && fingerprintDetails.DeltaCoordinates.Count > 0) hasDeltaImage.gameObject.SetActive(true);
        if (fingerprintDetails.MinutiaeCoordinates.Count > 0) hasMinutiaeImage.gameObject.SetActive(true);
      }
    }
  }
}
