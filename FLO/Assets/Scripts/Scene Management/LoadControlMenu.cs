using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadControlMenu : MonoBehaviour
{
   [SerializeField] private GameObject _playerUI;
   [SerializeField] private GameObject _exitButtonUI;
   
   public void LoadMenu()
   {
      SceneManager.LoadScene("Controls Scene", LoadSceneMode.Additive);
      _playerUI.SetActive(false);
      _exitButtonUI.SetActive(true);
      Time.timeScale = 1;
   }

   public void ExitMenu()
   {
      SceneManager.UnloadSceneAsync("Controls Scene");
      _playerUI.SetActive(true);
      Time.timeScale = 0;
      _exitButtonUI.SetActive(false);
   }
}
