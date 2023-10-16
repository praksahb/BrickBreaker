using System;
using UnityEngine;

namespace BrickBreaker.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuPanel mainMenuPanel;
        [SerializeField] private PlayMenuPanel playMenuPanel;
        [SerializeField] private OptionsMenuPanel optionMenuPanel;

        public Action<MenuType> SelectMenu;

        private void Awake()
        {
            InitializeMenu();
        }

        private void OnEnable()
        {
            SelectMenu += SelectPanel;
        }

        private void OnDisable()
        {
            SelectMenu -= SelectPanel;
        }

        private void SelectPanel(MenuType type)
        {
            switch (type)
            {
                case MenuType.SelectOptions:
                    {
                        EnableOptionsMenu();
                        break;
                    }
                case MenuType.PlayLevel:
                    {
                        EnablePlayMenu();
                        break;
                    }
                case MenuType.MainMenu:
                    {
                        EnableMainMenu();
                        break;
                    }
                default:
                    {
                        Debug.LogError("Invalid Option");
                        break;
                    }
            }
        }

        private void SetActivePanel(GameObject panel, bool active)
        {
            panel.SetActive(active);
        }

        private void InitializeMenu()
        {
            mainMenuPanel.UIManager = this;
            playMenuPanel.UIManager = this;
            optionMenuPanel.UIManager = this;
            EnableMainMenu();
        }

        private void EnableMainMenu()
        {
            SetActivePanel(mainMenuPanel.gameObject, true);
            SetActivePanel(playMenuPanel.gameObject, false);
            SetActivePanel(optionMenuPanel.gameObject, false);
        }

        private void EnablePlayMenu()
        {
            SetActivePanel(playMenuPanel.gameObject, true);
            SetActivePanel(mainMenuPanel.gameObject, false);
            SetActivePanel(optionMenuPanel.gameObject, false);
        }

        private void EnableOptionsMenu()
        {
            SetActivePanel(optionMenuPanel.gameObject, true);
            SetActivePanel(mainMenuPanel.gameObject, false);
            SetActivePanel(playMenuPanel.gameObject, false);
        }
    }
}
