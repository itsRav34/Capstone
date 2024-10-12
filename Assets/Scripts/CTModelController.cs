using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CTModelController : MonoBehaviour
{
    public GameObject ctModelPrefab;  // Your OBJ model prefab
    private GameObject ctModelInstance;

    public HumanBodyTracker bodyTracker;

    private void Start()
    {
        // Subscribe to body tracking events
        bodyTracker.OnBodyAdded += HandleBodyAdded;
        bodyTracker.OnBodyUpdated += HandleBodyUpdated;
        bodyTracker.OnBodyRemoved += HandleBodyRemoved;
    }

    private void HandleBodyAdded(ARHumanBody humanBody)
    {
        if (ctModelInstance == null)
        {
            // Instantiate the model when a body is detected
            ctModelInstance = Instantiate(ctModelPrefab);
        }

        UpdateCTModelPosition(humanBody);
    }

    private void HandleBodyUpdated(ARHumanBody humanBody)
    {
        UpdateCTModelPosition(humanBody);
    }

    private void HandleBodyRemoved(ARHumanBody humanBody)
    {
        if (ctModelInstance != null)
        {
            Destroy(ctModelInstance);
        }
    }

    private void UpdateCTModelPosition(ARHumanBody humanBody)
    {
        // Use a joint's index directly
        if (humanBody.joints.Length > 0)
        {
            // Example: Accessing the spine joint (modify index based on your specific use case)
            var spineJoint = humanBody.joints[13]; // This is an example; find the correct index for the spine
            ctModelInstance.transform.SetPositionAndRotation(spineJoint.anchorPose.position, spineJoint.anchorPose.rotation);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events when destroyed
        bodyTracker.OnBodyAdded -= HandleBodyAdded;
        bodyTracker.OnBodyUpdated -= HandleBodyUpdated;
        bodyTracker.OnBodyRemoved -= HandleBodyRemoved;
    }
}
