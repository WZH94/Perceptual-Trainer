using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class NavigationButton : MonoBehaviour
  {
    [SerializeField] private GameObject panel = null;
    [SerializeField] private bool deactivatesButton = true;

    private MultiTargetButton button;

    private void Awake()
    {
      button = GetComponent<MultiTargetButton>();
    }

    public void SetMenuActive()
    {
      if (deactivatesButton) button.interactable = false;
      panel.SetActive(true);
    }

    public void SetMenuInactive()
    {
      if (deactivatesButton) button.interactable = true;
      panel.SetActive(false);
    }
  }
}
