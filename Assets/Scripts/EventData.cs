using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Data", menuName = "Resources/Event Data")]
[FilePath("Resources/Event Data", FilePathAttribute.Location.ProjectFolder)]
public class EventData : ScriptableSingleton<EventData>
{
    public string onShootEventName;
    public string onFailedToShootEventName;
    public string onShootHitEventName;
    public string onAddAmmoEventName;
    public string onReloadEventName;
    public string onReloadStartEventName;
    public string onReloadFinishEventName;
    public string onPlayerDieEventName;
    public string onDisplayAmmoInfoEventName;
    public string onDisplayHealthInfoEventName;
    public string onDisplayInteractionInfo;
    public string onRestorePlayerHealth;
    public string onRestorePlayerShield;
}