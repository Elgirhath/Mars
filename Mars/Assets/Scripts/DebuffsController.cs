using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DebuffsController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public static DebuffsController instance;
    public float pulsingSpeed;
    public float textLettersSpeed;
    private Text debuffAlert;
    
    private void Awake() {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
        }
        debuffAlert = GameObject.Find("DebuffAlert").GetComponent<Text>();
        debuffAlert.text = "";
    }

    public void DeactivateAll()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void AddDebuff(GameObject debuff, string alert)
    {
        debuffAlert.text = "_";
        debuff.SetActive(true);
        StartCoroutine(DebuffPulse(debuff, 3));
        StartCoroutine(ShowAlert(alert));
    }

    public void RemoveDebuff(GameObject debuff)
    {
        debuff.SetActive(false);
        if(debuffAlert.text != "")
            StopCoroutine(ShowAlert(""));
    }

    // Update is called once per frame
    private IEnumerator DebuffPulse(GameObject debuff, int rep)
    {
        Image image = debuff.GetComponent<Image>();
        image.CrossFadeAlpha(1,pulsingSpeed,true);
        yield return new WaitForSeconds(pulsingSpeed);
        if (rep > 0)
        {
            image.CrossFadeAlpha(0, pulsingSpeed, true);
            yield return new WaitForSeconds(pulsingSpeed);
            StartCoroutine(DebuffPulse(debuff, rep - 1));
        }
    }

    private IEnumerator ShowAlert(string alert)
    {
        for (var i = 0; i < alert.Length; ++i)
        {
            debuffAlert.text = debuffAlert.text.Insert(debuffAlert.text.Length-1, alert[i].ToString());
            yield return new WaitForSeconds(textLettersSpeed);
        }
        yield return new WaitForSeconds(2.0f);
        debuffAlert.text = "";
    }
}
