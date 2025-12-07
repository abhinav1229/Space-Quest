using UnityEngine;
using System;

public class MenuParallex : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    float backgroundImageWidth;

    private void Start() {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgroundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    private void Update()
    {
        float moveX = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(moveX, 0);

        if (Math.Abs(transform.position.x) - backgroundImageWidth > 0)
        {
            transform.position = new Vector3(0, transform.position.y);
        }
    }
}
