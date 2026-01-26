using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private string clickSoundID;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    private void HandleOnClick()
    {
        AudioService.TryPlaySoundEffect(clickSoundID);
    }
}
