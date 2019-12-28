using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Turrets;

/// <summary>
/// Controls the shooting mechanisim of gazling guns.
/// </summary>
public class MountedGun : MonoBehaviour
{
    [Header("-Shooting-")]

    /// <summary>
    /// The amount of damage gun shots does to an object or players health.
    /// </summary>
    [Tooltip("The amount of damage gun shoots does to an object or players health.")]    
    public float damage = 10f;

    /// <summary>
    /// The maximum distance the gun shots can reach.
    /// </summary>
    [Tooltip("The maximum distance the gun shots can reach.")]    
    public float range = 100f;

    /// <summary>
    /// How fast the gun can fire. Higher values means faster shooting guns.
    /// </summary>
    [Tooltip("How fast the gun can fire. Higher values means faster shooting guns")]    
    public float fireRate = 15f;

    /// <summary>
    /// Time which the gun is allowed to shoot.
    /// </summary>   
    [Tooltip("Time which the gun is allowed to shoot.")]
    private float nextFireTime = 0f;

    /// <summary>
    /// Bullet object to be instantiated with each shot.
    /// </summary>
    [Tooltip("Bullet object to be instantiated with each shot.")]    
    public GameObject bulletPrefab;

    /// <summary>
    /// Force bullets apply to a rigidbody on impact.
    /// </summary>
    [Tooltip("Force bullets apply to a rigidbody on impact.")]
    public float BulletImpactForce = 30f;

    /// <summary>
    /// Flash to instantiate when the gun is shooting.
    /// </summary>
    [Tooltip("Flash to instantiate when the gun is shooting.")]
    public GameObject muzzleFlash;

    /// <summary>
    /// Point in the gun from which Raycast, MuzzleFlashes and Bullets are instantiated from.
    /// </summary>
    [Tooltip("Point in the gun from which Raycast, MuzzleFlashes and Bullets are instantiated from.")]    
    public GameObject barrel;

    /// <summary>
    /// When true the gun will use the DefenceControll script to automatically face enemies. When false  the gun will return to its idle position.
    /// </summary>
    [Tooltip("When true the gun will use the DefenceControll script to automatically face enemies. When false  the gun will return to its idle position.")]
    public bool autoAim;

    /// <summary>
    /// Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.
    /// </summary>
    [Tooltip("Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.")]    
    LaserAim laserAim;

    /// <summary>
    /// Should the gun shoot automatically when enemies are detected or await key press.
    /// </summary>
    [Tooltip("Should the gun shoot automatically when enemies are detected or await key press.")]
    public bool automaticFire;        


    [Header("-Ammo-")]

    /// <summary>
    /// The amount of times the gun can shoot, which is the amount of ammo the gun has.
    /// </summary>
    [Tooltip("The amount of times the gun can shoot, which is the amount of ammo the gun has.")]    
    public int maxAmmo = 10;

    /// <summary>
    /// The amount of ammo left with each shot.
    /// </summary>
    [Tooltip("The amount of ammo left with each shot.")]    
    public int currentAmmo;

    /// <summary>
    /// Number of times the gun can additionally reload. Simply the number of magazines the gun has.
    /// </summary>
    [Tooltip("Number of times the gun can additionally reload. Simply the number of magazines the gun has.")]
    public int magazineCount = 20;

    /// <summary>
    /// How long it takes the gun to reload its ammo.
    /// </summary>
    [Tooltip("How long it takes the gun to reload its ammo.")]    
    public float reloadTime = 3f;

    /// <summary>
    /// Should the gun automatically reload or wait a key press.
    /// </summary>
    [Tooltip("Should the gun automatically reload or wait a key press.")]    
    public bool autoReload = true;    

    /// <summary>
    /// This is set to true when the gun is reloading.Gun can't shoot when isReloading is true.
    /// </summary>
    [Tooltip("This is set to true when the gun is reloading.Gun can't shoot when isReloading is true.")]    
    bool isReloading = false;


    [Header ("Spent Shell")]

