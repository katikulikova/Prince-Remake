using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReactToHit()
    {
        WanderingAI wanderingAI = GetComponent<WanderingAI>();

        if (wanderingAI.isAlive)
        {
            StartCoroutine(Die());
        } else
        {
            float angle = Random.Range(-15f, 15f);
            transform.Rotate(angle, 0, 0);
        }

        if (wanderingAI != null)
        {
            wanderingAI.SetAlive(false);
        }

    }

    private IEnumerator Die()
    {
        transform.Rotate(-75f, 0, 0);

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
