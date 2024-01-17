using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurderManager : MonoBehaviour
{
    public const float DISTANCE_MULT = 0.2f;
    public const float MIN_TIME = 5f;

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

        foreach (MurderEvent e in activeEvents)
        {
            //this gouverns how the timer for the murder works
            if (e.timeTillExpire >= 0)
            {
                e.timeTillExpire -= Time.deltaTime;
            }
            else
            {
                // expired.
                expiredEvents.Add(e);       
            }
        }

        foreach (MurderEvent e in expiredEvents)
        {
            Destroy(e.evnt);
            activeEvents.Remove(e);
        }


        nextMurderSpawnTime -= Time.deltaTime;
        if(nextMurderSpawnTime < 0)
        {
            tryAddMurder();
            nextMurderSpawnTime = Random.Range(7f, 10f);
        }

    }
    
    public void stoppedMurder(GameObject g)
    {
        var idx = activeEvents.FindIndex(e => e.evnt == g);
        // give points
        GameManager.scoreSaves += (int)activeEvents[idx].timeTillExpire;
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

        }
    }
}
