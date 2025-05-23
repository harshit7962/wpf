﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Description:
//  ContentType class parses and validates the content-type string.
//  It provides functionality to compare the type/subtype values.
//
// Details:
// Grammar which this class follows - 
//
// Content-type grammar MUST conform to media-type grammar as per 
// RFC 2616 (ABNF notation):
// 
// media-type     = type "/" subtype *( ";" parameter )
// type           = token
// subtype        = token
// parameter      = attribute "=" value
// attribute      = token
// value          = token | quoted-string
// quoted-string  = ( <"> *(qdtext | quoted-pair ) <"> )
// qdtext         = <any TEXT except <">>
// quoted-pair    = "\" CHAR
// token          = 1*<any CHAR except CTLs or separators>
// separators     = "(" | ")" | "<" | ">" | "@"
//                  | "," | ";" | ":" | "\" | <">
//                  | "/" | "[" | "]" | "?" | "="
//                  | "{" | "}" | SP | HT
// TEXT           = <any OCTET except CTLs, but including LWS>
// OCTET          = <any 8-bit sequence of data>
// CHAR           = <any US-ASCII character (octets 0 - 127)>
// CTL            = <any US-ASCII control character(octets 0 - 31)and DEL(127)>
// CR             = <US-ASCII CR, carriage return (13)>
// LF             = <US-ASCII LF, linefeed (10)>
// SP             = <US-ASCII SP, space (32)>
// HT             = <US-ASCII HT, horizontal-tab (9)>
// <">            = <US-ASCII double-quote mark (34)>
// LWS            = [CRLF] 1*( SP | HT )
// CRLF           = CR LF
// Linear white space (LWS) MUST NOT be used between the type and subtype, nor 
// between an attribute and its value. Leading and trailing LWS are prohibited.

using System.Text;

namespace MS.Internal
{
    /// <summary>
    /// Content Type class
    /// </summary>
    internal sealed class ContentType
    {       
        //------------------------------------------------------
        //
        //  Internal Constructors
        //
        //------------------------------------------------------

        #region Internal Constructors

        /// <summary>
        /// This constructor creates a ContentType object that represents 
        /// the content-type string. At construction time we validate the 
        /// string as per the grammar specified in RFC 2616.
        /// Note: We allow empty strings as valid input. Empty string should
        /// we used more as an indication of an absent/unknown ContentType. 
        /// </summary>
        /// <param name="contentType">content-type</param>
        /// <exception cref="ArgumentNullException">If the contentType parameter is null</exception>
        /// <exception cref="ArgumentException">If the contentType string has leading or 
        /// trailing Linear White Spaces(LWS) characters</exception>
        /// <exception cref="ArgumentException">If the contentType string invalid CR-LF characters</exception>
        internal ContentType(string contentType)
        {
            ArgumentNullException.ThrowIfNull(contentType);

            if (contentType.Length == 0)
            {
                _contentType = String.Empty;
            }
            else
            {
                if (IsLinearWhiteSpaceChar(contentType[0]) || IsLinearWhiteSpaceChar(contentType[contentType.Length - 1]))
                    throw new ArgumentException(SR.ContentTypeCannotHaveLeadingTrailingLWS);

                //Carriage return can be expressed as '\r\n' or '\n\r'
                //We need to make sure that a \r is accompanied by \n
                ValidateCarriageReturns(contentType);

                //Begin Parsing                
                int semiColonIndex = contentType.IndexOf(_semicolonSeparator);

                if (semiColonIndex == -1)
                {
                    // Parse content type similar to - type/subtype
                    ParseTypeAndSubType(contentType);
                }
                else
                {
                    // Parse content type similar to - type/subtype ; param1=value1 ; param2=value2 ; param3="value3"
                    ParseTypeAndSubType(contentType.AsSpan(0, semiColonIndex));
                    ParseParameterAndValue(contentType.AsSpan(semiColonIndex));
                }
            }

            // keep this untouched for return from OriginalString property
            _originalString = contentType;

            //This variable is used to print out the correct content type string representation
            //using the ToString method. This is mainly important while debugging and seeing the
            //value of the content type object in the debugger.
            _isInitialized = true;
        }

        #endregion Internal Constructors

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal Properties

        /// <summary>
        /// TypeComponent of the Content Type
        /// If the content type is "text/xml". This property will return "text"
        /// </summary>
        internal string TypeComponent
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// SubType component
        /// If the content type is "text/xml". This property will return "xml"
        /// </summary>
        internal string SubTypeComponent
        {
            get
            {
                return _subType;
            }
        }

