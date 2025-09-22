using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RedFlash : MonoBehaviour
{
    [SerializeField] private float flashTime = 0.1f;

    private Image screen;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screen = GetComponent<Image>();
    }

    public void StartFlash()
    {
        StartCoroutine(Flash());
        Debug.Log("FLASH");
    }
    
    private IEnumerator Flash()
    {
        float t = 0;
        
        while (t <= flashTime / 2)
        {
            Color color = screen.color;
            color.a = Mathf.Lerp(0, 1, t / flashTime);
            screen.color = color;
            t += Time.deltaTime;
            yield return null;
        }
        
        t = 0;
        while (t <= flashTime / 2)
        {
            Color color = screen.color;
            color.a = Mathf.Lerp(1, 0, t / flashTime);
            screen.color = color;
            t += Time.deltaTime;
            yield return null;
        }
        
        Color transColor = screen.color;
        transColor.a = 0;
        screen.color = transColor;
    }
}
