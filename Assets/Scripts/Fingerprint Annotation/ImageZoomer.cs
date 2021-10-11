using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CwispyStudios.FingerprintTrainer.FingerprintAnnotation
{
  using Modules;

  [RequireComponent(typeof(ScrollRect))]
  public class ImageZoomer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    [SerializeField, Range(5f, 15f)] private float maxZoomLevel = 10f;
    
    private ScrollRect scrollRect;
    private ExerciseHandler exercisePlanner;

    private bool isPointerOverImage = false;
    private float zoomLevel = 1f;
    public float ZoomLevel => zoomLevel;

    public void OnPointerEnter( PointerEventData eventData )
    {
      isPointerOverImage = true;
    }

    public void OnPointerExit( PointerEventData eventData )
    {
      isPointerOverImage = false;
    }

    private void Awake()
    {
      scrollRect = GetComponent<ScrollRect>();
      exercisePlanner = GetComponentInParent<ExerciseHandler>();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
      if (exercisePlanner) exercisePlanner.OnNewPrint += ResetZoom;
    }

    private void OnDisable()
    {
      if (exercisePlanner) exercisePlanner.OnNewPrint -= ResetZoom;
    }
#endif

    private void Update()
    {
      if (isPointerOverImage) ZoomImage();
    }

    private void ZoomImage()
    {
      Vector2 scrollDelta = Input.mouseScrollDelta;

      // Zoom
      if (Input.GetKey(KeyCode.LeftControl))
      {
        zoomLevel += scrollDelta.y * Time.deltaTime * scrollRect.scrollSensitivity;
        zoomLevel = Mathf.Clamp(zoomLevel, 1f, maxZoomLevel);

        scrollRect.content.localScale = Vector2.one * zoomLevel;
      }

      // Horizontal scroll
      else if (Input.GetKey(KeyCode.LeftShift))
      {
        float targetHorizontalPosition = scrollRect.horizontalNormalizedPosition;
        targetHorizontalPosition += -scrollDelta.y * Time.deltaTime * scrollRect.scrollSensitivity / zoomLevel;
        targetHorizontalPosition = Mathf.Clamp(targetHorizontalPosition, 0f, 1f);
        scrollRect.horizontalNormalizedPosition = targetHorizontalPosition;
      }

      else
      {
        float targetVerticalPosition = scrollRect.verticalNormalizedPosition;
        targetVerticalPosition += scrollDelta.y * Time.deltaTime * scrollRect.scrollSensitivity / zoomLevel;
        targetVerticalPosition = Mathf.Clamp(targetVerticalPosition, 0f, 1f);
        scrollRect.verticalNormalizedPosition = targetVerticalPosition;
      }
    }

    private void ResetZoom()
    {
      zoomLevel = 1f;
      scrollRect.content.localScale = Vector2.one;
      scrollRect.horizontalNormalizedPosition = 0.5f;
      scrollRect.verticalNormalizedPosition = 0.5f;
    }
  }
}
