using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    /*
     * Certain spots are interactable and allows the player to hide.
     * - Hiding essentially removes the player's hit box which the enemies use to identify the player.
     * - Hiding works only when the enemy has no vision of the player when they enter a hiding spot.
     * - Hiding should not be a full proof solution to avoid enemies, which means, 
     *   enemies could check hiding places over time. (decide a condition later).
     */
}
