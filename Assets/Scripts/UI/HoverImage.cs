using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class HoverImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    [SerializeField] private Image defaultImage = null;
    [SerializeField] private Image hoverImage = null;

    [SerializeField, Range(0f, 2f)] private float crossfadeTime = 0.5f;

    public void OnPointerEnter( PointerEventData eventData )
    {
      defaultImage.CrossFadeAlpha(0f, crossfadeTime, false);
      hoverImage.CrossFadeAlpha(1f, crossfadeTime, false);
    }

    public void OnPointerExit( PointerEventData eventData )
    {
      defaultImage.CrossFadeAlpha(1f, crossfadeTime, false);
      hoverImage.CrossFadeAlpha(0f, crossfadeTime, false);
    }
  }
}
