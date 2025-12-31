using UnityEngine;
using UnityEngine.UI;

public class ButtonSFXManager : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PlaySFX());
        }
    }
    private void PlaySFX()
    {
        AudioManager.Instance.PlaySFX(EnumData.SFX.Button1SFX);
    }
}