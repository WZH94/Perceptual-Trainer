using UnityEngine;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public class ClassificationTools : MonoBehaviour
  {
    private TMP_Dropdown patternTypeDropdownList;

    private void Awake()
    {
      patternTypeDropdownList = GetComponentInChildren<TMP_Dropdown>();
    }

    public void SetAvailableTools( bool testingPattern )
    {
      if (!testingPattern) gameObject.SetActive(false);
    }

    public int GetPatternAnswer()
    {
      return patternTypeDropdownList.value;
    }
  }
}