    /// <summary>
    /// The bullet spent shell to instatiate while shooting.
    /// </summary>
    [Tooltip("The bullet spent shell to instatiate while shooting.")]
    public GameObject shell;

    /// <summary>
    /// The speed at which bullet spent shells are instatiated while shooting.
    /// </summary>
    [Tooltip("The speed at which bullet spent shells are instatiated while shooting.")]
    public int shellEjectSpeed = 5;

    /// <summary>
    /// The point at which bullet spent shells are instatiated while shooting.
    /// </summary>
    [Tooltip("The point at which bullet spent shells are instatiated while shooting.")]    
    public Transform shellEjectSpot;


    [Header("-Animation-")]

    /// <summary>
    /// The Animator controlling the gun rotation.
    /// </summary>
    [Tooltip("The Animator controlling the gun rotation.")]
    public Animator animator;

    /// <summary>
    /// Controls if gun animation should be applied or not.
    /// </summary>
    [Tooltip("Controls if gun animation should be applied or not.")]
    public bool useAnimation;

    /// <summary>
    /// The name of the animation to play when shooting.
    /// </summary>
    [Tooltip("The name of the animation to play when shooting.")]
    public string fireAnimationName = "Fire";


    [Header("-ImpactEffects-")]

    /// <summary>
    /// How long bullet impacts stay before being destroyed.
    /// </summary>
    [Tooltip("How long bullet impacts stay before being destroyed.")]
    public float impactLife = 20f;
    
    /// <summary>
    /// Script containing bullet impact effects.
    /// </summary>
    [Tooltip("Script containing bullet impact effects.")]
    MyHitEffects myHitEffects;


    [Header("-Audio-")]

    /// <summary>
    /// Audio to play when ejecting a magazine.
    /// </summary>
    [Tooltip("Audio to play when ejecting a magazine.")]
    public AudioClip Eject;

    /// <summary>
    /// Audio to play when a magazine is put inside the gun.
    /// </summary>
    [Tooltip("Audio to play when a magazine is put inside the gun.")]
    public AudioClip Rechamber;

    /// <summary>
    /// Audio to play on each bullet fire.
    /// </summary>
    [Tooltip("Audio to play on each bullet fire.")]
    public AudioClip[] shootingSound;

    /// <summary>
    /// Audio to play on each fire attempt with an empty magazine.
    /// </summary>
    [Tooltip("Audio to play on each fire attempt with an empty magazine.")]
    public AudioClip emptyFireSound;

    /// <summary>
    /// Audio to play when reloading.
    /// </summary>
    [Tooltip("Audio to play when reloading.")]
    public AudioClip reloadingSound;

    /// <summary>
    /// Audio to play when reloading is done.
    /// </summary>
    [Tooltip("Audio to play when reloading is done.")]    
	public AudioClip reloadedSound;

    /// <summary>
    /// Audio source to play audios.
    /// </summary>
    [Tooltip("Audio source to play audios.")] 
    private new AudioSource audio;

    [Header("-AI Controlls-")]

    /// <summary>
    /// Decides if the gun is controlled by the player or AI.
    /// </summary>
    [Tooltip("Decides if the gun is controlled by the player or AI.")] 
    public bool isAIControlled = true;

    /// <summary>
    /// A random time to wait before AI can start shooting.
    /// </summary>
    [Tooltip("A random time to wait before AI can start shooting.")] 
    float wait;

    /// <summary>
    /// Time at which AI can start shooting.
    /// </summary>
    [Tooltip("Time at which AI can start shooting.")] 
    float startTime;

    /// <summary>
    /// Component that controls the aim of the gun towards the Radar target.
    /// </summary>
    [Tooltip("Component that controls the aim of the gun towards the Radar target.")]
    public DefenceControl defence;

    /// <summary>
    /// Target AI shoots at which is feed in from the Radar through DefenceControl.
    /// </summary>
    [Tooltip("Target AI shoots at which is feed in from the Radar through DefenceControl.")]
    [HideInInspector]
    public Transform targetTransform;   


