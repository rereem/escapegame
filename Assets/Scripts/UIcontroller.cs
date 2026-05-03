using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public Image DamageEffect;
    public float DamageALPA = -0.3f;
    public float DamageFadeSpeed ; 

    public static UIcontroller Instance;

  
    void Start()
    {
        Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (DamageEffect.color.a != 0)
        {
            DamageEffect.color = new Color(DamageEffect.color.r, DamageEffect.color.g, DamageEffect.color.b, Mathf.MoveTowards(DamageEffect.color.a,0f,DamageFadeSpeed * Time.deltaTime));
        }
    }

    public void ShowDamage()
    {
        DamageEffect.color = new Color(DamageEffect.color.r, DamageEffect.color.g, DamageEffect.color.b, DamageALPA);
    }
}
