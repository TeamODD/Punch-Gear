using System.Collections;
using UnityEngine;

namespace PunchGear.Entity
{
    public static class Sprites
    {
        public static Coroutine StartBlink(this MonoBehaviour monoBehaviour, SpriteRenderer spriteRenderer, int count, float blinkRate)
        {
            Material material = spriteRenderer.material;
            return monoBehaviour.StartCoroutine(StartBlinkAnimation(material, count, blinkRate));
        }

        private static IEnumerator StartBlinkAnimation(Material material, int count, float blinkRate)
        {
            Color originalColor = material.color;
            float originalAlpha = originalColor.a;
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(blinkRate);
                originalColor.a = 0;
                material.color = originalColor;
                yield return new WaitForSeconds(blinkRate);
                originalColor.a = originalAlpha;
                material.color = originalColor;
            }
            yield return new WaitForSeconds(blinkRate);
            originalColor.a = 0;
            material.color = originalColor;
        }
    }
}
