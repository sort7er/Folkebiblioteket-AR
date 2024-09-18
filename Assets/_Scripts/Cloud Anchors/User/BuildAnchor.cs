using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class BuildAnchor : MonoBehaviour
{
    [TextArea][SerializeField] private string anchorString;
    [SerializeField] private MainMenuUser mainMenu;
    [SerializeField] private SessionControllerUser controller;


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
        CheckAnchor();
    }

    private void CheckAnchor()
    {
        if (ValidateInput())
        {
            Vector3 localPos = new Vector3(xLocalPos, yLocalPos, zLocalPos);

            controller.CreateCloudAnchor(anchorName, id, localPos, localEulerY, localScale);
        }
        else
        {
            mainMenu.Warning();
        }
    }

    private bool ValidateInput()
    {

        string[] parameters = anchorString.Split(';');

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
        for (int i = 0; i < floatParams.Length; i++)
        {
            if (!float.TryParse(paramaters[i + 2], out floatParams[i]))
            {
                return false;
            }
        }
        return true;
    }
}
