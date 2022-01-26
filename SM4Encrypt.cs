using Org.BouncyCastle.Utilities.Encoders;
using SM4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace youxiaoxing
{
    public class SM4Encrypt
    {
        /// 加密ECB模式
        ///
        /// 密钥 /// 明文是否是十六进制 /// 明文 /// 返回密文
        public static string Encrypt_ECB_SM4(String secretKey, bool hexString, string plainText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_ENCRYPT;
            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }
            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(plainText));
            String cipherText = BitConverter.ToString(encrypted).Replace("-", string.Empty); 
            return cipherText;
        }
        ///
        /// 解密ECB模式
        ///
        /// 密钥 /// 明文是否是十六进制 /// 密文 /// 返回明文
        public static String Decrypt_ECB_SM4(String secretKey, bool hexString, string cipherText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_DECRYPT;
            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }
            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
            String plainText = BitConverter.ToString(decrypted).Replace("-", string.Empty);
            return plainText;
        }
        public static String Encrypt_CBC_SM4(String secretKey, bool hexString, string iv, string plainText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_ENCRYPT;
            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Convert.FromBase64String(secretKey);
                ivBytes = Convert.FromBase64String(iv);
            }
            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes,Hex.Decode(plainText));
            String cipherText = BitConverter.ToString(encrypted).Replace("-", string.Empty); ;
           
            return cipherText;
        }

        public static String Decrypt_CBC_SM4(String secretKey, bool hexString, string iv, String cipherText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4_calss.SM4_DECRYPT;
            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }
            SM4_calss sm4 = new SM4_calss();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
            return Encoding.Default.GetString(decrypted);
        }
    }
}