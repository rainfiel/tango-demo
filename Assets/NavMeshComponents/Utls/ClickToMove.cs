using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();

    bool inited = false;
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!inited)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position, out hit, 10, NavMesh.AllAreas);
            transform.position = hit.position + new Vector3(0, 0.12f, 0);
            inited = true;
        }

        if (m_Agent && Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                m_Agent.destination = m_HitInfo.point;
            else
                Debug.Log("...........");
        }
    }
}
