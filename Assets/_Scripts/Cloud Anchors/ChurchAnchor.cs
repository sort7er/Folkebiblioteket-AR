using UnityEngine;

public class ChurchAnchor
{
    public string name;
    public string id;

    public float localPosX;
    public float localPosY;
    public float localPosZ;

    public float localEulerY;
    public float localScale;

    public ChurchAnchor(string name, string id)
    {
        this.name = name;
        this.id = id;

        localPosX= 0;
        localPosY= 0;
        localPosZ= 1;
        localEulerY= 0;
        localScale= 1;
    }

    public void SetLocalPosition(Vector3 localPos)
    {
        localPosX= localPos.x;
        localPosY= localPos.y;
        localPosZ= localPos.z;
    }

    public void SetLocalEulerY(float y)
    {
        localEulerY = y;
    }
    public void SetLocalScale(float scale)
    {
        localScale = scale;
    }

    public string GetChurchAnchorString()
    {
        return name + ";" + id + ";" + localPosX.ToString("f") + ";" + localPosY.ToString("f") + ";" + localPosZ.ToString("f") + ";" + localEulerY.ToString("f") + ";" + localScale.ToString("f");
    }
}
