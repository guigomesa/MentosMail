using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MentosMail.AttributesHelper;

namespace MentosMail
{
    public class TemplateService : IDisposable
    {
        private string _templateBase { get; set; }

        public TemplateService(string templateBase)
        {
            _templateBase = templateBase;
            
        }

        public string GenerateTemplateFromViewModel(object model,bool ignoreErrors=true)
        {
            var templateFinal = _templateBase;

            var properties = model.GetType().GetProperties()
                .Where(v => v.IsDefined(typeof(SenderFieldInMailAttribute), false));

            foreach (var prop in properties)
            {
                try
                {
                    var valueField = model.GetType().GetProperty(prop.Name).GetValue(model, null);

                    var valueReplace = $"[{prop.Name}]";

                    var custons = prop.GetCustomAttributes(false);
                    try
                    {
                        var fieldCustom = (SenderFieldInMailAttribute)custons.FirstOrDefault(m => m.GetType() == typeof(SenderFieldInMailAttribute));

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
                    if (ignoreErrors)
                    {
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return templateFinal;
        }

        public string GenerateTemplateFromAnonymous(object model)
        {
            Dictionary<string, object> properties = model.GetType()
                                     .GetProperties()
                                     .ToDictionary(p => p.Name, p => p.GetValue(model));
            var templateFinal = _templateBase;

            foreach (var prop in properties)
            {
                var fieldReplace = $"[{prop.Key}]";
                var valueField = prop.Value.ToString();

                templateFinal = templateFinal.Replace(fieldReplace, valueField);
            }
            return templateFinal;
        }

        public static TemplateService GenerateTemplateServiceFromFile(string path)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public static TemplateService GenerateTemplateServiceFromUrl(string url)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public void Dispose()
        {
            _templateBase = null;
        }
    }
}
