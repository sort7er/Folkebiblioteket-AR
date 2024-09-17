using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class EnterAnchorUI : MonoBehaviour
{
    [SerializeField] private GameObject resolveFailed;
    [SerializeField] private TMP_InputField inputField;

    private void OnEnable()
    {
        resolveFailed.SetActive(false);
        inputField.text = string.Empty;
    }

    public void Resolve()
    {
        ValidateInput();
    }


    public void HideResolveFailed()
    {
        resolveFailed.SetActive(false);
    }

    private void ValidateInput()
    {
        if (inputField.text.Length > 0)
        {
            string[] parameters = inputField.text.Split(';');
            for(int i = 0; i < parameters.Length; i++)
            {
                Debug.Log(parameters[i]);
            }
            Debug.Log(parameters.Length);
        }
    }
}
