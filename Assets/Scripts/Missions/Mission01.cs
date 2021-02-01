using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission01 : MonoBehaviour {
    //DESCRIPTION
    //Defeat 3 Sargents
    private int sargentsDefeated = 3;
    private bool completed = false;

    public bool SargentDefeated() {
        --sargentsDefeated;

        if (sargentsDefeated <= 0)
            completed = true;

        return completed;
    }



}
