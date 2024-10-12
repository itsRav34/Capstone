using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System.Linq;

public class HumanBodyTracker : MonoBehaviour
{
    public ARHumanBodyManager humanBodyManager;

    public delegate void BodyAdded(ARHumanBody humanBody);
    public delegate void BodyUpdated(ARHumanBody humanBody);
    public delegate void BodyRemoved(ARHumanBody humanBody);

    public event BodyAdded OnBodyAdded;
    public event BodyUpdated OnBodyUpdated;
    public event BodyRemoved OnBodyRemoved;

    private HashSet<TrackableId> trackedBodyIds = new HashSet<TrackableId>();

    private void Update()
    {
        // Iterate through all tracked human bodies
        foreach (var humanBody in humanBodyManager.trackables)
        {
            if (humanBody.trackingState == TrackingState.Tracking)
            {
                // If the body is not already tracked, invoke OnBodyAdded
                if (!trackedBodyIds.Contains(humanBody.trackableId))
                {
                    trackedBodyIds.Add(humanBody.trackableId);
                    OnBodyAdded?.Invoke(humanBody);
                }
                else
                {
                    // If it is already tracked, invoke OnBodyUpdated
                    OnBodyUpdated?.Invoke(humanBody);
                }
            }
        }

        // Check for removed bodies
        foreach (var id in trackedBodyIds.ToArray())
        {
            if (!humanBodyManager.trackables.TryGetTrackable(id, out var humanBody) ||
                humanBody.trackingState != TrackingState.Tracking)
            {
                trackedBodyIds.Remove(id);
                OnBodyRemoved?.Invoke(humanBody);
            }
        }
    }
}
