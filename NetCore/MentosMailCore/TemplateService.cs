using System;
using System.Collections.Generic;
using System.Linq;

namespace MentosMailCore
{
    public class TemplateService : IDisposable
    {
        private string _templateBase { get; set; }

        public TemplateService(string templateOrPathFile)
        {
            _templateBase = System.IO.File.Exists(templateOrPathFile)
                ? GetTemplateFromFile(templateOrPathFile)
                : templateOrPathFile;
        }

        public TemplateService(Uri uri)
        {
            _templateBase = GetTemplateFromUrl(uri);
        }

        public string GenerateTemplateFromViewModel(object model, bool ignoreErrors = true)
        {
            var templateFinal = _templateBase.ToString();

            var properties = model.GetType().GetProperties()
                .Where(v => v.IsDefined(typeof(AttributesHelper.SenderFieldInMailAttribute), false));

            foreach (var props in properties)
            {
                try
                {
                    var valueField = model.GetType().GetProperty(props.Name).GetValue(model, null);

                    var valueReplace = $"[{props.Name}]";

                    var custons = props.GetCustomAttributes(false);
                    try
                    {
                        var fieldCustom = 
                            (AttributesHelper.SenderFieldInMailAttribute)custons.FirstOrDefault(m => m.GetType() == typeof(AttributesHelper.SenderFieldInMailAttribute));

                        if (!string.IsNullOrEmpty(fieldCustom?.FieldReplace))
                        {
                            valueReplace = fieldCustom.FieldReplace;
                        }
                    }
                    catch (Exception)
                    {
                        if (!ignoreErrors)
                        {
                            throw;
                        }
                    }
                    templateFinal = templateFinal.Replace(valueReplace, valueField?.ToString() ?? string.Empty);
                }
                catch (Exception ex)
                {
                    if (!ignoreErrors) throw;
                }
            }
            return templateFinal;
        }

        public string GenerateTemplateFromAnonymous(dynamic model)
        {
            object internalModel = (object) model;
            Dictionary<string, object> properties = internalModel.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(internalModel));
            var templateFinal = _templateBase;

            foreach (var prop in properties)
            {
                var fieldReplace = $"[{prop.Key}]";
                var valueField = prop.Value.ToString();

                templateFinal = templateFinal.Replace(fieldReplace, valueField);
            }
            return templateFinal;
        }

        private static string GetTemplateFromFile(string pathFile)
        {
            return System.IO.File.ReadAllText(pathFile);
        }

        private string GetTemplateFromUrl(Uri uri)
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                return wc.DownloadString(uri);
            }
        }

        public void Dispose()
        {
            _templateBase = null;
        }
    }
}