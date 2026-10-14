using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterCore))]
public class AgentController : MonoBehaviour
{
    NavMeshAgent agent;
    public float DistanceToStartFollowing = 6f;
    private GameObject targetToFollow;
    private CharacterCore core;

    private void Awake()
    {
        core = GetComponent<CharacterCore>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (GameManager.Instance.HumanElephantDistance >= DistanceToStartFollowing)
        {
            //agent.isStopped = false;
            //agent.SetDestination(targetToFollow.transform.position);
        }
        else
        {
            //agent.isStopped = true;
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        targetToFollow = newTarget;
    }

    private void OnEnable()
    {
        //agent.enabled = true;
    }

    private void OnDisable()
    {
       // agent.enabled = false;
    }
}