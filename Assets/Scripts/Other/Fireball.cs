using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 3.0f;
    public int damage = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
        if (playerCharacter != null)
        {
            playerCharacter.Hurt(damage);
        }
        Destroy(gameObject);
    }

}
