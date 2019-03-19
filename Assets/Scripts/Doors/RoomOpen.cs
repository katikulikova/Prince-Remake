using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOpen : MonoBehaviour
{
    public GameObject targetQuad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Destroy(gameObject);
            StartCoroutine(FadeTo(0f, 0.5f));
        }
    }

    private IEnumerator FadeTo(float aValue, float aTime)
    {
        MeshRenderer r = targetQuad.GetComponent<MeshRenderer>();
        float alpha = r.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            r.material.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }

}
