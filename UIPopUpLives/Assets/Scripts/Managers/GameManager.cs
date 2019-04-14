using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Accessible<GameManager>
{

    private string currentFormatLiveRessurection = "FULL";
    public const int MAX_LIVES = 5;

    public string formatTimeLiveRessurection
    {
        get
        {
            return currentFormatLiveRessurection;
        }
    }

    /*public bool isUnlimitedLives
    {
        get
        {
            return false;//currentlySelectedProfile.currentTimeUnlimitedLives > 0;
        }
    }*/


}
