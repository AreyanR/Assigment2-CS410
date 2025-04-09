using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    // This will hold the reference to the goal object in the scene
    private Transform goal;

    void Start()
    {
        // When the prefab spawns, try to find the GameEnding object in the scene
        FindGoalInScene();
    }

    void Update()
    {
        // Just in case the goal wasn't found yet (like right after respawn), try again
        if (goal == null)
        {
            FindGoalInScene();
        }

        // If the goal exists, point the arrow at it
        if (goal != null)
        {
            // Get the direction from the player to the goal
            Vector3 toGoal = (goal.position - transform.parent.position).normalized;

            // Get the forward direction the player is facing
            Vector3 playerForward = transform.parent.forward;

            // DOT PRODUCT: Calculate the dot product between the player's forward direction and the direction to the goal
            float dot = Vector3.Dot(playerForward, toGoal);

            // Convert the goal direction into local space so the arrow (child object) can rotate properly
            transform.localRotation = Quaternion.LookRotation(transform.parent.InverseTransformDirection(toGoal));
        }
    }

    // Looks for the GameEnding object in the scene by name and assigns it to the goal variable
    void FindGoalInScene()
    {
        GameObject endObj = GameObject.Find("GameEnding");
        if (endObj != null)
        {
            goal = endObj.transform;
        }
        else
        {
            Debug.LogWarning("GameEnding object not found in scene.");
        }
    }
}
