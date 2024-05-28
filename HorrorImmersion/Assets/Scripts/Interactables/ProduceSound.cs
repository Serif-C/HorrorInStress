using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceSound : MonoBehaviour
{
    /*
     * Certain actions produces louder sounds, in the scale of 1-10 where 1 is silent and 10 being the loudest,
     * the following actions have the corresponding sound value:
     * - Hiding: 1
     * - Crouch walking (crawling): 2
     * - Walking: 3.5
     * - Opening/Closing doors: 4
     * - Sprinting: 5
     * - Glass Breakin: 8
     * - Baby Crying: 10
     * 
     * Checklists:
     * 1. Check if there is a wall between the source of the sound and the enemy.
     * 2. If the sound signal (ray cast) was not blocked by a wall and hits the enemy, then send sound value to enemy.
     * 3. Then alert the enemy depending on the sound value and delete this game object.
     * 4. Otherwise, just destroy this game object.
     */

    [SerializeField] private Transform enemy_Transform;
    [SerializeField] private SoundType soundSource;
    [SerializeField] private float soundRange;

    private Dictionary<string, float> soundSourceValues = new Dictionary<string, float>();

    private void Start()
    {
        soundSourceValues.Add("Hiding", 1f);
        soundSourceValues.Add("Crawling", 2f);
        soundSourceValues.Add("Walking", 3.5f);
        soundSourceValues.Add("OpeningDoor", 4f);
        soundSourceValues.Add("ClosingDoor", 4f);
        soundSourceValues.Add("Sprinting", 5f);
        soundSourceValues.Add("GlassBreaking", 8f);
        soundSourceValues.Add("BabyCrying", 10f);
    }

    public enum SoundType
    {
        Hiding,
        Crawling,
        Walking,
        OpeningDoor,
        ClosingDoor,
        Sprinting,
        GlassBreaking,
        BabyCrying
    }

    private void Update()
    {
        ///////////////////////////////////////////////////////
        //SHOULD ADD A CONDITION FOR WHEN SOUNDS ARE PRODUCED//
        ///////////////////////////////////////////////////////
        ///i.e.
        ///When player is walking
        ///When baby is crying
        ///etc..

        RaycastHit hit;
        string enumToString = soundSource.ToString();

        foreach (KeyValuePair<string, float> pair in soundSourceValues)
        {
            if (pair.Key == enumToString)
            {
                soundRange = pair.Value;
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            Debug.Log("Making Noise");
            Vector3 direction = (enemy_Transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, direction, out hit, soundRange))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    //Do number 2 and 3 of checklist
                    EnemyReactToSound enemyHearing = hit.collider.GetComponent<EnemyReactToSound>();
                    enemyHearing.SetSoundHeard(soundRange);
                    enemyHearing.SetLocationSoundWasHeard(transform.position);

                    Debug.Log("Sound Sent to Enemy");
                }
            }
        }
    }

    public void SetSoundSource(SoundType source)
    {
        soundSource = source;
    }
}
