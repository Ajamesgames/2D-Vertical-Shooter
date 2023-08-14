using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 2f;

    [SerializeField] //0 = speed, 1 = attack, 2 = defense powerup
    private int _powerupID;

    [SerializeField]
    private AudioClip _powerupClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0: //ammo
                        player.AmmoPowerupActivate();
                        break;
                    case 1: //attack
                        player.LaserPowerupActive();
                        break;
                    case 2: //defense
                        player.ShieldActivate();
                        break;
                }

                AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
}
