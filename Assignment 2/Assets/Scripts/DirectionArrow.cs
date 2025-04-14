using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    private Transform goal;
    private Renderer[] arrowRenderers; 
    public float maxDistance = 40f;

    void Start()
    {
        FindGoalInScene();

        
        arrowRenderers = GetComponentsInChildren<Renderer>();

        
        for (int i = 0; i < arrowRenderers.Length; i++)
        {
            arrowRenderers[i].material = new Material(arrowRenderers[i].material);
        }
    }

    void Update()
    {
        if (goal == null)
        {
            FindGoalInScene();
        }

        if (goal != null)
        {
            // Rotate the arrow toward the goal (Dot Product) - Areyan
            Vector3 toGoal = (goal.position - transform.parent.position).normalized;
            Vector3 playerForward = transform.parent.forward;
            float dot = Vector3.Dot(playerForward, toGoal);
            Vector3 localDirection = transform.parent.InverseTransformDirection(toGoal);
            transform.localRotation = Quaternion.LookRotation(localDirection);

            // Calculate distance-based color Lerp (linear interpolation) - Areyan
            float distance = Vector3.Distance(transform.parent.position, goal.position);
            float t = Mathf.Clamp01(1f - (distance / maxDistance));
            Color lerpedColor = Color.Lerp(Color.red, Color.green, t);

            foreach (Renderer rend in arrowRenderers)
            {
                rend.material.color = lerpedColor;
            }
        }
    }

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
