using UnityEngine;
using TMPro;

public class CollectedBloodScript : MonoBehaviour
{
    public AltarScript altar;

    [SerializeField] private TMP_Text _collectedBloodText;

    public void UpdateText()
    {
        _collectedBloodText.text = altar.bloodLiters.ToString() + " LITERS";
    }
}
