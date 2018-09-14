using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
    public bool destructible;
    public GameObject destroyedVersion;
    private Animator animator;
	public static int count = 0;
    
    public  void Awake()
    {
        count++;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
			GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;            
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        if (destructible)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Animator>().SetBool("isDead", true);
            yield return new WaitForSeconds(10);
            Destroy(gameObject);    
        }
        count--;
    }
}
