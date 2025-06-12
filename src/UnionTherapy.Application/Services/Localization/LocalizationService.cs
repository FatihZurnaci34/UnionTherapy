using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text.Json;

namespace UnionTherapy.Application.Services.Localization;

public class LocalizationService : ILocalizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Dictionary<string, Dictionary<string, string>> _localizedStrings;
    private readonly string[] _supportedCultures = { "tr", "en" };
    private const string DefaultCulture = "tr";

    public LocalizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _localizedStrings = new Dictionary<string, Dictionary<string, string>>();
        LoadLocalizedStrings();
    }

    public string GetLocalizedString(string key, string? culture = null)
    {
        culture ??= GetCurrentCulture();
        
        if (_localizedStrings.TryGetValue(culture, out var cultureStrings) &&
            cultureStrings.TryGetValue(key, out var localizedString))
        {
            return localizedString;
        }

        // Fallback to default culture
        if (culture != DefaultCulture &&
            _localizedStrings.TryGetValue(DefaultCulture, out var defaultCultureStrings) &&
            defaultCultureStrings.TryGetValue(key, out var defaultString))
        {
            return defaultString;
        }

        // Return key if not found
        return key;
    }

    public string GetLocalizedString(string key, object[] parameters, string? culture = null)
    {
        var template = GetLocalizedString(key, culture);
        
        try
        {
            return string.Format(template, parameters);
        }
        catch
        {
            return template;
        }
    }

    public string GetCurrentCulture()
    {
        // 1. Custom header kontrolü (X-Language) - En yüksek öncelik
        var customLang = _httpContextAccessor.HttpContext?.Request.Headers["X-Language"].FirstOrDefault();
        if (!string.IsNullOrEmpty(customLang) && _supportedCultures.Contains(customLang.ToLower()))
        {
            return customLang.ToLower();
        }

        // 2. Query parameter kontrolü (?lang=tr)
        var langParam = _httpContextAccessor.HttpContext?.Request.Query["lang"].FirstOrDefault();
        if (!string.IsNullOrEmpty(langParam) && _supportedCultures.Contains(langParam.ToLower()))
        {
            return langParam.ToLower();
        }

        // 3. Header'dan Accept-Language kontrolü
        var acceptLanguage = _httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].FirstOrDefault();
        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            var preferredLanguage = acceptLanguage.Split(',')[0].Split('-')[0].ToLower();
            if (_supportedCultures.Contains(preferredLanguage))
            {
                return preferredLanguage;
            }
        }

        // 4. Thread culture kontrolü
        var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
        if (_supportedCultures.Contains(currentCulture))
        {
            return currentCulture;
        }

        return DefaultCulture;
    }

    public IEnumerable<string> GetSupportedCultures()
    {
        return _supportedCultures;
    }

    private void LoadLocalizedStrings()
    {
        foreach (var culture in _supportedCultures)
        {
            try
            {
                var resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                    "Resources", $"Messages.{culture}.json");
                
                if (File.Exists(resourcePath))
                {
                    var jsonContent = File.ReadAllText(resourcePath);
                    var strings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);
                    
                    if (strings != null)
                    {
                        _localizedStrings[culture] = strings;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but continue
                Console.WriteLine($"Error loading localization file for culture {culture}: {ex.Message}");
            }
        }
    }
} 