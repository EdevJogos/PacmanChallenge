using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    public Transform playerStartPos, foeStartPos;
    public PlayerCharacter playerCharacter;
    public Foe[] foes;

    public void Initate()
    {
        playerCharacter.Initiate();

        for (int __i = 0; __i < foes.Length; __i++)
        {
            foes[__i].Initiate();
        }
    }

    public void InitializeCharacters()
    {
        playerCharacter.Initialize();

        for (int __i = 0; __i < foes.Length; __i++)
        {
            
            foes[__i].Initialize();
        }
    }

    public void ActiveCharacters()
    {
        playerCharacter.transform.localPosition = playerStartPos.localPosition;
        playerCharacter.Active();

        for (int __i = 0; __i < foes.Length; __i++)
        {
            foes[__i].transform.localPosition = foeStartPos.localPosition;
            foes[__i].Active();
        }
    }

    public void DeactiveCharacters()
    {
        playerCharacter.Deactive();

        for (int __i = 0; __i < foes.Length; __i++)
        {
            foes[__i].Deactive();
        }
    }

    public void RestartPlayerCharacter()
    {
        playerCharacter.Invunerable();
        playerCharacter.transform.localPosition = playerStartPos.localPosition;
    }
}
