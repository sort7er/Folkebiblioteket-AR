using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ChurchSpawn : MonoBehaviour
{
    public ARExperienceManager arExperienceManager;
    public ARAnchorManager anchorManager;

    public GameObject churchPrefab;

    private GameObject church;
    private ARAnchor anchor;

    private void Awake()
    {
        InitializeChurches();
        arExperienceManager.OnPlaneFound += ResetChurches;
        arExperienceManager.OnDelete += Delete;
    }


    private void InitializeChurches()
    {
        anchor = anchorManager.AddAnchor(new Pose(Vector3.zero, Quaternion.identity));
        church = Instantiate(churchPrefab);
        church.SetActive(false);
    }

    private void ResetChurches()
    {
        church.SetActive(true);
        church.transform.parent = anchor.transform;
        church.transform.position = anchor.transform.position;
        church.transform.rotation = Quaternion.identity;
    }




    private void Delete()
    {
        church.SetActive(false);
    }
}
