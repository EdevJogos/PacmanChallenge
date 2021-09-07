using UnityEngine;

public class Character : MonoBehaviour
{
    public static event System.Action onCoinCollected;

    public enum State
    {
        NORMAL,
        BOOSTED,
        INVULNERABLE,
    }

    public State state;
    public float speed;

    public virtual void Initiate()
    {

    }

    public void Initialize()
    {
        
    }

    public virtual void Active()
    {
        state = State.NORMAL;
        gameObject.SetActive(true);
    }

    public virtual void Deactive()
    {
        CancelInvoke();
        state = State.NORMAL;
        gameObject.SetActive(false);
    }

    protected void Collect(PickableObject p_object)
    {
        switch (p_object.ID)
        {
            case PickableObjects.COIN:
                p_object.Disable();
                onCoinCollected?.Invoke();
                break;
        }
    }
}
