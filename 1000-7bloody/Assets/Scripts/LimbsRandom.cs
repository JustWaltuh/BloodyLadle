using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbsRandom : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] limbs;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, limbs.Length);
        rend.sprite = limbs[rand];
    }
}
