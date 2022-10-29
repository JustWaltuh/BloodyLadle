using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BloodBottleScript : MonoBehaviour
{
    [SerializeField] public float _bloodFilled = 0f;
    [SerializeField] private float _maxBlood = 25f;
    [SerializeField] private TMP_Text _bloodAmountText;

    public GameObject fullBottle;
    public GameObject halfBottle;
    public GameObject lowBottle;
    public GameObject emptyBottle;

    private void Awake()
    {
        fullBottle = GameObject.Find("Full Bottle");
        halfBottle = GameObject.Find("Half Bottle");
        lowBottle = GameObject.Find("Low Bottle");
        emptyBottle = GameObject.Find("Empty Bottle");
    }

    private void Start()
    {
        fullBottle.SetActive(false);
        halfBottle.SetActive(false);
        lowBottle.SetActive(false);

        _bloodAmountText.text = "0";
    }

    public void UpdateBottle(float bloodTaken)
    {
        //_bloodFilled = PlayerMovement.Instance.bloodFilled;
        _bloodFilled += bloodTaken;
        if (_bloodFilled > _maxBlood) _bloodFilled = _maxBlood;
        if (_bloodFilled < 0) _bloodFilled = 0;

        _bloodAmountText.text = _bloodFilled.ToString();

        if (_bloodFilled >= 100f)
        {
            fullBottle.SetActive(true);
            halfBottle.SetActive(false);
            lowBottle.SetActive(false);
            emptyBottle.SetActive(false);
        }
        else if (_bloodFilled >= 50f && _bloodFilled < 100f)
        {
            fullBottle.SetActive(false);
            halfBottle.SetActive(true);
            lowBottle.SetActive(false);
            emptyBottle.SetActive(false);
        }
        else if (_bloodFilled > 0 && _bloodFilled < 50f)
        {
            fullBottle.SetActive(false);
            halfBottle.SetActive(false);
            lowBottle.SetActive(true);
            emptyBottle.SetActive(false);
        }
        else
        {
            fullBottle.SetActive(false);
            halfBottle.SetActive(false);
            lowBottle.SetActive(false);
            emptyBottle.SetActive(true);
        }
    }


}
