using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Characters
{
    public class BlinkingEyeCoordinator : MonoBehaviour
    {
        [SerializeField] private List<BlinkingEye> eyes;

        private void Start()
        {
            StartCoroutine(InitBlinking());
        }

        private IEnumerator InitBlinking()
        {
            while (true)
            {
                var secondsTillNextBlink = 4 + Random.value * 5;
                var secondsInTransition = 0.1f;
                var secondsWithEyesClosed = Random.value * 2;
            
                eyes.ForEach(eye => eye.ToOpen());
                yield return new WaitForSeconds(secondsTillNextBlink);
            
                eyes.ForEach(eye => eye.ToMedium());
                yield return new WaitForSeconds(secondsInTransition);
            
                eyes.ForEach(eye => eye.ToClose());
                yield return new WaitForSeconds(secondsWithEyesClosed);
            
                eyes.ForEach(eye => eye.ToMedium());
                yield return new WaitForSeconds(secondsInTransition);
            }
        }
    }
}