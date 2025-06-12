namespace UnionTherapy.Application.Constants;

/// <summary>
/// Çok dilli mesajlar için anahtar sabitleri
/// </summary>
public static class ResponseMessages
{
    // Authentication Messages
    public const string InvalidToken = "InvalidToken";
    public const string TokenExpired = "TokenExpired";
    public const string InvalidCredentials = "InvalidCredentials";
    public const string UserNotFound = "UserNotFound";
    public const string EmailAlreadyExists = "EmailAlreadyExists";
    public const string LoginSuccessful = "LoginSuccessful";
    public const string RegisterSuccessful = "RegisterSuccessful";
    public const string LogoutSuccessful = "LogoutSuccessful";
    public const string UnauthorizedAccess = "UnauthorizedAccess";
    public const string TokenNotFound = "TokenNotFound";
    public const string InvalidTokenAlgorithm = "InvalidTokenAlgorithm";
    public const string TokenSecurityError = "TokenSecurityError";
    public const string InvalidRefreshToken = "InvalidRefreshToken";
    public const string RefreshTokenExpired = "RefreshTokenExpired";
    public const string InvalidTokenFormat = "InvalidTokenFormat";
    public const string JwtConfigurationNotFound = "JwtConfigurationNotFound";
    
    // Validation Messages
    public const string ValidationFailed = "ValidationFailed";
    public const string RequiredField = "RequiredField";
    public const string InvalidEmail = "InvalidEmail";
    public const string InvalidPhoneNumber = "InvalidPhoneNumber";
    public const string PasswordTooWeak = "PasswordTooWeak";
    public const string InvalidDateFormat = "InvalidDateFormat";
    public const string PasswordCannotBeEmpty = "PasswordCannotBeEmpty";
    public const string InvalidPasswordHashLength = "InvalidPasswordHashLength";
    public const string InvalidPasswordSaltLength = "InvalidPasswordSaltLength";
    public const string HashedPasswordCannotBeEmpty = "HashedPasswordCannotBeEmpty";
    
    // Business Logic Messages
    public const string OperationSuccessful = "OperationSuccessful";
    public const string OperationFailed = "OperationFailed";
    public const string RecordNotFound = "RecordNotFound";
    public const string RecordAlreadyExists = "RecordAlreadyExists";
    public const string InsufficientPermissions = "InsufficientPermissions";
    
    // System Messages
    public const string DatabaseError = "DatabaseError";
    public const string UnexpectedError = "UnexpectedError";
    public const string ServiceUnavailable = "ServiceUnavailable";
    public const string MaintenanceMode = "MaintenanceMode";
    
    // Session Messages
    public const string SessionNotFound = "SessionNotFound";
    public const string SessionFull = "SessionFull";
    public const string SessionCancelled = "SessionCancelled";
    public const string SessionCompleted = "SessionCompleted";
    
    // Payment Messages
    public const string PaymentSuccessful = "PaymentSuccessful";
    public const string PaymentFailed = "PaymentFailed";
    public const string InsufficientFunds = "InsufficientFunds";
    public const string PaymentNotFound = "PaymentNotFound";
} 