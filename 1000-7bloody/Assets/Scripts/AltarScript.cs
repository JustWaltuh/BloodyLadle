using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class AltarScript : MonoBehaviour
{
    public float bloodLiters = 0f;
    public TMP_Text bloodText;

    public GameObject levelCompleteUI;

    public AudioSource claimSound;
    public AudioSource perehodSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerScript.Instance.gameObject && PlayerScript.Instance.bloodFilled > 0)
        {
            claimSound.Play();
            bloodLiters += PlayerScript.Instance.bloodFilled;
            bloodText.text = bloodLiters.ToString() + " liters filled";
            PlayerScript.Instance.bloodFilled = 0;
            PlayerScript.Instance.bloodBottle.UpdateBottle(-100f);
            Debug.Log(bloodLiters);

            if (bloodLiters >= 1000)
            {
                StartCoroutine(LevelComplete());
            }
        }
    }
    private IEnumerator LevelComplete()
    {
        CinemaMachineShake.Instance.ShakeCamera(6f, .5f);
        perehodSound.Play();
        levelCompleteUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
