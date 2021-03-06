﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

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
        
        /// <summary>
        /// Replace template based in model decored with attribute 'SenderfieldInMail'. The base pattern to replace is [propertie] but is changed with attribute
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ignoreErrors"></param>
        /// <returns></returns>
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
                catch (Exception)
                {
                    if (!ignoreErrors) throw;
                }
            }
            return templateFinal;
        }
        
        /// <summary>
        /// Generate template based in Anonymous object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get template from a specified file
        /// </summary>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        private static string GetTemplateFromFile(string pathFile)
        {
            return System.IO.File.ReadAllText(pathFile);
        }

        /// <summary>
        /// Get template from a specified uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
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