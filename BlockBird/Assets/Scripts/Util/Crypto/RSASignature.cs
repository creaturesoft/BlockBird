using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class RSASignature
{
    public static string SignData(string data)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] hash;

        byte[] modulus = { 0x00, 0xA8, 0x51, 0x49, 0x8D, 0x54, 0xFD, 0x54, 0x0D, 0xC9, 0x3D, 0x74, 0xF1, 0x97, 0xBF, 0x75, 0x53, 0x72, 0x28, 0x79, 0x20, 0xB7, 0x56, 0x5D, 0x1F, 0xE5, 0xEF, 0xF1, 0x14, 0xDF, 0x06, 0x94, 0xF7, 0x8B, 0x7E, 0x0B, 0xFC, 0xDC, 0x47, 0x30, 0x26, 0x16, 0x8A, 0xE0, 0x09, 0xCD, 0x0A, 0xD8, 0x72, 0x14, 0x58, 0x0E, 0x5E, 0x16, 0xCD, 0xF8, 0xBD, 0xB2, 0xAC, 0x3D, 0xA3, 0x4A, 0x06, 0xA3, 0xCF, 0x74, 0x34, 0x19, 0x08, 0x71, 0x18, 0x78, 0xA0, 0x3A, 0x5F, 0x3C, 0x35, 0xEA, 0xE5, 0x23, 0xA5, 0x7C, 0x00, 0xE2, 0x2D, 0x2B, 0x1D, 0xD4, 0xAF, 0xEF, 0x04, 0x57, 0xD0, 0xA8, 0x7F, 0x9B, 0x7C, 0xC5, 0x05, 0x9E, 0x7A, 0x65, 0x66, 0x70, 0xB9, 0xAB, 0xD5, 0x2B, 0x21, 0x5A, 0xFC, 0x28, 0xC8, 0x86, 0x82, 0x75, 0x08, 0x72, 0x88, 0x7E, 0x13, 0xB1, 0x45, 0x55, 0x72, 0x2B, 0xF5, 0xCA, 0xC4, 0xBC, 0x26, 0xF5, 0xDE, 0x88, 0x9E, 0x50, 0x8C, 0xB0, 0xD1, 0xAA, 0x0B, 0x65, 0xB9, 0x57, 0xEF, 0x0D, 0x0A, 0xF5, 0xB7, 0x30, 0xC4, 0x92, 0xE5, 0xD4, 0xC5, 0xC3, 0x1C, 0xC5, 0x0B, 0xE4, 0xA8, 0x82, 0x69, 0x4A, 0xFF, 0xE6, 0xB6, 0x31, 0xE7, 0x1C, 0x0C, 0x95, 0x37, 0xAE, 0x6F, 0xA4, 0xB4, 0x9E, 0x37, 0xB7, 0xD5, 0x4C, 0x87, 0x89, 0x25, 0xFD, 0xCD, 0xA5, 0x66, 0x3C, 0x32, 0x37, 0xB5, 0xA3, 0x1E, 0xDB, 0xFD, 0x3C, 0x00, 0x71, 0xA0, 0x23, 0xC6, 0x4D, 0x0E, 0x0B, 0xB6, 0x80, 0x8C, 0xC7, 0x96, 0xE2, 0xDD, 0xA8, 0xE2, 0xD0, 0x6C, 0x8A, 0x60, 0x71, 0xBF, 0x01, 0xEA, 0x30, 0x6C, 0x82, 0xE4, 0x8E, 0x30, 0xB8, 0x04, 0x64, 0x44, 0xFB, 0x5B, 0x75, 0xAA, 0x25, 0x11, 0x10, 0x35, 0xB3, 0x58, 0xED, 0xF6, 0xB6, 0x9B, 0x3F, 0x60, 0x71, 0x4C, 0xAC, 0xE6, 0x66, 0x3B, 0x36, 0x9B };
        byte[] exponent = { 0x01, 0x00, 0x01 };
        byte[] D = { 0x44, 0x0B, 0x9C, 0xB7, 0x18, 0xD1, 0x53, 0x2E, 0x41, 0x99, 0x69, 0x52, 0x14, 0x1D, 0x70, 0x32, 0x9C, 0x77, 0x95, 0x44, 0x6F, 0x29, 0xE8, 0x7A, 0xF1, 0x4B, 0xB7, 0xC4, 0x4E, 0xC7, 0x8B, 0xE8, 0xA9, 0x89, 0x7B, 0x12, 0x2E, 0x01, 0x4F, 0x8B, 0x4E, 0x58, 0xB5, 0x6A, 0xF0, 0xEC, 0x3E, 0x05, 0x9C, 0x88, 0xDE, 0xC2, 0x15, 0xE9, 0x0D, 0xF2, 0xAE, 0xAD, 0x68, 0x3B, 0xBF, 0xBD, 0x00, 0x73, 0x69, 0x50, 0x78, 0x80, 0x4B, 0xAC, 0x6F, 0x73, 0xA3, 0x35, 0x86, 0x13, 0x6C, 0x13, 0x57, 0x7E, 0x5A, 0xAB, 0xC6, 0xC4, 0x28, 0xE6, 0xA5, 0xE9, 0xBC, 0x30, 0x9E, 0xC9, 0xB9, 0x9C, 0xE1, 0x00, 0xA7, 0x6B, 0xF6, 0x9E, 0x17, 0xA9, 0x3A, 0xD0, 0x2D, 0x12, 0x00, 0x1E, 0x3B, 0x78, 0xAE, 0x8A, 0x26, 0xAA, 0xCD, 0xE2, 0x6C, 0xDF, 0x16, 0x4D, 0x22, 0xC0, 0xDB, 0x62, 0xCD, 0x37, 0xD0, 0x51, 0x60, 0x8B, 0x9D, 0x68, 0x45, 0x6E, 0x1B, 0xE0, 0x98, 0x68, 0xF4, 0x52, 0xD1, 0x7D, 0x0D, 0x73, 0x4F, 0x78, 0x0D, 0xEE, 0x3E, 0x7B, 0xF6, 0x3C, 0xBD, 0xBA, 0xD8, 0xA7, 0x04, 0x70, 0xFD, 0x95, 0x44, 0x13, 0xC3, 0xE8, 0xDD, 0x43, 0xB7, 0xA0, 0xBA, 0x04, 0xE6, 0xC7, 0x52, 0xD9, 0x49, 0x58, 0x60, 0x93, 0xB3, 0x8C, 0xC1, 0x3C, 0xBB, 0x2C, 0x09, 0xC3, 0x53, 0xD1, 0x30, 0x70, 0x02, 0x74, 0x29, 0x54, 0x6D, 0xC3, 0xFB, 0x73, 0xA9, 0x2E, 0xBE, 0xC2, 0x58, 0xDA, 0x77, 0x30, 0x5F, 0x4B, 0x8D, 0xB4, 0x99, 0xAE, 0xA4, 0x47, 0x6F, 0xEA, 0xE7, 0x2D, 0xD9, 0xAF, 0x7F, 0xFB, 0x13, 0x05, 0x55, 0xE1, 0xE6, 0x5A, 0x35, 0xD4, 0x3D, 0xFD, 0x9E, 0xF5, 0x6B, 0x0F, 0xA2, 0xCD, 0xCB, 0xA2, 0x72, 0xB0, 0xC4, 0xEC, 0x49, 0x00, 0x03, 0x2D, 0x9A, 0x8F, 0x08, 0x4E, 0x44, 0x0D, 0x26, 0x51 };
        byte[] P = { 0x00, 0xEA, 0x1B, 0xA1, 0x00, 0xA8, 0x66, 0x23, 0x24, 0xEE, 0xC8, 0xC6, 0xA7, 0x42, 0xEB, 0x7D, 0x10, 0x52, 0xED, 0xA1, 0x12, 0xD7, 0x2D, 0x87, 0x18, 0x11, 0x10, 0x30, 0x91, 0x87, 0x47, 0xC4, 0x8A, 0x5A, 0x61, 0xBC, 0xB4, 0x1E, 0x61, 0xAB, 0xAE, 0x3D, 0x7E, 0x74, 0x32, 0x26, 0x05, 0x4D, 0x9F, 0x80, 0xCE, 0xF2, 0xB0, 0xAB, 0xC1, 0xF6, 0xDE, 0x7E, 0x8D, 0x64, 0xE0, 0xB0, 0xCB, 0x3D, 0x4F, 0xBF, 0x07, 0xF4, 0xC9, 0x14, 0x29, 0xE0, 0xA5, 0x60, 0x96, 0x03, 0xCE, 0x02, 0x6E, 0xDA, 0x1F, 0x31, 0xDD, 0x03, 0xF9, 0xE4, 0xC2, 0x32, 0x56, 0x0A, 0x70, 0xFB, 0x4C, 0x3B, 0xCA, 0x50, 0xA7, 0xCE, 0x29, 0x89, 0x0E, 0x6D, 0xDC, 0xAC, 0x21, 0xC0, 0x60, 0x69, 0x9B, 0xDF, 0x80, 0x58, 0x7A, 0xE8, 0xFA, 0x57, 0x3E, 0x67, 0x72, 0xF1, 0xED, 0x7A, 0x3F, 0x1B, 0xE1, 0xDB, 0xC4, 0xD3, 0x2B };
        byte[] Q = { 0x00, 0xB8, 0x0E, 0xAF, 0x50, 0x02, 0x6E, 0x26, 0x01, 0xBC, 0x22, 0xA6, 0x96, 0x85, 0xB3, 0x29, 0xC8, 0xB6, 0x48, 0xE4, 0xFB, 0x09, 0x6F, 0x09, 0x70, 0x79, 0x78, 0xEA, 0xFF, 0xDF, 0xFF, 0x58, 0x91, 0x5A, 0xC5, 0x6E, 0x73, 0xCF, 0xE6, 0x5D, 0x51, 0x26, 0x45, 0xE2, 0xA3, 0x9B, 0xB4, 0x8C, 0xDB, 0x5D, 0x12, 0xED, 0xA2, 0x3B, 0x3C, 0x18, 0x8C, 0xCB, 0xDB, 0x0C, 0x26, 0xC1, 0x0C, 0xFB, 0xD6, 0x07, 0xA4, 0x2E, 0x45, 0x7B, 0xA5, 0xF5, 0x5A, 0x70, 0x9F, 0x67, 0x06, 0xCE, 0x38, 0x5A, 0xCE, 0x5F, 0x79, 0xA1, 0xA3, 0x35, 0x10, 0xBE, 0xC3, 0x45, 0x99, 0xE4, 0xD8, 0xEB, 0x0E, 0xE9, 0x69, 0x8D, 0x4F, 0x82, 0x60, 0x6E, 0x78, 0xBF, 0xF0, 0xFA, 0x11, 0x81, 0xD2, 0x02, 0x3E, 0x49, 0xDB, 0x39, 0x57, 0x00, 0xAF, 0xD1, 0x14, 0x6C, 0xDF, 0xF6, 0xCF, 0x63, 0xEE, 0x20, 0x22, 0x32, 0x51 };
        byte[] DP = { 0x00, 0xA7, 0x43, 0x26, 0x3D, 0x3A, 0x13, 0xFF, 0x78, 0x1B, 0xC3, 0x07, 0x6B, 0xE9, 0xBC, 0x26, 0x96, 0xCB, 0x29, 0x4C, 0xB4, 0x11, 0x59, 0x4D, 0xF4, 0x3B, 0xFC, 0xBD, 0x36, 0xBC, 0xD8, 0xE1, 0xEB, 0x97, 0xB7, 0xCD, 0x03, 0x43, 0xD1, 0xB4, 0xBF, 0xC0, 0xDF, 0xE0, 0x55, 0x14, 0x25, 0x25, 0xD8, 0x98, 0x47, 0x43, 0xCE, 0x46, 0x69, 0x46, 0xE0, 0xA0, 0xBA, 0x95, 0x20, 0x94, 0x30, 0x21, 0x96, 0x20, 0x7E, 0xA6, 0xBE, 0x23, 0xD1, 0xE7, 0xD7, 0x40, 0xB2, 0xED, 0xF4, 0xFA, 0x78, 0x09, 0x0F, 0xD0, 0xA8, 0x80, 0x76, 0xC2, 0xA2, 0x9D, 0x24, 0x2C, 0x41, 0x9C, 0xCA, 0x52, 0xCD, 0xB1, 0xE2, 0xB2, 0xC1, 0xAA, 0x52, 0xCC, 0xDB, 0xA5, 0x80, 0x57, 0xDA, 0x8A, 0x99, 0xFB, 0x8D, 0xCF, 0xA9, 0xC1, 0x5B, 0x5B, 0x8A, 0x12, 0x8F, 0x65, 0x9A, 0xAE, 0x84, 0xF8, 0x7E, 0xDE, 0x6E, 0x3A, 0x11 };
        byte[] DQ = { 0x3F, 0x93, 0xE2, 0xFE, 0xF2, 0x37, 0xF6, 0x2C, 0xF7, 0x3D, 0xC8, 0xE9, 0x89, 0xB9, 0x7F, 0x9F, 0x73, 0x47, 0xEE, 0xC0, 0xC0, 0x5B, 0x78, 0x99, 0x3F, 0x7C, 0x83, 0x40, 0x6B, 0xB1, 0x9A, 0x78, 0x6B, 0x30, 0x73, 0x9C, 0xD1, 0x9D, 0xB3, 0x72, 0x4A, 0x94, 0x2D, 0x5B, 0x72, 0x77, 0x85, 0x88, 0x68, 0xB8, 0x17, 0x19, 0xC8, 0xF8, 0x53, 0x4A, 0x9F, 0x48, 0x45, 0x04, 0x45, 0xFF, 0x24, 0x26, 0xA4, 0x71, 0x14, 0x02, 0xB0, 0x59, 0x7D, 0x4D, 0x06, 0x46, 0x29, 0xA2, 0x72, 0x2D, 0x89, 0x40, 0x6C, 0x3E, 0x69, 0x95, 0x24, 0xC9, 0x69, 0xFB, 0xAD, 0xD9, 0x20, 0xF1, 0xC5, 0x10, 0x5B, 0x94, 0x38, 0x59, 0xD4, 0xA1, 0x56, 0xC7, 0xA3, 0x15, 0xAC, 0x6B, 0xCB, 0xBA, 0x2D, 0x48, 0x32, 0xDE, 0xE5, 0x09, 0xA6, 0x95, 0x14, 0xD5, 0xC7, 0x5D, 0xFD, 0xB5, 0x59, 0xC1, 0x71, 0x9C, 0x1E, 0x61 };
        byte[] InverseQ = { 0x79, 0x81, 0x63, 0x67, 0x01, 0x52, 0xFB, 0xFA, 0xB7, 0x5A, 0x6B, 0x21, 0x3F, 0x3B, 0xB8, 0xA0, 0x92, 0xB3, 0xC6, 0xC1, 0x94, 0xE6, 0x43, 0x96, 0xEA, 0xAC, 0x87, 0x77, 0xC4, 0x06, 0x66, 0xF6, 0x59, 0x48, 0x4C, 0x41, 0xD1, 0x1B, 0x97, 0xD5, 0x7C, 0xD8, 0x29, 0xA1, 0x62, 0xEB, 0xB5, 0xEA, 0x23, 0x91, 0x2E, 0xC7, 0x64, 0x00, 0xF4, 0x8A, 0xAB, 0xDA, 0x19, 0x9E, 0x23, 0x64, 0x89, 0x77, 0x58, 0x25, 0xCA, 0xE6, 0x7F, 0x47, 0xE6, 0x8B, 0x50, 0xB0, 0x80, 0xC5, 0xF1, 0x3C, 0x93, 0x3F, 0x55, 0x8E, 0x36, 0xFB, 0x4B, 0x78, 0xB4, 0x3D, 0xF2, 0x8C, 0xDD, 0x26, 0x5C, 0x25, 0x9D, 0x86, 0x97, 0x30, 0xFB, 0xCD, 0xA0, 0x03, 0xEE, 0xDA, 0xA2, 0xB7, 0xDE, 0x4D, 0xCC, 0x27, 0x49, 0x45, 0x85, 0xCC, 0x13, 0xBC, 0xF7, 0x03, 0xF9, 0x3A, 0x72, 0x79, 0x77, 0x3D, 0x87, 0x49, 0xD2, 0x03 };

        RSAParameters parameters = new RSAParameters();
        parameters.Modulus = modulus;
        parameters.Exponent = exponent;
        parameters.D = D;
        parameters.P = P;
        parameters.Q = Q;
        parameters.DP = DP;
        parameters.DQ = DQ;
        parameters.InverseQ = InverseQ;

        using (SHA256 sha256 = SHA256.Create())
        {
            hash = sha256.ComputeHash(dataBytes);
        }

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(parameters);
            RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));

            return Convert.ToBase64String(rsaFormatter.CreateSignature(hash)); // 서명을 Base64로 인코딩하여 반환
        }
    }

    public static bool VerifyData(string data, string signature)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] hash;

        RSAParameters parameters = new RSAParameters();

        byte[] modulus = { 0x00, 0x88, 0x60, 0xE2, 0x50, 0xB0, 0x17, 0x11, 0x7E, 0xA3, 0xFC, 0x58, 0x03, 0x60, 0x34, 0x2D, 0x80, 0xE5, 0xA6, 0x53, 0x6D, 0x4A, 0x1D, 0x03, 0xE9, 0xA1, 0x93, 0x40, 0x8C, 0xC7, 0x20, 0x74, 0xD3, 0x82, 0x50, 0xE9, 0x75, 0x2F, 0xCF, 0x18, 0x43, 0x11, 0x49, 0x7E, 0xD7, 0x05, 0xDB, 0x5C, 0xCA, 0xB9, 0x6E, 0x90, 0x88, 0x9C, 0xFA, 0x5B, 0xE5, 0x43, 0x6D, 0x09, 0xD8, 0x5A, 0x96, 0x1B, 0xC3, 0x70, 0x3B, 0x37, 0x87, 0x26, 0x64, 0x67, 0x5F, 0xA0, 0xD4, 0x8A, 0xD3, 0xD3, 0x1C, 0x4C, 0x19, 0xE7, 0xB1, 0x0A, 0x89, 0x2E, 0x86, 0x33, 0xAD, 0xBD, 0x3A, 0xE4, 0x40, 0xED, 0x0D, 0x1A, 0xA6, 0x15, 0x5B, 0x43, 0xFC, 0xDF, 0xBB, 0x80, 0xDB, 0x28, 0x40, 0x11, 0x1C, 0x66, 0xC5, 0x37, 0xCF, 0x56, 0x61, 0x6A, 0xC3, 0xBB, 0x6C, 0xB1, 0x4C, 0x1F, 0xA8, 0x61, 0xEA, 0xF7, 0x1E, 0xA6, 0xE3, 0x7A, 0x07, 0x52, 0x79, 0xC1, 0x71, 0x26, 0xA6, 0x86, 0xC0, 0xBD, 0x42, 0xEC, 0xBD, 0xF4, 0x64, 0xAD, 0x9A, 0x01, 0x23, 0x23, 0x0B, 0x5D, 0xAF, 0xB1, 0x31, 0x01, 0x20, 0x45, 0xD1, 0x82, 0x5F, 0xA7, 0x4B, 0x94, 0x95, 0x67, 0x5D, 0x81, 0x23, 0x9E, 0xFB, 0x99, 0xB2, 0x00, 0x22, 0x0E, 0x9D, 0x07, 0xA1, 0x85, 0x1C, 0xD1, 0x78, 0x95, 0xDC, 0xEC, 0x0D, 0x50, 0x16, 0xC3, 0x81, 0xE6, 0x56, 0xCF, 0x27, 0xEC, 0x50, 0x1A, 0xAE, 0x0F, 0x73, 0xA5, 0xB8, 0xCB, 0x90, 0x7E, 0x30, 0x7C, 0x9D, 0x6A, 0xD9, 0x35, 0xF9, 0xBA, 0x98, 0x5E, 0xF7, 0xBF, 0x75, 0x76, 0x1F, 0xCF, 0x93, 0x85, 0xCA, 0x6F, 0x28, 0x1C, 0xAA, 0xD0, 0x9E, 0x38, 0xEB, 0xFF, 0xD3, 0x8B, 0x46, 0x20, 0xED, 0x2B, 0xBB, 0x8A, 0x0A, 0x82, 0x60, 0xD2, 0xCF, 0x0C, 0x36, 0xF8, 0xBA, 0xBB, 0x0A, 0xCD, 0xC0, 0x29, 0x9D };
        byte[] exponent = { 0x01, 0x00, 0x01 };

        parameters.Modulus = modulus;
        parameters.Exponent = exponent;


        using (SHA256 sha256 = SHA256.Create())
        {
            hash = sha256.ComputeHash(dataBytes);
        }

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(parameters);

            // 서명 검증을 위한 객체 생성
            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm(nameof(SHA256));

            // 서명 검증
            return rsaDeformatter.VerifySignature(hash, Convert.FromBase64String(signature));
        }
    }
}