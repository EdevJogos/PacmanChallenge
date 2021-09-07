using UnityEngine;

public class PlayerCharacter : Character
{
    public static Transform Transform;

    private Vector2 _currentDirection;
    private Vector2 _requestedDirection;
    private int _obstacleLayerMask;

    public override void Initiate()
    {
        Transform = transform;
        
        _obstacleLayerMask = 1 << LayerMask.NameToLayer("Block") | 1 << LayerMask.NameToLayer("Wall");
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _requestedDirection.Set(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            _requestedDirection.Set(-1, 0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            _requestedDirection.Set(0, 1);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            _requestedDirection.Set(0, -1);
        }
    }

    private void FixedUpdate()
    {
        Vector2 __currentPosition = transform.position;

        bool __updateDirection = Physics2D.Linecast(__currentPosition, __currentPosition + (_requestedDirection * 0.55f), _obstacleLayerMask).transform == null;

        if (__updateDirection)
        {
            _currentDirection = _requestedDirection;
        }

        bool __canMove = Physics2D.Linecast(__currentPosition, __currentPosition + (_currentDirection * 0.55f), _obstacleLayerMask).transform == null;

        if(__canMove)
        {
            Vector2 __moveToPosition = Vector2.MoveTowards(__currentPosition, __currentPosition + _currentDirection, speed * Time.deltaTime);

            transform.localPosition = __moveToPosition;
        }
    }

    public void Invunerable()
    {
        _currentDirection = _requestedDirection = transform.localPosition;
        state = State.INVULNERABLE;
        Invoke("Normal", 3f);
    }

    public void Normal()
    {
        state = State.NORMAL;
    }

    private void OnTriggerEnter2D(Collider2D p_other)
    {
        if(p_other.tag == "Foe" && state == State.BOOSTED)
        {
            Character __agent = p_other.GetComponentInParent<Character>();

            if(__agent.state == State.NORMAL)
            {
                __agent.Deactive();
            }
        }
        else if(p_other.tag == "PickableObject")
        {
            Collect(p_other.GetComponent<PickableObject>());
        }
    }
}