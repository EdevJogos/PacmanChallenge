using UnityEngine;

public class EScale : MonoBehaviour
{
    public bool useOriginal;
    public Vector2 from;
    public Vector2 to;

    public Vector3 speed;

    private bool up;

    private void Start()
    {
        if(useOriginal)
        {
            from = transform.localScale;
        }
    }

    private void Update()
    {
        if(up)
        {
            transform.localScale += speed * Time.deltaTime;
        }
        else
        {
            transform.localScale -= speed * Time.deltaTime;
        }
        
        if((up && transform.localScale.x >= to.x) || (!up && transform.localScale.x <= from.x))
        {
            up = !up;
        }
    }

    public void Disable()
    {
        enabled = false;
        transform.localScale = from;
    }
}