        /// <summary>
        /// Enumerator which iterates over the Parameter and Value pairs which are stored 
        /// in a dictionary. We hand out just the enumerator in order to make this property
        /// ReadOnly
        /// Consider following Content type - 
        /// type/subtype ; param1=value1 ; param2=value2 ; param3="value3"
        /// This will return a enumerator over a dictionary of the parameter/value pairs. 
        /// </summary>
        internal Dictionary<string, string>.Enumerator ParameterValuePairs
        {
            get
            {
                EnsureParameterDictionary();
                return _parameterDictionary.GetEnumerator();
            }
        }


        /// <summary>
        /// Static property that represents a content type that is empty ""
        /// This is not a valid content type as per the grammar and should be used
        /// in places where the content type is missing or not available. 
        /// </summary>
        internal static ContentType Empty
        {
            get
            {                
                return _emptyContentType;
            }           
        }

        /// <summary>
        /// Original string provided to constructor
        /// </summary>
        internal string OriginalString
        {
            get
            {
                return _originalString;
            }
        }
        #endregion Internal Properties

        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal Methods

        /// <summary>
        /// This method does a strong comparison of the content types, as parameters are not allowed. 
        /// We only compare the type and subType values in an ASCII case-insensitive manner.
        /// Parameters are not allowed to be present on any of the content type operands.
        /// </summary>
        /// <param name="contentType">Content type to be compared with</param>
        /// <returns></returns>
        internal bool AreTypeAndSubTypeEqual(ContentType contentType)
        {
            return AreTypeAndSubTypeEqual(contentType, false);     
        }

        /// <summary>
        /// This method does a weak comparison of the content types. We only compare the
        /// type and subType values in an ASCII case-insensitive manner.
        /// Parameter and value pairs are not used for the comparison. 
        /// If you wish to compare the paramters too, then you must get the ParameterValuePairs from
        /// both the ContentType objects and compare each parameter entry.
        /// The allowParameterValuePairs parameter is used to indicate whether the 
        /// comparison is tolerant to parameters being present or no.
        /// </summary>
        /// <param name="contentType">Content type to be compared with</param>
        /// <param name="allowParameterValuePairs">If true, allows the presence of parameter value pairs. 
        /// If false, parameter/value pairs cannot be present in the content type string.
        /// In either case, the parameter value pair is not used for the comparison.</param>
        /// <returns></returns>
        internal bool AreTypeAndSubTypeEqual(ContentType contentType, bool allowParameterValuePairs)
        {
            bool result = false;

            if (contentType != null)
            {
                if (!allowParameterValuePairs)
                {
                    //Return false if this content type object has parameters
                    if (_parameterDictionary != null && _parameterDictionary.Count > 0)
                        return false;

                    //Return false if the content type object passed in has parameters
                    Dictionary<string, string>.Enumerator contentTypeEnumerator;
                    contentTypeEnumerator = contentType.ParameterValuePairs;
                    contentTypeEnumerator.MoveNext();
                    if (contentTypeEnumerator.Current.Key != null)
                        return false;
                }

                // Perform a case-insensitive comparison on the type/subtype strings.  This is a
                // safe comparison because the _type and _subType strings have been restricted to
                // ASCII characters, digits, and a small set of symbols.  This is not a safe comparison
                // for the broader set of strings that have not been restricted in the same way.
                result = string.Equals(_type, contentType.TypeComponent, StringComparison.OrdinalIgnoreCase) &&
                         string.Equals(_subType, contentType.SubTypeComponent, StringComparison.OrdinalIgnoreCase);
            }           
            return result;
        }

        /// <summary>
        /// ToString - outputs a normalized form of the content type string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {   
            if (_contentType == null)
            {
                //This is needed so that while debugging we get the correct 
                //string
                if (!_isInitialized)
                    return String.Empty;

                Debug.Assert(!string.IsNullOrEmpty(_type)
                             || !string.IsNullOrEmpty(_subType));
            
                StringBuilder stringBuilder = new StringBuilder(_type);
                stringBuilder.Append('/');
                stringBuilder.Append(_subType);

                if (_parameterDictionary != null && _parameterDictionary.Count > 0)
                {   
                    foreach (string paramterKey in _parameterDictionary.Keys)
                    {
                        stringBuilder.Append(_linearWhiteSpaceChars[0]);
                        stringBuilder.Append(_semicolonSeparator);
                        stringBuilder.Append(_linearWhiteSpaceChars[0]);
                        stringBuilder.Append(paramterKey);
                        stringBuilder.Append(_equalSeparator);
                        stringBuilder.Append(_parameterDictionary[paramterKey]);
                    }
                }

                _contentType = stringBuilder.ToString();
            }
            
