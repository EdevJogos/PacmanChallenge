using UnityEngine;

public class Display : MonoBehaviour
{
    public static System.Action<Displays, int> onActionRequested;

    public Displays ID;

    public virtual void Initiate()
    {

    }

    public virtual void Initialize()
    {

    }

    public virtual void Show(bool p_show)
    {
        if(p_show)
        {
            GetComponent<Animator>().SetTrigger("In");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Out");
        }
    }

    public virtual void UpdateDisplay(int p_operation)
    {

    }

    public virtual void UpdateDisplay(int p_operation, int p_value)
    {

    }

    public virtual void UpdateDisplay(string p_value)
    {

    }

    public virtual void UpdateDisplay(int p_operation, object p_data)
    {

    }

    public virtual void RequestAction(int p_action)
    {
        onActionRequested?.Invoke(ID, p_action);
    }
}
