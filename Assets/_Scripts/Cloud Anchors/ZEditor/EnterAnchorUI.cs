using TMPro;
using UnityEngine;

public class EnterAnchorUI : MonoBehaviour
{
    [SerializeField] private GameObject resolveFailed;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private SessionControllerEditor controller;
    [SerializeField] private MainMenuEditor mainMenuUI;

    private string anchorName;
    private string id;
    private float xLocalPos;
    private float yLocalPos;
    private float zLocalPos;
    private float localEulerY;
    private float localScale;

    private float[] floatParams;

    private void Awake()
    {
        floatParams = new float[5];
    }

    private void OnEnable()
    {
        resolveFailed.SetActive(false);
        inputField.text = string.Empty;
    }

    public void Resolve()
    {
        if (inputField.text.Length > 0)
        {
            if (ValidateInput())
            {
                controller.SaveCurrentCloudAnchorId(anchorName, id);

                Vector3 localPos = new Vector3(xLocalPos, yLocalPos, zLocalPos);
                controller.SaveTransform(localPos, localEulerY, localScale);
                mainMenuUI.EditChurch();
                
            }
            else
            {
                resolveFailed.SetActive(true);
            }
        }
    }


    public void HideResolveFailed()
    {
        resolveFailed.SetActive(false);
    }

    private bool ValidateInput()
    {

        string[] parameters = inputField.text.Split(';');

        if (parameters.Length != 7)
        {
            return false;
        }

        anchorName = parameters[0];
        id = parameters[1];

        if (!Parse(parameters))
        {
            return false;
        }


        xLocalPos = floatParams[0];
        yLocalPos = floatParams[1];
        zLocalPos = floatParams[2];
        localEulerY = floatParams[3];
        localScale = floatParams[4];

        return true;
    }
    private bool Parse(string[] paramaters)
    {
        for(int i = 0; i < floatParams.Length; i++)
        {
            if(!float.TryParse(paramaters[i + 2], out floatParams[i]))
            {
                return false;
            }
        }
        return true;
    }

}
