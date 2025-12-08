using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Transform BulletPool;

    private List<Bullet> bullets = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {

        for (int i = 0; i < 50; i++)
        {
            var instance = Instantiate(bulletPrefab, BulletPool);
            var bullet = instance.GetComponent<Bullet>();
            bullets.Add(bullet);
            instance.SetActive(false);


        }



        while (true)
        {

            var available = bullets.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
         
         if (available)
         {
             available.transform.position = firePoint.position;
             available.gameObject.SetActive(true);
             available.direction = firePoint.up;
             
         }

         


            yield return new WaitForSeconds(0.5f);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}