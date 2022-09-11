using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private float _levelLoadProgress;
    private bool loading;

    private static LevelManager instance;

    

    [SerializeField]
    private CanvasGroup level;

    [SerializeField] private Slider progressBar;

    [SerializeField]
    private LoadAsset asset;

    public void FadeScreenIn()
    {
        level.gameObject.SetActive(true);
        level.alpha = 0;
        LeanTween.alphaCanvas(level, 1, 0.8f);
    }

    public void FadeScreenOut()
    {
        level.alpha = 1;
        LeanTween.alphaCanvas(level, 0, 0.8f).setOnComplete(() =>
        {
            level.gameObject.SetActive(false);
        });
    }

    public async void LoadLevel(string sceneName)
    {
        Debug.Log("Loading "+ sceneName);
        Time.timeScale = 1;
        progressBar.value = 0;
        _levelLoadProgress = 0;

        FadeScreenIn();

        await Task.Delay(800);

        loading = true;
        var scena = SceneManager.LoadSceneAsync(sceneName);
        scena.allowSceneActivation = false;
        StartCoroutine(UpdateProgressBar());

        do
        {
            _levelLoadProgress = scena.progress + (scena.progress / 9);
            await Task.Delay(100);

        }while (scena.progress < 0.9f);

        _levelLoadProgress = scena.progress + (scena.progress / 9);
        loading = false;

        

        IEnumerator UpdateProgressBar()
        {
            float time = 0f;
            while (loading || progressBar.value<0.95f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, _levelLoadProgress, time*0.5f);
                time += Time.deltaTime;
                yield return null;
            }

            scena.allowSceneActivation = true;
            yield return new WaitForSeconds(0.5f);
            FadeScreenOut();

        }
    }

    public void OnEnable(){
        asset.OnSceneLoader += LoadLevel;
    }
    public void OnDisable(){
        asset.OnSceneLoader -= LoadLevel;
    }

    public void Awake()
    {
        
        if (instance!=null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        FadeScreenOut();
        DontDestroyOnLoad(this.gameObject);
        
    }
}