using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dissappearTxt : MonoBehaviour
{
    public TextMeshProUGUI uiText;  
    public Image uiImage;           
    public float textDisappearTime = 5f;  
    public float imageDisappearTime = 3f; 

    private void Start()
    {
        
        if (uiText != null)
        {
            StartCoroutine(FadeOutElement(uiText, textDisappearTime));
        }

        if (uiImage != null)
        {
            StartCoroutine(FadeOutElement(uiImage, imageDisappearTime));
        }
    }

    private IEnumerator FadeOutElement(Graphic uiElement, float fadeTime)
    {
        float elapsedTime = 0f;
        Color originalColor = uiElement.color;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime); 

            
            uiElement.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null; 
        }

        
        uiElement.gameObject.SetActive(false);
    }
}