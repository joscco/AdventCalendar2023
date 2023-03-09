using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameScene.Items.Scripts.Field;
using UnityEngine;

namespace GameScene.Items.Scripts.Item.General
{
    public class FieldEntityElementRenderer : MonoBehaviour
    {
        public FieldEntityElement element;
        public FieldSpot currentSpot;
        public ParticleSystem sparkleEmitter;
        public SpriteRenderer renderer;
        public PolygonCollider2D collider;

        private bool _shown;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<PolygonCollider2D>();
        }
    
        public IEnumerator BlendIn(Sprite newSprite)
        {
            _shown = true;
            renderer.sprite = newSprite;
            UpdatePolyCollider2DToSprite(collider, newSprite);
            yield return transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);

        }

        public IEnumerator BlendOut()
        {
            _shown = false;
            yield return transform.DOScale(0, 0.5f).SetEase(Ease.InBack);
            sparkleEmitter.Play();
        }

        public bool IsShown()
        {
            return _shown;
        }

        public void UpdateSpriteWhileBlendIn(Sprite newSprite)
        {
            var scaleSequence = DOTween.Sequence();
            scaleSequence.Append(transform.DOScale(0.75f, 0.1f).SetEase(Ease.InOutQuad));
            scaleSequence.AppendCallback(() =>
            {
                renderer.sprite = newSprite;
                UpdatePolyCollider2DToSprite(collider, newSprite);
            });
            scaleSequence.Append(transform.DOScale(1f, 0.1f).SetEase(Ease.InOutQuad));
        }

        private void OnMouseUp()
        {
            if (element != null)
            {
                element.OnClick();
            }
        }

        private static void UpdatePolyCollider2DToSprite(PolygonCollider2D collider, Sprite sprite)
        {
            if (collider != null && sprite != null)
            {
                collider.pathCount = sprite.GetPhysicsShapeCount();

                List<Vector2> path = new List<Vector2>();

                for (int i = 0; i < collider.pathCount; i++)
                {
                    path.Clear();
                    sprite.GetPhysicsShape(i, path);
                    collider.SetPath(i, path.ToArray());
                }
            }
        }
    }
}