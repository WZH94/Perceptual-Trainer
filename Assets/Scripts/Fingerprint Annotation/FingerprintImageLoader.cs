using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public class FingerprintImageLoader : MonoBehaviour
  {
    [SerializeField] private FingerprintImageSet imageButtonPrefab = null;
    [SerializeField] private VerticalLayoutGroup content = null;
    [SerializeField] private Button loadImageButton = null;
    [SerializeField] private Annotater annotater = null;
    [SerializeField] private TMP_Dropdown patternTypeDropdownList = null;
    [SerializeField] private TMP_InputField ridgeCountInputField = null;
    [SerializeField] private Toggle canUseForCoreToggle = null;
    [SerializeField] private Toggle canUseForDeltaToggle = null;
    [SerializeField] private Slider detailScaleSlider = null;
    [SerializeField] private TMP_Text imageNameText = null;
    [SerializeField] private TMP_Text[] patternCountTexts = null;
    [SerializeField] private Button deleteDetailsButton = null;
    [SerializeField] private Button deleteJsonButton = null;
     
    private const string DatabaseFolderName = "Fingerprint Database";

    private Dictionary<PatternType, int> numberOfPatterns = new Dictionary<PatternType, int>();
    private List<FingerprintImageSet> listOfSets = new List<FingerprintImageSet>();
    private FingerprintImageSet selectedSet = null;
    private string currentLoadedImage = "";

#if !UNITY_EDITOR
    private string standaloneJsonFolderPath;

    private void Awake()
    {
      standaloneJsonFolderPath = $"{Application.dataPath}/Fingerprint Json Information";
    }
#endif

    private void OnEnable()
    {
      loadImageButton.interactable = false;

      numberOfPatterns.Clear();
      foreach (PatternType pattern in Enum.GetValues(typeof(PatternType))) numberOfPatterns.Add(pattern, 0);

      List<(Sprite, FingerprintDetails)> fingerprintDatabase = GetFingerprintDatabase();

      CreateUIDatabaseList(fingerprintDatabase);
      ShowPatternCounts();
    }

    private void OnDisable()
    {
      foreach (FingerprintImageSet imageSet in listOfSets)
      {
        imageSet.OnImageSetSelected -= SetSelectedSet;
        Destroy(imageSet.gameObject);
      }

      listOfSets.Clear();

      selectedSet = null;
    }

    private List<(Sprite, FingerprintDetails)> GetFingerprintDatabase()
    {
      List<(Sprite, FingerprintDetails)> fingerprintDatabase = new List<(Sprite, FingerprintDetails)>();

      UnityEngine.Object[] fingerprintImages = Resources.LoadAll(DatabaseFolderName, typeof(Sprite));

      foreach (UnityEngine.Object fingerprintImageObject in fingerprintImages)
      {
        Sprite fingerprintImage = (Sprite)fingerprintImageObject;
        string jsonFingerprintDetails = string.Empty;

#if UNITY_EDITOR
        string pathName = $"{Application.dataPath}/Resources/{DatabaseFolderName}/{fingerprintImage.name}details.json";

        if (File.Exists(pathName)) jsonFingerprintDetails = Resources.Load<TextAsset>($"{DatabaseFolderName}/{fingerprintImage.name}details").text;
#else
        if (!Directory.Exists(standaloneJsonFolderPath)) Directory.CreateDirectory(standaloneJsonFolderPath);
        else 
        {
          string pathName = $"{standaloneJsonFolderPath}/{fingerprintImage.name}details.json";

          if (File.Exists(pathName)) jsonFingerprintDetails = File.ReadAllText(pathName);
        }
#endif
        FingerprintDetails fingerprintDetails = null;

        if (!string.IsNullOrEmpty(jsonFingerprintDetails))
        {
          fingerprintDetails = JsonUtility.FromJson<FingerprintDetails>(jsonFingerprintDetails);
          ++numberOfPatterns[fingerprintDetails.PatternType];
        }

        fingerprintDatabase.Add((fingerprintImage, fingerprintDetails));
      }

      return fingerprintDatabase;
    }

    private void CreateUIDatabaseList( List<(Sprite, FingerprintDetails)> fingerprintDatabase )
    {
      foreach ((Sprite, FingerprintDetails) fingerprintSet in fingerprintDatabase)
      {
        FingerprintImageSet fingerprintImageSet = Instantiate(imageButtonPrefab, content.transform);
        fingerprintImageSet.SetImageSet(fingerprintSet);
        fingerprintImageSet.OnImageSetSelected += SetSelectedSet;
        listOfSets.Add(fingerprintImageSet);

        if (fingerprintSet.Item1.name == currentLoadedImage)
        {
          selectedSet = fingerprintImageSet;
          selectedSet.GetComponent<Button>().interactable = false;
          selectedSet.GetComponent<Image>().color = Color.green;

          loadImageButton.interactable = true;
        }
      }
    }

    private void ShowPatternCounts()
    {
      patternCountTexts[0].text = $"LOOPS: {numberOfPatterns[PatternType.LeftLoop] + numberOfPatterns[PatternType.RightLoop]}";
      patternCountTexts[1].text = $"Left Loops: {numberOfPatterns[PatternType.LeftLoop]}";
      patternCountTexts[2].text = $"Right Loops: {numberOfPatterns[PatternType.RightLoop]}";
      patternCountTexts[3].text = $"WHORLS: {numberOfPatterns[PatternType.PlainWhorl] + numberOfPatterns[PatternType.CentralPocketLoop] + numberOfPatterns[PatternType.DoubleLoopWhorl] + numberOfPatterns[PatternType.AccidentalWhorl]}";
      patternCountTexts[4].text = $"Plain Whorls: {numberOfPatterns[PatternType.PlainWhorl]}";
      patternCountTexts[5].text = $"Central Pocket Loops: {numberOfPatterns[PatternType.CentralPocketLoop]}";
      patternCountTexts[6].text = $"Double Loop Whorls: {numberOfPatterns[PatternType.DoubleLoopWhorl]}";
      patternCountTexts[7].text = $"Accidental Whorls: {numberOfPatterns[PatternType.AccidentalWhorl]}";
      patternCountTexts[8].text = $"ARCHES: {numberOfPatterns[PatternType.PlainArch] + numberOfPatterns[PatternType.TentedArch]}";
      patternCountTexts[9].text = $"Plain Arches: {numberOfPatterns[PatternType.PlainArch]}";
      patternCountTexts[10].text = $"Tented Arches: {numberOfPatterns[PatternType.TentedArch]}";
    }

    private void SetSelectedSet( FingerprintImageSet newlySelectedSet )
    {
      if (selectedSet) selectedSet.GetComponent<Button>().interactable = true;
      selectedSet = newlySelectedSet;
      selectedSet.GetComponent<Button>().interactable = false;
      loadImageButton.interactable = true;
    }

    public void ActivateImageLoader()
    {
      gameObject.SetActive(true);
    }

    public void DeactivateImageLoader()
    {
      gameObject.SetActive(false);
    }

    public void LoadImage()
    {
      annotater.LoadAnnotationsFromImageSet(selectedSet.ImageSet);
      FingerprintDetails fingerprintDetails = selectedSet.ImageSet.Item2;

      deleteJsonButton.gameObject.SetActive(false);

      canUseForCoreToggle.isOn = true;
      canUseForDeltaToggle.isOn = true;

      if (fingerprintDetails != null)
      {
        patternTypeDropdownList.value = (int)Mathf.Log((int)fingerprintDetails.PatternType, 2);
        ridgeCountInputField.text = fingerprintDetails.RidgeCount.ToString();
        detailScaleSlider.value = fingerprintDetails.DetailsScale;
        canUseForCoreToggle.isOn = fingerprintDetails.CanUseForCore;
        canUseForDeltaToggle.isOn = fingerprintDetails.CanUseForDelta;

        deleteJsonButton.gameObject.SetActive(true);
      }

      imageNameText.text = selectedSet.ImageSet.Item1.name;
      currentLoadedImage = selectedSet.ImageSet.Item1.name;

      DeactivateImageLoader();

      deleteDetailsButton.gameObject.SetActive(true);
    }

    public void DeleteLoadedJson()
    {
      if (!string.IsNullOrEmpty(currentLoadedImage))
      {
        string pathName;
#if UNITY_EDITOR
        pathName = $"{Application.dataPath}/Resources/{DatabaseFolderName}/{currentLoadedImage}details.json";
#else
        pathName = $"{standaloneJsonFolderPath}/{currentLoadedImage}details.json";
#endif

        File.Delete(pathName);
      }
    }
  }
}
