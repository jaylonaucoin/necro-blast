using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class EnemyMovement : MonoBehaviour {

    public float speed = 1f;
    public float minDistance = 1.5f;
    public Transform target;
	public int damage = 1;
    
    private NavMeshAgent agent;
	private Animator animator;
	private CharacterController characterController;

    // Start is called before the first frame update
    void Start () 
    {
        agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();

        //sets the speed of the NavMeshAgent
        agent.speed = speed;

		//sets the stopping distance of the NavMesh
		agent.stoppingDistance = minDistance;

        //if theres no target set, assume it is the player
        if (target == null) {

            if (GameObject.FindWithTag("Player") != null) {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
    }
    
    // Update is called once per frame
    void Update () 
    {
        if (target == null) {
            return;
        }

        //grabs the distance between the enemy and the target
        float distance = Vector3.Distance(transform.position, target.position);

        //moves towards the target as long as its greater than minimum distance
        if (distance > minDistance) {
			//uses NavMeshAgent to move the player
            agent.SetDestination(target.position);
        }
		
		//grabs the current movementSpeed of the enemy
		float movementSpeed = agent.velocity.magnitude;

		//updates the animator with the current movement speed
		animator.SetFloat("Speed", movementSpeed);
    }

	//check if the enemy enters the target's collider
	private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player")) {
            if (gameObject.CompareTag("Golem")) {
                animator.Play("Giant@UnarmedAttack01");
            } else {
                animator.Play("Z_Attack");
            }
            //calls method to take damage
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(damage);
            }
        }*/
    }

    // Set the target of the enemy
    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }
}
