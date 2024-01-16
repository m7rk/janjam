
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public enum MenuState
    {
        TITLE,
        OPTIONS,
        CREDITS,
        PLAYER_COUNT_SELECT,
        CHARACTER_SELECT,
        START_GAME
    }

    public GameObject title;
    public GameObject options;
    public GameObject credits;
    public GameObject playerCountSelect;
    public GameObject characterSelect;
    public GameObject startGame;

    private MenuState currState = MenuState.TITLE;

    public void setStateTitle() => setCurrentState(MenuState.TITLE);
    public void setStateOptions() => setCurrentState(MenuState.OPTIONS);
    public void setStateCredits() => setCurrentState(MenuState.CREDITS);
    public void setStatePlayerCountSelect() => setCurrentState(MenuState.PLAYER_COUNT_SELECT);
    public void setStateCharacterSelect() => setCurrentState(MenuState.CHARACTER_SELECT);
    public void setStateCharacterStartGameDog() => setCurrentState(MenuState.START_GAME);
    public void setStateCharacterStartGameKomodo() => setCurrentState(MenuState.START_GAME);
    public void setStateStartGame2P() => setCurrentState(MenuState.START_GAME);

    private void setCurrentState(MenuState nextState)
    {
        currState = nextState;
        // disable all other states for now
        title.SetActive(nextState == MenuState.TITLE);
        options.SetActive(nextState == MenuState.OPTIONS);
        credits.SetActive(nextState == MenuState.CREDITS);
        playerCountSelect.SetActive(nextState == MenuState.PLAYER_COUNT_SELECT);
        characterSelect.SetActive(nextState == MenuState.CHARACTER_SELECT);
        startGame.SetActive(nextState == MenuState.START_GAME);
    }

    public void Start()
    {
        setCurrentState(MenuState.TITLE);
    }
    public void launchGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void Update()
    {
        
    }

}
