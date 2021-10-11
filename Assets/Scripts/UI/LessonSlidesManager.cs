using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using User;

  public class LessonSlidesManager : MonoBehaviour
  {
    [Header("Components")]
    [SerializeField] private GameObject[] slides;
    [SerializeField] private TMP_Text slideNumberText = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button backButton = null;
    [SerializeField] private Button completeLessonButton = null;

    private int currentSlideIndex = 0;

    private float timeStarted;

    private void Awake()
    {
      backButton.gameObject.SetActive(false);
      completeLessonButton.gameObject.SetActive(false);
      slides[0].SetActive(true);

      for (int i = 1; i < slides.Length; ++i)
      {
        slides[i].SetActive(false);
      }

      UpdateSlideNumber();

      timeStarted = Time.time;
    }

    private void OnDisable()
    {
      UserProfile.Instance.AddTimeSpentInLessonsAndExercises(Time.time - timeStarted);
    }

    private void UpdateSlideNumber()
    {
      if (slideNumberText != null) slideNumberText.text = $"Slide {currentSlideIndex + 1}/{slides.Length}";
    }

    public void OnNext()
    {
      slides[currentSlideIndex].SetActive(false);
      ++currentSlideIndex;
      slides[currentSlideIndex].SetActive(true);

      if (currentSlideIndex > 0)
      {
        backButton.gameObject.SetActive(true);
      }

      // Lesson completed, can already save data
      if (currentSlideIndex == slides.Length - 1)
      {
        nextButton.gameObject.SetActive(false);
        completeLessonButton.gameObject.SetActive(true);
      }

      UpdateSlideNumber();
    }

    public void OnBack()
    {
      slides[currentSlideIndex].SetActive(false);
      --currentSlideIndex;
      slides[currentSlideIndex].SetActive(true);

      if (currentSlideIndex < slides.Length - 1)
      {
        nextButton.gameObject.SetActive(true);
        completeLessonButton.gameObject.SetActive(false);
      }

      if (currentSlideIndex == 0)
      {
        backButton.gameObject.SetActive(false);
      }

      UpdateSlideNumber();
    }
  }
}
