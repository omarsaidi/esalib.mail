﻿using System;
using System.Collections.Generic;
using System.Net;

namespace EsaLib.Mail
{
    public class Tokenizer : ITokenizer
    {
        private readonly StringComparison _stringComparison;

        /// <summary>
        /// Ctor
        /// </summary>
        public Tokenizer()
        {
            _stringComparison = StringComparison.OrdinalIgnoreCase;
        }

        /// <summary>
        /// Replace all of the token key occurences inside the specified template text with corresponded token values
        /// </summary>
        /// <param name="template">The template with token keys inside</param>
        /// <param name="tokens">The sequence of tokens to use</param>
        /// <param name="htmlEncode">The value indicating whether tokens should be HTML encoded</param>
        /// <returns>Text with all token keys replaces by token value</returns>
        public string Replace(string template, IEnumerable<Token> tokens, bool htmlEncode)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException("template");

            if (tokens == null)
                throw new ArgumentNullException("tokens");

            foreach (var token in tokens)
            {
                string tokenValue = token.Value;
                //do not encode URLs
                if (htmlEncode && !token.NeverHtmlEncoded)
                    tokenValue = WebUtility.HtmlEncode(tokenValue);
                template = Replace(template, $@"%{token.Key}%", tokenValue);
            }
            return template;

        }

        private string Replace(string original, string pattern, string replacement)
        {
            if (_stringComparison == StringComparison.Ordinal)
            {
                return original.Replace(pattern, replacement);
            }

            int count = 0;
            int position0 = 0;
            int position1 = 0;
            
            int inc = original.Length / pattern.Length * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = original.IndexOf(pattern, position0, _stringComparison)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);

        }
    }
}