using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingInfoTextController : MonoBehaviour
{
    public float stackTime;
    private float fadingDuration;

    private Text textController;

    public void SetValues(float time, string name, int quantity)
    {
        fading = false;
        itemName = name;
        stackTime = time;
        this.quantity = quantity;
    }

    public int quantity { get; set; }

    public string itemName { get; set; }

    public bool fading { get; set; }

    public void Diminish()
    {
        fading = true;
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        textController.CrossFadeAlpha(0, fadingDuration,true);
        yield return new WaitForSeconds(fadingDuration);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        fadingDuration = 1.5f;
        textController = gameObject.GetComponent<Text>();
    }
}