    [HideInInspector]
    /// <summary>
    /// When true, this will cause the gun to rotate to its forward position and remain there.
    /// </summary>
    [Tooltip("When true, this will cause the gun to rotate to its forward position and remain there.")]     
    public bool turretsIdle = false;    

    private void Start() 
    {
        animator = GetComponentInChildren<Animator>();
        myHitEffects = GetComponent<MyHitEffects>();
        laserAim = GetComponentInChildren<LaserAim>();

        currentAmmo = maxAmmo;

        //Time at which AI can start shooting.
        startTime = Time.time;
        // A random time at which AI can start shooting.
        wait = Random.Range(3.0f, 10.0f);

        //Attaches an audio source if its already on the gameobject, if not, then it creates one.
        if (!audio) 
        {
			audio = this.GetComponent<AudioSource> ();
			if (!audio) 
            {
				this.gameObject.AddComponent<AudioSource>();	
			}
		}
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (defence != null)
        {
           targetTransform = defence.allTargetTransform;
        }

        // When AI is in controll, AI can only shoot the gun when radar has been trigered, when the laser hits an Object with health and when wait time and nextFireTime has been reached. 
        if (isAIControlled == true)
        {        
            if (targetTransform != null && Time.time - startTime > wait && Time.time >= nextFireTime)
            {
                if (laserAim.foundObject == true)
                {
                    nextFireTime = Time.time + 1f / fireRate;

                    if (currentAmmo >0)
                    {
                        Shoot();
                        return;                         
                    }
                }               
            }  
        
        }else if (isAIControlled == false)
        {
            if (automaticFire == true)
            {
                if (laserAim.foundObject == true && Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + 1f / fireRate;

                    if (currentAmmo >0)
                    {
                        Shoot();
                        return;                         
                    }                                        
                }
            }   else
                {
                    if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
                    {
                        nextFireTime = Time.time + 1f / fireRate;

                        if (currentAmmo >0)
                        {
                            Shoot();
                            return;                         
                        }else
                        {
                            //Play empty fire sound
                            if (emptyFireSound) 
                            {
				                if (audio) 
                                {
					                audio.PlayOneShot (emptyFireSound);
				                }
			                }        
                        }                        
                    }             
                }

            AutoTargeting();
        }     

        //When ammo is empty play empty fire sound and there are still magazines left start reloading.
        if (currentAmmo <= 0 & magazineCount > 0)
        {
            if (autoReload == true)
            {
                //Start Relaod Coroutine.
                StartCoroutine(Reload());
                return; 
            }else
            {
               if (Input.GetKeyDown("q"))
               {
                    //Start Relaod Coroutine.
                    StartCoroutine(Reload());
                    return;  
               } 
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
       // Debug.Log ("Reloading.....");

        //Play reloading sound
        if (reloadingSound) 
            {
				if (audio) 
                {
					audio.PlayOneShot (reloadingSound);
				}
			} 

        yield return new WaitForSeconds (reloadTime - 0.25f);

        //Play realoded sound just before shooting resumes
        if (reloadedSound) 
            {
				if (audio) 
                {
					audio.PlayOneShot (reloadedSound);
				}
			}  
        // put reload animation here

        yield return new WaitForSeconds (0.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
        magazineCount--;
    }

    public void Shoot()
    {
        currentAmmo --;

        //Instantiate muzzleFlash at barrel position.
        if (barrel)
        {
            var muzzleFlashInstance = Instantiate(muzzleFlash, barrel.transform.position, barrel.transform.rotation) as GameObject;
            Destroy(muzzleFlashInstance, 4);                
        }

        // Instantiate spent bullet shells at shellEjectPosition.
        if (shell && shellEjectSpot)
        {
            Vector3 shellEjectPosition = shellEjectSpot.position;
			Quaternion shellEjectRotation = shellEjectSpot.rotation;
			GameObject shellInstance = Instantiate(shell, shellEjectPosition, shellEjectRotation);

            if (shellInstance.GetComponent<Rigidbody>())
			{
				Rigidbody rigidB = shellInstance.GetComponent<Rigidbody>();
				rigidB.AddForce(shellEjectSpot.forward * shellEjectSpeed, ForceMode.Impulse);
			} 

            Destroy(shellInstance, 7);                
        }

        //Instantiate bullet object at barrel position.
        if (bulletPrefab && barrel)
        {
            Instantiate (bulletPrefab, barrel.transform.position, barrel.transform.rotation);
        }

        //Play shooting sound.
		if (shootingSound.Length > 0) 
        {
			if (audio) 
            {
				audio.PlayOneShot (shootingSound [Random.Range (0, shootingSound.Length)]);
			}
		}        

        //Raycast is instantiated at barrel position.
        RaycastHit hit;
        if (Physics.Raycast(barrel.transform.position, barrel.transform.forward, out hit, range))
        {
          //  Debug.Log(hit.transform.name);

            //Cause damage to object health.
            ObjectHealth objecthealth = hit.transform.GetComponent<ObjectHealth>();
            if (objecthealth !=null && objecthealth.isAlive == true)
            {
                objecthealth.TakeDamage(damage);
            }

            //Playes impact sounds, via MyHitSounds script on object hit by the raycast.
           MaterialType materialType = hit.transform.GetComponent<MaterialType>();
           MyHitSounds myHitSounds = GetComponentInChildren<MyHitSounds>();
           if (materialType != null && myHitSounds != null)
           {
               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Brick)
               { 
    	            var clips = myHitSounds.Brick [UnityEngine.Random.Range (0, myHitSounds.Brick.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Concrete)
               { 
    	            var clips = myHitSounds.Concrete [UnityEngine.Random.Range (0, myHitSounds.Concrete.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Dirt)
               { 
    	            var clips = myHitSounds.Dirt [UnityEngine.Random.Range (0, myHitSounds.Dirt.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Folliage)
               { 
    	            var clips = myHitSounds.Folliage [UnityEngine.Random.Range (0, myHitSounds.Folliage.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

            /*   if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Glass)
               { 
    	            var clips = myHitSounds.Glass [UnityEngine.Random.Range (0, myHitSounds.Glass.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               } */

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Metall)
               { 
    	            var clips = myHitSounds.Metall [UnityEngine.Random.Range (0, myHitSounds.Metall.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Plaster)
               { 
    	            var clips = myHitSounds.Plaster [UnityEngine.Random.Range (0, myHitSounds.Plaster.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Rock)
               { 
    	            var clips = myHitSounds.Rock [UnityEngine.Random.Range (0, myHitSounds.Rock.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }

            /*   if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Water)
               { 
    	            var clips = myHitSounds.Water [UnityEngine.Random.Range (0, myHitSounds.Water.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }*/

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Wood)
               { 
    	            var clips = myHitSounds.Wood [UnityEngine.Random.Range (0, myHitSounds.Wood.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }   

               if (materialType.TypeOfMaterial == MaterialType.MaterialTypeEnum.Flesh)
               { 
    	            var clips = myHitSounds.Flesh [UnityEngine.Random.Range (0, myHitSounds.Flesh.Length)];
                    AudioSource.PlayClipAtPoint (clips, hit.point);  
               }                                                                                                                                                    
           }

            //Applies force to rigidbody on bullet impact.
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * BulletImpactForce, ForceMode.Impulse);
            }

            //Instantiate hit effects on object hit via MyHitEffect script
            var effect = myHitEffects.GetImpactEffect(hit.transform.gameObject);
            if (effect==null)
            return;
            var effectIstance = Instantiate(effect, hit.point, new Quaternion()) as GameObject;
            effectIstance.transform.LookAt(hit.point + hit.normal);
            Destroy(effectIstance, impactLife);    
        }
        //Plays an animation only when useAnimation is true.
        if (useAnimation)
            animator.Play(fireAnimationName);      
    }

    public void AutoTargeting()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            defence.turretsIdle = !defence.turretsIdle;
        }

        if (defence.turretsIdle == true)
        {
            autoAim = false;
        }else
        {
            autoAim = true;
        }
    }
}
