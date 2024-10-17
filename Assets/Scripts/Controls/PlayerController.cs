using System;
using BreathingMinigame;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private UIController _ui;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private CircleController _minigameController;

        public NPCController _closestNpc;

        public NPCController ClosestNpc => _closestNpc;


        private void Update()
        {
            if (!_closestNpc)
                _ui.HideInteract();
        }

        /// <summary>
        /// Interacts with closest NPC if possible.
        /// </summary>
        /// <param name="context">InputAction context.</param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.started || _closestNpc == null || !_movement.CanMove)
            {
                return;
            }

            SetDialogue();
            PauseMovement();
            _closestNpc.StartConversation();
        }

        /// <summary>
        /// Triggers minigame
        /// </summary>
        /// <param name="context">InputAction context.</param>
        public void OnPlayMinigame(InputAction.CallbackContext context)
        {
            if (context.started && (_closestNpc == null || !_closestNpc.talking))
            {
                _minigameController.Play();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("NPC")) return;
            if (_closestNpc != null) return;
            if (other.gameObject.GetComponent<NPCController>() == null || !other.gameObject.GetComponent<NPCController>().enabled) return;
            _closestNpc = other.gameObject.GetComponent<NPCController>();
            if (_closestNpc.contents == null)
            {
                _ui.ShowInteract(_closestNpc.mainContents);
            }
            else
            {
                _ui.ShowInteract(_closestNpc.contents);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out NPCController otherController)) return;
            if (_closestNpc == otherController && _closestNpc.talking)
            {    
                _closestNpc.EndConversation();
            }

            _closestNpc = null;
            _ui.HideInteract();
        }

        /// <summary>
        /// Hide the Interaction UI and show the basic dialogue.
        /// </summary>
        private void SetDialogue()
        {
            _ui.HideInteract();
            if (_closestNpc.contents == null)
            {
                _ui.ShowAdvancedDialogue(_closestNpc.mainContents);
            }
            else
            {
                _ui.ShowBasicDialogue(_closestNpc.contents);
            }
        }

        /// <summary>
        /// Pauses the player's movement.
        /// </summary>
        private void PauseMovement()
        {
            _movement.CanMove = false;
        }

        /// <summary>
        /// Resumes the player's movement.
        /// </summary>
        public void ResumeMovement()
        {
            _movement.CanMove = true;
        }
    }
}
