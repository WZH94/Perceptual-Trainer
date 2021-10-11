using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using Modules;
  using SceneNavigation;

  public class LessonContentScreen : MonoBehaviour
  {
    [SerializeField] private TMP_Text moduleNameText = null;
    [SerializeField] private TMP_Text moduleDescriptionText = null;
    [SerializeField] private Image completedImage = null;
    [SerializeField] private SceneLoader beginLessonButton = null;

    public void SetLessonContent( GameObject lessonDescriptionObject )
    {
      gameObject.SetActive(true);
      completedImage.gameObject.SetActive(false);

      LessonSelectionButton moduleDescription = lessonDescriptionObject.GetComponent<LessonSelectionButton>();
      LessonDescriptionContentScriptableObject lessonDescription = moduleDescription.LessonDescription;

      moduleNameText.text = lessonDescription.ModuleName;
      moduleDescriptionText.text = lessonDescription.ModuleDescription;
      beginLessonButton.SetSceneToLoad(lessonDescription.SceneName);

      if (moduleDescription.LessonCompleted) completedImage.gameObject.SetActive(true);
    }
  }
}
