using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class DialogPopup : MonoBehaviour
  {
    public void ShowDialogBox()
    {
      gameObject.SetActive(true);
    }

    public void HideDialogBox()
    {
      gameObject.SetActive(false);
    }
  }
}
