using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using User;
  
  public class ShowDialogPopupOnAwake : MonoBehaviour
  {
    [SerializeField] private DialogPopup dialogPopup = null;
    [SerializeField] private bool isProgrammePopup = false;
    [SerializeField] private bool isCoursesPopup = false;
    [SerializeField] private bool isHoverImage = false;
    [SerializeField] private bool isPopupImage = false;

    private void Start()
    {
      UserProfile userProfile = UserProfile.Instance;

      if (isProgrammePopup && !userProfile.GetProgrammePopupEncountered())
      {
        dialogPopup.ShowDialogBox();
        userProfile.SetProgrammePopupEncountered();
      }

      if (isCoursesPopup && !userProfile.GetCoursePopupEncountered())
      {
        dialogPopup.ShowDialogBox();
        userProfile.SetCoursePopupEncountered();
      }

      if (isHoverImage && !userProfile.GetHoverImageEncountered())
      {
        dialogPopup.ShowDialogBox();
        userProfile.SetHoverImageEncountered();
      }

      if (isPopupImage && !userProfile.GetPopupImageEncountered())
      {
        dialogPopup.ShowDialogBox();
        userProfile.SetPopupImageEncountered();
      }

      Destroy(gameObject);
    }
  }
}
