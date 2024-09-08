using TMPro;
using UnityEngine;

public class BaseMoneyView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Base _moneyContainer;

    private void OnEnable()
    {
        UpdateView(_moneyContainer.CrystalsCount);
        _moneyContainer.CrystalCountChanged += UpdateView;
    }

    private void OnDisable()
    {
        _moneyContainer.CrystalCountChanged -= UpdateView;
    }

    private void UpdateView(ulong amount)
    {
        _text.text = amount.ToString();
    }
}
