using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Controls
{
    public class PlayerPanicControl : MonoBehaviour
    {
        public static PlayerPanicControl instance;
      
        [Header("Panic Settings")] 
        public float panicLevel;
        public float panicModifier;
        public bool earphonesIn;
        private float earphoneReduction;
        private int npcThreshold;

        [Header("Confidence Settings")] 
        private float confidenceLevel;

        [Header("Panic Attack Settings")]
        [SerializeField] private UnityEvent _onPanicAttack;

        [Header("Update Settings")]
        private const float UpdateTime = 0.8f;
        private float difficultyModifier = 1f;
        private float updateCountdown;
        private float lerpSpeed = 0.25f;
        private float time;
        public bool isPaused;

        [Header("UI Elements")]
        [SerializeField] private Slider panicSlider;
        [SerializeField] private Slider confidenceSlider;
        public List<GameObject> npcInRange = new();

        public UnityEvent OnPanicAttack => _onPanicAttack;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            earphonesIn = false;
            isPaused = false;
            panicLevel = 0;
            panicModifier = 5f;
            earphoneReduction = 10f;
            panicSlider.value = 0;
            confidenceSlider.value = 0;
            confidenceLevel = 0;
            npcThreshold = 1;
        }

        private void Update()
        {

            if (isPaused) return;

            AnimatePanicMetre();
            AnimateConfidenceMetre();
            updateCountdown -= Time.deltaTime;
            if (updateCountdown < 0)
            {
                updateCountdown = 0;
            }
            if (updateCountdown != 0) return;
            int npcCount = npcInRange.Count;

            //Run actions based on the current NPCs in range
            if (npcCount > npcThreshold)
            {
                panicLevel += IncreasePanicLevel();
            }
            else
            {
                switch (panicLevel)
                {
                    case 0:
                        break;
                    case > 0 when earphonesIn:
                        panicLevel -= earphoneReduction;
                        break;
                    case > 0:
                        panicLevel -= 5;
                        break;
                }
            }
            switch (panicLevel)
            {
                case < 0:
                    panicLevel = 0;
                    break;
                case > 100:
                    GameOver();
                    break;
            }
            updateCountdown = UpdateTime;
            time = 0;
        }

        private void AnimatePanicMetre()
        {
            var targetLevel = panicLevel;
            var startLevel = panicSlider.value;
            time += Time.deltaTime * lerpSpeed;

            panicSlider.value = Mathf.Lerp(startLevel, targetLevel, time);
        }
        
        private void AnimateConfidenceMetre()
        {
            var targetLevel = confidenceLevel;
            var startLevel = confidenceSlider.value;
            time += Time.deltaTime * lerpSpeed;

            confidenceSlider.value = Mathf.Lerp(startLevel, targetLevel, time);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("NPC")) return;
            npcInRange.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("NPC")) return;
            npcInRange.Remove(other.gameObject);
        }

        /// <summary>
        /// Returns an int depending on the NPCs in range
        /// </summary>
        private float IncreasePanicLevel()
        {
            var totalPanic = panicModifier * npcInRange.Count * difficultyModifier;
            return totalPanic;
        }

        /// <summary>
        /// When the panic level exceeds the threshold end / reset the Game
        /// </summary>
        private void GameOver()
        {
            panicLevel = 25;
            npcInRange.Clear();
            _onPanicAttack.Invoke();
        }

        /// <summary>
        /// When the player finishes a main NPC conversation
        /// </summary>
        public void MainImprovePanicDurability()
        {
            confidenceLevel += 10f;
            panicModifier -= 1f;
        }

        /// <summary>
        /// If the player interacts with a minor NPC conversation
        /// </summary>
        public void MinorImprovePanicDurability()
        {
            confidenceLevel += 4f;
            panicModifier -= 0.4f;
        }
        
        /// <summary>
        /// Set the difficulty to easy and change the modifier
        /// </summary>
        public void SetEasyDifficulty()
        {
            difficultyModifier = 0.5f;
        }
        /// <summary>
        /// Set the difficulty to medium and change the modifier
        /// </summary>
        public void SetMediumDifficulty()
        {
            difficultyModifier = 1f;
        }
        /// <summary>
        /// Set the difficulty to hard and change the modifier
        /// </summary>
        public void SetHardDifficulty()
        {
            difficultyModifier = 1.5f;
        }
    }
}
