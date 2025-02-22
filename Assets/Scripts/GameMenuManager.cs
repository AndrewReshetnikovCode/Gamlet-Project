using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
    {
    [SerializeField] KeyCode _enableKey;
    [SerializeField] KeyCode _disableKey;
    [SerializeField] GameObject _gameMenu;
    private void Update()
    {
        if (Input.GetKeyDown(_enableKey))
        {
            _gameMenu.SetActive(true);

            PlayerManager.Instance.CursorLocked = false;
        }
        if (Input.GetKeyDown(_disableKey)) {
            _gameMenu.SetActive(false);

            PlayerManager.Instance.CursorLocked = true;
        }
    }
}
