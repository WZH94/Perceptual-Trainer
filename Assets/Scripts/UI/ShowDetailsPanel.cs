using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using FingerprintAnnotation;

  public class ShowDetailsPanel : MonoBehaviour
  {
    [SerializeField] private Toggle coreToggle = null;
    [SerializeField] private Toggle deltaToggle = null;
    [SerializeField] private Toggle minutiaeToggle = null;
    [SerializeField] private Toggle answersToggle = null;
    [SerializeField] private Toggle answeredToggle = null;

    private bool isActiveThisExercise = true;

    public void SetAvailableOptions( DetailType testedDetailTypes )
    {
      if (testedDetailTypes == 0) isActiveThisExercise = false;

      if (!testedDetailTypes.HasFlag(DetailType.Core)) coreToggle.gameObject.SetActive(false);
      if (!testedDetailTypes.HasFlag(DetailType.Delta)) deltaToggle.gameObject.SetActive(false);
      if (!testedDetailTypes.HasFlag(DetailType.Minutiae)) minutiaeToggle.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
      if (isActiveThisExercise)
      {
        gameObject.SetActive(true);

        coreToggle.isOn = true;
        deltaToggle.isOn = true;
        minutiaeToggle.isOn = true;
        answersToggle.isOn = true;
        answeredToggle.isOn = true;
      }
    }

    public void HidePanel()
    {
      gameObject.SetActive(false);
    }
  }
}
