using UnityEngine;

namespace CwispyStudios.FingerprintTrainer.UI
{
  public class Navigation : MonoBehaviour
  {
    [SerializeField] private NavigationButton currentMenu = null;
    [SerializeField] private NavigationButton[] managedMenus = null;

    private void Start()
    {
      foreach (NavigationButton navigationMenu in managedMenus)
      {
        navigationMenu.SetMenuInactive();
      }

      currentMenu?.SetMenuActive();
    }

    public void SetCurrentMenu( NavigationButton activeMenu )
    {
      currentMenu?.SetMenuInactive();
      currentMenu = activeMenu;
      currentMenu.SetMenuActive();
    }
  }
}
