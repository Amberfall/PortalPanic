using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;

public class GuestLogin : MonoBehaviour
{
    public enum GameState { MenuIdle, LoggingIn, Error, LoggedIn, Play };
    public GameState gameState = GameState.MenuIdle;
    // public bool isLoggedIn;

    void Start()
    {
        StartCoroutine(GuestLoginRoutine());
    }

    // public void PressButton()
    // {
    //     if (ValidAnimationIsPlaying() == false)
    //     {
    //         return;
    //     }
    //     switch (gameState)
    //     {
    //         case GameState.MenuIdle:
    //             Login();
    //             break;
    //         case GameState.Error:
    //             Login();
    //             break;
    //         case GameState.LoggedIn:
    //             Play();
    //             break;
    //         case GameState.Play:
    //             gameState = GameState.MenuIdle;
    //             break;
    //         default:
    //             break;
    //     }
    // }

    private IEnumerator GuestLoginRoutine()
    {
        bool gotResponse = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                gotResponse = true;
                gameState = GameState.LoggedIn;
            }
            else
            {
                gameState = GameState.Error;
                gotResponse = true;
            }
        });

        yield return new WaitWhile(() => gotResponse == false);
    }


    // private void SimpleGuestLogin()
    // {
    //     LootLockerSDKManager.StartGuestSession((response) => { if (response.success) isLoggedIn = true; });
    // }

}