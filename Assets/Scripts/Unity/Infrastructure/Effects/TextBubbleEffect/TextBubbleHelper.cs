using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Infrastructure.Effects
{
    public class TextBubbleHelper : MonoBehaviour
    {
        [SerializeField] 
        private string _hitBubblesTextsString;
        [SerializeField]
        private PopupType[] _hitBubbleTypes;
        
        private string[] _hitBubbleTexts;
        
        private void Start()
        {
            _hitBubbleTexts = _hitBubblesTextsString.Split(", ");
        }

        public string GetRandomBubbleText()
        {
            return _hitBubbleTexts[Random.Range(0, _hitBubbleTexts.Length)];
        }

        public PopupType GetRandomBubbleType()
        {
            return  _hitBubbleTypes[Random.Range(0, _hitBubbleTypes.Length)];
        }
        
        
    }
}