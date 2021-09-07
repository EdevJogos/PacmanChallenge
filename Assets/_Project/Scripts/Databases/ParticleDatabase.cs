using UnityEngine;

public class ParticleDatabase : MonoBehaviour
{
    public static ParticleDatabase Instance;

    public ParticleSystem roundHitYellow;
    public ParticleSystem roundHitBlue;
    public ParticleSystem swordHitGreenCritical;

    private void Awake()
    {
        Instance = this;
    }
}
