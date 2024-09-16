using Google.XR.ARCoreExtensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Hosting : MonoBehaviour
{
    [SerializeField] private GameObject instructionBar;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private SessionController controller;

    private const float startPrepareTime = 3.0f;

    private ChurchAnchor churchAnchor;
    private HostCloudAnchorPromise hostPromise;
    private HostCloudAnchorResult hostResult;
    private IEnumerator hostCoroutine;
    private MapQualityIndicator mapQualityIndicator = null;
    private ARAnchor anchor;
    private float timeSinceStart;

    private void OnEnable()
    {
        timeSinceStart = 0.0f;
        anchor = null;
        mapQualityIndicator = null;
        hostPromise = null;
        hostResult = null;
        hostCoroutine = null;

        controller.SetIsReturning(false);
        controller.UpdatePlaneVisibility(true);

    }

    private void OnDisable()
    {
        CheckDoAndNull(ref mapQualityIndicator, () => Destroy(mapQualityIndicator.gameObject));
        CheckDoAndNull(ref anchor, () => Destroy(anchor.gameObject));
        CheckDoAndNull(ref hostCoroutine, () => StopCoroutine(hostCoroutine));
        CheckDoAndNull(ref hostResult, () => hostPromise.Cancel());
        CheckDoAndNull(ref hostResult);

        controller.UpdatePlaneVisibility(false);
    }

    private void Update()
    {
        if (timeSinceStart < startPrepareTime)
        {
            instructionText.text = "Initializing";
            timeSinceStart += Time.deltaTime;
            if (timeSinceStart >= startPrepareTime)
            {
                instructionText.text = "Tap to place an object.";
            }

            return;
        }

        controller.ErrorCheckAndDisableSleep();

        if (controller.isReturning)
        {
            return;
        }

    }

    private void CheckDoAndNull<T>(ref T type, Action thingToDo = null) where T : class
    {
        if(type!= null)
        {
            thingToDo?.Invoke();
            type = null;
        }
    }


}
