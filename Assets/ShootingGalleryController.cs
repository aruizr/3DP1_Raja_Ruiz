using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShootingGalleryController : MonoBehaviour
{

    [SerializeField]
    List<GameObject> targets;

    [SerializeField]
    TextMeshProUGUI scoreUI;

    [SerializeField]
    GameObject scoreGUI;

    private float score;
    private bool running;

    void OnEnable(){
        
        EventManager.StartListening("monitor_destroyed", MonitorDestroyer);
        EventManager.StartListening("subscribe_monitor", MonitorSubscribed);
        EventManager.StartListening(EventData.Instance.onChallengeStartedByPlayer, StartShootingChallangeListener);
    }

    void OnDisable(){
        EventManager.StopListening("monitor_destroyed", MonitorDestroyer);
        EventManager.StopListening("subscribe_monitor", MonitorSubscribed);
        EventManager.StopListening(EventData.Instance.onChallengeStartedByPlayer, StartShootingChallangeListener);
    }

    void Start(){
        ResetAllTargets();
    }

    void MonitorDestroyer(Dictionary<string,object> args){
        Debug.Log(args["monitor"]);
        score += 10;
        
        scoreUI.SetText("Score: " + score);
    }

    void MonitorSubscribed(Dictionary<string,object> args){

        if(args["monitor"] != null){
            Debug.Log(args["monitor"] + "SUBSCRIBED");
            targets.Add((GameObject)args["monitor"]);
        }
    }

    void StartShootingChallangeListener(Dictionary<string,object> args){
        StartCoroutine("StartShootingChallange");
    }

    IEnumerator StartShootingChallange() 
    {

        if (!running){
        running = true;
        score = 0f;

        yield return new WaitForSeconds(3f);
        ShuffleTargetOrder();
        ResetAllTargets();
        Debug.Log("Starting shooting gallery...");
        foreach (GameObject target in targets){
            target.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            if(!target.GetComponent<ExplodableMonitor>().broken){
                target.SetActive(false);
            }
            
        }

        yield return new WaitForSeconds(3f);
        if(score>=100){
            EventManager.TriggerEvent("challenge_exit", new Dictionary<string, object>(){});
        }
        ResetAllTargets();

        running = false;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("PLAYER CAME IN");
            scoreGUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("PLAYER LEAVING");
            scoreGUI.SetActive(false);
        }
    }

    void ResetAllTargets(){
        foreach (GameObject target in targets)
        {
            target.GetComponent<ExplodableMonitor>().resetMonitor();
            target.SetActive(false);
        }
    }

    public void ShuffleTargetOrder() {
         for (int i = 0; i < targets.Count; i++) {
             int rnd = Random.Range(0, targets.Count);
             GameObject tempTarget;
             tempTarget = targets[rnd];
             targets[rnd] = targets[i];
             targets[i] = tempTarget;
         }
     }
}
