using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void StartLoadingScene(int sceneId)
    {
        FadeController.Instance.StartFadeIn(2,
        () =>
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneId);
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
        FadeController.Instance.StartFadeIn(2,
        () =>
        {
            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            loading.completed +=
                (AsyncOperation) =>
                {
                    FadeController.Instance?.StartFadeOut();  // Start game with fade on
                };

        },
        true);
    }
}
