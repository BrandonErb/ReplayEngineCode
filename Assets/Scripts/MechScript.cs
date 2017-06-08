/*
MechScript.cs
Assignment: Unity Capstone
Brandon Erb
Date: Feb 12, 2017
Description: Controls the mechs movement, animation and effects
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MechScript : MonoBehaviour
{
    public Animator anim; //Animator
    public Rigidbody rbMech;
    public Rigidbody target;
    public GameObject goMech, hardpoint1, hardpoint2, bodypoint;
    public GameObject weapon1, weapon2;
    private GameObject cWeapon1, cWeapon2;
    public GameObject fxDamage, fxDestroy;
    private GameObject cfxDamage, cfxDestroy;
    public GameObject[] visualFX;
    private GameObject[] cVisualFX;
    public AudioSource[] soundFX;
    [HideInInspector]
    public Vector3 newPos;
    private Vector3 pTarget;
    [HideInInspector]
    public Quaternion newRot;
    [HideInInspector]
    public bool move, rotate, shoot1, shoot2, walk, destroyM;
    private float distance;

    // Use this for initialization
    void Start()
    {
        move = false;
        rotate = false;
        shoot1 = false;
        shoot2 = false;
        walk = false;
        destroyM = false;
        newPos = rbMech.position;
        newRot = rbMech.rotation;
        //Load Game DB to objects	
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        //Case for unpredictable Y value
        //if (Mathf.Approximately(rbMech.transform.position.x, newPos.x) && Mathf.Approximately(rbMech.transform.position.z, newPos.z))
        //{ 
        //    walk = false;
        //    anim.SetBool("movement", walk);
        //}
        //else
        //{
        //    walk = true;
        //    anim.SetBool("movement", walk);
        //}

        if (move == true)
        {
            walk = true;
            anim.SetBool("movement", walk);
        }
        else
        {
            walk = false;
            anim.SetBool("movement", walk);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (move == true || rotate == true)
        {
            Movement();
        }
        if (shoot1 == true)
        {
            fireWeapon1();
            StartCoroutine("Weapon1FX");
            shoot1 = false;
        }
        if (shoot2 == true)
        {
            fireWeapon2();
            StartCoroutine("Weapon2FX");
            shoot2 = false;
        }

        //AttachedFX();
        DestroyProjectile();
        Destroy();
    }

    //Name: Movement
    //Parameters: N/A
    //Return value: N/A
    //Description: Controls movement of mech and animation of walking
    //note: has alternative code for further change
    void Movement()
    {
        //if (rbMech.transform.position == newPos)
        if (Mathf.Approximately(rbMech.position.x, newPos.x) && Mathf.Approximately(rbMech.position.z, newPos.z))
        {
            move = false;
        }
        //Check to see if it has completed rotating
        if (rbMech.transform.rotation == newRot)
        {
            rotate = false;
        }

        rbMech.transform.position = Vector3.MoveTowards(rbMech.transform.position, newPos, 8 * Time.deltaTime);
        rbMech.transform.rotation = Quaternion.RotateTowards(rbMech.transform.rotation, newRot, 40 * Time.deltaTime);

    }

    //Name: FireWeapon1
    //Parameters: N/A
    //Return value: N/A
    //Description: Controls firing of weapon 1 and its effects
    void fireWeapon1()
    {
        rbMech.transform.LookAt(target.transform);
        distance = Vector3.Distance(rbMech.position, target.position);
        cWeapon1 = (GameObject)Instantiate(weapon1, hardpoint1.transform.position, hardpoint1.transform.rotation);
        //cWeapon1.GetComponent<Rigidbody>().AddForce(rbMech.gameObject.transform.forward * 300);
        cWeapon1.GetComponent<Rigidbody>().velocity = cWeapon1.transform.forward * 50;
    }

    //Name: FireWeapon2
    //Parameters: N/A
    //Return value: N/A
    //Description: Controls firing of weapon 2 and its effects
    void fireWeapon2()
    {
        rbMech.transform.LookAt(target.transform);
        distance = Vector3.Distance(rbMech.position, target.position);
        cWeapon2 = (GameObject)Instantiate(weapon2, hardpoint2.transform.position, hardpoint2.transform.rotation);
        //cWeapon2.GetComponent<Rigidbody>().AddForce(rbMech.gameObject.transform.forward * 300); //Alternative way
        cWeapon2.GetComponent<Rigidbody>().velocity = cWeapon2.transform.forward * 50;
    }


    //Name: Damage
    //Parameters: N/A
    //Return value: N/A
    //Description: Calls Damage coroutine
    void Damage()
    {
        //cVisualFX[2] = (GameObject)Instantiate(cVisualFX[2], hardpoint1.transform.position, bodypoint.transform.rotation); //Alternative way
        StartCoroutine("DamageFX");
    }

    //Name: OnTriggerEnter
    //Parameters: Collider
    //Return value: N/A
    //Description: Finds collisions and calculates damage
    void OnTriggerEnter(Collider other)
    {

    }

    //Name: DestroyProjectile
    //Parameters: N/A
    //Return value: N/A
    //Description: Controls FX for a destroyed mech
    void DestroyProjectile()
    {
        float dist;

        if (cWeapon1 != null)
        {
            dist = Vector3.Distance(cWeapon1.transform.position, rbMech.position);

            if (dist == distance || dist > distance)
            {
                Destroy(cWeapon1);
                StartCoroutine("DamageFX");
            }
        }
        if (cWeapon2 != null)
        {
            dist = Vector3.Distance(cWeapon2.transform.position, rbMech.position);

            if (dist == distance || dist > distance)
            {
                Destroy(cWeapon2);
                StartCoroutine("DamageFX");
            }
        }
    }

    //Name: Destroy
    //Parameters: N/A
    //Return value: N/A
    //Description: Controls FX for a destroyed mech
    void Destroy()
    {
        if(destroyM == true)
        {
            cfxDestroy = (GameObject)Instantiate(fxDestroy, rbMech.transform.position + new Vector3(0, 8, 0), rbMech.rotation);
            destroyM = false;
        }
    }

    //Name: AttachedFX
    //Parameters: N/A
    //Return value: N/A
    //Description: Attaches effects to mech so that every frame the effect stays in the same place.
    void AttachedFX()
    {
        if (cVisualFX[0] != null)
        {
            cVisualFX[0].transform.parent = hardpoint1.transform;
            cVisualFX[0].transform.position = hardpoint1.transform.position;
        }
        if (cVisualFX[1] != null)
        {
            cVisualFX[1].transform.parent = hardpoint2.transform;
            cVisualFX[1].transform.position = hardpoint2.transform.position;
        }
        if (cVisualFX[2] != null)
        {
            cVisualFX[2].transform.parent = bodypoint.transform;
            cVisualFX[2].transform.position = bodypoint.transform.position;
        }
    }

    //Name: Weapon2FX
    //Parameters: N/A
    //Return value: N/A
    //Description: Corountine for effects for mech
    IEnumerator Weapon1FX()
    {
        soundFX[0].Play();
        //cVisualFX[0] = (GameObject)Instantiate(visualFX[0], hardpoint1.transform.position, rbMech.rotation);
        yield return new WaitForSeconds(3.0f);
        //Destroy(cVisualFX[0], 0.0f);
    }

    //Name: Weapon2FX
    //Parameters: N/A
    //Return value: N/A
    //Description: Corountine for effects for mech
    IEnumerator Weapon2FX()
    {
        soundFX[1].Play();
        //cVisualFX[1] = (GameObject)Instantiate(visualFX[1], hardpoint2.transform.position, rbMech.rotation);
        yield return new WaitForSeconds(3.0f);
        //Destroy(cVisualFX[1], 0.0f);
    }

    //Name: Damage
    //Parameters: N/A
    //Return value: N/A
    //Description: Corountine for damage effect on mech
    IEnumerator DamageFX()
    {
        soundFX[2].Play();
        cfxDamage = (GameObject)Instantiate(fxDamage, target.transform.position + new Vector3 (0,8,0), target.rotation);
        yield return new WaitForSeconds(1.0f);
        Destroy(cfxDamage, 1.0f);
    }
}
