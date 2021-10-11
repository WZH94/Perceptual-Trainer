using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.Modules.MCQ
{
  public class MinutiaeMCQSelection : MonoBehaviour
  {
    [SerializeField] private Button submitButton = null;

    private (Toggle, MinutiaeType) selectedToggle = (null, MinutiaeType.None);

    private void Awake()
    {
      submitButton.interactable = false;
    }

    public void SetMCQOption( Toggle option )
    {
      // Deselect
      if (option == selectedToggle.Item1)
      {
        ResetSelection();
      }

      else
      {
        if (selectedToggle.Item1 != null) selectedToggle.Item1.isOn = false;
        selectedToggle = (option, option.GetComponent<MinutiaeChoice>().MinutiaeType);
        selectedToggle.Item1.isOn = true;

        submitButton.interactable = true;
      }
    }

    public void ResetSelection()
    {
      if (selectedToggle.Item1 != null)
      {
        selectedToggle.Item1.isOn = false;
        selectedToggle = (null, MinutiaeType.None);
      }

      submitButton.interactable = false;
    }

    public MinutiaeType GetMCQSelection()
    {
      return selectedToggle.Item2;
    }
  }
}
