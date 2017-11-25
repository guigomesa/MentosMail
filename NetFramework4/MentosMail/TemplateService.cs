using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MentosMail.AttributesHelper;
using System.Web;


namespace MentosMail
{
    public class TemplateService : IDisposable
    {
        private string _templateBase { get; set; }

        /// <summary>
        /// Construct class based in template or path of file for template
        /// </summary>
        /// <param name="template">Path or template</param>
        public TemplateService(string template)
        {
            _templateBase = System.IO.File.Exists(template) ? GetTemplateFromFile(template) : template;
        }

        /// <summary>
        /// Construct class based in Uri for template
        /// </summary>
        /// <param name="uri">Uri to path template</param>
        public TemplateService(Uri uri)
        {
            _templateBase = GetTemplateFromUrl(uri);
        }

        /// <summary>
        /// Replace template based in model decored with attribute 'SenderfieldInMail'. The base pattern to replace is [propertie] but is changed with attribute
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ignoreErrors"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Replace template based in dynamic model.
        /// The replace pattern is [propertie-name]
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GenerateTemplateFromAnonymous(dynamic model)
        {
            object internalModel = (object) model;
            Dictionary<string, object> properties = internalModel.GetType()
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


        /// <summary>
        /// Retry template from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetTemplateFromFile(string path)
        {
            var content = System.IO.File.ReadAllText(path);
            return content;
        }

        /// <summary>
        /// Retry template from Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string GetTemplateFromUrl(Uri uri)
        {
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                var content = wc.DownloadString(uri.ToString());
                return content;
            }
        }

        public void Dispose()
        {
            _templateBase = null;
        }
    }
}
