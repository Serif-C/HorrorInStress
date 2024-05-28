using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReactToSound : MonoBehaviour
{
    private bool wasSoundHeard = false;
    private Vector3 locationSoundWasHeardFrom;

    /// <summary>
    /// Sound is heard according to the following values:
    /// 1 - No sound is heard
    /// 2,3,4 - Sound is heard but small chance to react (get alerted)
    /// 5,6 - Sound is heard but slightly higher chance to react (get alerted)
    /// 7,8 - Sound is heard but more likely to react (get alerted)
    /// 9,10 - Sound is heard and guaranteed to react (get alerted)
    /// </summary>
    /// <param name="soundValue"></param>
    public void SetSoundHeard(float soundValue)
    {
        int heardChance = Random.Range(1, 10);

        // If soundValue is between 2 and 4 react to sound if heardChance is 7 or higher
        if((soundValue >= 2 && soundValue <= 4) && heardChance >= 7)
        {
            wasSoundHeard = true;
        }
        else if((soundValue >= 5 && soundValue <= 6) && heardChance >= 5)
        {
            wasSoundHeard = true;
        }
        else if((soundValue >= 7 && soundValue <= 8) && heardChance >= 3)
        {
            wasSoundHeard = true;
        }
        else if((soundValue >= 9 && soundValue <= 10) && heardChance >= 1)
        {
            wasSoundHeard = true;
        }
        else
        {
            wasSoundHeard = false; 
        }

        Debug.Log("Was sound heard: " + wasSoundHeard);
    }

    public bool GetSoundHeard()
    {
        return wasSoundHeard;
    }

    public void SetLocationSoundWasHeard(Vector3 soundOrigin)
    {
        locationSoundWasHeardFrom = soundOrigin;
    }

    public Vector3 GetLocationSoundWasHeard()
    {
        return locationSoundWasHeardFrom;
    }
}
