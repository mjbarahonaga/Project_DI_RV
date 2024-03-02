using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManager_M : MonoBehaviour
{
    public void StartLoadingScene(int sceneId)
    {
        Timing.KillCoroutines();
        FadeController.Instance.StartFadeIn(2,
        () =>
        {
            var loading = SceneManager.LoadSceneAsync(sceneId);
            loading.completed +=
                (AsyncOperation) =>
                {
                    FadeController.Instance?.StartFadeOut();  // Start game with fade on
                };

        },
        true);
    }

    public void StartLoadingScene(string sceneName)
    {
        Timing.KillCoroutines();
        FadeController.Instance.StartFadeIn(2,
        () =>
        {
            var loading = SceneManager.LoadSceneAsync(sceneName);
            loading.completed +=
                (AsyncOperation) =>
                {
                    FadeController.Instance?.StartFadeOut();  // Start game with fade on
                };

        },
        true);
    }
}
