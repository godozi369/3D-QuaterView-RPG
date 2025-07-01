using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private TextMeshProUGUI tipText;

    private float currentProgress = 0f;

    private string[] tips =
    {
        "TIP : Press the TAB key to switch elements. The basic attack will change!"

    };

    private void Start()
    {
        ShowRandomTip();
        StartCoroutine(LoadSceneProcess());
    }
    private void ShowRandomTip()
    {
        int index = Random.Range(0, tips.Length);
        tipText.text = tips[index];
    }
    IEnumerator LoadSceneProcess()
    {
        float fakeTarget = 0.99f;
        while (Mathf.Abs(currentProgress - fakeTarget) > 0.001f)
        {
            currentProgress = Mathf.Lerp(currentProgress, fakeTarget, Time.deltaTime * 0.5f);
            loadingSlider.value = currentProgress;
            percentText.text = Mathf.FloorToInt(currentProgress * 100f) + "%";
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.ChangeState(GameState.Town);
    }

}
