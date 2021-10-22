using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Data", menuName = "Resources/Event Data")]
[FilePath("Resources/Event Data", FilePathAttribute.Location.ProjectFolder)]
public class EventData : ScriptableSingleton<EventData>
{
    public string onShoot;
    public string onFailedToShoot;
    public string onShootHit;

    public string onReload;
    public string onReloadStart;
    public string onReloadFinish;

    public string onGivePlayerAmmo;
    public string onRestorePlayerHealth;
    public string onRestorePlayerShield;

    public string onUpdateAmmo;
    public string onUpdatePlayerHealth;
    public string onUpdateInteractionTarget;
    public string onUpdatePlayerShield;

    public string onPlayerDie;
    public string onPlayerTakeDamage;
    
    public string onItemDestroyed;

    public string onChallengeStartedByPlayer;
}