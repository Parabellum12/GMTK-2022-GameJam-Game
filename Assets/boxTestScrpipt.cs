using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxTestScrpipt : MonoBehaviour
{

    [SerializeField] StatsHandler statsHandler;
    [SerializeField] SpriteRenderer thisSprite;

    private void Start()
    {
        statsHandler.deathCall = () =>
        {
            Destroy(gameObject);
        };
        statsHandler.DamageCall = (damage) =>
        {
            StopCoroutine(takeDamage());
            StartCoroutine(takeDamage());
        };
    }

    IEnumerator takeDamage()
    {
        thisSprite.color = Color.red;
        float time = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - time < .15f)
        {
            yield return null;
        }
        thisSprite.color = Color.white;
    }
}
