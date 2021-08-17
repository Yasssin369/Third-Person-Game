using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class WeaponManager : MonoBehaviour
{
    public Transform raycastraycastOrigin;
    Ray ray;
    RaycastHit hitinfo;
    public Transform raycastDistination;

    /// <summary>
    /// /////Lazer Rifle animations rigging
    /// </summary>
    public Rig aimLayer;
    public Rig handLayer;
    public float aimDuration = 0.3f;
    public bool isFiring;
    public float damage = 25f;
    public ParticleSystem laser;


    public GameObject[] weapons= new GameObject[2];
    public int weaponindex;
    public LayerMask enemyMask;

    /// <summary>
    /// //Sword 
    /// </summary>
    Animator anim;
    AnimatorOverrideController animatorOverride;
    //sword
    public DamageCollider swordCollider;
    public Transform swordPosition;
    public ParticleSystem swordFX;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        animatorOverride = anim.runtimeAnimatorController as AnimatorOverrideController;
        SwitchWeapon(0);

    }

    void Update()
    {
        if (weaponindex == 0)
        {
            anim.SetLayerWeight(1, 0.0f);
        if(Input.GetButtonDown("Fire1"))
        {
         
            Shoot();
        }

        if(Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }
            aimLayer.weight = 1.0f;
            handLayer.weight = 1.0f;

        }

        else
        {

            anim.SetLayerWeight(1, 1f);
            LoadDamageCollider();
            aimLayer.weight = 0;
            handLayer.weight = 0;
            if (Input.GetButtonDown("Fire1"))
            {

                Attack();
            }


        }
        

    }
   public void Shoot()
    {
        isFiring = true;
        laser.Emit(150);

        ray.origin = raycastraycastOrigin.position;
        ray.direction = raycastDistination.position- raycastraycastOrigin.position;

        if ( Physics.Raycast(ray, out hitinfo))
        {
            // Debug.DrawLine(ray.origin, hitinfo.point, Color.red, 1.0f) ;
            // Debug.Log("shoot");
            Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.Hit(damage);
            }
        }
    }
  
    public void Attack()
    {

        anim.SetTrigger("Attack");

    }
    public void SwitchWeapon(int index)
    {
        for(int i=0; i<weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[index].SetActive(true);
        weaponindex = index;
    }
    public void OpenDamageCollider()
    {

        swordCollider.EnableDamageCollider();
        if(swordFX != null)
        { 
        swordFX.Emit(1);
        swordFX.transform.position = swordPosition.position;
        swordFX.transform.rotation = swordPosition.rotation;
        }
    }
   
    public void CloseDamageCollider()
    {
        swordCollider.DisableDamageCollider();
    }
    public void LoadDamageCollider()
    {
         swordCollider = GetComponentInChildren<DamageCollider>();

    }
    /*  void OnDrawGizmos()
      {
          Gizmos.color = Color.red;
          //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
          //if (m_Started)
              //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
              Gizmos.DrawWireCube(swordPosition.position, transform.localScale);
      }*/
}
