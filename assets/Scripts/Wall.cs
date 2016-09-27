using UnityEngine;
using System.Collections;
using System;

    public class Wall : MonoBehaviour
    {

        public Sprite dmgSprite;
        public int hp = 3;


        public SpriteRenderer spriteRenderer;


        // Use this for initialization
        public void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void DamageWall(int loss)
        {
            spriteRenderer.sprite = dmgSprite;
            hp -= loss;
            if (hp <= 0)
                gameObject.SetActive(false);
        }
    }