            return _contentType;
        }

        #endregion Internal Methods

        //------------------------------------------------------
        //
        //  Nested Classes
        //
        //------------------------------------------------------

        #region Nested Classes

        /// <summary>
        /// Comparer class makes it easier to put ContentType objects in collections.
        /// Only compares type and subtype components of the ContentType.  Could be
        /// expanded to optionally compare parameters as well.
        /// </summary>
        internal class StrongComparer : IEqualityComparer<ContentType>
        {
            /// <summary>
            /// This method does a strong comparison of the content types.
            /// Only compares the ContentTypes' type and subtype components.
            /// </summary>
            public bool Equals(ContentType x, ContentType y)
            {
                if (x == null)
                {
                    return (y == null);
                }
                else
                {
                    return x.AreTypeAndSubTypeEqual(y);
                }
            }            

            /// <summary>
            /// ContentTypes that are equal (which ignores casing) must have the same hash code.
            /// </summary>
            public int GetHashCode(ContentType obj)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.ToString());
            }
        }

        internal class WeakComparer : IEqualityComparer<ContentType>
        {            
            /// <summary>            
            /// This method does a weak comparison of the content types. 
            /// Parameter and value pairs are not used for the comparison. 
            /// </summary>
            public bool Equals(ContentType x, ContentType y)
            {
                if (x == null)
                {
                    return (y == null); 
                }
                else
                {
                    return x.AreTypeAndSubTypeEqual(y, true);
                }
            }

            /// <summary>
            /// ContentTypes that are equal (which ignores casing) must have the same hash code.
            /// </summary>
            public int GetHashCode(ContentType obj)
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(obj._type) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(obj._subType);
            }
        }
        #endregion Nested Classes

        //------------------------------------------------------
        //
        //  Private Methods
        //
        //------------------------------------------------------

        #region Private Methods


        /// <summary>
        /// This method validates if the content type string has
        /// valid CR-LF characters. Specifically we test if '\r' is 
        /// accompanied by a '\n' in the string, else its an error.
        /// </summary>
        /// <param name="contentType"></param>
        private static void ValidateCarriageReturns(string contentType)
        {
            Debug.Assert(!IsLinearWhiteSpaceChar(contentType[0]) && !IsLinearWhiteSpaceChar(contentType[contentType.Length - 1]));

            //Prior to calling this method we have already checked that first and last
            //character of the content type are not Linear White Spaces. So its safe to
            //assume that the index will be greater than 0 and less that length-2.

            int index = contentType.IndexOf(_linearWhiteSpaceChars[2]);
            
            while (index != -1)
            {
                if (contentType[index - 1] == _linearWhiteSpaceChars[1] || contentType[index + 1] == _linearWhiteSpaceChars[1])
                {
                    index = contentType.IndexOf(_linearWhiteSpaceChars[2], ++index);
                }
                else
                    throw new ArgumentException(SR.InvalidLinearWhiteSpaceCharacter);
            }
        }

        /// <summary>
        /// Parses the type ans subType tokens from the string. 
        /// Also verifies if the Tokens are valid as per the grammar.
        /// </summary>
        /// <param name="typeAndSubType">substring that has the type and subType of the content type</param>
        /// <exception cref="ArgumentException">If the typeAndSubType parameter does not have the "/" character</exception>
        private void ParseTypeAndSubType(ReadOnlySpan<char> typeAndSubType)
        {
            //okay to trim at this point the end of the string as Linear White Spaces(LWS) chars are allowed here.
            typeAndSubType = typeAndSubType.TrimEnd(_linearWhiteSpaceChars);

            int forwardSlashPos = typeAndSubType.IndexOf('/');
            if (forwardSlashPos < 0 || // no slashes
                typeAndSubType.Slice(forwardSlashPos + 1).IndexOf('/') >= 0) // more than one slash
            {
                throw new ArgumentException(SR.InvalidTypeSubType);
            }

            _type    = ValidateToken(typeAndSubType.Slice(0, forwardSlashPos).ToString());
            _subType = ValidateToken(typeAndSubType.Slice(forwardSlashPos + 1).ToString());
        }

        /// <summary>
        /// Parse the individual parameter=value strings
        /// </summary>
        /// <param name="parameterAndValue">This string has the parameter and value pair of the form
        /// parameter=value</param>
        /// <exception cref="ArgumentException">If the string does not have the required "="</exception>
        private void ParseParameterAndValue(ReadOnlySpan<char> parameterAndValue)
        {
            while (!parameterAndValue.IsEmpty)
            {
                //At this point the first character MUST be a semi-colon
                //First time through this test is serving more as an assert.
                if (parameterAndValue[0] != _semicolonSeparator)
                    throw new ArgumentException(SR.ExpectingSemicolon);

                //At this point if we have just one semicolon, then its an error.
                //Also, there can be no trailing LWS characters, as we already checked for that
                //in the constructor.
                if (parameterAndValue.Length == 1)
                    throw new ArgumentException(SR.ExpectingParameterValuePairs);

                //Removing the leading ; from the string
                parameterAndValue = parameterAndValue.Slice(1);

                //okay to trim start as there can be spaces before the begining
                //of the parameter name.
                parameterAndValue = parameterAndValue.TrimStart(_linearWhiteSpaceChars);

                int equalSignIndex = parameterAndValue.IndexOf(_equalSeparator);

                if (equalSignIndex <= 0 || equalSignIndex == (parameterAndValue.Length - 1))
                    throw new ArgumentException(SR.InvalidParameterValuePair);

                int parameterStartIndex = equalSignIndex + 1;

                //Get length of the parameter value
                int parameterValueLength = GetLengthOfParameterValue(parameterAndValue, parameterStartIndex);

                EnsureParameterDictionary();

                _parameterDictionary.Add(
                    ValidateToken(parameterAndValue.Slice(0, equalSignIndex).ToString()),
                    ValidateQuotedStringOrToken(parameterAndValue.Slice(parameterStartIndex, parameterValueLength).ToString()));

                parameterAndValue = parameterAndValue.Slice(parameterStartIndex + parameterValueLength).TrimStart(_linearWhiteSpaceChars);
            }
        }

        /// <summary>
        /// This method returns the length of the first parameter value in the input string.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex">Starting index for parsing</param>
        /// <returns></returns>
        private static int GetLengthOfParameterValue(ReadOnlySpan<char> s, int startIndex)
        {
            int length;
            
            //if the parameter value does not start with a '"' then,
            //we expect a valid token. So we look for Linear White Spaces or
            //a ';' as the terminator for the token value.
            if (s[startIndex] != '"')
            {
                int semicolonIndex = s.Slice(startIndex).IndexOf(_semicolonSeparator);

                if (semicolonIndex != -1)
                {
                    int lwsIndex = s.Slice(startIndex).IndexOfAny(_linearWhiteSpaceChars);
                    length = lwsIndex != -1 && lwsIndex < semicolonIndex ? lwsIndex : semicolonIndex;
                    length += startIndex; // the indexes from IndexOf{Any} are based on slicing from startIndex
                }
                else
                {
                    //If there is no linear white space found we treat the entire remaining string as 
                    //parameter value.
                    length = s.Length;
                }
            }
            else
            {
                //if the parameter value starts with a '"' then, we need to look for the
                //pairing '"' that is not preceded by a "\" ["\" is used to escape the '"']
                bool found = false;
                length = startIndex;

                while (!found)
                {
                    int startingLength = ++length;
                    length = s.Slice(startingLength).IndexOf('"');

                    if (length == -1)
                        throw new ArgumentException(SR.InvalidParameterValue);
                    length += startingLength; // IndexOf result is based on slicing from startingLength

                    if (s[length - 1] != '\\')
                    {
                        found = true;
                        length++;
                    }
                }
            }
            return length - startIndex;
        }

        /// <summary>
        /// Validating the given token
        /// The following checks are being made - 
        /// 1. If all the characters in the token are either ASCII letter or digit.
        /// 2. If all the characters in the token are either from the remaining allowed character set.
        /// </summary>
        /// <param name="token">string token</param>
        /// <returns>validated string token</returns>
        /// <exception cref="ArgumentException">If the token is Empty</exception>
        private static string ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException(SR.InvalidToken);

            for (int i = 0; i < token.Length; i++)
            {
                if (IsAsciiLetterOrDigit(token[i]))
                    continue;
                else
                    if (IsAllowedCharacter(token[i]))
                        continue;
                    else
                        throw new ArgumentException(SR.InvalidToken);
            }

            return token;
        }

        /// <summary>
        /// Validating if the value of a parameter is either a valid token or a 
        /// valid quoted string
        /// </summary>
        /// <param name="parameterValue">paramter value string</param>
        /// <returns>validate parameter value string</returns>
        /// <exception cref="ArgumentException">If the paramter value is empty</exception>
        private static string ValidateQuotedStringOrToken(string parameterValue)
        {
            if (string.IsNullOrEmpty(parameterValue))
                throw new ArgumentException(SR.InvalidParameterValue);

            if (parameterValue.Length >= 2 && 
                parameterValue.StartsWith(_quote, StringComparison.Ordinal) && 
                parameterValue.EndsWith(_quote, StringComparison.Ordinal))
                ValidateQuotedText(parameterValue.AsSpan(1, parameterValue.Length-2));
            else
                ValidateToken(parameterValue);

            return parameterValue;
        }

        /// <summary>
        /// This method validates if the text in the quoted string
        /// </summary>
        /// <param name="quotedText"></param>
        private static void ValidateQuotedText(ReadOnlySpan<char> quotedText)
        {
            //empty is okay

            for (int i = 0; i < quotedText.Length; i++)
            {
                if (IsLinearWhiteSpaceChar(quotedText[i]))
                    continue;

                if (quotedText[i] <= ' ' || quotedText[i] >= 0xFF)
                    throw new ArgumentException(SR.InvalidParameterValue);
                else
                    if (quotedText[i] == '"' && 
                        (i==0 || quotedText[i-1] != '\\'))
                        throw new ArgumentException(SR.InvalidParameterValue);
            }
        }

        /// <summary>
        /// Returns true if the input character is an allowed character
        /// Returns false if the input cha----ter is not an allowed character
        /// </summary>
        /// <param name="character">input character</param>
        /// <returns></returns>
        private static bool IsAllowedCharacter(char character) => AllowedCharacters.Contains(character);

        /// <summary>
        /// Returns true if the input character is an ASCII digit or letter
        /// Returns false if the input character is not an ASCII digit or letter
        /// </summary>
        /// <param name="character">input character</param>
        /// <returns></returns>
        private static bool IsAsciiLetterOrDigit(char character) =>
            ((((uint)character - 'A') & ~0x20) < 26) ||
            (((uint)character - '0') < 10);
 
        /// <summary>
        /// Returns true if the input character is one of the Linear White Space characters - 
        /// ' ', '\t', '\n', '\r'
        /// Returns false if the input character is none of the above
        /// </summary>
        /// <param name="ch">input character</param>
        /// <returns></returns>
        private static bool IsLinearWhiteSpaceChar(char ch) => new ReadOnlySpan<char>(_linearWhiteSpaceChars).Contains(ch);

        /// <summary>
        /// Lazy initialization for the ParameterDictionary
        /// </summary>
        private void EnsureParameterDictionary()
        {
            if (_parameterDictionary == null)
            {
                _parameterDictionary = new Dictionary<string, string>(); //initial size 0
            }
        }        
       
        #endregion Private Methods

        //------------------------------------------------------
        //
        //  Private Members
        //
        //------------------------------------------------------
        #region Private Members
 
        private string _contentType = null;
        private string _type    = String.Empty;
        private string _subType = String.Empty;
        private string _originalString;
        private Dictionary<string, string> _parameterDictionary = null;
        private bool   _isInitialized = false;

        private const string     _quote              = "\"";
        private const char       _semicolonSeparator = ';';
        private const char       _equalSeparator     = '=';

        //This array is sorted by the ascii value of these characters.
        private static ReadOnlySpan<char> AllowedCharacters => [
        '!' /*33*/, '#' /*35*/ , '$'  /*36*/,
        '%' /*37*/, '&' /*38*/ , '\'' /*39*/,
        '*' /*42*/, '+' /*43*/ , '-'  /*45*/,
        '.' /*46*/, '^' /*94*/ , '_'  /*95*/,
        '`' /*96*/, '|' /*124*/, '~'  /*126*/,
        ];
        
        //Linear White Space characters
        private static readonly char[] _linearWhiteSpaceChars = [
           ' ',  // space           - \x20
           '\n', // new line        - \x0A
           '\r', // carriage return - \x0D
           '\t'  // horizontal tab  - \x09
         ];

        private static readonly ContentType _emptyContentType = new ContentType("");

        #endregion Private Members
    }
}

