using UnityEngine;

public class PlayerCollisionTrigger : MonoBehaviour
{
    public string grappleScriptName = "PlayerGrapple";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var grappleScripts = other.GetComponents<Component>();
            foreach (var script in grappleScripts)
            {
                if (script.GetType().Name == grappleScriptName)
                {
                    var grappleScript = (MonoBehaviour)script;
                    var canGrappleField = grappleScript.GetType().GetField("canGrapple");
                    if (canGrappleField != null)
                    {
                        canGrappleField.SetValue(grappleScript, false);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var grappleScripts = other.GetComponents<Component>();
            foreach (var script in grappleScripts)
            {
                if (script.GetType().Name == grappleScriptName)
                {
                    var grappleScript = (MonoBehaviour)script;
                    var canGrappleField = grappleScript.GetType().GetField("canGrapple");
                    if (canGrappleField != null)
                    {
                        canGrappleField.SetValue(grappleScript, true);
                    }
                }
            }
        }
    }
}
