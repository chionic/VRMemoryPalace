using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class submenuJson
{
    public string name;
    public int id;
    public bool isSubmenu;
    public string topLevelMenuRep;
    public List<objectJson> Values;
}
