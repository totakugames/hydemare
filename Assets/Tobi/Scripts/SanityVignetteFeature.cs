using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
public class SanityVignetteFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public Material material;

        [Header("Vignette")]
        [Range(0, 1)] public float vignetteIntensity = 0.5f;
        [Range(0, 1)] public float vignetteRadius = 0.5f;
        [Range(0.01f, 1)] public float vignetteSoftness = 0.3f;

        [Header("Rays")]
        [Range(8, 128)] public int rayCount = 48;
        [Range(0, 1)] public float rayIntensity = 0.5f;
        [Range(0, 0.5f)] public float rayLength = 0.2f;
        [Range(1, 20)] public float raySharpness = 8f;
        [Range(0.1f, 2f)] public float rayWidth = 1f;

        [Header("Animation")]
        [Range(0, 5)] public float pulseSpeed = 1f;
        [Range(0, 0.5f)] public float pulseAmount = 0.15f;
        [Range(0, 10)] public float waveSpeed = 3f;
        [Range(1, 20)] public float waveFrequency = 5f;
        [Range(0, 0.3f)] public float waveAmplitude = 0.08f;

        [Header("Variation")]
        [Range(0, 1)] public float rayVariation = 0.3f;
    }

    public Settings settings = new();
    private SanityVignettePass pass;

    public override void Create()
    {
        pass = new SanityVignettePass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material == null) return;
        if (renderingData.cameraData.cameraType != CameraType.Game) return;

        renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing)
    {
        pass?.Dispose();
    }

    class SanityVignettePass : ScriptableRenderPass
    {
        private Settings settings;
        private Material material;
        private RTHandle tempRT;

        private static readonly int VignetteIntensityID = Shader.PropertyToID("_VignetteIntensity");
        private static readonly int VignetteRadiusID = Shader.PropertyToID("_VignetteRadius");
        private static readonly int VignetteSoftnessID = Shader.PropertyToID("_VignetteSoftness");
        private static readonly int RayCountID = Shader.PropertyToID("_RayCount");
        private static readonly int RayIntensityID = Shader.PropertyToID("_RayIntensity");
        private static readonly int RayLengthID = Shader.PropertyToID("_RayLength");
        private static readonly int RaySharpnessID = Shader.PropertyToID("_RaySharpness");
        private static readonly int RayWidthID = Shader.PropertyToID("_RayWidth");
        private static readonly int PulseSpeedID = Shader.PropertyToID("_PulseSpeed");
        private static readonly int PulseAmountID = Shader.PropertyToID("_PulseAmount");
        private static readonly int WaveSpeedID = Shader.PropertyToID("_WaveSpeed");
        private static readonly int WaveFrequencyID = Shader.PropertyToID("_WaveFrequency");
        private static readonly int WaveAmplitudeID = Shader.PropertyToID("_WaveAmplitude");
        private static readonly int RayVariationID = Shader.PropertyToID("_RayVariation");

        public SanityVignettePass(Settings settings)
        {
            this.settings = settings;
            this.material = settings.material;
            renderPassEvent = settings.renderPassEvent;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (material == null) return;

            var resourceData = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            if (resourceData.isActiveTargetBackBuffer) return;

            var source = resourceData.activeColorTexture;

            var destinationDesc = renderGraph.GetTextureDesc(source);
            destinationDesc.name = "_SanityVignetteTempRT";
            destinationDesc.clearBuffer = false;

            TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            UpdateMaterialProperties();

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Sanity Vignette", out var passData))
            {
                passData.source = source;
                passData.material = material;

                builder.UseTexture(source, AccessFlags.Read);
                builder.SetRenderAttachment(destination, 0, AccessFlags.Write);

                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    Blitter.BlitTexture(ctx.cmd, data.source, new Vector4(1, 1, 0, 0), data.material, 0);
                });
            }

            using (var builder = renderGraph.AddRasterRenderPass<PassData>("Sanity Vignette Copy Back", out var passData))
            {
                passData.source = destination;
                passData.material = null;

                builder.UseTexture(destination, AccessFlags.Read);
                builder.SetRenderAttachment(source, 0, AccessFlags.Write);

                builder.SetRenderFunc((PassData data, RasterGraphContext ctx) =>
                {
                    Blitter.BlitTexture(ctx.cmd, data.source, new Vector4(1, 1, 0, 0), 0, false);
                });
            }
        }

        private class PassData
        {
            public TextureHandle source;
            public Material material;
        }

        private void UpdateMaterialProperties()
        {
            if (material == null) return;

            material.SetFloat(VignetteIntensityID, settings.vignetteIntensity);
            material.SetFloat(VignetteRadiusID, settings.vignetteRadius);
            material.SetFloat(VignetteSoftnessID, settings.vignetteSoftness);

            material.SetFloat(RayCountID, settings.rayCount);
            material.SetFloat(RayIntensityID, settings.rayIntensity);
            material.SetFloat(RayLengthID, settings.rayLength);
            material.SetFloat(RaySharpnessID, settings.raySharpness);
            material.SetFloat(RayWidthID, settings.rayWidth);

            material.SetFloat(PulseSpeedID, settings.pulseSpeed);
            material.SetFloat(PulseAmountID, settings.pulseAmount);
            material.SetFloat(WaveSpeedID, settings.waveSpeed);
            material.SetFloat(WaveFrequencyID, settings.waveFrequency);
            material.SetFloat(WaveAmplitudeID, settings.waveAmplitude);

            material.SetFloat(RayVariationID, settings.rayVariation);
        }

        public void Dispose()
        {
            tempRT?.Release();
        }
    }
}