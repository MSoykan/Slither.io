using UnityEngine;
using TMPro;

public class UIPlayerStats : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI lengthText;

    private void OnEnable() {
        PlayerLength.OnLengthChanged += PlayerLength_OnLengthChanged;
    }

    private void OnDisable() {
        PlayerLength.OnLengthChanged -= PlayerLength_OnLengthChanged;
    }

    private void PlayerLength_OnLengthChanged(ushort length) {

        lengthText.text = length.ToString();

    }
}
