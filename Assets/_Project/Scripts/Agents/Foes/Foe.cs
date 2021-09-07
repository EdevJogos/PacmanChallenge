using System.Collections;
using UnityEngine;

public class Foe : Character
{
    public float startDelay;

    public static event System.Action<Character> onPlayerCaptured;

    private void Awake()
    {
        StageManager.onLevelChanged += StageManager_onLevelChanged;
    }

    private void StageManager_onLevelChanged(int p_level)
    {
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3 + ((p_level - 1) * 0.5f);
    }

    public void OnEnable()
    {
        GetComponent<Pathfinding.IAstarAI>().canMove = false;

        StartCoroutine(RoutineStart());
    }

    private void OnDestroy()
    {
        StageManager.onLevelChanged -= StageManager_onLevelChanged;
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if (p_other.tag == "Player")
        {
            Character __agent = p_other.GetComponentInParent<Character>();

            if (__agent != null && __agent.state == State.NORMAL)
            {
                onPlayerCaptured?.Invoke(__agent);
            }
        }
    }

    private IEnumerator RoutineStart()
    {
        yield return new WaitForSeconds(startDelay);

        GetComponent<Pathfinding.IAstarAI>().canMove = true;
    }
}
