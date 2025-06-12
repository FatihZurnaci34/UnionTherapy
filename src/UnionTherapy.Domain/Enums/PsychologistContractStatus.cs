namespace UnionTherapy.Domain.Enums;

public enum PsychologistContractStatus
{
    Draft,          // Taslak
    Pending,        // Onay Bekliyor
    Active,         // Aktif
    Suspended,      // Askıya Alınmış
    Terminated,     // Feshedilmiş
    Expired,        // Süresi Dolmuş
    Renewed         // Yenilenmiş
} 