using Jacobi.Vst.Core;
using Jacobi.Vst.Plugin.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacobi.Vst.Samples.Delay.Dsp
{
    internal class ReverbParameters
    {
        private const string ParameterCategoryName = "Reverb";

        public ReverbParameters(PluginParameters parameters)
        {
            Throw.IfArgumentIsNull(parameters, nameof(parameters));

            InitializeParameters(parameters);
        }

        public VstParameterManager RoomSizeMgr { get; private set; }
        public VstParameterManager DampingMgr { get; private set; }
        public VstParameterManager WidthMgr { get; private set; }
        public VstParameterManager DryLevelMgr { get; private set; }
        public VstParameterManager WetLevelMgr { get; private set; }

        private void InitializeParameters(PluginParameters parameters)
        {
            // all parameter definitions are added to a central list.
            VstParameterInfoCollection parameterInfos = parameters.ParameterInfos;

            // retrieve the category for all delay parameters.
            VstParameterCategory paramCategory =
                parameters.GetParameterCategory(ParameterCategoryName);

            // room size parameter
            var paramInfo = new VstParameterInfo
            {
                Category = paramCategory,
                CanBeAutomated = true,
                Name = "RoomSize",
                Label = "",
                ShortLabel = "",
                MinInteger = 0,
                MaxInteger = 1,
                LargeStepFloat = 0.2f,
                StepFloat = 0.05f,
                SmallStepFloat = 0.05f,
                DefaultValue = 0.5f
            };
            RoomSizeMgr = paramInfo
                .Normalize()
                .ToManager();

            parameterInfos.Add(paramInfo);

            // damping parameter
            paramInfo = new VstParameterInfo
            {
                Category = paramCategory,
                CanBeAutomated = true,
                Name = "Damping",
                Label = "",
                ShortLabel = "",
                MinInteger = 0,
                LargeStepFloat = 0.2f,
                StepFloat = 0.05f,
                SmallStepFloat = 0.05f,
                DefaultValue = 0
            };
            DampingMgr = paramInfo
                .Normalize()
                .ToManager();

            parameterInfos.Add(paramInfo);

            // width parameter
            paramInfo = new VstParameterInfo
            {
                Category = paramCategory,
                CanBeAutomated = true,
                Name = "Width",
                Label = "",
                ShortLabel = "",
                MaxInteger = 1,
                LargeStepFloat = 0.2f,
                StepFloat = 0.05f,
                SmallStepFloat = 0.05f,
                DefaultValue = 0f
            };
            WidthMgr = paramInfo
                .Normalize()
                .ToManager();

            parameterInfos.Add(paramInfo);

            // dry Level parameter
            paramInfo = new VstParameterInfo
            {
                Category = paramCategory,
                CanBeAutomated = true,
                Name = "RDry Lvl",
                Label = "Decibel",
                ShortLabel = "Db",
                MaxInteger = 1,
                LargeStepFloat = 0.2f,
                StepFloat = 0.05f,
                SmallStepFloat = 0.05f,
                DefaultValue = 0.4f
            };
            DryLevelMgr = paramInfo
                .Normalize()
                .ToManager();

            parameterInfos.Add(paramInfo);

            // wet Level parameter
            paramInfo = new VstParameterInfo
            {
                Category = paramCategory,
                CanBeAutomated = true,
                Name = "RWet Lvl",
                Label = "Decibel",
                ShortLabel = "Db",
                MaxInteger = 1,
                LargeStepFloat = 0.2f,
                StepFloat = 0.05f,
                SmallStepFloat = 0.05f,
                DefaultValue = 0f
            };
            WetLevelMgr = paramInfo
                .Normalize()
                .ToManager();

            parameterInfos.Add(paramInfo);
        }
    }
}
