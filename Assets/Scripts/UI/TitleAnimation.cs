using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class TitleAnimation : MonoBehaviour
    {
        public List<GameObject> letters;
        private List<Vector3> _initialPositions;
        public int yOffsetWhenFadeOut = -1000;
        public float secondOffsetLetterAnimations = 0.1f;
        public float secondDurationLetterAnimation = 0.5f;

        public void Start()
        {
            _initialPositions = new List<Vector3>(letters.Select(letter => letter.transform.localPosition));
            Hide();
        }

        public void Hide()
        {
            for (int i = 0; i < letters.Count; i++)
            {
                letters[i].transform.localPosition = _initialPositions[i] + yOffsetWhenFadeOut * Vector3.up;
            }
        }

        public void Show()
        {
            for (int i = 0; i < letters.Count; i++)
            {
                letters[i].transform.localPosition = _initialPositions[i];
            }
        }

        public Sequence FadeIn()
        {
            var seq = DOTween.Sequence();
            for (int i = 0; i < letters.Count; i++)
            {
                seq.Insert(
                    i * secondOffsetLetterAnimations,
                    letters[i].transform.DOLocalMoveY(
                            _initialPositions[i].y,
                            secondDurationLetterAnimation)
                        .SetEase(Ease.InOutBack)
                );
            }

            return seq;
        }

        public void FadeOut()
        {
            var seq = DOTween.Sequence();
            for (int i = 0; i < letters.Count; i++)
            {
                seq.Insert(
                    i * secondOffsetLetterAnimations,
                    letters[i].transform.DOLocalMoveY(
                            _initialPositions[i].y + yOffsetWhenFadeOut,
                            secondDurationLetterAnimation)
                        .SetEase(Ease.InOutBack)
                );
            }

            seq.Play();
        }
    }
}