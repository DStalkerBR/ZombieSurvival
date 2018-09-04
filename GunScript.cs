using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public int maxAmmo = 30;
    public int currentAmmo =  -1;
    public float reloadTime = 2f;
    public float spreadAmount = 0.01f;
    public bool isAutomatic = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Recoil RecoilObject;
    public Animator weaponAnimator;
    private float nextTimeToFire = 0f;
    bool isReloading = false;
    private AudioManager audioManager;

	void Start(){
        currentAmmo = maxAmmo;
    }

    void OnEnable(){
        isReloading = false;
        weaponAnimator.SetBool("Reloading", false);
        audioManager =  gameObject.GetComponent<AudioManager>();
    }
	// Update is called once per frame
	void Update () {

        if (isReloading){
            return;
        }

        if (Input.GetKey(KeyCode.R)){
            StartCoroutine(Reload());  
            audioManager.Play("Reload");       
            return;
        }

        if (currentAmmo <= 0)
        {
            //Indicar que o player precisa recarregar com o som e/ou com o texto de "Reload"
            return;
        }        
		
        if (isAutomatic){
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                audioManager.Play("Tiro");  
            }
        }else {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                audioManager.Play("Tiro");
            }
        }
	}

    void Shoot()
    {
        RaycastHit hit;
        muzzleFlash.Play();
        currentAmmo--;
        RecoilObject.recoil += 0.1f;

        if (Physics.Raycast(fpsCam.transform.position, (fpsCam.transform.forward + Random.insideUnitSphere*spreadAmount).normalized, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        weaponAnimator.SetBool("Reloading",true);
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime - .25f);
        weaponAnimator.SetBool("Reloading",false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

}
