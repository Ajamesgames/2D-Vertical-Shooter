using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3f;
    [SerializeField] //0 = ammo, 1 = attack, 2 = defense, 3 = life powerup, 4 = bomb, 5 = Slow Down, 6 = homing.
    private int _powerupID;
    [SerializeField]
    private AudioClip _powerupClip;

    private bool _hasBeenSuckedIn = false;
    private Vector3 _playerPos;


    void Update()
    {
        if (_hasBeenSuckedIn == true)
        {
            transform.Translate((_playerPos - transform.position).normalized * 10 * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        }

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }

    public void GoToPlayerPosition()
    {
        _hasBeenSuckedIn = true;
        _playerPos = GameObject.Find("Player").transform.position;
        StartCoroutine(ResumeNormalMovement());
    }

    IEnumerator ResumeNormalMovement()
    {
        yield return new WaitForSeconds(1f);
        _hasBeenSuckedIn = false;
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
                    case 6: //homing
                        player.HomingLaserPowerupActive();
                        break;
                }

                AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
                Destroy(this.gameObject);
            }
        }

        if (collision.CompareTag("Enemy Laser"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
