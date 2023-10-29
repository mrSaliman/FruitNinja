using UnityEngine;
using UnityEngine.SceneManagement;

namespace App.Mixed
{
    public class SceneTransition : MonoBehaviour
{
    private static bool _shouldPlayOpeningAnimation = false; 
    
    [SerializeField] private Animator componentAnimator;
    private AsyncOperation _loadingSceneOperation;
    private static readonly int SceneClosing = Animator.StringToHash("sceneClosing");
    private static readonly int SceneOpening = Animator.StringToHash("sceneOpening");

    public void SwitchToScene(string sceneName)
    {
        componentAnimator.SetTrigger(SceneClosing);

        _loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        _loadingSceneOperation.allowSceneActivation = false;
    }
    
    private void Start()
    {
        if (_shouldPlayOpeningAnimation) 
        {
            componentAnimator.SetTrigger(SceneOpening);
            
            _shouldPlayOpeningAnimation = false; 
        }
    }

    public void OnAnimationOver()
    {
        _shouldPlayOpeningAnimation = true;
        
        _loadingSceneOperation.allowSceneActivation = true;
    }
}
}