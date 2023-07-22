using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public class AnimatorHandler : MonoBehaviour
    {
        private Animator animator;
        private NavMeshAgent agent;
    
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponentInParent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (agent != null && animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
            }
        }
    }
}
