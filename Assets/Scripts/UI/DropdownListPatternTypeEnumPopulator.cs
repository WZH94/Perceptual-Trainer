using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using FingerprintAnnotation;

  [RequireComponent(typeof(TMP_Dropdown))]
  public class DropdownListPatternTypeEnumPopulator : MonoBehaviour
  {
    private TMP_Dropdown dropdownList;

    private void Awake()
    {
      dropdownList = GetComponent<TMP_Dropdown>();

      List<string> enumNames = new List<string>(Enum.GetNames(typeof(PatternType)));
      for (int i = 0; i < enumNames.Count; ++i)
      {
        enumNames[i] = Utils.AddSpaceBeforeCapitals(enumNames[i]);
      }

      dropdownList.AddOptions(enumNames);
    }

    public void SetBasicPatternTypes()
    {
      dropdownList.ClearOptions();

      List<string> basicPatternTypes = new List<string> { "Loop", "Whorl", "Arch" };

      dropdownList.AddOptions(basicPatternTypes);
    }
  }
}
