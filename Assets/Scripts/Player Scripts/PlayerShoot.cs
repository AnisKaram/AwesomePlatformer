using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject fireBullet;

    void Update()
    {
        ShootBullet();
    }

    void ShootBullet()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject bullet = Instantiate (fireBullet, transform.position, Quaternion.identity);
            // When the player faces to the right, the scale will be (+1) so we will multiply the bullet speed by the localScale of the player to face his position
           // When the player faces to the left, the scale will be (-1) so we will multiply the bullet speed by the locaScale of the player to face his current scale (of position) 
            bullet.GetComponent<FireBullet>().Speed *= transform.localScale.x;
        }
    }
} // class
