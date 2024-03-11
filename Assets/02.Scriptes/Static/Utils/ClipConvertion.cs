using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Rendering;
using System.IO;
namespace Game.Utils
{
    public class ClipConvertion : MonoBehaviour
    {
        public AnimationClip animationClip;
        public int textureSize = 512;
        public int frameRate = 30;

        private Texture2D animationTexture;
        private RenderTexture renderTexture;
        private NativeArray<Color32> pixelData;

        void Start() {
            // RenderTexture ����
            renderTexture = new RenderTexture(textureSize, textureSize, 24);

            // AnimationClip�� �ؽ�ó�� ��ȯ
            ConvertAnimationToTexture();
        }

        void ConvertAnimationToTexture() {
            // �ؽ�ó ����
            animationTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

            // �� ������ ������ �� �̹����� ����
            float frameLength = 1.0f / frameRate;
            float timeStep = frameLength / animationClip.frameRate;
            pixelData = new NativeArray<Color32>(textureSize * textureSize, Allocator.Temp);

            for (int i = 0; i < animationClip.frameRate * animationClip.length; i++) {
                float time = i * timeStep;
                RenderAnimationFrame(time);
                AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGBA32, ReadbackCompleted);
            }

            // �ؽ�ó ����
            animationTexture.Apply();
            SaveTexture();
        }

        void SaveTexture() {
            byte[] textureBytes = animationTexture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Assets/Test.png", textureBytes);
        }

        void RenderAnimationFrame(float time) {
            // ���� �ؽ�ó�� ���� ������ ������
            RenderTexture.active = renderTexture;
            animationClip.SampleAnimation(gameObject, time);
            Graphics.Blit(null, renderTexture, new Material(Shader.Find("Hidden/Universal Render Pipeline/Blit")));
        }

        void ReadbackCompleted(AsyncGPUReadbackRequest request) {
            if (request.hasError) {
                Debug.LogError("GPU readback error: ");
                return;
            }

            pixelData.CopyFrom(request.GetData<Color32>());
            animationTexture.SetPixelData(pixelData, 0);
        }
    }
}
