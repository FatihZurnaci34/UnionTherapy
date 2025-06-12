namespace UnionTherapy.Application.Services.Localization;

public interface ILocalizationService
{
    /// <summary>
    /// Belirtilen anahtar için çevrilmiş mesajı döndürür
    /// </summary>
    /// <param name="key">Mesaj anahtarı</param>
    /// <param name="culture">Dil kodu (opsiyonel, null ise mevcut culture kullanılır)</param>
    /// <returns>Çevrilmiş mesaj</returns>
    string GetLocalizedString(string key, string? culture = null);

    /// <summary>
    /// Parametreli mesaj için çevrilmiş metni döndürür
    /// </summary>
    /// <param name="key">Mesaj anahtarı</param>
    /// <param name="parameters">Mesaj parametreleri</param>
    /// <param name="culture">Dil kodu (opsiyonel)</param>
    /// <returns>Çevrilmiş ve formatlanmış mesaj</returns>
    string GetLocalizedString(string key, object[] parameters, string? culture = null);

    /// <summary>
    /// Mevcut culture'ı döndürür
    /// </summary>
    string GetCurrentCulture();

    /// <summary>
    /// Desteklenen dilleri döndürür
    /// </summary>
    IEnumerable<string> GetSupportedCultures();
} 