using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 2f;
    [SerializeField] //0 = ammo, 1 = attack, 2 = defense, 3 = life powerup, 4 = bomb, 5 = Slow Down,
    private int _powerupID;
    [SerializeField]
    private AudioClip _powerupClip;

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
                    case 3: //life
                        player.LifePowerupPickup();
                        break;
                    case 4: //bomb
                        player.BombPowerupActive();
                        break;
                    case 5: //slow down
                        player.SlowDownPowerupActive();
                        break;
                }

                AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
}
