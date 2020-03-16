using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IssueCreator
{
    public class Settings
    {
        private static readonly byte[] s_entropy = { 23, 61, 24, 8, 77, 52 }; //the entropy

        public List<string> Repositories { get; set; } = new List<string>();
        public string ZenHubToken { get; set; } = string.Empty;

        public string GitHubToken { get; set; } = string.Empty;

        public string DefaultTitle { get; set; } = string.Empty;

        public List<string> DefaultLabels { get; set; } = new List<string>();

        public void Serialize(string file)
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                Settings encrypted = EncryptSettings(this);
                string jsonObject = JsonSerializer.Serialize(encrypted);
                sw.Write(jsonObject);
            }
        }

        private static Settings DecryptSettings(Settings input)
        {
            Settings decrypted = new Settings();
            if (input == null)
                return decrypted;

            decrypted = input.Clone();
            decrypted.ZenHubToken = string.IsNullOrEmpty(input.ZenHubToken) ? string.Empty : Decrypt(input.ZenHubToken);
            decrypted.GitHubToken = string.IsNullOrEmpty(input.GitHubToken) ? string.Empty : Decrypt(input.GitHubToken);
            return decrypted;
        }

        private static Settings EncryptSettings(Settings input)
        {
            Settings encrypted = input.Clone();
            encrypted.ZenHubToken = Encrypt(input.ZenHubToken);
            encrypted.GitHubToken = Encrypt(input.GitHubToken);
            return encrypted;
        }

        public static Settings Deserialize(string file)
        {
            if (!File.Exists(file))
            {
                return new Settings();
            }

            using (StreamReader sr = new StreamReader(file))
            {
                return DecryptSettings(JsonSerializer.Deserialize<Settings>(sr.ReadToEnd()));
            }
        }

        private static string Encrypt(string text)
        {
            // first, convert the text to byte array 
            byte[] originalText = Encoding.Unicode.GetBytes(text);

            // then use Protect() to encrypt your data 
            byte[] encryptedText = ProtectedData.Protect(originalText, s_entropy, DataProtectionScope.CurrentUser);

            //and return the encrypted message 
            return Convert.ToBase64String(encryptedText);
        }

        private static string Decrypt(string text)
        {
            // the encrypted text, converted to byte array 
            byte[] encryptedText = Convert.FromBase64String(text);

            // calling Unprotect() that returns the original text 
            byte[] originalText = ProtectedData.Unprotect(encryptedText, s_entropy, DataProtectionScope.CurrentUser);

            // finally, returning the result 
            return Encoding.Unicode.GetString(originalText);
        }

        public Settings Clone()
        {
            Settings newSettings = new Settings()
            {
                Repositories = new List<string>(this.Repositories),
                ZenHubToken = this.ZenHubToken,
                GitHubToken = this.GitHubToken,
                DefaultTitle = this.DefaultTitle,
                DefaultLabels = new List<string>(this.DefaultLabels)
            };
            return newSettings;
        }
    }
}
