using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacobi.Vst.Samples.Delay.Dsp
{
    internal class Reverb : IVstEffect
    {
        private readonly short[] _combTunings = new short[] { 1116, 1188, 1277, 1356, 1422, 1491, 1557, 1617 }; // (at 44100Hz)
        private readonly short[] _allPassTunings = new short[] { 556, 441, 341, 225 };
        const float roomScaleFactor = 0.28f;
        const float roomOffset = 0.7f;
        const float dampScaleFactor = 0.4f;
        private readonly ReverbParameters _parameters;
        private float _sampleRate;
        private float[] _reverbBuffer;
        private int _bufferLength;
        private List<AllPassFilter> _allPassFilters;
        private List<CombFilter> _combFilters;

        public Reverb(ReverbParameters reverbParameters)
        {
            _parameters = reverbParameters;
            _parameters.DryLevelMgr.PropertyChanged += Parameters_Changed;
            _parameters.DampingMgr.PropertyChanged += Parameters_Changed;
            _parameters.RoomSizeMgr.PropertyChanged += Parameters_Changed;
            _parameters.WetLevelMgr.PropertyChanged += Parameters_Changed;
            _parameters.WidthMgr.PropertyChanged += Parameters_Changed;
        }

        private void Parameters_Changed(object? sender, PropertyChangedEventArgs e)
        {
            SetFilters();
        }

        public float SampleRate
        {
            get { return _sampleRate; }
            set
            {
                _sampleRate = value;

                // allocate buffer for max reverb time
                int bufferLength = (int)(_parameters.WidthMgr.ParameterInfo.MaxInteger * _sampleRate / 1000);
                _reverbBuffer = new float[bufferLength];

                SetFilters();
                SetBufferLength();
            }
        }

        private void SetFilters()
        {
            var allPassFilters = new List<AllPassFilter>();
            for (int i = 0; i < _allPassTunings.Length; i++)
            {
                var allPassFilter = new AllPassFilter();
                allPassFilter.SetSize(Convert.ToInt32(_sampleRate) * _allPassTunings[i] / 44100);
                allPassFilters.Add(allPassFilter);
            }
            _allPassFilters = allPassFilters;

            var combFilters = new List<CombFilter>();
            for (int i = 0; i < _combTunings.Length; i++)
            {
                var combFilter = new CombFilter(_parameters.DampingMgr.CurrentValue * dampScaleFactor, _parameters.RoomSizeMgr.CurrentValue * roomScaleFactor + roomOffset);
                combFilter.SetSize(Convert.ToInt32(_sampleRate) * _combTunings[i] / 44100);
                combFilters.Add(combFilter);
            }
            _combFilters = combFilters;
        }

        private void SetBufferLength()
        {
            // logical buffer length
            _bufferLength = (int)(_parameters.WidthMgr.CurrentValue * _sampleRate / 1000);
        }

        public float ProcessSample(float sample)
        {
            const float wetScaleFactor = 0.2f;
            const float dryScaleFactor = 2.0f;
            var output = 0.0f;

            for (int i = 0; i < _combFilters.Count; i++)
            {
                output += _combFilters[i].ProcessSample(sample);
            }

            for (int i = 0; i < _allPassFilters.Count; i++)
            {
                output = _allPassFilters[i].ProcessSample(output);
            }

            var wetGain = _parameters.WetLevelMgr.CurrentValue * wetScaleFactor * 0.5f * (1.0f + _parameters.WidthMgr.CurrentValue);

            return output * wetGain + sample * _parameters.DryLevelMgr.CurrentValue * dryScaleFactor;
        }
    }

    class AllPassFilter : BufferFilter
    {
        public float ProcessSample(float sample)
        {
            var bufferedValue = buffer[bufferIndex];
            buffer[bufferIndex] = sample + bufferedValue * 0.5f;
            bufferIndex = (bufferIndex + 1) % bufferSize;
            return bufferedValue - sample;
        }
    }

    class CombFilter : BufferFilter
    {
        private float _damping, _last, _feedback;

        public CombFilter(float damping, float feedback)
        {
            _damping = damping;
            _feedback = feedback;
        }

        public float ProcessSample(float sample)
        {
            float output = buffer[bufferIndex];
            _last = (output * (1.0f - _damping)) + (_last * _damping);
            buffer[bufferIndex] = sample + (_last * _feedback);
            bufferIndex = (bufferIndex + 1) % bufferSize;

            return output;
        }
    }

    abstract class BufferFilter
    {
        protected float[] buffer;
        protected int bufferSize, bufferIndex;

        public void SetSize(int size)
        {
            if (size != bufferSize)
            {
                bufferIndex = 0;
                buffer = new float[size];
                bufferSize = size;
            }

            Clear();
        }

        private void Clear()
        {
            buffer = new float[bufferSize];
        }
    }
}
