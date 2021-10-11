using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class PopupContent : MonoBehaviour
  {
    [SerializeField] private string title;
    [SerializeField] private Sprite image;
    [SerializeField, TextArea(3, 7)] private string description;

    public string Title => title;
    public Sprite Image => image;
    public string Description => description;
  }
}
