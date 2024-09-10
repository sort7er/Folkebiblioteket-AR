using System.Collections.Generic;
using System;
using UnityEngine;

public class StoredCloudAnchor : MonoBehaviour
{
    public string Name;
    public string Id;
    public string SerializedTime;
    public StoredCloudAnchor(string name, string id, DateTime time)
    {
        Name = name;
        Id = id;
        SerializedTime = time.ToString();
    }
    public StoredCloudAnchor(string name, string id) : this(name, id, DateTime.Now)
    {

    }
    public DateTime CreatedTime
    {
        get
        {
            return Convert.ToDateTime(SerializedTime);
        }
    }
    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }

}
