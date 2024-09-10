using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolveMenu : MonoBehaviour
{
    public CloudAnchorController controller;
    //public MultiselectionDropdown multiselection;
    public TMP_InputField inputField;
    public GameObject warning;
    public Button resolveButton;
    public Image resolveButtonImage;

    private StoredCloudAnchorCollection history;
    private Color activeColor;
    private Regex regex;

    private void Awake()
    {
        regex = regex = new Regex("^[a-zA-Z0-9-_,]*$");
        resolveButtonImage = resolveButton.GetComponent<Image>();
        activeColor = resolveButtonImage.color;
    }

    public void OnInputFieldChanged(string inputString)
    {
        warning.SetActive(!regex.IsMatch(inputString));
    }

    public void OnInputFieldEndEdit(string inputString)
    {
        if (warning.activeSelf)
        {
            return;
        }

        OnResolvingSelectionChanged();
    }
    public void OnResolvingSelectionChanged()
    {
        controller.resolvingSet.Clear();

        //List<int> selectedIndex = multiselection.selectedValues;

        //if(selectedIndex.Count > 0 )
        //{
        //    for (int i = 0; i < selectedIndex.Count; i++)
        //    {
        //        controller.resolvingSet.Add(history.collection[i].Id);
        //    }
        //}

        if(!warning.activeSelf && inputField.text.Length > 0)
        {
            string[] inputIDs = inputField.text.Split(",");
            if(inputIDs.Length > 0 )
            {
                controller.resolvingSet.UnionWith(inputIDs);
            }
        }

        SetButtonActive(controller.resolvingSet.Count > 0);
    }

    private void SetButtonActive(bool active)
    {
        resolveButtonImage.color = active ? activeColor : Color.grey;
        resolveButton.enabled = active;
    }

}
