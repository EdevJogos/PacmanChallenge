using UnityEngine;

public class BuildObject : MonoBehaviour
{
    public Vector2 myPosition;
    public SpriteRenderer spriteRenderer;

    public void Initiate(Vector2 p_position)
    {
        myPosition = p_position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void Deactive()
    {
        gameObject.SetActive(false);
    }
}
