using UnityEngine;

public class Coin : PickableObject
{
    public override void Disable()
    {
        Instantiate(ParticleDatabase.Instance.roundHitYellow, transform.localPosition, Quaternion.identity);
        gameObject.SetActive(false);
    }
}