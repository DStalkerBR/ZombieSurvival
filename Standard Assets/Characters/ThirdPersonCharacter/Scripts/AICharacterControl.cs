using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;    
		public Transform zombie;  
		public Animator animator; // target to aim for
		bool colliderPlayer = false;


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
			animator = GetComponent<Animator>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
			if (!animator.GetBool ("isDead")) {
				if (target != null)
					agent.SetDestination (target.position);

				if (Vector3.Distance (target.position, gameObject.transform.position) > 2f) {
					agent.isStopped = false;
					animator.SetBool ("Attack", false);                
					character.Move (agent.desiredVelocity, false, false);
				} else {
					FaceTarget (target.position);
					agent.isStopped = true;
					animator.SetBool ("Attack", true);
				}
			}
        }

		IEnumerator OnTriggerEnter (Collider collider){
			if (collider.gameObject.tag == "Player" && !animator.GetBool ("isDead")) {
				colliderPlayer = true;
			}
			yield return new WaitForSeconds (5);
			if (colliderPlayer) {
				Debug.Log ("Game Over");
			}
		}

		void OnCollisionExit (){
			colliderPlayer = false;
		}


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

		private void FaceTarget(Vector3 destination)
		{
			Vector3 lookPos = destination - transform.position;
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);  
		}
    }
}
