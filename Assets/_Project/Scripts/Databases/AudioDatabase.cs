using UnityEngine;

public class AudioDatabase : MonoBehaviour
{
    public static AudioDatabase Instance;

    public AudioSource buildSource;

    private void Awake()
    {
        Instance = this;
    }
}
