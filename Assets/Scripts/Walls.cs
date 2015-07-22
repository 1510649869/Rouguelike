using UnityEngine;
using System.Collections;

public class Walls : MonoBehaviour
{

    public Sprite damSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = damSprite;
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
