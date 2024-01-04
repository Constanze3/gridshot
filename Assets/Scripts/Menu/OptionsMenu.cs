using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private SensitivityMenu sensitivityMenu;

    private OptionsMenuTab currentTab;

    public SensitivityMenu SensitivityMenu {
        get {
            return sensitivityMenu;
        }
    }

    private void OnEnable()
    {
        OpenTab(sensitivityMenu);
    }

    private void OpenTab(OptionsMenuTab tab) {
        if (currentTab) currentTab.gameObject.SetActive(false);
        tab.gameObject.SetActive(true);
        currentTab = tab;
    }
}
