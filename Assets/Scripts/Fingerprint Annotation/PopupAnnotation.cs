using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  using UI;

  public class PopupAnnotation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    private PopupAnnotationInformation popupInformation;
    private PopupAnnotation linkedAnnotation;
    private AnnotationAnswerPanel annotationAnswerPanel;
    
    public Image ImageComponent;

    private bool pointerIsOverElement = false;
    public bool PointerIsOverElement => pointerIsOverElement;

    public DetailType DetailType => popupInformation.DetailType;

    public void OnPointerEnter( PointerEventData eventData )
    {
      pointerIsOverElement = true;

      annotationAnswerPanel.SetInformation(popupInformation);

      // Glow
      ImageComponent.CrossFadeColor(Color.gray, 0.2f, true, false);
      if (linkedAnnotation) linkedAnnotation.ImageComponent.CrossFadeColor(Color.gray, 0.2f, false, false);
    }

    public void OnPointerExit( PointerEventData eventData )
    {
      pointerIsOverElement = false;

      annotationAnswerPanel.Hide();

      if (!linkedAnnotation || !linkedAnnotation.PointerIsOverElement)
      {
        // Stop glow
        ImageComponent.CrossFadeColor(Color.white, 0.2f, true, false);
        if (linkedAnnotation) linkedAnnotation.ImageComponent.CrossFadeColor(Color.white, 0.2f, false, false);
      }
    }

    public void Initialise( PopupAnnotationInformation info, PopupAnnotation linkedTo )
    {
      ImageComponent = GetComponent<Image>();
      annotationAnswerPanel = FindObjectOfType<AnnotationAnswerPanel>(true);

      popupInformation = info;
      linkedAnnotation = linkedTo;
    }
  }
}
