using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour
{
    NavMeshAgent agent;
    public float DistanceToStartFollowing = 6f;
    private GameObject targetToFollow;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GameManager.Instance.HumanElephantDistance >= DistanceToStartFollowing)
        {
            agent.isStopped = false;
            agent.SetDestination(targetToFollow.transform.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        targetToFollow = newTarget;
    }

    private void OnEnable()
    {
        agent.enabled = true;
    }

    private void OnDisable()
    {
        agent.enabled = false;
    }
}