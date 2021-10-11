using UnityEngine;
using UnityEngine.UI;

namespace CwispyStudios.FingerprintTrainer.UI
{
  using User;
  using Modules;

  [RequireComponent(typeof(Image), typeof(GraphicsGradientParent))]
  public class StarRankImage : MonoBehaviour
  {
    [SerializeField] private Image starBaseImage = null;
    [SerializeField] private Color defaultOutlineColour;

    private Image outlineImage;
    private GraphicsGradientParent gradientParent;

    private bool isInitialised = false;

    private void Awake()
    {
      if (!isInitialised) Initialise();
    }

    private void Initialise()
    {
      outlineImage = GetComponent<Image>();
      gradientParent = GetComponent<GraphicsGradientParent>();

      isInitialised = true;
    }
    
    public void SetStarImageBasedOnRank( ExerciseRank rank )
    {
      if (!isInitialised) Initialise();

      switch (rank)
      {
        case ExerciseRank.Fail:
          gradientParent.SetGraphicsNegative();
          outlineImage.color = defaultOutlineColour;
          starBaseImage.gameObject.SetActive(false);
          break;

        case ExerciseRank.Pass:
          gradientParent.SetGraphicsNegative();
          starBaseImage.gameObject.SetActive(true);
          break;

        case ExerciseRank.Silver:
          gradientParent.SetGraphicsNeutral();
          starBaseImage.gameObject.SetActive(true);
          break;

        case ExerciseRank.Gold:
          gradientParent.SetGraphicsPositive();
          starBaseImage.gameObject.SetActive(true);
          break;
      }
    }
  }
}
