using UnityEngine;
using UnityEngine.AI;

// Walk to a random position and repeat
[RequireComponent(typeof(NavMeshAgent))]
public class RandomWalk : MonoBehaviour
{
    public float m_Range = 25.0f;
    NavMeshAgent m_agent;

    bool inited = false;
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        var target = Camera.main.transform;
        GetComponent<GunAimer>().aimTarget = target;
        GetComponent<WeaponStateController>().aimTarget = target;
        GetComponent<HeadLookController>().targetTransform = target;
    }

    void Update()
    {
        if (!inited)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + new Vector3(0, -1, -0.9f), out hit, 10, NavMesh.AllAreas);
            transform.position = hit.position + new Vector3(0, 0.12f, 0);

          //  transform.position = new Vector3(0.5f, -1, 1.6f);
            inited = true;
            return;
        }

        if (m_agent.pathPending || m_agent.remainingDistance > 0.1f)
            return;

        m_agent.destination = m_Range * Random.insideUnitCircle;
    }
}
