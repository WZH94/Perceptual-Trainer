using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class PopupScreen : MonoBehaviour
  {
    [SerializeField] private TMP_Text titleText = null;
    [SerializeField] private Image popupImage = null;
    [SerializeField] private TMP_Text descriptionText = null;

    public void SetContents( PopupContent content )
    {
      SetContents(content.Title, content.Image, content.Description);
    }    
    
    public void SetContents( string title, Sprite image, string description )
    {
      titleText.text = title;
      descriptionText.text = description;

      if (image != null) popupImage.sprite = image;
    }
  }
}
