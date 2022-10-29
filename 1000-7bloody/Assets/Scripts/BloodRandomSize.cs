using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodRandomSize : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] blood;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, blood.Length);
        rend.sprite = blood[rand];
    }

   
}
