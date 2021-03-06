using TMPro;
using UnityEngine;

namespace SceneControllers
{
    public class Initial : MonoBehaviour
    {

        public AudioClip soundClip;
        public AudioSource AS;
        private char _initial;
        private TextMeshProUGUI _initialText;

        private void Awake()
        {
            _initialText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _initial = _initialText.text[0];
        }

        public void UpInitial()
        {
            AS.clip = soundClip;
            AS.Play();
            _initial++;
            UpdateInitial();
        }

        public void DownInitial()
        {
            AS.clip = soundClip;
            AS.Play();
            _initial--;
            UpdateInitial();
        }

        private void UpdateInitial()
        {
            // modulo wouldn't work lol
            if (_initial > 'A' + 25) _initial = 'A';
            if (_initial < 'A') _initial = 'Z';
            _initialText.text = _initial.ToString();
        }
        
        public char GetInitial() => _initial;
    }
}
