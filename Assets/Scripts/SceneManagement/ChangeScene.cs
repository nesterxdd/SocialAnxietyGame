using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SceneManagement
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private string _scene;
        [SerializeField] private GameObject _exitTransition;
        [SerializeField] private int _transitionDurationMilliseconds = 500;

        public void SwitchScene()
        {
            try
            {
                EventSystem.current.enabled = false;
            }
            catch (NullReferenceException)
            {
            }

            _exitTransition.SetActive(true);
            Cursor.visible = true;
            _ = LoadGame();
        }

        private async Task LoadGame()
        {
            await Task.Delay(_transitionDurationMilliseconds);
            SceneManager.LoadScene(_scene);
        }
        
        public void SwitchSceneInstant()
        {
            SceneManager.LoadScene(_scene);
        }
    }
}
