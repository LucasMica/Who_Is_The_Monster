using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager UIInstance { get; private set; } = null;
    public Canvas dialogCanvas;
    public Canvas playerHUD;
    public TextMeshProUGUI personName;
    public TextMeshProUGUI dialogText;
    public Canvas fadeCanvas;

    void Awake()
    {
        if (UIInstance != null && UIInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        UIInstance = this;
    }
}
