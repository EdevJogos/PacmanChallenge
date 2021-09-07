using UnityEngine;

public class CubeDrop : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector2.down * 10f * Time.deltaTime);

        if(transform.localPosition.y <= -1)
        {
            gameObject.SetActive(false);
        }
    }

    public void Active(Vector2 p_position)
    {
        transform.localPosition = p_position;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        Instantiate(ParticleDatabase.Instance.roundHitBlue, transform.localPosition, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        Instantiate(ParticleDatabase.Instance.roundHitBlue, transform.localPosition, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
