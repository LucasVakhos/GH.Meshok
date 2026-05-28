using GH.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace MeshokBrowser.Workers
{
    public class WorkerTemplate
    {
        private readonly string yesNoText;
        private readonly Type processorType;
        public Func<bool> DialogAction { get; set; } = null;
        public WorkerTemplate(Type processorType, string yesNoText = null)
        {
            this.yesNoText = yesNoText;
            this.processorType = processorType;
        }
        public bool CanDo()
        {
            if (DialogAction != null)
                return DialogAction();
            if (yesNoText == null)
                return true;
            return DlgHelper.DlgYesNo(yesNoText);
        }
        public Type ProcessorType => processorType;
        public AbstractWorker CreateNew(params object[] constructorArguments)
        {
            ConstructorInfo constructor = null;
            Type[] suppliedArgTypes = constructorArguments
                .Select(param => param == null ? typeof(object) : param.GetType())
                .ToArray();
            foreach (ConstructorInfo ctor in processorType.GetConstructors())
            {
                ParameterInfo[] ctorParams = ctor.GetParameters();
                Type paramArrayType =
                    ctorParams.LastOrDefault() != null &&
                    ctorParams.Last().GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0
                        ? ctorParams.Last().ParameterType
                        : null;
                constructor = ctor;
                for (int i = 0; i < suppliedArgTypes.Length; ++i)
                {
                    if (i >= ctorParams.Length &&
                        (paramArrayType == null || !paramArrayType.IsAssignableFrom(suppliedArgTypes[i])))
                    {
                        constructor = null;
                        continue;
                    }
                    if (!ctorParams[i].ParameterType.IsAssignableFrom(suppliedArgTypes[i]))
                        constructor = null;
                }
                if (constructor != null && ctorParams.Length > suppliedArgTypes.Length)
                {
                    List<object> newArgs = new List<object>(constructorArguments);
                    for (int i = suppliedArgTypes.Length; i < ctorParams.Length; ++i)
                    {
                        if (!ctorParams[i].IsOptional && !(paramArrayType != null && i == ctorParams.Length - 1))
                        {
                            constructor = null;
                        }
                        else if (ctorParams[i].IsOptional)
                        {
                            newArgs.Add(ctorParams[i].DefaultValue);
                        }
                    }
                    if (constructor != null)
                        constructorArguments = newArgs.ToArray();
                }
                if (constructor != null)
                    break;
            }
            if (constructor == null)
                throw new ArgumentException(nameof(constructorArguments), $"No constructor found for {processorType}.");
            return constructor.Invoke(constructorArguments) as AbstractWorker;
        }
    }
}
