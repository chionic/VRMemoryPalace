using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class menuJson
{
    public bool isAllInOneFolder;
    public string BaseFolder;
    public string menuName;
    public List<submenuJson> Menu;
}
