using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderManager : MonoBehaviour
{
    public const float DISTANCE_MULT = 0.25f;
    public const float MIN_TIME = 8f;

    // this is which material get assigned to which.
    // this is disgusting.
    public Material[] pulseMaterials;

    [System.Serializable]
    public class MurderEvent
    {
        public Vector2 location;
        public float timeTillExpire;
        public float timeMax;
        public GameObject evnt;

        public MurderEvent(Vector2 location, float timeTillExpire, GameObject evnt)
        {
            this.location = location;
            this.timeTillExpire = timeTillExpire;
            this.timeMax = timeTillExpire;
            this.evnt = evnt;
            evnt.transform.position = this.location;
        }
    }

    public List<MurderEvent> activeEvents = new List<MurderEvent> ();
    [SerializeField] public List<Vector2> murderSpotList;
    public GameObject murderEventPrefab;
    private float nextMurderSpawnTime;


    void Update()
    {
        List<MurderEvent> expiredEvents = new List<MurderEvent>();

        int i = 0;
        foreach (MurderEvent e in activeEvents)
        {
            // this sucks, but each item needs a seperate material, this forces that.
            e.evnt.transform.Find("PulseCircle").GetComponent<SpriteRenderer>().material= pulseMaterials[i];

            //this gouverns how the timer for the murder works
            if (e.timeTillExpire >= 0)
            {
                if (GameManager.timeDelay <= 0)
                {
                    e.timeTillExpire -= Time.deltaTime;
                }
            }
            else
            {
                // expired.
                expiredEvents.Add(e);
                SoundManager.playSound(soundTriggers.Murder);
            }

            ++i;
        }

        foreach (MurderEvent e in expiredEvents)
        {
            Destroy(e.evnt);
            GameCanvas.addToMessageQueue("MURDER COMMITTED...");
            activeEvents.Remove(e);
        }


        nextMurderSpawnTime -= Time.deltaTime;
        if(nextMurderSpawnTime < 0)
        {
            tryAddMurder();
            nextMurderSpawnTime = Random.Range(8f, 12f);
        }

    }
    
    public void stoppedMurder(GameObject g)
    {
        SoundManager.playSound(soundTriggers.MurderStopped);
        var idx = activeEvents.FindIndex(e => e.evnt == g);
        // give points
        GameManager.scoreSaves += (5 + (int)(1.5 * (int)activeEvents[idx].timeTillExpire));

        GameCanvas.addToMessageQueue("MURDER STOPPED! + " + (5 + (int)(1.5 * (int)activeEvents[idx].timeTillExpire)));
        // play cutscene TODO
        Destroy(activeEvents[idx].evnt);
        activeEvents.RemoveAt(idx);
        FindObjectOfType<GameCanvas>().playCutscene();

    }



    //initiates a murder on a random spot out of the list, then removes the spot from the list
    private void tryAddMurder()
    {
        // max of 3 murders hardcoded by UI..
        if(activeEvents.Count < 3 && murderSpotList.Count > 0)
        {
            // room to add new murder.
            var idx = activeEvents.FindIndex(e => e != null);
            int rng = Random.Range(0, murderSpotList.Count);
            var newPos = murderSpotList[rng];
            murderSpotList.RemoveAt(rng);
            activeEvents.Add(new MurderEvent(newPos, MIN_TIME + (Vector2.Distance(newPos, GameObject.FindWithTag("Player").transform.position) * DISTANCE_MULT), Instantiate(murderEventPrefab)));
            SoundManager.playSound(soundTriggers.MurderNew);
            GameCanvas.addToMessageQueue("NEW MURDER!!!");
        }
    }
}
