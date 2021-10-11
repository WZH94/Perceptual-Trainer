using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  public class Annotater : MonoBehaviour, IPointerClickHandler
  {
    [Header("Annotation Prefabs")]
    [SerializeField] private Detail minutiaePrefab = null;
    [SerializeField] private PopupAnnotation answerPrefab = null;
    [Header("Detail Information")]
    [SerializeField] private DetailType currentSelectedDetail;
    [SerializeField] private DetailColoursScriptableObject detailColours = null;
    [Header("UI Components")]
    [SerializeField] private TMP_Text currentSeletedText = null;
    [SerializeField] private Button eraseButton = null;

    private RectTransform imageTransform;
    private Image originalImage;

    private Dictionary<DetailType, List<Detail>> annotatedDetails = new Dictionary<DetailType, List<Detail>>();
    private Dictionary<DetailType, List<PopupAnnotation>> answerPopupAnnotations = new Dictionary<DetailType, List<PopupAnnotation>>();

    private Button currentSelectedButton = null;
    private bool isErasing = false;
    private float detailsScale = 1f;

    // Visibility of details when viewing answers
    private bool coreAreVisible = true;
    private bool deltaAreVisible = true;
    private bool minutiaeAreVisible = true;
    private bool answersAreVisible = true;
    private bool answeredAreVisible = true;

    private const string CurrentSelectedText = "Selected Tool: ";

    private void Awake()
    {
      imageTransform = GetComponent<RectTransform>();
      originalImage = GetComponent<Image>();
      detailColours.Initialise();
    }

    public void OnPointerClick( PointerEventData eventData )
    {
      if (originalImage.sprite == null) return;

      if (currentSelectedDetail != 0)
      {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(imageTransform, eventData.position, eventData.pressEventCamera, out Vector2 point);

        CreateDetailAtPoint(currentSelectedDetail, point);
      }

      else if (isErasing)
      {
        Detail clickedUiElement = eventData.pointerPressRaycast.gameObject.GetComponent<Detail>();

        if (clickedUiElement != null)
        {
          annotatedDetails[clickedUiElement.DetailType].Remove(clickedUiElement.GetComponent<Detail>());
          Destroy(clickedUiElement.gameObject);
        }
      }
    }

    private void CreateDetailAtPoint( DetailType detailType, Vector2 point )
    {
      Detail detail = Instantiate(minutiaePrefab, transform);
      detail.DetailType = detailType;

      detail.GetComponent<RectTransform>().anchoredPosition = point;
      detail.transform.localScale = Vector3.one * detailsScale;

      Color detailColour = detailColours.GetColourOfDetail(detailType);
      detailColour.a = detail.GetComponent<Image>().color.a;
      detail.GetComponent<Image>().color = detailColour;

      if (!annotatedDetails.ContainsKey(detailType)) 
        annotatedDetails.Add(detailType, new List<Detail> { detail });
      else annotatedDetails[detailType].Add(detail);
    }

    public void RemoveAllAnnotations()
    {
      foreach (List<Detail> details in annotatedDetails.Values)
      {
        foreach (Detail detail in details)
        {
          Destroy(detail.gameObject);
        }
      }

      annotatedDetails.Clear();

      foreach (List<PopupAnnotation> answers in answerPopupAnnotations.Values)
      {
        foreach (PopupAnnotation answer in answers)
        {
          Destroy(answer.gameObject);
        }
      }

      answerPopupAnnotations.Clear();
    }

    public void SwitchDetailType( Button button )
    {
      isErasing = false;
      currentSelectedDetail = button.GetComponent<Detail>().DetailType;

      currentSeletedText.text = CurrentSelectedText + Utils.AddSpaceBeforeCapitals(currentSelectedDetail.ToString());

      if (currentSelectedButton != null) currentSelectedButton.interactable = true;
      currentSelectedButton = button;
      currentSelectedButton.interactable = false;

      eraseButton.interactable = true;
    }

    public void SetEraseTool()
    {
      isErasing = true;
      currentSelectedDetail = 0;

      currentSeletedText.text = CurrentSelectedText + "Erase Tool";

      if (currentSelectedButton != null) currentSelectedButton.interactable = true;
      eraseButton.interactable = false;
    }

    public void LoadAnnotationsFromImageSet( (Sprite, FingerprintDetails) imageSet )
    {
      // Remove existing annotations first
      RemoveAllAnnotations();

      // Change the sprites of the images
      originalImage.sprite = imageSet.Item1;

      FingerprintDetails fingerprintDetails = imageSet.Item2;

      // Check if there is annotations to create
      if (fingerprintDetails != null)
      {
        Array detailTypes = Enum.GetValues(typeof(DetailType));

        foreach (DetailType detailType in detailTypes)
        {
          IList<SerializedVector2> serializedPoints = fingerprintDetails.GetDetailsOfType(detailType);

          foreach (SerializedVector2 serializedPoint in serializedPoints)
            CreateDetailAtPoint(detailType, serializedPoint.ToVector2());
        }
      }
    }

    public void LoadAnnotationsOfDetailType( DetailType detailType, IList<SerializedVector2> coordinates )
    {
      foreach (SerializedVector2 serializedVector2 in coordinates)
      {
        CreateDetailAtPoint(detailType, serializedVector2.ToVector2());
      }
    }

    public void LoadAnswerAndLinkToDetail( PopupAnnotationInformation info, Vector2 point, Detail existingDetail )
    {
      // Create an answer annotation component on the existing detial if there is one to link to
      PopupAnnotation existingDetailPopupAnnotation = null;
      if (existingDetail != null) existingDetailPopupAnnotation = existingDetail.gameObject.AddComponent<PopupAnnotation>();

      // Create the answer annotation and link both together
      PopupAnnotation answerPopupAnnotation = Instantiate(answerPrefab, transform);
      answerPopupAnnotation.Initialise(info, existingDetailPopupAnnotation);
      if (existingDetailPopupAnnotation) existingDetailPopupAnnotation.Initialise(info, answerPopupAnnotation);

      Color annotationColor = detailColours.GetColourOfDetail(info.DetailType);
      if (existingDetail == null) 
      { 
        annotationColor.r *= 0.75f; 
        annotationColor.g *= 0.75f; 
        annotationColor.b *= 0.75f; 
        annotationColor.a *= 0.75f; 
      }
      answerPopupAnnotation.GetComponent<Image>().color = annotationColor;
      answerPopupAnnotation.GetComponent<RectTransform>().anchoredPosition = point;
      answerPopupAnnotation.transform.localScale = Vector3.one * detailsScale;

      if (!answerPopupAnnotations.ContainsKey(info.DetailType))
        answerPopupAnnotations.Add(info.DetailType, new List<PopupAnnotation> { answerPopupAnnotation });
      else answerPopupAnnotations[info.DetailType].Add(answerPopupAnnotation);
    }

    public void LoadUnlinkedAnnotationAnswerToDetail( Detail detail )
    {
      PopupAnnotationInformation emptyPopupAnnotationInformation = new PopupAnnotationInformation(detail.DetailType, false, 0f, 0f, 0f);
      detail.gameObject.AddComponent<PopupAnnotation>().Initialise(emptyPopupAnnotationInformation, null);

      Image imageComponent = detail.GetComponent<Image>();
      Color detailColour = imageComponent.color;
      detailColour.r *= 0.75f;
      detailColour.g *= 0.75f;
      detailColour.b *= 0.75f;
      detailColour.a *= 0.9f;
      imageComponent.color = detailColour;
    }

    public IList<Detail> GetAnnotationsOfDetailType( DetailType detailType )
    {
      if (annotatedDetails.ContainsKey(detailType)) return annotatedDetails[detailType];
      else return new List<Detail>();
    }

    public void LoadPrint( Sprite image, float scale )
    {
      RemoveAllAnnotations();

      originalImage.sprite = image;
      detailsScale = scale;
    }

    public void SetDetailsScale( float scale )
    {
      detailsScale = scale;

      foreach (List<Detail> details in annotatedDetails.Values)
      {
        foreach (Detail detail in details) detail.transform.localScale = Vector3.one * detailsScale;
      }
    }

    public float GetPrefabRadius()
    {
      return minutiaePrefab.GetComponent<RectTransform>().rect.width * 0.5f * detailsScale;
    }

    public void ToggleCoreVisibility( bool isVisible )
    {
      coreAreVisible = isVisible;

      if (!isVisible || answeredAreVisible)
      {
        List<Detail> details = annotatedDetails[DetailType.Core];
        foreach (Detail detail in details) detail.gameObject.SetActive(isVisible);
      }
      
      if (!isVisible || answersAreVisible)
      {
        List<PopupAnnotation> answers = answerPopupAnnotations[DetailType.Core];
        foreach (PopupAnnotation answer in answers) answer.gameObject.SetActive(isVisible);
      }
    }

    public void ToggleDeltaVisibility( bool isVisible )
    {
      deltaAreVisible = isVisible;

      if (!isVisible || answeredAreVisible)
      {
        List<Detail> details = annotatedDetails[DetailType.Delta];
        foreach (Detail detail in details) detail.gameObject.SetActive(isVisible);
      }

      if (!isVisible || answersAreVisible)
      {
        List<PopupAnnotation> answers = answerPopupAnnotations[DetailType.Delta];
        foreach (PopupAnnotation answer in answers) answer.gameObject.SetActive(isVisible);
      }
    }

    public void ToggleMinutiaeVisibility( bool isVisible )
    {
      minutiaeAreVisible = isVisible;

      if (!isVisible || answeredAreVisible)
      {
        List<Detail> details = annotatedDetails[DetailType.Minutiae];
        foreach (Detail detail in details) detail.gameObject.SetActive(isVisible);
      }

      if (!isVisible || answersAreVisible)
      {
        List<PopupAnnotation> answers = answerPopupAnnotations[DetailType.Minutiae];
        foreach (PopupAnnotation answer in answers) answer.gameObject.SetActive(isVisible);
      }
    }

    public void ToggleAnswersVisiblity( bool isVisible )
    {
      answersAreVisible = isVisible;

      Array detailTypes = Enum.GetValues(typeof(DetailType));

      foreach (DetailType detailType in detailTypes)
      {
        if (!answerPopupAnnotations.ContainsKey(detailType)) continue;

        if ((!isVisible || (detailType == DetailType.Core && coreAreVisible)) || 
          (detailType == DetailType.Delta && deltaAreVisible) ||
          (detailType == DetailType.Minutiae && minutiaeAreVisible))
        {
          List<PopupAnnotation> answersList = answerPopupAnnotations[detailType];

          foreach (PopupAnnotation answer in answersList)
          {
            answer.gameObject.SetActive(isVisible);
          }
        }
      }
    }

    public void ToggleAnsweredVisiblity( bool isVisible )
    {
      answeredAreVisible = isVisible;

      Array detailTypes = Enum.GetValues(typeof(DetailType));

      foreach (DetailType detailType in detailTypes)
      {
        if (!annotatedDetails.ContainsKey(detailType)) continue;

        if ((!isVisible || (detailType == DetailType.Core && coreAreVisible)) ||
          (detailType == DetailType.Delta && deltaAreVisible) ||
          (detailType == DetailType.Minutiae && minutiaeAreVisible))
        {
          List<Detail> detailsList = annotatedDetails[detailType];

          foreach (Detail detail in detailsList)
          {
            detail.gameObject.SetActive(isVisible);
          }
        }
      }
    }
  }
}
