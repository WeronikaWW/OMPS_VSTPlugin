namespace Jacobi.Vst.Samples.Delay.Dsp
{
    internal interface IVstEffect
    {
        float ProcessSample(float sample);
        float SampleRate { get; set; }
    }
}
