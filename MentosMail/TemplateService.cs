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
        private string HtmlFinal { get; set; }

        private string _templateBase { get; set; }
        private object _model { get; set; }

        public TemplateService(object model, string templateBase)
        {
            _templateBase = templateBase;
            _model = model;
        }

        public string GenerateTemplate(bool ignoreErrors=true)
        {
            var templateFinal = _templateBase;

            var properties = _model.GetType().GetProperties()
                .Where(v => v.IsDefined(typeof(SenderFieldInMailAttribute), false));

            foreach (var prop in properties)
            {
                try
                {
                    var valueField = _model.GetType().GetProperty(prop.Name).GetValue(_model, null);

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

        public void Dispose()
        {
            _templateBase = null;
            _model = null;
        }
    }
}
