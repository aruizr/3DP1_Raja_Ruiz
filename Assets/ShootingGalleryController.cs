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

    private float score;

    void OnEnable(){
        
        EventManager.StartListening("monitor_destroyed", MonitorDestroyer);
        EventManager.StartListening("subscribe_monitor", MonitorSubscribed);
    }

    void OnDisable(){
        EventManager.StopListening("monitor_destroyed", MonitorDestroyer);
        EventManager.StopListening("subscribe_monitor", MonitorSubscribed);
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

    // Start is called before the first frame update
    void Start()
    {
        score = 0f;
        StartCoroutine("StartShootingChallange");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartShootingChallange() 
    {

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
        ResetAllTargets();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("PLAYER CAME IN");
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("PLAYER LEAVING");
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
