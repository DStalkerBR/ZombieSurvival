using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
    public bool destructible;
    public GameObject destroyedVersion;
    private Animator animator;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            GetComponent<Animator>().SetBool("Attack", false);
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;            
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
    }
}
