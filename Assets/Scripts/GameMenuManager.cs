using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] KeyCode _enableKey;
    [SerializeField] KeyCode _disableKey;
    [SerializeField] UIInventoryManager _gameMenu;
    private void Update()
    {
        if (Input.GetKeyDown(_enableKey) && _gameMenu.Enabled == false)
        {
            _gameMenu.SetTraderWindowActive(false);
            _gameMenu.Display(true);

            PlayerManager.Instance.OpenInventory(false);
        }
        if (Input.GetKeyDown(_disableKey))
        {
            _gameMenu.Display(false);

            PlayerManager.Instance.CloseInventory();
        }
    }
}
