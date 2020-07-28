using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class uniqueIdGenerator {
    private static int uniqueID = 0;

    public static int getID()
    {
        return uniqueID;
    }

    public static void setID()
    {
        uniqueID++;
    }
}
