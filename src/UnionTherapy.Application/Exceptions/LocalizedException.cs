using UnionTherapy.Application.Constants;

namespace UnionTherapy.Application.Exceptions;

/// <summary>
/// Çok dilli hata mesajları için özel exception sınıfı
/// </summary>
public class LocalizedException : Exception
{
    public string MessageKey { get; }
    public object[]? Parameters { get; }

    public LocalizedException(string messageKey) : base(messageKey)
    {
        MessageKey = messageKey;
        Parameters = null;
    }

    public LocalizedException(string messageKey, params object[] parameters) : base(messageKey)
    {
        MessageKey = messageKey;
        Parameters = parameters;
    }

    public LocalizedException(string messageKey, Exception innerException) : base(messageKey, innerException)
    {
        MessageKey = messageKey;
        Parameters = null;
    }

    public LocalizedException(string messageKey, Exception innerException, params object[] parameters) 
        : base(messageKey, innerException)
    {
        MessageKey = messageKey;
        Parameters = parameters;
    }
}

/// <summary>
/// Çok dilli business exception
/// </summary>
public class LocalizedBusinessException : LocalizedException
{
    public LocalizedBusinessException(string messageKey) : base(messageKey) { }
    public LocalizedBusinessException(string messageKey, params object[] parameters) : base(messageKey, parameters) { }
    public LocalizedBusinessException(string messageKey, Exception innerException) : base(messageKey, innerException) { }
}

/// <summary>
/// Çok dilli validation exception
/// </summary>
public class LocalizedValidationException : LocalizedException
{
    public LocalizedValidationException(string messageKey) : base(messageKey) { }
    public LocalizedValidationException(string messageKey, params object[] parameters) : base(messageKey, parameters) { }
    public LocalizedValidationException(string messageKey, Exception innerException) : base(messageKey, innerException) { }
}

/// <summary>
/// Çok dilli not found exception
/// </summary>
public class LocalizedNotFoundException : LocalizedException
{
    public LocalizedNotFoundException(string messageKey) : base(messageKey) { }
    public LocalizedNotFoundException(string messageKey, params object[] parameters) : base(messageKey, parameters) { }
    public LocalizedNotFoundException(string messageKey, Exception innerException) : base(messageKey, innerException) { }
} 