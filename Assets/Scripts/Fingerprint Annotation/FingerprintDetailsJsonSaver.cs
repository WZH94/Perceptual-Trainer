using System.IO;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public class FingerprintDetailsJsonSaver : MonoBehaviour
  {
    [SerializeField] private Annotater annotater = null;
    [SerializeField] private TMP_Dropdown patternTypeDropdownList = null;
    [SerializeField] private TMP_InputField ridgeCountInputField = null;
    [SerializeField] private Slider detailScaleSlider = null;
    [SerializeField] private Toggle canUseForCoreToggle = null;
    [SerializeField] private Toggle canUseForDeltaToggle = null;

#if UNITY_EDITOR
    private const string EditorDatabaseDirectory = "Assets/Resources/Fingerprint Database";
#else
    private string standaloneDatabaseDirectory;

    private void Awake()
    {
      standaloneDatabaseDirectory = $"{Application.dataPath}/Fingerprint Json Information";
    }
#endif

    public void OnSaveJSON()
    {
      Detail[] allMinutiae = annotater.GetComponentsInChildren<Detail>();
      Sprite fingerprintSprite = annotater.GetComponent<Image>().sprite;

      FingerprintDetails fingerprintDetails = new FingerprintDetails();
      fingerprintDetails.FingerprintSpriteName = fingerprintSprite.name;
      if (int.TryParse(ridgeCountInputField.text, out int ridgeCount)) fingerprintDetails.RidgeCount = ridgeCount;
      fingerprintDetails.PatternType = (PatternType)(1 << patternTypeDropdownList.value);
      fingerprintDetails.DetailsScale = detailScaleSlider.value;
      fingerprintDetails.CanUseForCore = canUseForCoreToggle.isOn;
      fingerprintDetails.CanUseForDelta = canUseForDeltaToggle.isOn;

      foreach (Detail minutiae in allMinutiae)
      {
        switch (minutiae.DetailType)
        {
          case DetailType.Core:
            fingerprintDetails.CoreCoordinates.Add(new SerializedVector2(minutiae.GetComponent<RectTransform>().anchoredPosition));
            break;

          case DetailType.Delta:
            fingerprintDetails.DeltaCoordinates.Add(new SerializedVector2(minutiae.GetComponent<RectTransform>().anchoredPosition));
            break;

          case DetailType.Minutiae:
            fingerprintDetails.MinutiaeCoordinates.Add(new SerializedVector2(minutiae.GetComponent<RectTransform>().anchoredPosition));
            break;
        }
      }

      string jsonData = JsonUtility.ToJson(fingerprintDetails);
      string imageName = annotater.GetComponent<Image>().sprite.name;

      string fullPath = "";

#if UNITY_EDITOR
      fullPath = $"{EditorDatabaseDirectory}/{imageName}details.json";
#else
      fullPath = $"{standaloneDatabaseDirectory}/{imageName}details.json";
#endif

      using (FileStream fs = new FileStream(fullPath, FileMode.Create))
      {
        using (StreamWriter writer = new StreamWriter(fs))
        {
          writer.Write(jsonData);
        }
      }

#if UNITY_EDITOR
      UnityEditor.AssetDatabase.Refresh();
#endif
    }
  }
}
