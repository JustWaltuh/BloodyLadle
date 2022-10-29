using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBloodRandomSize1 : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] deathBlood;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, deathBlood.Length);
        rend.sprite = deathBlood[rand];
    }

   
}
